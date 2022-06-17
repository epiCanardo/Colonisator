using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class IslandsDTO
    {

        public List<string> islandNames { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static IslandsDTO LoadFromFile(string file)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(file))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            return JsonConvert.DeserializeObject<IslandsDTO>(jsonMoq);
        }
    }
}