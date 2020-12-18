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
    public class CompanyApiController : ControllerBase
    {
        private readonly DataContext _context;

        public CompanyApiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/CompanyApi
        /// <summary>
        /// Get list of companies
        /// </summary>
        /// <remarks></remarks>
        /// <param name="pageNo">Page number</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="searchString">String used to search for companies</param>
        /// <returns>CompanyPagingViewModel</returns>
        [HttpGet]
        public CompanyPagingViewModel GetCompanies(int pageNo = 1, int pageSize = 4, [FromQuery(Name = "search")] string searchString = "")
        {
            int totalPage, totalRecord;

            var collection = _context.Companies.Where(o => o.Name.Contains(searchString));

            totalRecord = collection.Count();
            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = (from c in collection
                          orderby c.Name
                          select c).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

            CompanyPagingViewModel compData = new CompanyPagingViewModel
            {
                Companies = record,
                TotalPage = totalPage
            };

            return compData;
        }

        // GET: api/CompanyApi/5
        /// <summary>
        /// Get one of companies
        /// </summary>
        /// <remarks>Get for one element</remarks>
        /// <param name="id">ID of company</param>
        /// <returns>Company</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        // PUT: api/CompanyApi/5
        /// <summary>
        /// Modify company
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">ID of company</param>
        /// <param name="comp">Company</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany([FromRoute] int id, [FromBody] Company comp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comp.Id)
            {
                return BadRequest();
            }

            _context.Entry(comp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/CompanyApi
        /// <summary>
        /// Create company
        /// </summary>
        /// <remarks></remarks>
        /// <param name="comp">Company</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostCompany([FromBody] Company comp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Companies.Add(comp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = comp.Id }, comp);
        }

        // DELETE: api/CompanyApi/5
        /// <summary>
        /// Delete company
        /// </summary>
        /// <remarks></remarks>
        /// <param name="id">ID of company</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comp = await _context.Companies.FindAsync(id);
            if (comp == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(comp);
            await _context.SaveChangesAsync();

            return Ok(comp);
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
