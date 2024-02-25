using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tournaments.Web.Data;
using Tournaments.Web.Entities;
using Tournaments.Web.Filters;
using Tournaments.Web.Helpers;
using Tournaments.Web.Models;
using Tournaments.Web.Service.TeamService;
using Tournaments.Web.Service.TournamentService.DTOs;
using Tournaments.Web.Services.TournamentService.DTOs;
using Tournaments.Web.Services.TournamentsService;
using static System.Net.Mime.MediaTypeNames;

namespace Tournaments.Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        private List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private int _maxAllowedSize = 2097152;
        public TournamentController(ApplicationDbContext context, IMapper mapper,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }

        #region Populate view Model
        private TournamentRequestDto PopulateViewModel(TournamentRequestDto? model = null)
        {
            TournamentRequestDto viewModel = model is null ? new TournamentRequestDto() : model;

            var teams = _context.teams.OrderBy(a => a.Name).ToList();

            viewModel.Teams = _mapper.Map<IEnumerable<SelectListItem>>(teams);

            return viewModel;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult GetTournaments()
        {
            var skip = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);

            var searchValue = Request.Form["search[value]"];

            var sortColumnIndex = Request.Form["order[0][column]"];
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            IQueryable<_Tournament> tournaments = _context.tournaments
                .Include(b => b.Teams).ThenInclude(t=>t.Team);

            if (!string.IsNullOrEmpty(searchValue))
                tournaments = tournaments.Where(b => b.Name.Contains(searchValue));

            tournaments = tournaments.OrderBy($"{sortColumn} {sortColumnDirection}");

            var data = tournaments.Skip(skip).Take(pageSize).ToList();

            var mappedData = _mapper.Map<IEnumerable<TournamentResponseDto>>(data);

            var recordsTotal = tournaments.Count();

            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = mappedData };

            return Ok(jsonData);
        }

        public IActionResult Details(int id)
        {
            var tournament = _context.tournaments.Include(b => b.Teams).ThenInclude(t => t.Team).SingleOrDefault(b => b._TournamentId == id);

            if (tournament is null)
                return NotFound();

            var viewModel = _mapper.Map<TournamentResponseDto>(tournament);

            return View(viewModel);
        }
        public IActionResult Create()
        {
            return View("_form", PopulateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(TournamentRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tournament = _mapper.Map<_Tournament>(model);
            byte[] bytes = null;
            if (model.Logo is not null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    model.Logo.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray(); // Assuming LogoData is a byte[] property in your Team model
                }
                tournament.Logo = bytes;

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
            foreach (var teams in model.SelectedTeams)
                tournament.Teams.Add(new TeamTournament { TeamId = teams });

            _context.Add(tournament);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = tournament._TournamentId });
        }

        public IActionResult Edit(int id)
        {
            var tournament= _context.tournaments.Include(b => b.Teams).SingleOrDefault(b => b._TournamentId == id);

            if (tournament is null)
                return NotFound();

            var model = _mapper.Map<TournamentRequestDto>(tournament);
            var viewModel = PopulateViewModel(model);

            viewModel.SelectedTeams = tournament.Teams.Select(c => c.TeamId).ToList();

            return View("_form", viewModel);
        }

        [HttpPost]
       
        public async Task<IActionResult> Edit(TournamentRequestDto model)
        {
            if (!ModelState.IsValid)
                return View("_form", PopulateViewModel(model));

            var tournament = _context.tournaments.Include(b => b.Teams).SingleOrDefault(b => b._TournamentId == model._TournamentId);

            if (tournament is null)
                return NotFound();

            byte[] bytes = null;

            if (model.Logo is not null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    model.Logo.CopyTo(memoryStream);
                    bytes = memoryStream.ToArray(); // Assuming LogoData is a byte[] property in your Team model
                }
                tournament.Logo = bytes;


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

                using var stream = System.IO.File.Create(path);
                await model.Logo.CopyToAsync(stream);
                stream.Dispose();

            }
            tournament = _mapper.Map(model, tournament);
          
            foreach (var team in model.SelectedTeams)
                tournament.Teams.Add(new TeamTournament { TeamId = team });

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = tournament._TournamentId });
        }

        #region old code
        /*private readonly ITournamentService _tournamentService;
        private readonly ITeamService _teamService;
        public TournamentController(ITournamentService tournamentService, ITeamService teamService)
        {
            _teamService = teamService;
            _tournamentService = tournamentService;
        }
        public async Task<IActionResult> Index()
        {
            // Assuming you have a service or data access layer to get the list of teams
            var touraments = await _tournamentService.GetListOfTournamnets(); // You need to implement this method
            return View(touraments);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
           *//* var teams = await _teamService.GetListOfTeams();
            var model = new TournamentRequestDto
            {
                Name = string.Empty,
                Description = string.Empty,
                TournamentVideoEmbed = string.Empty,
                Teams = teams.ToList().Select(x => new SelectListItem
                {
                    Value = x.TeamId.ToString(),
                    Text = x.Name,

                }).ToList()

            };*//*

            return View("_form");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TournamentRequestDto tournament)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var createdtournament = await _tournamentService.CreateTournament(tournament);

            return PartialView("TournamentRow",createdtournament);
        }
  
        [HttpGet]
        public async Task<IActionResult> EditTournament(int id)
        {
            var Viewmodel = await _tournamentService.UpdateTournament(id);
            return PartialView("_form", Viewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TournamentForUpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var tournamnet = await _tournamentService.UpdateTournament(model._TournamentId);
           
            if (tournamnet is null)
                return NotFound();


            return PartialView("TournamentRow", tournamnet);
        }*/
        #endregion
    }
}
