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
    public class DoctorPatientsController : Controller
    {
        private readonly ApplicationDbContext context;

        public DoctorPatientsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: DoctorPatients
        public async Task<IActionResult> Index(Int32? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            var items = await this.context.DoctorPatients
                .Include(d => d.Doctor)
                .Include(p => p.Patient)
                .Where(x => x.DoctorId == doctor.Id)
                .ToListAsync();

            this.ViewBag.Doctor = doctor;
            return this.View(items);
        }

        // GET: DoctorPatients/Create
        public async Task<IActionResult> Create(Int32? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Doctor = doctor;
            this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name");
            return this.View(new DoctorPatientCreateModel());
        }

        // POST: DoctorPatients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? doctorId, DoctorPatientCreateModel model)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }
            if (this.context.DoctorPatients.Any(x => x.DoctorId == doctorId && x.PatientId == model.PatientId))
            {
                this.ModelState.AddModelError("", "This patient already added.");
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var doctorPatient = new DoctorPatient
                {
                    DoctorId = doctor.Id,
                    PatientId = model.PatientId
                };

                this.context.Add(doctorPatient);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { doctorId = doctor.Id});
            }

            this.ViewBag.Doctor = doctor;
            this.ViewData["PatientId"] = new SelectList(this.context.Patients, "Id", "Name", model.PatientId);
            return this.View(model);
        }

        // GET: DoctorPatients/Delete/5
        public async Task<IActionResult> Delete(Int32? doctorId, Int32? patientId)
        {
            if (doctorId == null || patientId == null)
            {
                return this.NotFound();
            }

            var doctorPatient = await this.context.DoctorPatients
                .Include(d => d.Doctor)
                .Include(d => d.Patient)
                .SingleOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            
            if (doctorPatient == null)
            {
                return this.NotFound();
            }

            return this.View(doctorPatient);
        }

        // POST: DoctorPatients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 doctorId, Int32 patientId)
        {
            var doctorPatient = await this.context.DoctorPatients.SingleOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            this.context.DoctorPatients.Remove(doctorPatient);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { doctorId = doctorId });
        }
    }
}
