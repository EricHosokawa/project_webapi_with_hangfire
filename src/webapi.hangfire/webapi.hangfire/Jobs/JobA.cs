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
    public class JobA : IJobA
    {
        private readonly ILogger<JobA> logger;
        private readonly IEntityApplication entityApplication;

        public JobA(ILogger<JobA> logger, IEntityApplication entityApplication)
        {
            this.logger = logger;
            this.entityApplication = entityApplication;
        }

        public void Executar()
        {
            logger.LogInformation($"{DateTime.Now} JobA - Executando o JobA");

            var response = entityApplication.Create(
                new EntityRequest
                {
                    Name = "Eric",
                    Date = DateTime.Now
                });

            if (response == null)
                logger.LogWarning($"{DateTime.Now} JobA - Erro ao gerar entidade.");

            logger.LogInformation($"{DateTime.Now} JobA - Gerado a nova entidade de id {response.Id}");
        }
    }
}
