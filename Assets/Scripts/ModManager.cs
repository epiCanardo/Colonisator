using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Colonisator.ModsDTO;
using ColanderSource;
using UnityEditor.PackageManager.Requests;

namespace Colfront.GamePlay
{
    public class ModManager
    {
        private static string activeMod = "Core";

        private MainConfig mainConfig;
        private List<string> activeCards = new List<string>(); // la liste des cartes atives
        private List<SentenceObject> activeSentences = new List<SentenceObject>(); // le dico des phrases

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
            mainConfig = MainConfig.LoadFromFile($"Mods/Config.json");
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

        public void LoadSentences()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{mainConfig.activeMods[0]}/Values/Sentences/{mainConfig.language}/sentences.json";
            
            Sentence sentence = Sentence.LoadFromFile(txtName);
            activeSentences = sentence.sentences;
        }

        /// <summary>
        /// donne la valeur en string associée à la clé passée
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <returns></returns>
        public string GetValue(string sourceKey)
        {
            if (!activeSentences.Any(x=>x.key.Equals(sourceKey)))
                return $"[{sourceKey}] undefinded";

            return activeSentences.First(x => x.key.Equals(sourceKey)).value;
        }
    }
}