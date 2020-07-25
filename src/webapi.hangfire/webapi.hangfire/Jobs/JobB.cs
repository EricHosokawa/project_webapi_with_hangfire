using DnsClient.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;

namespace webapi.hangfire.Jobs
{
    public class JobB : IJobB
    {
        private readonly ILogger<JobB> logger;
        private readonly IEntityApplication entityApplication;

        public JobB(ILogger<JobB> logger, IEntityApplication entityApplication)
        {
            this.logger = logger;
            this.entityApplication = entityApplication;
        }

        public void Executar()
        {
            logger.LogInformation($"{DateTime.Now} JobB - Executando o JobB");

            var responses = entityApplication.GetAll();

            responses.ForEach(r =>
            {
                var result = entityApplication.Update(
                r.Id,
                new EntityRequest
                {
                    Name = r.Name,
                    Date = DateTime.Now
                });

                if (!result)
                    logger.LogWarning($"{DateTime.Now} JobB - Erro ao atualizar entidade de id {r.Id}.");

                logger.LogInformation($"{DateTime.Now} JobB - Entidade de id {r.Id} atualizada com sucesso.");
            });
        }
    }
}
