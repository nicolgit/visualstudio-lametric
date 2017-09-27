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

            if (string.IsNullOrEmpty(result.Error) && string.IsNullOrEmpty(result.message))
            {
                return $"OK! query returned {result.count} items!";
            }
            else
            {
                return $"Error {result.Error} - {result.ErrorDescription} - {result.message}";
            }
        }

        [HttpPost]
        async public Task<string> UpdateURL([FromBody]UrlParameter input)
        {
            _visualstudio.access_token = input.auth_token;
            var profile = await _visualstudio.GetUserProfileAsync();

            if (string.IsNullOrEmpty(profile.Error) && string.IsNullOrEmpty(profile.message))
            {
                return await _azureTableManager.UpdateUrl(profile.emailAddress, input.url) ? "OK": "Error";
            }
            else
            {
                return $"Error {profile.Error} - {profile.ErrorDescription}";
            }
        }

        [HttpGet]
        async public Task<LametricMessage> Lametric(string id)
        {
            LametricMessage result = new LametricMessage();

            try
            {
                var infos = await _azureTableManager.GetRow(id);

                var token = await _visualstudio.RefreshAccessCode(infos.RefreshToken);

                infos.AccessToken = token.access_token;
                infos.RefreshToken = token.refresh_token;
                await _azureTableManager.UpdateRow(infos);

                _visualstudio.access_token = infos.AccessToken;
                var changeset = await _visualstudio.GetLatestChangesets(infos.VSO_Url);

                if (changeset.count > 0)
                {
                    string user = changeset.value.FirstOrDefault().checkedInBy.displayName;
                    string time = "";
                    var diff = (DateTime.Now - changeset.value.FirstOrDefault().createdDate);

                    if (diff.TotalDays >= 1)
                    {
                        time = $"{(int)diff.TotalDays} days";
                    }
                    else if (diff.TotalHours >= 1)
                    {
                        time = $"{(int)diff.TotalHours} hours";
                    }
                    else if (diff.TotalMinutes >= 1)
                    {
                        time = $"{(int)diff.TotalSeconds} minutes";
                    }

                    result = new LametricMessage();
                    result.frames.Add(new Frame()
                    {
                        icon = "1",
                        text = $"{user} checked in {time} ago"
                    });
                }
                else
                {
                    result = new LametricMessage();
                    result.frames.Add(new Frame()
                    {
                        icon = "1",
                        text = $"no changesets yet!"
                    });

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError($"{e.Source} - {e.Message}");

                result = new LametricMessage();
                result.ErrorMessage = "Internal Server Error";              
            }

            return result;
        }
    }
}
