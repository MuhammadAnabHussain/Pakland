using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

using Pakland.Services;
using Pakland.Data;
using Pakland.Models;

namespace Pakland.Controllers
{

    public class PropertyDetailsController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public PropertyDetailsController(ApplicationDbContext context, IPropertyService propertyService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _propertyService = propertyService;
            _userManager = userManager;
        }

        // GET: PropertyDetails
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.PropertyDetails.ToListAsync());
        }

        public async Task<IActionResult> MyProperties()
        {
            var user = await _userManager.GetUserAsync(User);
            // if (user == null)
            // {
            //     return NotFound("User not found.");
            // }

            var properties = _context.PropertyDetails
                                     .Where(p => p.ApplicationUserId == user.Id && p.IsSold)
                                     .ToList();

            return View(properties);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsSold(int id)
        {
            var property = await _context.PropertyDetails.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // Handle case where user is not authenticated
            }

            // Associate the property with the current user
            property.ApplicationUserId = user.Id;

            // Update property status as sold
            property.IsSold = true;

            _context.Update(property);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: PropertyDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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

                // Set the UserId to the current user's ID
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound("User not found.");
                }
                propertyDetails.ApplicationUserId = user.Id;

                // Add the property details to the context and save changes
                _context.Add(propertyDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(propertyDetails);
        }


        // GET: PropertyDetails/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
