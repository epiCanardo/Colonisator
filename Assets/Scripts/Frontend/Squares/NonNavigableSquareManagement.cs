using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.Squares
{
    public class NonNavigableSquareManagement : SquareManagement
    {        
        protected override bool navigable => false;
    }
}
