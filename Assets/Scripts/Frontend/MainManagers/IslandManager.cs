using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.Squares;
using Assets.Scripts.Model;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.IslandActivityManagers
{
    /// <summary>
    /// gestion de toutes les ressources associées à une île
    /// </summary>
    public class IslandManager : UnityEngine.MonoBehaviour
    {
        private Island island;
        private List<SquareManagement> squares;
        private FactionManager factionManager;

        [SerializeField]
        private GameObject harborPrebab;

        private void Start()
        {

        }

        private void Update()
        {

        }

        public void SetIsland(Island island, List<SquareManagement> squares, FactionManager factionManager)
        {
            this.island = island;
            this.squares = squares;
            this.factionManager = factionManager;
        }
    }
}