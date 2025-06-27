using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using The_Scouts.Models;
using The_Scouts.Services.Interfaces;


namespace The_Scouts.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly INewsletterService _service;

        public NewsletterController(INewsletterService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Newsletter newsletter)
        {
            if (ModelState.IsValid)
            {
                bool isSubscribed = await _service.IsEmailSubscribedAsync(newsletter.Email);

                if (!isSubscribed)
                {
                    await _service.AddAsync(newsletter);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

