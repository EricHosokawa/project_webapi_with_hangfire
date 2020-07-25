using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Entities;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Jobs;

namespace webapi.hangfire.App
{
    public class JobManager : IJobsManager
    {
        private readonly ILogger<JobManager> logger;
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly IRecurringJobManager recurringJobManager;

        public JobManager(ILogger<JobManager> logger, 
            IBackgroundJobClient backgroundJobClient, 
            IRecurringJobManager recurringJobManager)
        {
            this.logger = logger;
            this.backgroundJobClient = backgroundJobClient;
            this.recurringJobManager = recurringJobManager;
        }

        public List<string> EnqueueJobs()
        {
            logger.LogInformation($"{DateTime.Now} JobManager - Enfileirando os jobs para execução.");

            var jobids = new List<string>();

            var jobid = backgroundJobClient.Enqueue<JobA>(j => j.Executar());
            logger.LogInformation($"{DateTime.Now} JobManager - Job de id {jobid} na fila para execução.");
            jobids.Add(jobid);

            jobid = backgroundJobClient.Enqueue<JobB>(j => j.Executar());
            logger.LogInformation($"{DateTime.Now} JobManager - Job de id {jobid} na fila para execução.");
            jobids.Add(jobid);

            return jobids;
        }

        public List<string> ScheduleJobs(int secdelay)
        {
            logger.LogInformation($"{DateTime.Now} JobManager - Agendando os jobs para execução.");

            var jobids = new List<string>();

            var jobid = backgroundJobClient.Schedule<JobA>(j => j.Executar(), TimeSpan.FromSeconds(secdelay));
            logger.LogInformation($"{DateTime.Now} JobManager - Job de id {jobid} agendado para execução em {secdelay} segundos.");
            jobids.Add(jobid);

            jobid = backgroundJobClient.Schedule<JobB>(j => j.Executar(), TimeSpan.FromSeconds(secdelay));
            logger.LogInformation($"{DateTime.Now} JobManager - Job de id {jobid} agendado para execução em {secdelay} segundos.");
            jobids.Add(jobid);

            return jobids;
        }

        public bool RecurringJob(string jobname, int mininterval)
        {
            logger.LogInformation($"{DateTime.Now} JobManager - Agendando o job {jobname} para execução recorrente de {mininterval} minutos.");

            var cronminutes = $"*/{mininterval} * * * *";

            switch (jobname)
            {
                case "JOB-A":
                    recurringJobManager.AddOrUpdate<JobA>("JOB-A", j => j.Executar(), cronminutes);
                    logger.LogInformation($"{DateTime.Now} JobManager - Job JOB-A agendado para recorrência.");
                    return true;
                case "JOB-B":
                    recurringJobManager.AddOrUpdate<JobB>("JOB-B", j => j.Executar(), cronminutes);
                    logger.LogInformation($"{DateTime.Now} JobManager - Job JOB-B agendado para recorrência.");
                    return true;
                default:
                    logger.LogWarning($"{DateTime.Now} JobManager - Nome do job informado é inválido.");
                    return false;
            }
        }

        public bool RemoveRecorringJob(string jobname)
        {
            logger.LogInformation($"{DateTime.Now} JobManager - Removendo job {jobname} recorrente.");

            recurringJobManager.RemoveIfExists(jobname);

            return true;
        }

        public bool TriggerReccuringJob(string jobname)
        {
            logger.LogInformation($"{DateTime.Now} JobManager - Forçando o disparado do job {jobname}.");

            recurringJobManager.Trigger(jobname);

            return true;
        }
    }
}
