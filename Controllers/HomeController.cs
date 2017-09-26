using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using nicold.visualstudio.to.lametric.Services;
using nicold.visualstudio.to.lametric.Utilities;

namespace nicold_visualstudio_to_lametric.Controllers
{ 
    public class HomeController : Controller
    {
        IVisualStudioEndpoints _visualstudio;
        public HomeController(IVisualStudioEndpoints visualstudio)
        {
            _visualstudio = visualstudio;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }

        [HttpPost]
        async public Task<string> Test([FromBody]TestAPIParameters input)
        {
            _visualstudio.access_token = input.auth_token;
            var result = await _visualstudio.GetLatestChangesets(input.url);

            if (string.IsNullOrEmpty(result.Error))
            {
                return $"OK! query returned {result.count} items!";
            }
            else
            {
                return $"Error {result.Error} - {result.ErrorDescription}";
            }
        }
    }
}
