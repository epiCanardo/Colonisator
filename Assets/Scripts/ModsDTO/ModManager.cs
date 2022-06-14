using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ModsDTO
{
    public class ModManager
    {
        private static string activeMod = "Core";

        private MainConfigDTO _mainConfigDto;
        private FactionsDTO _factionsDTO;
        private SentenceDTO _sentencesDTO;

        private List<string> activeCards = new List<string>(); // la liste des cartes atives

        private static ModManager _instance;

        public ModManager()
        {
            Initialization();
        }

        public static ModManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ModManager();
                }

                return _instance;
            }
        }

        /// <summary>
        /// initialisation des fichiers mod (config et repertoires liés aux langues)
        /// </summary>
        public void Initialization()
        {
            _mainConfigDto = MainConfigDTO.LoadFromFile($"Mods/Config.json");
        }

        public List<string> GetCards()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string txtName in Directory.GetFiles($"Mods/{activeMod}/Cards/", "*.json"))
            {
                using (StreamReader sr = new StreamReader(txtName))
                {
                    activeCards.Add(sr.ReadToEnd());
                }
            }

            return activeCards;
        }

        /// <summary>
        /// chargement des phrases selon les mods et la langue
        /// </summary>
        public void LoadSentences()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/sentences.json"; 
            // TODO : seul le Core est activé actuellement
            
            _sentencesDTO = SentenceDTO.LoadFromFile(txtName);
        }

        public void LoadFactions()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/factions.json";
            // TODO : seul le Core est activé actuellement

            _factionsDTO = FactionsDTO.LoadFromFile(txtName);
  }

        /// <summary>
        /// donne la valeur en string associée à la clé passée
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <returns></returns>
        public string GetSentence(string sourceKey)
        {
            if (!_sentencesDTO.sentences.Any(x=>x.key.Equals(sourceKey)))
                return $"[{sourceKey}] undefinded";

            return _sentencesDTO.sentences.First(x => x.key.Equals(sourceKey)).value;
        }

        /// <summary>
        /// donne la valeur en string associée à la clé passée
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <returns></returns>
        public string GetFactionLabel(string sourceKey, Func<FactionDTOObject, string> label)
        {
            if (!_factionsDTO.factions.Any(x => x.key.Equals(sourceKey)))
                return $"[{sourceKey}] undefinded";

            return label(_factionsDTO.factions.First(x => x.key.Equals(sourceKey)));
        }
    }
}