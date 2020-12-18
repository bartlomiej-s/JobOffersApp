using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication1.EntityFramework;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationApiController : ControllerBase
    {
        private readonly DataContext _context;

        public JobApplicationApiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/JobApplicationApi
        /// <summary>
        /// Get list of job applications
        /// </summary>
        /// <remarks></remarks>
        /// <param name="pageNo">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="offer">ID of offer for which application was made</param>
        /// <returns>JobApplicationPagingViewModel</returns>
        [HttpGet]
        public async Task<JobApplicationPagingViewModel> GetJobApplications(int pageNo = 1, int pageSize = 4, [FromQuery(Name = "offer")] int offer = 1)
        {
            int totalPage, totalRecord;

            var collection = await _context.JobApplications.Where(o => o.OfferId == offer).ToListAsync();

            totalRecord = collection.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = (from c in collection
                          orderby c.FirstName, c.LastName
                          select c).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            JobApplicationPagingViewModel jaData = new JobApplicationPagingViewModel
            {
                JobApplications = record,
                TotalPage = totalPage
            };

            return jaData;
        }

        // GET: api/JobApplicationApi/5
        /// <summary>
        /// Get one of job applications
        /// </summary>
        /// <remarks>Get for one element</remarks>
        /// <param name="id">ID of job application</param>
        /// <returns>JobApplication</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobApplication([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobApplication = await _context.JobApplications.FindAsync(id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            return Ok(jobApplication);
        }

        // PUT: api/JobApplicationApi/5
        /// <summary>
        /// Modify job application
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">ID of job application</param>
        /// <param name="ja">Job application</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobApplication([FromRoute] int id, [FromBody] JobApplication ja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ja.Id)
            {
                return BadRequest();
            }

            _context.Entry(ja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/JobApplicationApi
        /// <summary>
        /// Create job application
        /// </summary>
        /// <remarks></remarks>
        /// <param name="ja">Job application</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostJobApplication([FromBody] JobApplication ja)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.JobApplications.Add(ja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobApplication", new { id = ja.Id }, ja);
        }

        // DELETE: api/JobApplicationApi/5
        /// <summary>
        /// Delete job application
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">ID of job application</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobApplication([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ja = await _context.JobApplications.FindAsync(id);
            if (ja == null)
            {
                return NotFound();
            }

            _context.JobApplications.Remove(ja);
            await _context.SaveChangesAsync();

            return Ok(ja);
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
