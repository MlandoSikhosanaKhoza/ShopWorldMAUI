using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopWorld.MAUI
{
    public class Constants
    {
        public const string DatabaseFilename = "ShopWorld.db3";
        public static string ShopWorldApiUrl = "https://shopworldapi.thetalkzulu.co.za";
        public const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;
        public static string DatabasePath => Path.Combine("C:\\CSPROJ", DatabaseFilename);
        public static string ImageDirectory => $"{FileSystem.AppDataDirectory}/Images/";
        public static string GenerateImageUrl(string ImageName)
        {
            return $"{ImageDirectory}{ImageName}";
        }
    }
}
