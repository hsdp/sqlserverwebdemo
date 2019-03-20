using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SqlServerWebDemo.Models;

namespace SqlServerWebDemo.Controllers
{
    public class AuthorController : Controller
    {
        private DemoContext _context;

        public AuthorController(DemoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FirstName, LastName")] Author author)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}