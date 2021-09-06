using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ReportServiceWeb02.Data;
using ReportServiceWeb02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.DbModels
{
    public class MyDbContext : DbContext
    {
        private IConfiguration Configuration { get; }

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }

        public DbSet<Areports> Areports { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Correos> Correos { get; set; }

        private DbSet<ReporteDiario0> reporteDiario0s { get; set; }

        private DbSet<ReporteDiario1> reporteDiario1s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReporteDiario0>().HasNoKey();
            modelBuilder.Entity<ReporteDiario1>().HasNoKey();
        }
        internal List<IReport> GetReporteRDt0(DateTime todays)
        {
            //$"EXEC [dbo].[readdailyr] @usedate = '{todays.AddDays(1).ToString($"yyyy/MM/dd")}', @type = 1;"
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter{ ParameterName = "usedate", Value = todays.AddDays(1).ToString($"yyyy/MM/dd")},
                new SqlParameter{ ParameterName = "type", Value = 0}
            };

            List<IReport> data = new List<IReport>();

            do
            {
                try
                {
                    data = reporteDiario0s.FromSqlRaw<ReporteDiario0>("EXEC [dbo].[readdailyr] @usedate, @type;",
                        parameters.ToArray()).ToList<IReport>();

                    break;
                }catch(Exception sx)
                {
                    Thread.Sleep(500);
                }

            } while (true);

            return data;
        }

        internal List<IReport> GetReporteRDt1(DateTime todays)
        {
            //$"EXEC [dbo].[readdailyr] @usedate = '{todays.AddDays(1).ToString($"yyyy/MM/dd")}', @type = 1;"
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter{ ParameterName = "usedate", Value = todays.AddDays(1).ToString($"yyyy/MM/dd")},
                new SqlParameter{ ParameterName = "type", Value = 1}
            };
            
            List<IReport> data = new List<IReport>();
            do
            {

                try
                {
                    data = reporteDiario1s.FromSqlRaw<ReporteDiario1>("EXEC [dbo].[readdailyr] @usedate, @type;",
                        parameters.ToArray()).ToList<IReport>();

                    break;
                }
#pragma warning disable IDE0059 // Asignación innecesaria de un valor
                catch (Exception xs)
#pragma warning restore IDE0059 // Asignación innecesaria de un valor
                {
                    Thread.Sleep(500);
                }

            } while (true);

            return data;
        }

        internal List<Correos> GetMailRDt()
        {
            //"select * from [Report].[dbo].[Correos] where Mode like 'ALL' or Mode like 'Rdiario'"
            var Correos = this.Correos.Where(w => w.Mode.Contains("ALL") == true || w.Mode.Contains("Rdiario")).ToList();

            return Correos;
        }
    }
}
