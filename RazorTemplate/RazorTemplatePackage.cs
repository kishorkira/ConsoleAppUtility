
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;
using System.Security.Policy;
using System.Reflection;
using System.Security.Permissions;
using System.Security;
using RazorEngine.Configuration;
using RazorEngine.Text;

namespace RazorTemplate
{
    public class RazorTemplatePackage
    {
        static void CleanUpTempFiles()
        {
            if (AppDomain.CurrentDomain.IsDefaultAppDomain())
            {
                // RazorEngine cannot clean up from the default appdomain...
                Console.WriteLine("Switching to secound AppDomain, for RazorEngine...");
                AppDomainSetup adSetup = new AppDomainSetup();
                adSetup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var current = AppDomain.CurrentDomain;
                // You only need to add strongnames when your appdomain is not a full trust environment.
                var strongNames = new StrongName[0];

                var domain = AppDomain.CreateDomain(
                    "MyMainDomain", null,
                    current.SetupInformation, new PermissionSet(PermissionState.Unrestricted),
                    strongNames);
                var exitCode = domain.ExecuteAssembly(Assembly.GetExecutingAssembly().Location);
                // RazorEngine will cleanup. 
                AppDomain.Unload(domain);
               
            }

        }

        public static string Test()
        {
            var config = new TemplateServiceConfiguration();
            config.EncodedStringFactory = new HtmlEncodedStringFactory();
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
            var template = "@Model.TemplateName";
            var result = Engine.Razor.RunCompile(template, "TestTemplate", null, new { TemplateName = "Test" });
            return result;
        }
    }
}
