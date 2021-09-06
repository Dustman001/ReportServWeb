using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class ReporteEdac : IDisposable
    {
        private readonly ILogger<ReporteEdac> _logger;

        public ReporteEdac(ILogger<ReporteEdac> logger)
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
