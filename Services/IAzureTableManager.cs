using nicold.visualstudio.to.lametric.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Services
{
    public interface IAzureTableManager
    {
        Task<bool> UpdateRow(LametricEntity row);
        Task<LametricEntity> GetRow(string email);
        Task<bool> UpdateUrl(string email, string url);
    }
}
