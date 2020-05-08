using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigApp.Helpers
{
    public static class ConnectorVersion
    {
        /// <summary>
        /// get connector version
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                return version.ToString();
            }

            return null;
        }
    }
}
