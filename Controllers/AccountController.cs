using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Web;
using System.IO;
using System.Net.Http;
using nicold.visualstudio.to.lametric.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using nicold.visualstudio.to.lametric.Utilities;

namespace nicold_visualstudio_to_lametric.Controllers
{

    public class AccountController : Controller
    {
        private ISettings _settings { get; set; }
        private IVisualStudioEndpoints _visualStudioEndpoints { get; set; }
        private IAzureTableManager _azureTableManager { get; set; }

        public AccountController(ISettings settings, IVisualStudioEndpoints visualstudio, IAzureTableManager azuretable)
        {
            _settings = settings;
            _visualStudioEndpoints = visualstudio;
            _azureTableManager = azuretable;
        }

        [HttpGet]
        public async Task<IActionResult> Oauth2token([FromQuery]string code, [FromQuery]string state)
        {
            if (!string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(state))
            {
                var token = await _visualStudioEndpoints.GetAccessCodeAsync(code);
                if (!string.IsNullOrEmpty(token.Error))
                    {
                    return this.RedirectToAction("Index", "Home", token);
                    }
                _visualStudioEndpoints.access_token = token.access_token;

                var profile = await _visualStudioEndpoints.GetUserProfileAsync();
                if (!string.IsNullOrEmpty(profile.Error))
                {
                    return this.RedirectToAction("Index", "Home", profile);
                }

                var lametricEntity = await _azureTableManager.GetRow(profile.emailAddress);
                if (lametricEntity == null)
                {
                    lametricEntity = new LametricEntity(profile.emailAddress);
                    lametricEntity.Email = profile.emailAddress;
                    lametricEntity.VSO_Url = "";
                    lametricEntity.LastChangeset = 0;
                }

                lametricEntity.AccessToken = token.access_token;
                lametricEntity.RefreshToken = token.refresh_token;

                await _azureTableManager.UpdateRow(lametricEntity);

                Response.Cookies.Append("lametric-auth-accesstoken", token.access_token);
                Response.Cookies.Append("lametric-email", profile.emailAddress);
                Response.Cookies.Append("lametric-name", profile.displayName);
                Response.Cookies.Append("lametric-vso-url", lametricEntity.VSO_Url);

                return this.RedirectToAction("Index", "Home");
            }
            
            return this.RedirectToAction("Index","Home");
        }
    }
}