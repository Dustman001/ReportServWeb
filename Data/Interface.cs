using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public interface IReport
    {
        public DateTime Time { get; set; }

        public string Station { get; set; }

        public DateTime Ctime { get; set; }
    }

    public interface IStatus
    {
        string Check(string id);

        void Habilitar(string id);

        void Deshabilitar(string id);
    }
}
