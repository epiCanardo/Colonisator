using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class NpcsDTO
    {

        public List<string> firstNames_f { get; set; }
        public List<string> firstNames_m { get; set; }
        public List<string> lastNames { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static NpcsDTO LoadFromFile(string file)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(file))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            return JsonConvert.DeserializeObject<NpcsDTO>(jsonMoq);
        }
    }
}