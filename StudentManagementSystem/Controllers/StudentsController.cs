using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Data;
using StudentManagementSystem.Models;
using System.Text.Json;
namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StudentsController(ILogger<StudentsController> logger , ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchString)
        {
            var students = from s in _context.Students
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString) || s.Email.Contains(searchString));
            }

            return View(await students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                // If just deleted, show from TempData
                if (TempData.ContainsKey("DeletedStudentJson"))
                {
                    var json = TempData["DeletedStudentJson"] as string;
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        var deletedStudent = JsonSerializer.Deserialize<Student>(json);
                        ViewBag.Deleted = true;
                        ViewBag.StatusMessage = TempData["StatusMessage"] as string;
                        return View("StudentDetails", deletedStudent);
                    }
                }
                return NotFound();
            }

            ViewBag.StatusMessage = TempData["StatusMessage"] as string;
            return View("StudentDetails", student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Name,Age,Grade,Email,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                TempData["StatusMessage"] = "Student created successfully.";
                return RedirectToAction(nameof(Details), new { id = student.StudentId });
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,Name,Age,Grade,Email,EnrollmentDate")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["StatusMessage"] = "Student updated successfully.";
                return RedirectToAction(nameof(Details), new { id = student.StudentId });
            }
            return View(student);
        }
    
        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                // capture details for post-delete details view
                var json = JsonSerializer.Serialize(student);
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["DeletedStudentJson"] = json;
                TempData["StatusMessage"] = $"Student (ID: {id}) deleted successfully.";
                return RedirectToAction(nameof(Details), new { id });
            }
            TempData["StatusMessage"] = $"Student (ID: {id}) not found.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Report()
        {
            var reportData = _context.Students
                .GroupBy(s => s.Grade)
                .Select(g => new { Grade = g.Key, Count = g.Count() })
                .ToList();

            // Use existing view under Views/Home/Report.cshtml
            return View("~/Views/Home/Report.cshtml", reportData);
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
