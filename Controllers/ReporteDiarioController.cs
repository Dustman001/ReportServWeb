using Microsoft.AspNetCore.Mvc;
using ReportServiceWeb02.Data;
using ReportServiceWeb02.DbModels;
using ReportServiceWeb02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportServiceWeb02.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReporteDiarioController : ControllerBase
    {
        internal readonly DirectoryInfo Dinfo = new DirectoryInfo("C:\\ScadaReps");
        private readonly MyDbContext _myDbContext;

        public ReporteDiarioController(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }


        // GET: api/<ReporteDiarioController>
        [HttpGet]
        public JsonResult Get()
        {
            List<fileShow> files = listAllFiles(Dinfo.Parent.FullName, Dinfo);
            
            return new JsonResult(files);
        }

        private List<fileShow> listAllFiles(string parent, DirectoryInfo dinfo)
        {
            string dname = dinfo.FullName.Replace(parent, "");

            List<string> dfiles = new List<string>();

            List<fileShow> files = new List<fileShow>();

            var Dfiles = dinfo.GetFiles();

            foreach(var df in Dfiles)
            {
                string dfile = df.Name;

                dfiles.Add(dfile);
            }

            if (dfiles.Any())
                files.Add(new fileShow() { name = dname, files = dfiles });
            
            var Dinfos = dinfo.GetDirectories();

            foreach (var di in Dinfos)
            {
                files.AddRange(listAllFiles(parent, di));
            }

            return files;
        }

        // GET api/<ReporteDiarioController>/5
        [HttpGet("{id}")]
        public string Get(string id)
        {
            return id;
        }

        // POST api/<ReporteDiarioController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        private void Reporte_D()
        {

            string longdate1, longdate2, dirname;
            DataTable dt = new DataTable();
            int i = -7;

            Task[] t2 = new Task[7];

            do
            {
                DateTime todays = DateTime.Now.AddDays(i).AddMinutes(-15);

                string[] fechas = SharedFunctions.getdate(todays);

                longdate1 = fechas[4];
                longdate2 = fechas[5];
                dirname = fechas[6];

                string filerep = $"{dirname}\\REP\\", fname = $"REP {longdate1}.Xls";

                string filename1 = filerep + fname;

                filerep = $"{dirname}\\INC\\";
                fname = $"INC - ITC {longdate1}.Xls";

                string filename2 = $"{filerep}Interrupciones Diarias {fname}";

                FileInfo finf1 = new FileInfo(filename1);
                FileInfo ftxt1 = new FileInfo(filename1.Replace($".Xls", ".Txt"));

                FileInfo finf2 = new FileInfo(filename2);
                FileInfo ftxt2 = new FileInfo(filename2.Replace($".Xls", ".Txt"));

                if (finf1.Exists == false || finf2.Exists == false)
                {
                    finf1.Delete();
                    ftxt1.Delete();
                    finf1 = new FileInfo(filename1);
                    ftxt1 = new FileInfo(filename1.Replace($".Xls", ".Txt"));

                    finf2.Delete();
                    ftxt2.Delete();
                    finf2 = new FileInfo(filename2);
                    ftxt2 = new FileInfo(filename2.Replace($".Xls", ".Txt"));
                }

                if (finf1.Exists == false && finf2.Exists == false)
                {
                    List<string[]> FileAtt = new List<string[]>();
                    MailMessage dailyrepmail = new MailMessage();


                    var Drep0 = _myDbContext.GetReporteRDt1(todays);

                    try
                    {
                        SharedFunctions.createExcel5(finf1.FullName, Drep0, 1);
                    }
                    catch (Exception xp)
                    {
                        //_forms.printRichTBox($"Error Rdiario 440: {DateTime.Now.ToString($"dd/MM/yyyy HH:mm:ss")} {xp.Message}");
                        //_forms.printRichTBox($"Error Rdiario 440: {DateTime.Now.ToString($"dd/MM/yyyy HH:mm:ss")} {xp.StackTrace}");
                    }
                    finally
                    {
                        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                    }

                    do
                    {
                        try
                        {
                            Thread.Sleep(1500);

                            FileAtt.Add(new string[] { finf1.Name, finf1.FullName });
                            break;
                        }
                        catch (Exception xp)
                        {

                        }
                    } while (true);

                    Drep0 = _myDbContext.GetReporteRDt0(todays);
                    var Dtmail = _myDbContext.GetMailRDt();

                    dailyrepmail = SharedFunctions.CreateMail(Dtmail);
                    //GetDataTable($"EXEC [dbo].[readdailyr] @usedate = '{todays.AddDays(1).ToString($"yyyy/MM/dd")}', @type = 0;", Drep1);


                    try
                    {
                        SharedFunctions.createExcel5(finf2.FullName, Drep0, 0);
                    }
                    catch (Exception xp)
                    {
                        //_forms.printRichTBox($"Error Rdiario 490: {DateTime.Now.ToString($"dd/MM/yyyy HH:mm:ss")} {xp.Message}");
                        //_forms.printRichTBox($"Error Rdiario 490: {DateTime.Now.ToString($"dd/MM/yyyy HH:mm:ss")} {xp.StackTrace}");
                    }
                    finally
                    {
                        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
                    }

                    do
                    {
                        try
                        {
                            Thread.Sleep(1500);

                            FileAtt.Add(new string[] { finf2.Name, finf2.FullName });

                            dailyrepmail.Subject = $"Reporte Diario de Operaciones {longdate2}";
                            dailyrepmail.Body = $"Reporte";

                            //MMS.Add(new object[] { dailyrepmail, FileAtt });
                            break;
                        }
                        catch (Exception xp)
                        {

                        }
                        finally
                        {
                            ftxt1.Delete();
                            ftxt2.Delete();

                            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

                        }

                    } while (true);
                }

                ftxt1.Delete();
                ftxt2.Delete();
                i++;
            } while (i < 0);

            //_forms.printRichTBox($"Report Finished: {DateTime.Now.ToString($"dd/MM/yyyy HH:mm:ss")}");
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
}
