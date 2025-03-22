using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService; // Injecting email service

        public ToDoItemsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToDoItems.ToListAsync());
        }

        // GET: ToDoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var toDoItem = await _context.ToDoItems.FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null) return NotFound();

            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,IsCompleted,CreatedAt,Priority,DueDate,Email")] ToDoItem toDoItem)
        {
            if (!ModelState.IsValid) return View(toDoItem);

            var existingTask = await _context.ToDoItems.FirstOrDefaultAsync(t => t.Title == toDoItem.Title);
            if (existingTask != null)
            {
                ModelState.AddModelError("Title", "Task already exists.");
                return View(toDoItem);
            }

            // Save to database
            _context.Add(toDoItem);
            await _context.SaveChangesAsync();

            // Send email after saving the task
            try
            {
                await _emailService.SendEmailAsync(
                    toDoItem.Email,
                    "New Task Created",
                    $"Your task '{toDoItem.Title}' is created and is due on {toDoItem.DueDate:dd/MM/yyyy}."
                );
                Console.WriteLine($"✅ Email sent to {toDoItem.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Email error: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
        public class TestController : Controller
        {
            private readonly IEmailService _emailService;
            private readonly ILogger<TestController> _logger;

            public TestController(IEmailService emailService, ILogger<TestController> logger)
            {
                _emailService = emailService;
                _logger = logger;
            }

            public async Task<IActionResult> SendTestEmail()
            {
                try
                {
                    await _emailService.SendEmailAsync("recipient@example.com", "Test Email", "This is a test email.");
                    return Content("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Email failed: {ex.Message}");
                    return Content($"Email failed: {ex.Message}");
                }
            }
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null) return NotFound();

            toDoItem.IsCompleted = !toDoItem.IsCompleted; // Toggle completion status
            _context.Update(toDoItem);
            await _context.SaveChangesAsync();

            // Send email if task is marked as completed
            if (toDoItem.IsCompleted)
            {
                var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
                await emailService.SendEmailAsync(
                    toDoItem.Email,
                    "Task Completed",
                    $"Your task '{toDoItem.Title}' has been marked as completed."
                );
            }

            return RedirectToAction(nameof(Index)); // Refresh the page
        }


        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null) return NotFound();

            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,IsCompleted,CreatedAt,Priority,DueDate,Email")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id) return NotFound();

            if (!ModelState.IsValid) return View(toDoItem);

            try
            {
                _context.Update(toDoItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ToDoItems.Any(e => e.Id == toDoItem.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var toDoItem = await _context.ToDoItems.FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null) return NotFound();

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem != null)
            {
                _context.ToDoItems.Remove(toDoItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
