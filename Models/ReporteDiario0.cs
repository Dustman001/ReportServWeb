using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class ReporteDiario0 : IReport
    {
        public DateTime Time { get; set; }

        public string Station { get; set; }

        public string Estado { get; set; }

        public DateTime Ctime { get; set; }

    }
}
