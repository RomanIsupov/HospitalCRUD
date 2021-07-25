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
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext context;

        public WardStaffsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: WardStaffs
        public async Task<IActionResult> Index(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            var wardStaffs = await this.context.WardStaffs
                .Include(w => w.Ward)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            return this.View(wardStaffs);
        }

        // GET: WardStaffs/Details/5
        public async Task<IActionResult> Details(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaff = await context.WardStaffs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            return this.View(wardStaff);
        }

        // GET: WardStaffs/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            return this.View(new WardStaffCreateModel());
        }

        // POST: WardStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, WardStaffCreateModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this.context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var WardStaff = new WardStaff
                {
                    WardId = ward.Id,
                    Name = model.Name,
                    Position = model.Position
                };

                this.context.WardStaffs.Add(WardStaff);
                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            return this.View(model);
        }

        // GET: WardStaffs/Edit/5
        public async Task<IActionResult> Edit(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaff = await this.context.WardStaffs
                .SingleOrDefaultAsync(m => m.Id == id);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            var model = new WardStaffEditModel
            {
                Name = wardStaff.Name,
                Position = wardStaff.Position
            };

            this.ViewBag.WardId = wardStaff.WardId;
            return this.View(model);
        }

        // POST: WardStaffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardStaffEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaff = await this.context.WardStaffs
                .SingleOrDefaultAsync(x => x.Id == id);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                wardStaff.Name = model.Name;
                wardStaff.Position = model.Position;

                await this.context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = wardStaff.WardId });
            }

            return this.View(model);
        }

        // GET: WardStaffs/Delete/5
        public async Task<IActionResult> Delete(Int32? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaff = await this.context.WardStaffs
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (wardStaff == null)
            {
                return this.NotFound();
            }

            return this.View(wardStaff);
        }

        // POST: WardStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 id)
        {
            var wardStaff = await this.context.WardStaffs
                .SingleOrDefaultAsync(m => m.Id == id);
            this.context.WardStaffs.Remove(wardStaff);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { wardId = wardStaff.WardId });
        }
    }
}
