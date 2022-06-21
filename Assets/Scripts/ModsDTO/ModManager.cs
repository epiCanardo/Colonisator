using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Model;

namespace Assets.Scripts.ModsDTO
{
    public class ModManager
    {
        private MainConfigDTO _mainConfigDto;
        private FactionsDTO _factionsDTO;
        private SentenceDTO _sentencesDTO;
        private IslandsDTO _islandsDTO;
        private NpcsDTO _npcsDTO;
        private ShipsDTO _shipsDTO;
        private GeneralDTO _generalDTO;
        private PropertiesDTO _propertiesDTO;

        private static ModManager _instance;

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
        /// initialisation de toutes les configurations
        /// </summary>
        public void Initialization()
        {
            // chargement de la configuration globale
            _mainConfigDto = MainConfigDTO.LoadFromFile($"Mods/Config.json");

            // chargement des configurations liées aux mods actifs
            LoadSentences();
            LoadFactions();
            LoadGeneralValues();
            LoadIslands();
            LoadShips();
            LoadNpcs();
            LoadProperties();
        }

        public List<string> GetCards()
        {
            StringBuilder sb = new StringBuilder();
            List<string> activeCards = new List<string>();

            foreach (string txtName in Directory.GetFiles($"Mods/{_mainConfigDto.activeMods[0]}/Cards/{_mainConfigDto.language}", "*.json"))
            // TODO : seul le Core est activé actuellement
            {   
                using (StreamReader sr = new StreamReader(txtName))
                {
                    activeCards.Add(sr.ReadToEnd());
                }
            }

            return activeCards;
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
                return FactionsManager.Instance.Factions.First(x => x.Faction.playerTypeEnum.Equals(sourceKey)).Faction
                    .longName;

            return label(_factionsDTO.factions.First(x => x.key.Equals(sourceKey)));
        }

        /// <summary>
        /// retourne le label correspondant à une propriété
        /// </summary>
        /// <param name="sourceKey"></param>
        /// <returns></returns>
        public string GetPropertyLabel(string sourceKey)
        {
            if (!_propertiesDTO.boards.Any(x => x.key.Equals(sourceKey)))
                return $"[{sourceKey}] undefinded";

            return _propertiesDTO.boards.First(x => x.key.Equals(sourceKey)).value;
        }

        /// <summary>
        /// donne la valeur de gameplay associée à son 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public int GetGameplayValue(Func<GeneralDTO, int> property)
        {
            return property(_generalDTO);
        }

        /// <summary>
        /// retourne la borne max pour une propriété de ShipBoard
        /// </summary>
        /// <param name="ShipboardBounds">predicat sur la propriété</param>
        /// <param name="shipTypeEnum">le type de navire</param>
        /// <returns></returns>
        public int GetShipBoardBound(Func<GeneralDTO.ShipboardBounds, int> boardProperty, string shipTypeEnum)
        {
            return boardProperty(_generalDTO.shipDefinitions.First(x => x.shipTypeEnum.Equals(shipTypeEnum))
                .shipboardBounds);
        }

        /// <summary>
        /// retourne l'objet contenant les limites max selon le type de navire passé en paramètre
        /// </summary>
        /// <param name="shipTypeEnum"></param>
        /// <returns></returns>
        public GeneralDTO.ShipboardBounds GetShipboardBounds(string shipTypeEnum)
        {
            return _generalDTO.shipDefinitions.First(x => x.shipTypeEnum.Equals(shipTypeEnum)).shipboardBounds;
        }

        /// <summary>
        /// retourn les bornes min et max sur le nombre de matelots selon le type de navire
        /// </summary>
        /// <param name="shipTypeEnum"></param>
        /// <returns></returns>
        public Tuple<int, int> GetShipCrewBounds(string shipTypeEnum)
        {
            var bounds = _generalDTO.shipDefinitions.First(x => x.shipTypeEnum.Equals(shipTypeEnum)).crewBounds;
            return new Tuple<int, int>(bounds.min, bounds.max);
        }

        /// <summary>
        /// retourne le chemin d'accès vers le .bat du serveur colback
        /// </summary>
        /// <returns></returns>
        public string GetColbackLocation()
        {
            return _mainConfigDto.colbackLocation;
        }

        /// <summary>
        /// chargement des phrases selon les mods et la langue
        /// </summary>
        private void LoadSentences()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/sentences.json";
            // TODO : seul le Core est activé actuellement

            _sentencesDTO = SentenceDTO.LoadFromFile(txtName);
        }

        private void LoadFactions()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/factions.json";
            // TODO : seul le Core est activé actuellement

            _factionsDTO = FactionsDTO.LoadFromFile(txtName);
        }

        private void LoadIslands()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/islands.json";
            // TODO : seul le Core est activé actuellement

            _islandsDTO = IslandsDTO.LoadFromFile(txtName);
        }

        private void LoadNpcs()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/npcs.json";
            // TODO : seul le Core est activé actuellement

            _npcsDTO = NpcsDTO.LoadFromFile(txtName);
        }

        private void LoadShips()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/ships.json";
            // TODO : seul le Core est activé actuellement

            _shipsDTO = ShipsDTO.LoadFromFile(txtName);
        }
        private void LoadGeneralValues()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Gameplay/general.json";
            // TODO : seul le Core est activé actuellement

            _generalDTO = GeneralDTO.LoadFromFile(txtName);
        }

        private void LoadProperties()
        {
            StringBuilder sb = new StringBuilder();
            string txtName = $"Mods/{_mainConfigDto.activeMods[0]}/Values/Sentences/{_mainConfigDto.language}/properties.json";
            // TODO : seul le Core est activé actuellement

            _propertiesDTO = PropertiesDTO.LoadFromFile(txtName);
        }
    }
}