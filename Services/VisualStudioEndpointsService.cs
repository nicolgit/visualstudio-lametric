using Newtonsoft.Json;
using nicold.visualstudio.to.lametric.Utilities;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace nicold.visualstudio.to.lametric.Services
{
    public class VisualStudioEndpointsService : IVisualStudioEndpoints
    {
        private static readonly HttpClient client = new HttpClient();
        private ISettings _settings;

        public VisualStudioEndpointsService(ISettings settings)
        {
            _settings = settings;
        }

        public string access_token { get; set; }

        public async Task<VisualStudioGetAccesCode> GetAccessCodeAsync (string code)
        {
            var appSecret = _settings.AppSecret;
            var callback = _settings.AppCallback;

            string body = String.Format("client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer&assertion={1}&redirect_uri={2}",
                HttpUtility.UrlEncode(appSecret),
                HttpUtility.UrlEncode(code),
                callback
                );

            return await _getTokenAndRefresh(body);
        }

        public async Task<VisualStudioGetAccesCode> RefreshAccessCode (string refresh_token)
        {
            var appSecret = _settings.AppSecret;
            var callback = _settings.AppCallback;

            string body = String.Format(
                "client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion={0}&grant_type=refresh_token&assertion={1}&redirect_uri={2}",
                HttpUtility.UrlEncode(appSecret),
                HttpUtility.UrlEncode(refresh_token),
                callback
                );

            return await _getTokenAndRefresh(body);
        }

        private async Task<VisualStudioGetAccesCode> _getTokenAndRefresh(string body)
        {
            var content = new StringContent(body);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            VisualStudioEndpointsService.client.DefaultRequestHeaders.Authorization = null;

            var response = await VisualStudioEndpointsService.client.PostAsync("https://app.vssps.visualstudio.com/oauth2/token", content);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<VisualStudioGetAccesCode>(json);
            }
            catch (Exception e)
            {
                return new VisualStudioGetAccesCode() { Error = "99", ErrorDescription = e.Message };
            }
        }

        public async Task<VisuaStudioUserProfile> GetUserProfileAsync()
        {    
            string getProfileUrl = "https://app.vssps.visualstudio.com/_apis/profile/profiles/me?api-version=1.0";            
            VisualStudioEndpointsService.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            
            var response = await VisualStudioEndpointsService.client.GetAsync(getProfileUrl);
            var json = await response.Content.ReadAsStringAsync();
            
            try
            {
                return JsonConvert.DeserializeObject<VisuaStudioUserProfile>(json);
            }
            catch (Exception e)
            {
                return new VisuaStudioUserProfile() { Error = "99", ErrorDescription = e.Message };
            }
        }

        public async Task<GetChangeSetResponse> GetLatestChangesets (string url)
        {
            string getChangesetsUrl = url + "/_apis/tfvc/changesets?api-version=1.0";
            VisualStudioEndpointsService.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

            var response = await VisualStudioEndpointsService.client.GetAsync(getChangesetsUrl);
            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<GetChangeSetResponse>(json);
            }
            catch (Exception e)
            {
                return new GetChangeSetResponse() { Error = "99", ErrorDescription = e.Message };
            }
        }
    }
}
