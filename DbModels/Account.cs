using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReportServiceWeb02.DbModels
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string usr { get; set; }

        public string pwd { get; set; }
    }
}
