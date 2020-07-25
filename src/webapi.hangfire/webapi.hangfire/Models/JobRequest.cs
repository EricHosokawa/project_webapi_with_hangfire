using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.hangfire.Models
{
    public class JobRequest
    {
        public string JobName { get; set; }

        public int MunitesInterval { get; set; }
    }
}
