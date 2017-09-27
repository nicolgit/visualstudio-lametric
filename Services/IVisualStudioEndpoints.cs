using nicold.visualstudio.to.lametric.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Services
{
    public interface IVisualStudioEndpoints
    {
        string access_token { get; set; }

        Task<VisualStudioGetAccesCode> GetAccessCodeAsync(string code);
        Task<VisualStudioGetAccesCode> RefreshAccessCode(string refresh_token);
        Task<VisuaStudioUserProfile> GetUserProfileAsync();
        Task<GetChangeSetResponse> GetLatestChangesets(string url);
    }
}
