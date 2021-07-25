using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext context;

        public PlacementsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Placements
        public async Task<IActionResult> Index(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            var placements = await this.context.Placements
                .Include(a => a.Ward)
                .Include(a => a.Patient)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(placements);
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .Include(a => a.Patient)
                .Include(a => a.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (placement == null)
            {
                return this.NotFound();
            }

            return this.View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            this.ViewData["WardId"] = new SelectList(this.context.Wards, "Id", "Name");
            return this.View(new PlacementCreateModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, PlacementCreateModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this.context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var placement = new Placement
                {
                    PatientId = patient.Id,
                    Bed = model.Bed,
                    WardId = model.WardId
                };

                this.context.Placements.Add(placement);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }
            this.ViewBag.Patient = patient;
            this.ViewData["WardId"] = new SelectList(this.context.Wards, "Id", "Name");
            return this.View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .SingleOrDefaultAsync(m => m.Id == id);

            if (placement == null)
            {
                return this.NotFound();
            }

            var model = new PlacementEditModel
            {
                Bed = placement.Bed
            };
            this.ViewBag.PatientId = placement.PatientId;

            return this.View(model);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlacementEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .SingleOrDefaultAsync(x => x.Id == id);

            if (placement == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                placement.Bed = model.Bed;
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = placement.PatientId });
            }

            return this.View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this.context.Placements
                .Include(a => a.Patient)
                .Include(a => a.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            return this.View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var placement = await this.context.Placements
                .SingleOrDefaultAsync(m => m.Id == id);
            this.context.Placements.Remove(placement);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { patientId = placement.PatientId });
        }
    }
}
