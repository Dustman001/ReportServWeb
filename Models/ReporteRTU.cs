using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class ReporteRTU : IDisposable
    {
        private readonly ILogger<ReporteRTU> _logger;

        public ReporteRTU(ILogger<ReporteRTU> logger)
        {
            this._logger = logger;
        }

        public Task CheckReport()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}