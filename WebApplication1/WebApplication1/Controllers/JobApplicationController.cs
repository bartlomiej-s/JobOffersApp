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
    public class JobApplicationController : Controller
    {
        private readonly DataContext _context;
        public JobApplicationController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Apply(int? id)
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

            var model = new JobApplication
            {
                OfferId = id.Value,
                Offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == id.Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Apply(JobApplication model)
        {
            if (!ModelState.IsValid)
            {
                model.Offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == model.OfferId);
                return View(model);
            }


            JobApplication ja = new JobApplication
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                CvUrl = model.CvUrl,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description,
                OfferId = model.OfferId
            };

            await _context.JobApplications.AddAsync(ja);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "JobOffer", new {id = model.OfferId });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var application = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == id);
            application.Offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == application.OfferId);
            return View(application);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id should not be null");
            }

            var application = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == id);
            var offerId = application.OfferId;
            if (application != null)
            {
                _context.Entry(application).State = EntityState.Detached;
            }
            _context.JobApplications.Remove(new JobApplication() { Id = id.Value });
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "JobOffer", new { id = offerId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest($"id shouldn't not be null");
            }
            var application = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == id.Value);
            if (application == null)
            {
                return NotFound($"application not found in DB");
            }

            application.Offer = await _context.JobOfers.FirstOrDefaultAsync(x => x.Id == application.OfferId);
            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobApplication model)
        {
            var application = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == model.Id);
            application.PhoneNumber = model.PhoneNumber;
            application.EmailAddress = model.EmailAddress;
            application.CvUrl = model.CvUrl;
            application.Description = model.Description;
            _context.Update(application);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = model.Id });
        }
    }
}