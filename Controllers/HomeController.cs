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
        IAzureTableManager _azureTableManager;

        public HomeController(IVisualStudioEndpoints visualstudio, IAzureTableManager azureTableManager)
        {
            _visualstudio = visualstudio;
            _azureTableManager = azureTableManager;
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
        async public Task<string> Test([FromBody]UrlParameter input)
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

        [HttpPost]
        async public Task<string> UpdateURL([FromBody]UrlParameter input)
        {
            _visualstudio.access_token = input.auth_token;
            var profile = await _visualstudio.GetUserProfileAsync();

            if (string.IsNullOrEmpty(profile.Error))
            {
                return await _azureTableManager.UpdateUrl(profile.emailAddress, input.url) ? "OK": "Error";
            }
            else
            {
                return $"Error {profile.Error} - {profile.ErrorDescription}";
            }
        }
    }
}
