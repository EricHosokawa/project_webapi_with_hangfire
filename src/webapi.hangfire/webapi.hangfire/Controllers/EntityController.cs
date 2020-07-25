using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;
using webapi.hangfire.Repository;
using AutoMapper;
using webapi.hangfire.Entities;
using MongoDB.Driver.Core.Configuration;

namespace webapi.hangfire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntityController : ControllerBase
    {
        [HttpGet(Name = "GetAllEntities")]
        public ActionResult<List<EntityResponse>> GetAll([FromServices] IEntityApplication entityApplication)
        {
            try
            {
                var response = entityApplication.GetAll();

                if (response.Count() == 0)
                    return NotFound();

                return response;
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetEntity")]
        public ActionResult<EntityResponse> GetById([FromServices] IEntityApplication entityApplication, string id)
        {
            try
            {
                var response = entityApplication.GetById(id);

                if (response == null)
                    return NotFound();

                return response;
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }

        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<EntityResponse> Create([FromServices] IEntityApplication entityApplication, [FromBody] EntityRequest request)
        {
            try
            {
                var response = entityApplication.Create(request);

                return CreatedAtRoute("GetEntity", new { id = response.Id.ToString() }, response);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update([FromServices] IEntityApplication entityApplication, string id, [FromBody] EntityRequest request)
        {
            try
            {
                if (!entityApplication.Update(id, request))
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete([FromServices] IEntityApplication entityApplication, string id)
        {
            try
            {
                if (!entityApplication.Remove(id))
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }
    }
}