using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using The_Scouts.Services.Interfaces;
using The_Scouts.Models;


namespace The_Scouts.Controllers;

public class ApplicationsController : Controller
{
    
    private readonly IApplicationService _applicationService;
    private readonly IMapper _mapper;

    public ApplicationsController(IApplicationService applicationService, IMapper mapper)
    {
        _applicationService = applicationService;
        _mapper = mapper;
    }

    [HttpGet("/Application/Create")]
    public IActionResult Create()
    {
        return View(); // Only for displaying form
    }

    [HttpGet("/Application/Index")]
    public async Task<IActionResult> Index()
    {
        var dtoList = await _applicationService.GetAllAsync();
        var models = _mapper.Map<IEnumerable<Application>>(dtoList);
        return View(models); // Pass applications to the view
    }

    [HttpGet("/Application/Applications")]
    public async Task<IActionResult> Applications()
    {
        var dtoList = await _applicationService.GetAllAsync();
        var models = _mapper.Map<IEnumerable<Application>>(dtoList);
        return View(models); // Render list of all applications
    }
    }
