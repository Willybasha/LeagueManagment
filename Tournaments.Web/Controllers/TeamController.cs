using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tournaments.Web.Service.TeamService.DTOs;
using Tournaments.Web.Service.TeamService;
using Tournaments.Web.Services.TournamentsService;
using AutoMapper;
using Tournaments.Web.Data;
using Tournaments.Web.Services.TournamentService.DTOs;
using Microsoft.EntityFrameworkCore;
using Tournaments.Web.Entities;
using System.Linq.Dynamic.Core;
using Tournaments.Web.Models;
using Tournaments.Web.Helpers;
using Tournaments.Web.Filters;

namespace Tournaments.Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;
        public TeamController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }

        #region Populate view Model
        private TeamRequestDto PopulateViewModel(TeamRequestDto? model = null)
        {
            TeamRequestDto viewModel = model is null ? new TeamRequestDto() : model;

            var tournaments = _context.tournaments.OrderBy(a => a.Name).ToList();

            viewModel.Touraments = _mapper.Map<IEnumerable<SelectListItem>>(tournaments);

            return viewModel;
        }
        #endregion
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetTeams()
        {
            var skip = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);

            var searchValue = Request.Form["search[value]"];

            var sortColumnIndex = Request.Form["order[0][column]"];
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            IQueryable<Team> teams = _context.teams
                .Include(b => b.Tournaments).ThenInclude(t=>t.Tournament);

            if (!string.IsNullOrEmpty(searchValue))
                teams = teams.Where(b => b.Name.Contains(searchValue));

            teams = teams.OrderBy($"{sortColumn} {sortColumnDirection}");

            var data = teams.Skip(skip).Take(pageSize).ToList();

            var mappedData = _mapper.Map<IEnumerable<TeamResponseDto>>(data);

            var recordsTotal = teams.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = mappedData };

            return Ok(jsonData);
        }

        public IActionResult Details(int id)
        {
            var team = _context.teams.Include(b => b.Tournaments).ThenInclude(t=>t.Tournament).SingleOrDefault(b => b.TeamId == id);

            if (team is null)
                return NotFound();

            var viewModel = _mapper.Map<TeamResponseDto>(team);

            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View("_form", PopulateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teams = _mapper.Map<Team>(model);
            byte[] bytes = null;
            if (model.Logo is not null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    model.Logo.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray(); // Assuming LogoData is a byte[] property in your Team model
                }
                teams.Logo = bytes;

                var extension = Path.GetExtension(model.Logo.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Logo), Errors.NotAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }

                if (model.Logo.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Logo), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/Tournaments", imageName);
                var thumbPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/Tournaments/thumb", imageName);

                using var stream = System.IO.File.Create(path);
                await model.Logo.CopyToAsync(stream);
                stream.Dispose();


            }

            foreach (var tournament in model.SelectedTournaments)
                teams.Tournaments.Add(new TeamTournament { _TournamentId = tournament });

            _context.Add(teams);
            _context.SaveChanges();


            return RedirectToAction(nameof(Details), new { id = teams.TeamId });
        }

        public IActionResult Edit(int id)
        {
            var team = _context.teams.Include(b => b.Tournaments).SingleOrDefault(b => b.TeamId == id);

            if (team is null)
                return NotFound();

            var model = _mapper.Map<TeamRequestDto>(team);
            var viewModel = PopulateViewModel(model);

            viewModel.SelectedTournaments = team.Tournaments.Select(c => c.TeamId).ToList();

            return View("_form", viewModel);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(TeamRequestDto model)
        {
            if (!ModelState.IsValid)
                return View("_form", PopulateViewModel(model));

            var team = _context.teams.Include(b => b.Tournaments).SingleOrDefault(b => b.TeamId == model.TeamId);

            if (team is null)
                return NotFound();

            byte[] bytes = null;

            if (model.Logo is not null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    model.Logo.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray(); // Assuming LogoData is a byte[] property in your Team model
                }
                team.Logo = bytes;


                var extension = Path.GetExtension(model.Logo.FileName);

                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError(nameof(model.Logo), Errors.NotAllowedExtension);
                    return View("Form", PopulateViewModel(model));
                }

                if (model.Logo.Length > _maxAllowedSize)
                {
                    ModelState.AddModelError(nameof(model.Logo), Errors.MaxSize);
                    return View("Form", PopulateViewModel(model));
                }

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/Teams", imageName);

                using var stream = System.IO.File.Create(path);
                await model.Logo.CopyToAsync(stream);
                stream.Dispose();

            }
            team = _mapper.Map(model, team);

            foreach (var tournament in model.SelectedTournaments)
                team.Tournaments.Add(new TeamTournament { _TournamentId = tournament });

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = team.TeamId });
        }

      
        

    }
}
