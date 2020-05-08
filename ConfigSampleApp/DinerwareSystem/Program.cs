using ConfigApp.Helpers;
using ConfigApp.Provider;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.ServiceProcess;

namespace ConfigApp
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SetupResolver();
                Application.Run(new frmConfiguration());               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      

        /// <summary>
        /// Setup Resolver
        /// </summary>
        private static void SetupResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        /// <summary>
        /// Current Domain On Assembly Resolve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
                return typeof(JsonSerializer).Assembly;
            return null;
        }  
 
    }
}
