using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanManagement.Util
{
    internal class ConnectionUtil
    {
        private static IConfiguration _iconfiguration;
         
        static ConnectionUtil()
        {
            GetAppSettingsFile();
        }


        public static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _iconfiguration = builder.Build();
        }

      
        public static string GetConnectionString()
        {
            return _iconfiguration.GetConnectionString("LocalConnectionString");
        }
    }
}
