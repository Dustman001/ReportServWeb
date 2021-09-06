using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportServiceWeb02.DbModels;
using ReportServiceWeb02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class BackTask : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackTask> _logger;
        private readonly StatusTask _statusTask;

        private Timer timerRepD, timerRepE, timerRepR, timerMailer;
        private int numberRepD, numberRepR;

        private List<Mailer> Mailers = new List<Mailer>();
        public string RepD { get; set; }

        public string RepE { get; set; }

        public string RepR { get; set; }

        public BackTask(IServiceScopeFactory scopeFactory, ILogger<BackTask> logger, StatusTask statusTask)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _statusTask = statusTask;
        }

        public void Dispose()
        {
            timerRepD?.Dispose();
            timerRepE?.Dispose();
            timerRepR?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timerRepD = new Timer(o =>
            {
                var status = _statusTask.Reportes.Where(w => w.Ids == "RepD").FirstOrDefault().Status;

                if (status == "Habilitado")
                {
                    Interlocked.Increment(ref numberRepD);
                    _logger.LogInformation($"Printing the Reporte Diario worker number {numberRepD}");
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));

            timerRepE = new Timer(o =>
            {
                if (timerRepE.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan) == true)
                {
                    var status = _statusTask.Reportes.Where(w => w.Ids == "RepE").FirstOrDefault().Status;

                    if (status == "Habilitado")
                    {
                        //Reporte_D();
                    }
                }

                timerRepE.Change(TimeSpan.Zero, TimeSpan.FromSeconds(45));
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

            timerRepR = new Timer(o =>
            {
                var status = _statusTask.Reportes.Where(w => w.Ids == "RepR").FirstOrDefault().Status;

                if (status == "Habilitado")
                {
                    Interlocked.Increment(ref numberRepR);
                    _logger.LogInformation($"Printing the Reporte RTU worker number {numberRepR}");
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(10));

            timerMailer = new Timer(o =>
            {
                var status = Mailers.Count;

                if (status > 0)
                {
                    try
                    {
                        Mailers[0].SendExchange();

                    }catch(Exception x)
                    {

                    }finally
                    {
                        Mailers.RemoveAt(0);
                    }
                }
            },
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }        

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}
