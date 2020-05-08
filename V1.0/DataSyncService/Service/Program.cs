using System.ServiceProcess;
using System.Threading.Tasks;

namespace DataSyncService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            #if DEBUG
            var scheduler = new Scheduler();
            scheduler.OnDebug();

            #else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Scheduler()
            };
            ServiceBase.Run(ServicesToRun);
            #endif
        }


        //static void RunDataSync()
        //{
        //    //This is use for only development - we can debug the code using this.       
        //    Helper.DinerwareDataSync dataSync = new Helper.DinerwareDataSync();
        //    Task task = dataSync.DataSyncAsync();
        //    task.GetAwaiter().GetResult();
        //}

    }
}
