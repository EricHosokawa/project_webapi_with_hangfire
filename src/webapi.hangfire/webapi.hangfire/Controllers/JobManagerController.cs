using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;

namespace webapi.hangfire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobManagerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<string>> EnfileirarJobs([FromServices] IJobsManager jobsManager)
        {
            try
            {
                var jobs = jobsManager.EnqueueJobs();

                if (jobs == null || jobs.Count() == 0)
                    return NotFound();

                return jobs;
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpGet("{delaySegundos}")]
        public ActionResult<List<string>> AgendarJobs([FromServices] IJobsManager jobsManager, int delaySegundos)
        {
            try
            {
                var jobs = jobsManager.ScheduleJobs(delaySegundos);

                if (jobs == null || jobs.Count() == 0)
                    return NotFound();

                return jobs;
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult AgendaJobRecorrente([FromServices] IJobsManager jobsManager, [FromBody] JobRequest jobRequest)
        {
            try
            {
                var agendado = jobsManager.RecurringJob(jobRequest.JobName, jobRequest.MunitesInterval);

                if (!agendado)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpPut("{jobname}")]
        public ActionResult DisparaJob([FromServices] IJobsManager jobsManager, string jobname)
        {
            try
            {
                var executou = jobsManager.TriggerReccuringJob(jobname);

                if (executou)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        [HttpDelete("{jobname}")]
        public ActionResult RemoveJob([FromServices] IJobsManager jobsManager, string jobname)
        {
            try
            {
                var executou = jobsManager.RemoveRecorringJob(jobname);

                if (executou)
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