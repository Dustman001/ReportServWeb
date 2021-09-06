using ReportServiceWeb02.DbModels;
using ReportServiceWeb02.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace ReportServiceWeb02.Data
{
    public class SharedFunctions
    {
        public static string[] getdate(DateTime thedate)
        {
            string[] data = new string[7];

            //culturecheck();

            TextInfo tf = Thread.CurrentThread.CurrentCulture.TextInfo;

            DateTime useful = DateTime.Now;

            data[0] = useful.ToString($"MMM").ToUpper();          //            mes1
            data[1] = tf.ToTitleCase(useful.ToString($"MMMM"));   //            mes2
            data[2] = useful.ToString($"yyyy").ToUpper();         //            age
            data[3] = useful.ToString($"dd").ToUpper();           //            day

            data[4] = $"{data[0]} {data[2]} {data[3]}";   //          longdate1
            data[5] = $"{data[1]} {data[2]} {data[3]}";   //          longdate2

            data[0] = $"{useful.ToString($"MM").ToUpper()} - {useful.ToString($"MMM").ToUpper()}";  //      mes1

            useful = Convert.ToDateTime(thedate);

            data[0] = useful.ToString($"MMM").ToUpper();          //             mes1
            data[1] = tf.ToTitleCase(useful.ToString($"MMMM"));   //             mes2
            data[2] = useful.ToString($"yyyy").ToUpper();         //             age
            data[3] = useful.ToString($"dd").ToUpper();           //             day

            data[4] = $"{data[0]} {data[2]} {data[3]}";   //             longdate1
            data[5] = $"{data[1]} {data[2]} {data[3]}";   //             longdate2

            data[0] = $"{useful.ToString($"MM").ToUpper()} - {useful.ToString($"MMM").ToUpper()}";//          mes1

            data[6] = createdirs(data[2], $"{data[0]}\\test");
            data[6] = createdirs(data[2], data[0]);            //          dirname

            return data;
        }

        public static string createdirs(string age1, string mes1b)
        {
            string dirname = $"C:\\ScadaReps\\{age1}\\{mes1b}";//"D:\\Dropbox\\ScadaReps\\{age1}\\{mes1b;

            DirectoryInfo dinf = new DirectoryInfo(dirname);

            if (dinf.Exists == false)
            {
                dinf.Create();
            }

            if (dirname.Contains($"test") == true)
                return dirname;

            dinf = new DirectoryInfo($"{dirname}\\REP");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\REP");
                dinf.Create();
            }

            dinf = new DirectoryInfo($"{dirname}\\INC");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\INC");
                dinf.Create();
            }

            dinf = new DirectoryInfo($"{dirname}\\ITC");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\ITC");
                dinf.Create();
            }

            dinf = new DirectoryInfo($"{dirname}\\DES");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\DES");
                dinf.Create();
            }

            dinf = new DirectoryInfo($"{dirname}\\ALM");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\ALM");
                dinf.Create();
            }

            dinf = new DirectoryInfo($"{dirname}\\NEU");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\NEU");
                dinf.Create();
            }

            dinf = new DirectoryInfo($"{dirname}\\INTS");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\INTS");
                dinf.Create();
            }
            dinf = new DirectoryInfo($"{dirname}\\RTU");
            if (dinf.Exists == false)
            {
                dinf = new DirectoryInfo($"{dirname}\\RTU");
                dinf.Create();
            }

            return dirname;
        }

        public static MailMessage CreateMail(List<Correos> Dtmail)
        {
            var from = Dtmail.Where(w => w.Type == "From").FirstOrDefault();

            var to = Dtmail.Where(w => w.Type == "To").FirstOrDefault();

            var cct = Dtmail.Where(w => w.Type == "CC").ToList();

            MailMessage dailyrepmail = new MailMessage(new MailAddress(from.Email, from.Name),
                new MailAddress(to.Email, to.Name));

            foreach (Correos crs in cct)
            {
                dailyrepmail.CC.Add(new MailAddress(crs.Email, crs.Name));
            }

            dailyrepmail.Bcc.Add(new MailAddress(from.Email, from.Name));

            return dailyrepmail;
        }

        public static void createExcel5(string v1,
                                List<IReport> dt0,
                                int p)
        {
            string ftxt = "";

            if (p == 0)
            {
                ftxt = getdata4(v1, dt0);
            }
            else// if (p == 1)
            {
                ftxt = getdata3(v1, dt0);
            }

            string args = $"\"{v1}\" \"{ftxt}\" {p}";

            /*
            FileInfo fi = new FileInfo($"{Directory.GetCurrentDirectory()}\\ConsoleApp1.exe");

            if (fi.Exists == true)
            {
                Process pr = new Process();
                pr.StartInfo = new ProcessStartInfo();
                pr.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                pr.StartInfo.FileName = $"cmd.exe";
                pr.StartInfo.RedirectStandardInput = true;
                pr.StartInfo.RedirectStandardOutput = true;
                pr.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;


                pr.StartInfo.UseShellExecute = false;
                //pr.StartInfo.CreateNoWindow = true;
                //pr.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                pr.Start();
                StreamWriter sw = pr.StandardInput;
                StreamReader sr = pr.StandardOutput;

                sw.AutoFlush = true;

                sw.WriteLine($"ConsoleApp1.exe {args}");
                string t = sr.ReadLine();

                while (t.Contains($"Exit") == false)
                {
                    t = sr.ReadLine();
                };

                sw.WriteLine($"Exit");

                pr.Kill();

                pr.Dispose();
            }
            */
        }

        public string getdata3(string filename, List<IReport> dt0)
        {
            string swt = filename.Replace(".Xls", ".Txt");

            var datr = from dr in dt0
                       let dr1 = (ReporteDiario1)dr
                       let vl = dr1.Time.ToString() + '\t' + dr1.Message + '\t' + dr1.Station + '\t' + dr1.Operator + '\r' + '\n'
                       select vl;

            StreamWriter sw = new StreamWriter(swt);
            sw.AutoFlush = true;

            var props = dt0[0].GetType().GetProperties();

            string data = "";

            foreach (var prop in props)
            {
                data += prop.Name + "\t";
            }

            data += "\r\n";

            datr.ToList().ForEach(a => data += a);

            sw.WriteLine(data);

            sw.Close();

            return swt;
        }

        public string getdata4(string filename, List<IReport> dt0)
        {
            var props = dt0[0].GetType().GetProperties();

            object[,] data = new object[(dt0.Count * 2 + 10), (props.Length + 1)];

            int rw = -1, cl = 0;
            string pt = "";

            bool pos = true, apos = true;
            DateTime atime = new DateTime(), otime = new DateTime();

            foreach (ReporteDiario0 dr0 in dt0)
            {
                apos = pos;

                string messg = Convert.ToString(dr0.Estado);
                string point = Convert.ToString(dr0.Station);

                if (point.Contains($"CHIV105PUN") == true)//SMAR
                { }

                try
                {
                    if (messg == $"Abierto")
                        pos = true;
                    else
                        pos = false;

                    if (point != pt)
                    {
                        cl = 0;
                        pt = point;
                        rw++;
                        apos = !pos;
                    }

                    if (messg == $"Abierto")
                        atime = Convert.ToDateTime(dr0.Time);
                    else
                        otime = Convert.ToDateTime(dr0.Time);
                }
                catch (Exception x)
                { }

                try
                {
                    if (messg.Contains($"Abierto") == true)
                    {
                        TimeSpan ddif = atime - otime;

                        if (cl > 0 && (ddif.TotalSeconds > 0 && ddif.TotalSeconds < 10))
                        {
                            continue;
                        }
                        else if (apos == true)
                        {
                            //rw--;
                            continue;
                        }

                        if (pos == false)
                        {
                            rw--;
                        }

                        //pos = false;
                        data[rw, 1] = atime;
                        data[rw, 2] = atime.Date.AddDays(1).AddSeconds(-1);
                    }
                    else
                    {
                        if (cl == 0)
                        {
                            data[rw, 1] = otime.Date;
                            rw++;
                        }

                        //pos = true;
                        if (apos == false)
                        { //rw--; 
                        }

                        rw--;
                        data[rw, 2] = otime;
                    }
                }
                catch (Exception x)
                {

                }

                data[rw, 0] = point;
                cl++;
                rw++;
            }

            StreamWriter sw = new StreamWriter(filename.Replace($".Xls", ".Txt"));
            sw.AutoFlush = true;
            string ds = "";

            for (int i = 0, b = data.Length / 4; i < b; i++)
            {
                for (int c = 0; c < 3;)
                {
                    if (c == 0)
                    {
                        string d = "";

                        try
                        {
                            d = data[i, c].ToString();

                            if (d.Contains($"\n") == true)
                            {
                                d = d.Replace($"\n ", "");
                                d = d.Replace($"\n", " ");
                            }
                        }
                        catch (Exception x)
                        {
                            d = $"\r\n";
                            ds += d;
                            break;
                        }

                        ds += d;
                    }
                    else
                    {
                        ds += Convert.ToDateTime(data[i, c]).ToString($"yyyy/MM/dd HH:mm:ss");
                    }

                    c++;

                    if (c == 3)
                    {
                        ds += "\r\n";
                    }
                    else
                    {
                        ds += "\t";
                    }
                }
            }

            sw.Write(ds);
            sw.Close();

            return filename.Replace($".Xls", ".Txt");
        }
    }
}
