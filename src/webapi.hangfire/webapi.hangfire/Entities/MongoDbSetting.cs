using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Interfaces;

namespace webapi.hangfire.Entities
{
    public class MongoDbSetting : IMongoDbSetting
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
