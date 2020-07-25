using System.Collections.Generic;
using webapi.hangfire.Entities;
using webapi.hangfire.Models;

namespace webapi.hangfire.Interfaces
{
    public interface IEntityContext
    {
        List<Entity> GetAll();
        Entity GetById(string id);
        Entity Create(Entity entity);
        void Remove(string id);
        void Update(string id, Entity entityin);
    }
}