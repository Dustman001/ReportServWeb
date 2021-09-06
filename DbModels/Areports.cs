using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServiceWeb02.DbModels
{
    public class Areports
    {
        [Key]
        public int ID { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Message { get; set; }

        public DateTime CapturTime { get; set; }

        public string Station { get; set; }

        public string Point { get; set; }

        public string Operator { get; set; }

        public string State { get; set; }

        public string Sname { get; set; }

        public string Priority { get; set; }
    }
}
