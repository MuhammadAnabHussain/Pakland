using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using Pakland.Services;
using Pakland.Data;
using Pakland.Models;

namespace Pakland.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PropertyDetailsController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly ApplicationDbContext _context;

        public PropertyDetailsController(ApplicationDbContext context, IPropertyService propertyService)
        {
            _context = context;
            _propertyService = propertyService;
        }

        // GET: PropertyDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.PropertyDetails.ToListAsync());
        }
        [HttpPost]
        public IActionResult MarkAsSold(int id)
        {
            // Assume you have a service or repository to update the property status
            var property = _propertyService.GetPropertyById(id);

            if (property != null)
            {
                // Update property status as sold
                property.IsSold = true;
                _propertyService.UpdateProperty(property);

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        // GET: PropertyDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyDetails = await _context.PropertyDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyDetails == null)
            {
                return NotFound();
            }

            return View(propertyDetails);
        }

        // GET: PropertyDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PropertyDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,Size,City,DateOfPurchase,IsSold")] PropertyDetails propertyDetails, IFormFile Image)
        {
            if (ModelState.IsValid)


            {
                // Check if an image file is uploaded
                if (Image != null && Image.Length > 0)
                {
                    // Read the file into a byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        await Image.CopyToAsync(memoryStream);
                        propertyDetails.Image = memoryStream.ToArray();
                        propertyDetails.ImageType = Image.ContentType;
                    }
                }
                _context.Add(propertyDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propertyDetails);
        }

        // GET: PropertyDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyDetails = await _context.PropertyDetails.FindAsync(id);
            if (propertyDetails == null)
            {
                return NotFound();
            }
            return View(propertyDetails);
        }

        // POST: PropertyDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,Size,City,DateOfPurchase,IsSold")] PropertyDetails propertyDetails, IFormFile Image)
        {
            if (id != propertyDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null && Image.Length > 0)
                    {
                        // Read the file into a byte array
                        using (var memoryStream = new MemoryStream())
                        {
                            await Image.CopyToAsync(memoryStream);
                            propertyDetails.Image = memoryStream.ToArray();
                            propertyDetails.ImageType = Image.ContentType;
                        }
                    }
                    _context.Update(propertyDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyDetailsExists(propertyDetails.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(propertyDetails);
        }

        // GET: PropertyDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var propertyDetails = await _context.PropertyDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (propertyDetails == null)
            {
                return NotFound();
            }

            return View(propertyDetails);
        }

        // POST: PropertyDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var propertyDetails = await _context.PropertyDetails.FindAsync(id);
            if (propertyDetails != null)
            {
                _context.PropertyDetails.Remove(propertyDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyDetailsExists(int id)
        {
            return _context.PropertyDetails.Any(e => e.Id == id);
        }
    }
}
