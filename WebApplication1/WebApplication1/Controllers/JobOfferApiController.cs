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
    public class JobOfferApiController : ControllerBase
    {
        private readonly DataContext _context;

        public JobOfferApiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/JobOfferApi
        /// <summary>
        /// Get list of job offers
        /// </summary>
        /// <remarks></remarks>
        /// <param name="pageNo">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">String used to search for job offers</param>
        /// <returns>JobOfferPagingViewModel</returns>
        [HttpGet]
        public JobOfferPagingViewModel GetJobOffers(int pageNo = 1, int pageSize = 4, [FromQuery(Name = "search")] string searchString = "")
        {
            int totalPage, totalRecord;

            var collection = _context.JobOfers.Where(o => o.JobTitle.Contains(searchString));

            totalRecord = collection.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = (from c in collection
                          orderby c.JobTitle
                          select c).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            foreach (var offer in record)
            {
                offer.Company = _context.Companies.FirstOrDefault(x => x.Id == offer.CompanyId);
            }

            JobOfferPagingViewModel joData = new JobOfferPagingViewModel
            {
                JobOffers = record,
                TotalPage = totalPage
            };

            return joData;
        }

        // GET: api/JobOfferApi/5
        /// <summary>
        /// Get one of job offers
        /// </summary>
        /// <remarks>Get for one element</remarks>
        /// <param name="id">ID of job offer</param>
        /// <returns>JobOffer</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobOffer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobOffer = await _context.JobOfers.FindAsync(id);

            if (jobOffer == null)
            {
                return NotFound();
            }

            return Ok(jobOffer);
        }

        // PUT: api/JobOfferApi/5
        /// <summary>
        /// Modify job offer
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">ID of job offer</param>
        /// <param name="jo">Job offer</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJobOffer([FromRoute] int id, [FromBody] JobOffer jo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != jo.Id)
            {
                return BadRequest();
            }

            _context.Entry(jo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobOfferExists(id))
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

        // POST: api/JobOfferApi
        /// <summary>
        /// Create job offer
        /// </summary>
        /// <remarks></remarks>
        /// <param name="jo">Job offer</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostJobOffer([FromBody] JobOffer jo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.JobOfers.Add(jo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJobOffer", new { id = jo.Id }, jo);
        }

        // DELETE: api/JobOfferApi/5
        /// <summary>
        /// Delete job offer
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">ID of job offer</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobOffer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jo = await _context.JobOfers.FindAsync(id);
            if (jo == null)
            {
                return NotFound();
            }

            _context.JobOfers.Remove(jo);
            await _context.SaveChangesAsync();

            return Ok(jo);
        }

        private bool JobOfferExists(int id)
        {
            return _context.JobOfers.Any(e => e.Id == id);
        }
    }
}
