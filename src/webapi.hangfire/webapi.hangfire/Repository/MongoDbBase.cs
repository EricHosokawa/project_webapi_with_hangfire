using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;

namespace webapi.hangfire.Repository
{
    public class MongoDbBase
    {
        public readonly MongoClient client;

        public readonly IMongoDatabase database;

        public MongoDbBase(IMongoDbSetting mongoDbSetting)
        {
            client = new MongoClient(mongoDbSetting.ConnectionString);
            database = client.GetDatabase(mongoDbSetting.DatabaseName);
        }
    }
}
