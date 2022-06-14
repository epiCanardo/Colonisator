using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public abstract class ConfigDTO<T>
    {
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static T LoadFromFile(string file)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(file))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            return JsonConvert.DeserializeObject<T>(jsonMoq);
        }
    }
}