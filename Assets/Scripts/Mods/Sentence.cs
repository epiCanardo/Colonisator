using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColanderSource;
using Newtonsoft.Json;

namespace Colonisator.Mods
{
    public class Sentence
    {
        public static string OBJECTIVE_WANNA_COLONIZE = "OBJECTIVE_WANNA_COLONIZE";
        public static string OBJECTIVE_WANNA_PUNCTURE = "OBJECTIVE_WANNA_PUNCTURE";
        public static string OBJECTIVE_WANNA_FIRECREW = "OBJECTIVE_WANNA_FIRECREW";
        public static string OBJECTIVE_WANNA_BUY_RIGGING = "OBJECTIVE_WANNA_BUY_RIGGING";
        public static string OBJECTIVE_WANNA_BUY_FOOD = "OBJECTIVE_WANNA_BUY_FOOD";
        public static string OBJECTIVE_WANNA_BUY_CREW = "OBJECTIVE_WANNA_BUY_CREW";
        public static string OBJECTIVE_NOTHING = "OBJECTIVE_NOTHING";

        public List<SentenceObject> sentences { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Sentence LoadFromFile(string file)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(file))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            return JsonConvert.DeserializeObject<Sentence>(jsonMoq);
        }
    }

    public class SentenceObject
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}