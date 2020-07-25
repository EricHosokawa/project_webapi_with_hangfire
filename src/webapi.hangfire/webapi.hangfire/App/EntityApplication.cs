using AutoMapper;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Entities;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;

namespace webapi.hangfire.App
{
    public class EntityApplication : IEntityApplication
    {
        private readonly IMapper mapper;
        private readonly IEntityContext entityContext;

        public EntityApplication(IMapper mapper, IEntityContext entityContext)
        {
            this.mapper = mapper;
            this.entityContext = entityContext;
        }

        public EntityResponse Create(EntityRequest request)
        {
            var entity = mapper.Map<Entity>(request);

            entity.Id = ObjectId.GenerateNewId().ToString();
            entity.Status = "Inserido";

            entity = entityContext.Create(entity);

            var response = mapper.Map<EntityResponse>(entity);

            return response;
        }

        public List<EntityResponse> GetAll()
        {
            var entities = entityContext.GetAll();

            var response = mapper.Map<List<EntityResponse>>(entities);

            return response;
        }

        public EntityResponse GetById(string id)
        {
            var entity = entityContext.GetById(id);

            var response = mapper.Map<EntityResponse>(entity);

            return response;
        }

        public bool Remove(string id)
        {
            var entity = entityContext.GetById(id);

            if (entity == null)
                return false;

            entityContext.Remove(entity.Id);

            return true;
        }

        public bool Update(string id, EntityRequest request)
        {
            var entity = entityContext.GetById(id);

            if (entity == null)
                return false;

            entity = mapper.Map<Entity>(request);

            var statusOld = entity.Status;           

            entity.Id = id;
            entity.Status = "Alterado";
            entity.StatusOld = statusOld;

            entityContext.Update(id, entity);

            return true;
        }
    }
}
