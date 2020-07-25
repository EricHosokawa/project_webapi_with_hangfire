using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.hangfire.Models
{
    public class EntityResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string StatusOld { get; set; }
    }
}
