using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Entities;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;

namespace webapi.hangfire.Repository
{
    public class EntityContext : MongoDbBase, IEntityContext
    {
        private readonly IMongoCollection<Entity> collections;

        public EntityContext(IMongoDbSetting mongoDbSetting) : base(mongoDbSetting)
        {
            collections = database.GetCollection<Entity>(nameof(Entity));
        }

        public List<Entity> GetAll()
        {
            return collections.Find(c => true).ToList();
        }

        public Entity GetById(string id)
        {
            return collections.Find(c => c.Id == id).FirstOrDefault();
        }

        public Entity Create(Entity entity)
        {
            collections.InsertOne(entity);

            return entity;
        }

        public void Update(string id, Entity entityin)
        {
            collections.ReplaceOne(c => c.Id == id, entityin);
        }

        public void Remove(string id)
        {
            collections.DeleteOne(c => c.Id == id);
        }
    }
}
