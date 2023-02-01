using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudOperation.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.Intrinsics.X86;
using System.IO.Pipelines;

namespace CrudOperation.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Employees.Include(e => e.City).Include(e => e.Country).Include(e => e.State);
            return View(await appDbContext.ToListAsync());
        }
        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, IFormFile pictureFile)
        {
            if (ModelState.IsValid)
            {
                if (pictureFile != null && pictureFile.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/images",
                     pictureFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        pictureFile.CopyTo(stream);
                    }
                    employee.Picture = $"{pictureFile.FileName}";
                }
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName",
            employee.CityId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id",
            "CountryName", employee.CountryId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName",
            employee.StateId);
            return View(employee);

        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.City)
                .Include(e => e.Country)
                .Include(e => e.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", employee.CityId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", employee.CountryId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName", employee.StateId);
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want  to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee, IFormFile pictureFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emp = await _context.Employees.FindAsync(employee.Id);
                    if (pictureFile != null && pictureFile.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(),
                       "wwwroot/images",
                        pictureFile.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            pictureFile.CopyTo(stream);
                        }
                        employee.Picture = $"{pictureFile.FileName}";
                    }
                    else
                    {
                        employee.Picture = emp.Picture;
                    }
                    emp.Picture = employee.Picture;
                    emp.Name = employee.Name;
                    emp.Address = employee.Address;
                    emp.Gender = employee.Gender;
                    emp.Ssc = employee.Ssc;
                    emp.Hsc = employee.Hsc;
                    emp.Bsc = employee.Bsc;
                    emp.Msc = employee.Msc;
                    emp.CountryId = employee.CountryId;
                    emp.StateId = employee.StateId;
                    emp.CityId = employee.CityId;
                    _context.Update(emp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName",
           employee.CityId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id",
           "CountryName", employee.CountryId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName",
           employee.StateId);
            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            throw new NotImplementedException();
        }


        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.City)
                .Include(e => e.Country)
                .Include(e => e.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'AppDbContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //private bool EmployeeExists(int id)
        //{
        //    return _context.Employees.Any(e => e.Id == id);
        //}


        //Here additional Code for cascading dropdown
        public JsonResult GetStatesByCountryId(int countryId)
        {
            List<State> statesList = new List<State>();
            statesList = (from state in _context.States
                          where state.CountryId == countryId
                          select state).ToList();
            return Json(statesList);
        }
        public JsonResult GetCitiesByStateId(int stateId)
        {
            List<City> citiesList = new List<City>();
            citiesList = (from city in _context.Cities
                          where city.StateId == stateId
                          select city).ToList();
            return Json(citiesList);
        }
    }
}
