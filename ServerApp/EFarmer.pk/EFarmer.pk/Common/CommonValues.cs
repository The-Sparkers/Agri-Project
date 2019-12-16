using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
namespace EFarmer.pk.Common
{
    public static class CommonValues
    {
        public static readonly string APP_NAME = "e-Farmer";
        public static readonly string APP_NAME_PART = ".pk";
        public static readonly string CONNECTION_STRING;
        static CommonValues()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            //CONNECTION_STRING = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            CONNECTION_STRING = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
