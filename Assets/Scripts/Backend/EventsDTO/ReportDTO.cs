using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts.EventsDTO
{
    /// <summary>
    /// classe de stockage du rapport de tour
    /// il contient une liste d'évènements
    /// </summary>
    public class ReportDTO<T> where T : EventDTO
    {
        public List<T> events { get; set; }

        public ReportDTO()
        {
            events = new List<T>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
