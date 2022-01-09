using Newtonsoft.Json;
using System.IO;

namespace Config
{
    public class ConfigUtil
    {
        public static T ReadConfig<T>(string path) where T:new()
        {
            T config = new T();
            using (StreamReader file = File.OpenText(path))
            {
                var jsonData = file.ReadToEnd();
                config = JsonConvert.DeserializeObject<T>(jsonData);
            }
            return config;
        }
    }
}
