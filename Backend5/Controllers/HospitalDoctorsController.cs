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
    public class HospitalDoctorsController : Controller
    {
        private readonly ApplicationDbContext context;

        public HospitalDoctorsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: HospitalDoctors
        public async Task<IActionResult> Index(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);
            if (hospital == null)
            {
                return this.NotFound();
            }
            this.ViewBag.Hospital = hospital;

            var doctors = await this.context.HospitalDoctors
                .Include(d => d.Hospital)
                .Include(d => d.Doctor)
                .Where(x => x.HospitalId == hospital.Id)
                .ToListAsync();

            return this.View(doctors);
        }

        // GET: HospitalDoctors/Create
        public async Task<IActionResult> Create(Int32? hospitalId)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            this.ViewData["DoctorId"] = new SelectList(this.context.Doctors, "Id", "Name");
            this.ViewBag.Hospital = hospital;
            return this.View(new HospitalDoctorCreateModel());
        }

        // POST: HospitalDoctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? hospitalId, HospitalDoctorCreateModel model)
        {
            if (hospitalId == null)
            {
                return this.NotFound();
            }
            if (this.context.HospitalDoctors.Any(x => x.HospitalId == hospitalId && x.DoctorId == model.DoctorId))
            {
                this.ModelState.AddModelError("", "This doctor already added.");
            }

            var hospital = await this.context.Hospitals
                .SingleOrDefaultAsync(x => x.Id == hospitalId);

            if (hospital == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var hospitalDoctor = new HospitalDoctor
                {
                    HospitalId = hospital.Id,
                    DoctorId = model.DoctorId
                };

                this.context.Add(hospitalDoctor);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { hospitalId = hospital.Id });
            }

            this.ViewData["DoctorId"] = new SelectList(this.context.Doctors, "Id", "Name", model.DoctorId);
            this.ViewBag.Hospital = hospital;
            return this.View(model);
        }

        // GET: HospitalDoctors/Delete/5
        public async Task<IActionResult> Delete(Int32? hospitalId, Int32? doctorId)
        {
            if (hospitalId == null || doctorId == null)
            {
                return this.NotFound();
            }

            var hospitalDoctor = await this.context.HospitalDoctors
                .Include(h => h.Doctor)
                .Include(h => h.Hospital)
                .SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.DoctorId == doctorId);
            if (hospitalDoctor == null)
            {
                return this.NotFound();
            }

            return this.View(hospitalDoctor);
        }

        // POST: HospitalDoctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 hospitalId, Int32 doctorId)
        {
            var hospitalDoctor = await this.context.HospitalDoctors.SingleOrDefaultAsync(m => m.HospitalId == hospitalId && m.DoctorId == doctorId);
            this.context.HospitalDoctors.Remove(hospitalDoctor);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { hospitalId = hospitalId});
        }

        private Boolean HospitalDoctorExists(Int32 id)
        {
            return context.HospitalDoctors.Any(e => e.HospitalId == id);
        }
    }
}
