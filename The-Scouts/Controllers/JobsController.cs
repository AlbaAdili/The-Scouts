using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using The_Scouts.DTOs;
using The_Scouts.Models;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobsController(IJobService jobService, IMapper mapper)
        {
            _jobService = jobService;
            _mapper = mapper;
        }

        [HttpGet("/Jobs")]
        public async Task<IActionResult> Index()
        {
            var jobDtos = await _jobService.GetAllAsync(); // returns List<JobDto>
            var jobs = _mapper.Map<IEnumerable<Job>>(jobDtos); // map to Job model used in the view

            return View(jobs); // returns to Views/Jobs/Index.cshtml
        }

        [HttpGet("/Jobs/Details")]
        public IActionResult Details()
        {
            return View(); // Views/Jobs/Details.cshtml
        }

        [HttpGet("/Jobs/Create")]
        public IActionResult Create()
        {
            return View(); // Views/Jobs/Create.cshtml
        }

        [HttpGet("/Jobs/Edit")]
        public IActionResult Edit()
        {
            return View(); // Views/Jobs/Edit.cshtml
        }
    }
}