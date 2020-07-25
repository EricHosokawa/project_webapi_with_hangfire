using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.hangfire.Interfaces
{
    public interface IJobsManager
    {
        List<string> EnqueueJobs();

        List<string> ScheduleJobs(int secdelay);

        bool RecurringJob(string jobname, int mininterval);

        bool RemoveRecorringJob(string jobname);

        bool TriggerReccuringJob(string jobname);
    }
}
