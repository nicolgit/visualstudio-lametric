using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Utilities
{
    /// <summary>
    /// Lametric JSON compatible with lametric cloud
    /// </summary>
    public class LametricMessage
    {
        public LametricMessage()
        {
            frames = new List<Frame>();
        }

        public List<Frame> frames { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Frame
    {
        public string text { get; set; }
        public string icon { get; set; }
    }
}
