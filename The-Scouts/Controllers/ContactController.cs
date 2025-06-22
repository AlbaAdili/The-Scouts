using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using The_Scouts.DTOs;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMemoryCache _cache;
        private const string AllContactsCacheKey = "all_contacts";

        public ContactController(IContactService contactService, IMemoryCache cache)
        {
            _contactService = contactService;
            _cache = cache;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ContactMessageDto>>> GetAllMessages()
        {
            if (!_cache.TryGetValue(AllContactsCacheKey, out IEnumerable<ContactMessageDto> contacts))
            {
                contacts = await _contactService.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(AllContactsCacheKey, contacts, cacheOptions);
            }

            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactMessageDto>> GetById(int id)
        {
            var cacheKey = $"contact_{id}";

            if (!_cache.TryGetValue(cacheKey, out ContactMessageDto? contact))
            {
                contact = await _contactService.FindOneAsync(id);
                if (contact == null)
                    return NotFound("Message not found");

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(cacheKey, contact, cacheOptions);
            }

            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] ContactMessageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _contactService.AddAsync(dto);
            _cache.Remove(AllContactsCacheKey); // Invalidate all cache
            return Ok("Message submitted successfully");
        }
    }
}
