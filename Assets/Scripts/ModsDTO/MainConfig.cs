using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColanderSource;
using Newtonsoft.Json;

namespace Colonisator.ModsDTO
{
    public class MainConfig
    {       
        public string language { get; set; }
        public string version { get; set; }
        public List<string> activeMods { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static MainConfig LoadFromFile(string file)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(file))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            return JsonConvert.DeserializeObject<MainConfig>(jsonMoq);
        }
    }
}