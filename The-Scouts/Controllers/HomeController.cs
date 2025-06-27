using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using The_Scouts.Models;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(); // no model passed anymore
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}