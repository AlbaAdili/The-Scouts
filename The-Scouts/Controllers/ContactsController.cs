using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using The_Scouts.Models;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;

        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet("/Contact")]
        public IActionResult ContactForm()
        {
            return View(); // just a form
        }

        [HttpGet("/Contact/Admin")]
        public async Task<IActionResult> AdminMessages()
        {
            var dtoList = await _contactService.GetAllAsync();
            var models = _mapper.Map<IEnumerable<ContactMessage>>(dtoList);
            return View(models); // Pass contacts to admin view
        }

        [HttpGet("/Contact/View")]
        public IActionResult ViewSingle()
        {
            return View(); // if needed for single message
        }
    }
}