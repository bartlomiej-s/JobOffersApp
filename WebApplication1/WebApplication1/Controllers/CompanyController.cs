using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.EntityFramework;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly DataContext _context;

        public CompanyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.Companies.ToListAsync());

            List<Company> searchResult = await _context.Companies.Where(o => o.Name.Contains(searchString)).ToListAsync();
            return View(searchResult);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (company == null)
            {
                return NotFound($"offer not found in DB");
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == model.Id);
            company.Name = model.Name;
            _context.Update(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = model.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }

            _context.Companies.Remove(new Company() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new Company { };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Company model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Company co = new Company
            {
                Name = model.Name
            };

            await _context.Companies.AddAsync(co);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}