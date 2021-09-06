using ReportServiceWeb02.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Models
{
    public class ReporteDiario1 : IReport
    {
        public DateTime Time { get; set; }

        public string Message { get; set; }

        public string Station { get; set; }

        public string Operator { get; set; }

        public DateTime Ctime { get; set; }
    }
}
