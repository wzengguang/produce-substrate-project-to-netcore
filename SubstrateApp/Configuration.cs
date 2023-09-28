using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubstrateApp
{
    public static class AppConfiguration
    {
        public static string SubstratePath
        {
            get
            {
                return ConfigurationManager.AppSettings["SubstrateRootPath"];
            }
            set
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["SubstrateRootPath"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        }
    }
}
