using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Service;
using Models;
using BarCodeService;

namespace ConsoleApp
{
    class Program
    {
        readonly TodoClient _todoClient = new TodoClient(new HttpClient());

        static void Main(string[] args)
        {

            //if (!Directory.Exists(@"..\..\Excel")) Directory.Exists(@"..\..\Excel");

            //var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //Console.WriteLine(path);
            try
            {
                //Program.Test();
                var p = new Program();
                //Console.WriteLine(p.GetDateToInt(DateTime.Now));
                //Console.WriteLine(p.GetDateTimetoInt(DateTime.Now));

                //p.ExcelToModel("models", "mCore_Account");
                //p.CreateBarcode("this is a barcode.");
                //PdfService.IronPdfPackage.HtmlFileToPdf(@"C:\Users\hp\Desktop\Desktop\table.html");
                //var html = "<h1 style='color:Red;'>Test</h1>";
                //PdfService.IronPdfPackage.HtmlToPdf(html);
                //PdfService.IronPdfPackage.UrlToPdf("https://ironpdf.com/docs/");
                //MailService.MailKitPackage.Send();

                //p.DoAsync();

                p.TodosExcel("Todos");
                //var r = new Random();

                //for (int i = 0; i < 10; i++)
                //{
                //    //Console.WriteLine(p.Code(r));
                //    //p.GetTodo(1);
                //    //p.GetTodo(2);
                //    //p.GetTodo(3);
                //    //p.GetTodo(4);
                //    //p.GetTodo(5);

                //    //p.GetTodos();

                //    //p.GetTodo(6);
                //    //p.GetTodo(7);
                //    //p.GetTodo(8);
                //    //p.GetTodo(9);
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.TargetSite.Name);
                    while (ex != null)
                {
                    Console.WriteLine(ex.GetType().FullName);
                    Console.WriteLine("Message : " + ex.Message);
                    Console.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }

            }
            Console.ReadKey();
        }      
        
        static void Test()
        {
            Console.WriteLine("Test");
            var a = Convert.ToInt32("dfgdfg");
        }
        void CreateBarcode(string data)
        {
            IronBarCodePackage.CreateBarCode(data);
        }

        void ReadBarCode(string path)
        {
            IronBarCodePackage.ReadBarCode(path);

        }

        async void DoAsync()
        {
            var filePath = await Task.Run(() =>
             {
                 var list = new List<int>();
                 for (int i = 0; i < 100000000; i++)
                     list.Add(i);
                 return list.Count;

             });
            Console.WriteLine(filePath);

        }
        void ExcelToModel(string filename,string className)
        {
           var modeldata = Excel.ExcelUtil.ExcelToModelKeyValue(String.Format(@"C:\Users\hp\Desktop\Desktop\{0}.xlsx", filename));
            Logger.DictToModel(modeldata, className);
            Console.WriteLine($"{className} Model created");
        }

        async void TodosExcel(string filename)
        {
            Console.WriteLine("Creating excel......");

            var fileinfo = new FileInfo(String.Format(@"C:\Users\hp\Desktop\Desktop\{0}.xlsx", filename));
            var todos = await _todoClient.GetTodosAsync();
            var result = await Task.Run<string>(() =>
            {
                return Excel.ExcelUtil.Create(todos, filename, new DirectoryInfo(@"..\..\Excel"));
            });
            Console.WriteLine(result);
        }
        async void GetTodo(int id)
        {
            var todo = await _todoClient.GetTodoAsync(id);
            Console.WriteLine($" Id : {todo.Id} Title : {todo.Title} Completed :{ todo.Completed},UserId : {todo.UserId}");
        }

        async void GetTodos()
        {
            var todos = await _todoClient.GetTodosAsync();
            Console.WriteLine(todos.Count());
        }

        async void PostTodo(Todo todo)
        {
            var newTodo = await _todoClient.PostTodoAsync(todo);
            Console.WriteLine("new todo created");
            Console.WriteLine($" Id : {newTodo.Id} Title : {newTodo.Title} Completed :{ newTodo.Completed},UserId : {newTodo.UserId}");
        }

        async void DeleteTodo(int id)
        {
            var todo = await _todoClient.GetTodoAsync(id);
            if (todo != null)
            {
                var isDeleted = await _todoClient.DeleteTodoAsync(id);
                var msg = isDeleted ? "Todo Deleted" : "Somthing went wrong";
                Console.WriteLine(msg);
            }
            else
            {
                Console.WriteLine($"Todo with id : {id} doesent exist");
            }

        }

        async void PutTodo(int id, Todo updateTodo)
        {
            var todo = await _todoClient.GetTodoAsync(id);
            if (todo != null)
            {
                var updatedTodo = await _todoClient.PutTodoAsync(id, updateTodo);
                Console.WriteLine($"Todo with {id} Updated");
                Console.WriteLine($" Id : {updatedTodo.Id} Title : {updatedTodo.Title} Completed :{ updatedTodo.Completed},UserId : {updatedTodo.UserId}");
            }
            else
            {
                Console.WriteLine($"Todo with id : {id} doesent exist");
            }
        }

        string Code(Random r)
        {
            var code = "";
            char[] base36Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for (int i = 0; i < 3; i++)
            {
                code += base36Chars[r.Next(0, 35)];
            }
            return code;
        }
        public Int64 GetDateTimetoInt(DateTime dt)
        {
            Int64 val;
            val = Convert.ToInt64(dt.Year) * 8589934592 + Convert.ToInt64(dt.Month) * 33554432 + Convert.ToInt64(dt.Day) * 131072 + Convert.ToInt64(dt.Hour) * 4096 + Convert.ToInt64(dt.Minute) * 64 + Convert.ToInt64(dt.Second);
            return val;
        }
        public int GetDateToInt(DateTime dt)
        {
            int val;
            val = Convert.ToInt16(dt.Year) * 65536 + Convert.ToInt16(dt.Month) * 256 + Convert.ToInt16(dt.Day);
            return val;
        }
    }
}
