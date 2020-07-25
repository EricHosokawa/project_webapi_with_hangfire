using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Models;

namespace webapi.hangfire.Interfaces
{
    public interface IEntityApplication
    {
        List<EntityResponse> GetAll();
        EntityResponse GetById(string id);
        EntityResponse Create(EntityRequest request);
        bool Remove(string id);
        bool Update(string id, EntityRequest request);
    }
}
