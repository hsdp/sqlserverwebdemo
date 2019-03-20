using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SqlServerWebDemo.Models;
using SqlServerWebDemo.ViewModels.Home;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace SqlServerWebDemo.Controllers
{
    public class HomeController : Controller
    {
        private CloudFoundryServicesOptions CloudFoundryServices { get; set; }
        private CloudFoundryApplicationOptions CloudFoundryApplication { get; set; }

        private DemoContext _context;

        public HomeController(
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> serviceOptions,
            DemoContext context)
        {
            CloudFoundryServices = serviceOptions.Value;
            CloudFoundryApplication = appOptions.Value;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Authors.Include(a => a.Books).AsNoTracking().ToListAsync();
            return View(model);
        }

        public IActionResult CloudFoundry()
        {
            return View(new CloudFoundryViewModel(
                CloudFoundryApplication ?? new CloudFoundryApplicationOptions(),
                CloudFoundryServices ?? new CloudFoundryServicesOptions()));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}