using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ILogger<TeachersController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TeachersController(ILogger<TeachersController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Teachers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t =>
                    (t.FirstName + " " + t.LastName).Contains(search) ||
                    t.Email.Contains(search) ||
                    t.Department.Contains(search));
            }

            List<Teacher> teachers = await query.ToListAsync();
            ViewBag.Search = search;
            return View(teachers);
        }


        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherID == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,PhoneNumber,Department,HireDate,Status")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = teacher.TeacherID });
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherID,FirstName,LastName,Email,PhoneNumber,Department,HireDate,Status")] Teacher teacher)
        {
            if (id != teacher.TeacherID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Details), new { id = teacher.TeacherID });
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.TeacherID == id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.TeacherID == id);
        }
    }
}
