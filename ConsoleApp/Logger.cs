using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleApp
{
    public static class Logger
    {
        public static void ExceptionLogger(Exception ex)
        {
            try
            {               

                var path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), "../../App_Data/Logs/model.txt");
                var file = new FileInfo(path);
                file.Directory.Create();

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine();

                    while (ex != null)
                    {
                        writer.WriteLine(ex.GetType().FullName);
                        writer.WriteLine("Message : " + ex.Message);
                        writer.WriteLine("StackTrace : " + ex.StackTrace);

                        ex = ex.InnerException;
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("from ExceptionLogger:", e);
                //Console.WriteLine(e.Message);
            }

        }


        public static void TestLogger(string data)
        {
            try
            {
                var path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), "../../App_Data/Logs/model.txt");

               Console.WriteLine(path);

                var file = new FileInfo(path);
                file.Directory.Create();

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine(data);


                }
            }
            catch (Exception e)
            {
                throw new Exception("from testlogger:", e);
            }

        }

        public static void DictToModel(Dictionary<string, string> data,string className)
        {
            try
            {
                var path = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), "../../App_Data/Logs/models.txt");


                var file = new FileInfo(path);
                file.Directory.Create();

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------");
                    writer.WriteLine("Date : " + DateTime.Now.ToString());
                    writer.WriteLine("public class {0} {{", className);

                    foreach (var prop in data)
                    {
                        writer.WriteLine($"public { prop.Value} {prop.Key} {{get; set;}} ");
                    }
                    writer.WriteLine("}}");


                }
            }
            catch (Exception e)
            {
                throw new Exception("from testlogger:", e);
            }

        }
    }
}