using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using The_Scouts.DTOs;
using The_Scouts.Services.Interfaces;

namespace The_Scouts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMemoryCache _cache;
        private readonly UserManager<IdentityUser> _userManager;

        private const string JobListCacheKey = "all_jobs";

        public JobController(IJobService jobService, IMemoryCache cache, UserManager<IdentityUser> userManager)
        {
            _jobService = jobService;
            _cache = cache;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetAllJobs()
        {
            if (!_cache.TryGetValue(JobListCacheKey, out IEnumerable<JobDto> jobs))
            {
                jobs = await _jobService.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(JobListCacheKey, jobs, cacheOptions);
            }

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetJobById(int id)
        {
            var cacheKey = $"job_{id}";

            if (!_cache.TryGetValue(cacheKey, out JobDto? job))
            {
                job = await _jobService.FindOneAsync(id);

                if (job == null)
                    return NotFound();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, job, cacheOptions);
            }

            return Ok(job);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admins (HR) can add
        public async Task<IActionResult> AddJob([FromBody] JobDto jobDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _jobService.AddAsync(jobDto);
            _cache.Remove(JobListCacheKey); // Invalidate cache
            return Ok("Job added successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] JobDto jobDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _jobService.FindOneAsync(id);
            if (existing == null)
                return NotFound("Job not found");

            await _jobService.UpdateAsync(id, jobDto);
            _cache.Remove(JobListCacheKey);
            _cache.Remove($"job_{id}");
            return Ok("Job updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var existing = await _jobService.FindOneAsync(id);
            if (existing == null)
                return NotFound("Job not found");

            await _jobService.DeleteAsync(id);
            _cache.Remove(JobListCacheKey);
            _cache.Remove($"job_{id}");
            return Ok("Job deleted successfully");
        }
        
   
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<JobDto>>> SearchJobs([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query is required.");

            var results = await _jobService.SearchAsync(query);
            return Ok(results);
        }

    }
}
