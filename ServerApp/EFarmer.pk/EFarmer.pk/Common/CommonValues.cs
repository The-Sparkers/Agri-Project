using Microsoft.Extensions.Configuration;
using System;
namespace EFarmer.pk.Common
{
    public static class CommonValues
    {
        public static readonly string APP_NAME = "e-Farmer";
        public static readonly string APP_NAME_PART = ".pk";
        public static readonly string CONNECTION_STRING;
        public static readonly string CROP_DEFAULT_PIC = "agri_crop.jpg";
        public static readonly string VEG_DEFAULT_PIC = "agri_veg.jpg";
        public static readonly string FRUIT_DEFAULT_PIC = "agri_fruit.jpg";
        public static readonly double RADIUS_IN_KM = 25;
        public static readonly double RADIUS_IN_MILES = 25;
        static CommonValues()
        {
            //getting connection string from appsettings.json
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
