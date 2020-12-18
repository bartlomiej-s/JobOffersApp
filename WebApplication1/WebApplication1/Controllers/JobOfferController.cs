using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return View(await _context.JobOfers.Include(x => x.Company).ToListAsync());

            List<JobOffer> searchResult = await _context.JobOfers.Include(x => x.Company).Where(o => o.JobTitle.Contains(searchString)).ToListAsync();
            return View(searchResult);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (offer == null)
            {
                return NotFound($"offer not found in DB");
            }

            return View(offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            offer.Description = model.Description;
            _context.Update(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }

            _context.JobOfers.Remove(new JobOffer() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            bool wrongModel = false;
            if (model.SalaryFrom > model.SalaryTo)
            {
                model.wrongSalaries = true;
                wrongModel = true;
            }
            if (model.SalaryFrom <= 0)
            {
                model.wrongSalaryFrom = true;
                wrongModel = true;
            }
            if (model.SalaryTo <= 0)
            {
                model.wrongSalaryTo = true;
                wrongModel = true;
            }
            if (model.ValidUntil != null && DateTime.Compare(model.ValidUntil.Value, DateTime.Now) < 0)
            {
                model.wrongValidUntil = true;
                wrongModel = true;
            }

            if (!ModelState.IsValid || wrongModel)
            {
                model.Companies = await _context.Companies.ToListAsync();
                return View(model);
            }

            JobOffer jo = new JobOffer
            {
                CompanyId = model.CompanyId,
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };

            await _context.JobOfers.AddAsync(jo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == id);
            offer.Company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == offer.CompanyId);
            offer.JobApplications = _context.JobApplications.Where(y => y.OfferId == id).ToList();
            return View(offer);
        }
    }
}