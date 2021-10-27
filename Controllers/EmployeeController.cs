using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesManager.Data;
using EmployeesManager.Models;

namespace EmployeesManager.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            GetManagers();

            return View();
        }

        private void GetManagers()
        {
            var list = _context.Employee.Where(x => x.IsManager || x.IsCEO)
            .ToList()
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            });



            ViewData["Manager"] = list;
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Salary,IsCEO,IsManager,ManagerId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                GetManagers();

                if (!string.IsNullOrWhiteSpace(Request.Form["Manager"].ToString()))
                {
                    employee.ManagerId = Int32.Parse(Request.Form["Manager"].ToString());
                }

                if (employee.IsManager)
                {
                    double y = 1.725;
                    employee.Salary = Math.Pow(employee.Salary, y);
                }

                if (employee.IsCEO)
                {
                    double y = 2.725;
                    employee.Salary = Math.Pow(employee.Salary, y);

                    var employeeisCEO = _context.Employee.Where(s => s.IsCEO).Count();

                    if (employeeisCEO == 0)
                    {
                        _context.Add(employee);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return Conflict("There's already a CEO.");
                    }
                }
                else
                {
                    double y = 1.125;
                    employee.Salary = Math.Pow(employee.Salary, y);

                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Salary,IsCEO,IsManager,ManagerId")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (employee.IsManager)
                    {
                        double y = 1.725;
                        employee.Salary = Math.Pow(employee.Salary, y);

                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }

                    if (employee.IsCEO)
                    {
                        var employeeisCEO = _context.Employee.Where(s => s.IsCEO).Count();

                        if (employeeisCEO == 0)
                        {
                            double y = 2.725;
                            employee.Salary = Math.Pow(employee.Salary, y);

                            _context.Update(employee);
                            await _context.SaveChangesAsync();
                        }

                        else
                        {
                            return Conflict("There's already a CEO.");
                        }
                    }
                    else
                    {
                        double y = 1.125;
                        employee.Salary = Math.Pow(employee.Salary, y);

                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
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
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }



}
