using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Controllers
{
    public class QueriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QueriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Queries
        public async Task<IActionResult> Index()
        {
            return View(await _context.Queries.ToListAsync());
        }

        // GET: Queries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (query == null)
            {
                return NotFound();
            }

            return View(query);
        }

        // GET: Queries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Queries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Renter,From,To,CarId")] Query query)
        {
            bool f1 = false;
            bool f2 = false;
            if (ModelState.IsValid)
            {
                foreach (var c in _context.Cars)
                {
                    if (query.CarId == c.Id)
                    {
                        f1 = true; break;
                    }
                }
                foreach (var u in _context.Users)
                {
                    if (query.Renter == u.UserName)
                    {
                        f2 = true;
                        break;
                    }
                }
                if (f1 && f2)
                {
                    foreach (var q in _context.Queries)
                    {
                        if (q.CarId == query.CarId && (
                            (q.From <= query.From && q.To >= query.From) ||
                            (q.From <= query.To && q.To >= query.To) ||
                            (q.From >= query.From && q.To <= query.To) ||
                            (q.From <= query.From && q.To >= query.To)))
                        {
                            //Console.WriteLine("bob");
                            return RedirectToAction(nameof(Create));
                        }
                    }
                    _context.Add(query);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Console.WriteLine("bobec");
                    return RedirectToAction(nameof(Create));
                }
            }
            return View(query);
        }
        [Authorize(Roles = "Admin")]
        // GET: Queries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries.FindAsync(id);
            if (query == null)
            {
                return NotFound();
            }
            return View(query);
        }
        [Authorize(Roles = "Admin")]
        // POST: Queries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Renter,From,To,CarId")] Query query)
        {
            if (id != query.Id)
            {
                return NotFound();
            }
            bool f1 = false;
            bool f2 =false;
            if (ModelState.IsValid)
            {

                try
                {
                    foreach (var c in _context.Cars)
                    {
                        if (query.CarId == c.Id)
                        {
                            f1 = true; break;
                        }
                    }
                    foreach (var u in _context.Users)
                    {
                        if (query.Renter == u.UserName)
                        {
                            f2 = true;
                            break;
                        }
                    }
                    if (f1 && f2)
                    {
                        foreach (var q in _context.Queries)
                        {
                            if (q.CarId == query.CarId && (
                                (q.From <= query.From && q.To >= query.From) ||
                                (q.From <= query.To && q.To >= query.To) ||
                                (q.From >= query.From && q.To <= query.To) ||
                                (q.From <= query.From && q.To >= query.To)))
                            {
                                return RedirectToAction(nameof(Create));
                            }
                        }
                        _context.Update(query);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return RedirectToAction(nameof(Create));
                    }
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!QueryExists(query.Id))
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
            return View(query);
        }
        [Authorize(Roles = "Admin")]
        // GET: Queries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = await _context.Queries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (query == null)
            {
                return NotFound();
            }

            return View(query);
        }
        [Authorize(Roles = "Admin")]
        // POST: Queries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var query = await _context.Queries.FindAsync(id);
            _context.Queries.Remove(query);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QueryExists(int id)
        {
            return _context.Queries.Any(e => e.Id == id);
        }
    }
}
