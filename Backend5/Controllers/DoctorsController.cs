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
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContext context;

        public DoctorsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Doctors.ToListAsync());
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return this.NotFound();
            }

            return this.View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            return this.View(new DoctorCreateModel());
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorCreateModel model)
        {
            if (this.ModelState.IsValid)
            {
                var doctor = new Doctor
                {
                    Name = model.Name,
                    Specialty = model.Specialty
                };

                this.context.Doctors.Add(doctor);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            return this.View(model);
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return this.NotFound();
            }

            var model = new DoctorEditModel
            {
                Name = doctor.Name,
                Specialty = doctor.Specialty
            };
            return this.View(model);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, DoctorEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                doctor.Name = model.Name;
                doctor.Specialty = model.Specialty;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index");
            }
            return this.View(model);
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            if (doctor == null)
            {
                return this.NotFound();
            }

            return this.View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var doctor = await this.context.Doctors
                .SingleOrDefaultAsync(m => m.Id == id);
            this.context.Doctors.Remove(doctor);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index");
        }

        private Boolean DoctorExists(Int32 id)
        {
            return this.context.Doctors.Any(e => e.Id == id);
        }
    }
}
