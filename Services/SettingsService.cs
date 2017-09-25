using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace nicold.visualstudio.to.lametric.Services
{
    public class SettingsService : ISettings
    {
        private IConfigurationRoot _configuration;

        public SettingsService(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }
        public string AppId => _configuration["visualstudio-app-id"];
        public string AppSecret => _configuration["visualstudio-app-secret"];
        public string AppCallback => _configuration["thisapp-callback-uri"];
        public string BlobStorageConnectionString => _configuration["blobstorage-connectionstring"];
    }
}
