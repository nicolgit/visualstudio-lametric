using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicold.visualstudio.to.lametric.Utilities
{
    public class LametricEntity: TableEntity
    {
        public LametricEntity(string email)
        {
            this.PartitionKey = email;
            this.RowKey = email;
        }

        public LametricEntity() { }

        public string Email { get; set; }
        public string VSO_Url { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public int LastChangeset { get; set; }
    }
}
