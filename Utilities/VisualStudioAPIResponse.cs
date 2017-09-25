using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Utilities
{
    public class VisualStudio_Response
    {
        public string Error { get; set; }
        public string ErrorDescription { get; set; }
    }

    public class VisualStudioGetAccesCode : VisualStudio_Response
    { 
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
    }

    public class VisuaStudioUserProfile: VisualStudio_Response
    {
        public string emailAddress { get; set; }
        public string displayName { get; set; }
        public string coreRevision { get; set; }
        public string timeStamp { get; set; }
        public string id { get; set; }
        public string revision { get; set; }
    }
}
