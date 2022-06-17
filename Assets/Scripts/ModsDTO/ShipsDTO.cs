using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class ShipsDTO
    {

        public List<string> shipNames { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static ShipsDTO LoadFromFile(string file)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(file))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            return JsonConvert.DeserializeObject<ShipsDTO>(jsonMoq);
        }
    }
}