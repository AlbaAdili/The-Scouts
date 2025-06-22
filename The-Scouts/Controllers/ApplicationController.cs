using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using The_Scouts.DTOs;
using The_Scouts.Models;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey = "all_applications";

        public ApplicationController(IApplicationService applicationService, UserManager<IdentityUser> userManager, IMemoryCache cache)
        {
            _applicationService = applicationService;
            _userManager = userManager;
            _cache = cache;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitApplication([FromForm] ApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userEmail = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userManager.FindByNameAsync(userEmail);
            if (user == null)
                return Unauthorized();

            if (dto.Resume == null || dto.Resume.Length == 0)
                return BadRequest("Resume file is required.");

            var uploadsFolder = Path.Combine("Uploads", "Resumes");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.Resume.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Resume.CopyToAsync(stream);
            }

            await _applicationService.AddAsync(dto, user.Id, filePath);
            _cache.Remove(_cacheKey);

            return Ok("Application submitted successfully.");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllApplications()
        {
            if (!_cache.TryGetValue(_cacheKey, out IEnumerable<ApplicationDto> applications))
            {
                applications = await _applicationService.GetAllAsync();

                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };

                _cache.Set(_cacheKey, applications, options);
            }

            return Ok(applications);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var application = await _applicationService.FindOneAsync(id);
            if (application == null)
                return NotFound();

            return Ok(application);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetByUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            var application = await _applicationService.FindByUserEmailAsync(email);
            if (application == null)
                return NotFound();

            return Ok(application);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var updated = await _applicationService.UpdateStatusAsync(id, status);
            if (updated == null)
                return NotFound("Application not found or invalid status.");

            _cache.Remove(_cacheKey);
            return Ok("Status updated.");
        }
    }
}
