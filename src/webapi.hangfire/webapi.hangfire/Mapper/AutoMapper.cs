using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Entities;
using webapi.hangfire.Models;

namespace webapi.hangfire.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<EntityRequest, Entity>();
            CreateMap<Entity, EntityResponse>();
        }
    }
}
