using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Services
{
    public interface ISettings
    {
        string AppId { get; }
        string AppSecret { get; }
        string AppCallback { get; }
        string BlobStorageConnectionString { get; }
    }
}
