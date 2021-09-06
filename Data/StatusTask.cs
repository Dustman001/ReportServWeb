using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class StatusTask : IStatus
    {
        public List<Reportes> Reportes;

        public StatusTask()
        {
            Reportes = new List<Reportes>();

            Reportes.Add(new Data.Reportes { Name = "Reporte Diario", Ids = "RepD", Status = "Habilitado" });
            Reportes.Add(new Data.Reportes { Name = "Reporte EDAC", Ids = "RepE", Status = "Habilitado" });
            Reportes.Add(new Data.Reportes { Name = "Reporte RTU", Ids = "RepR", Status = "Habilitado" });
        }

        public string Check(string id)
        {
            var rep = Reportes.Where(w => w.Ids == id).FirstOrDefault();

            return rep.Status;
        }

        public void Deshabilitar(string id)
        {
            var rep = Reportes.Where(w => w.Ids == id).FirstOrDefault();

            rep.Status = "Deshabilitado";
        }

        public void Habilitar(string id)
        {
            var rep = Reportes.Where(w => w.Ids == id).FirstOrDefault();

            rep.Status = "Habilitado";
        }
    }
}
