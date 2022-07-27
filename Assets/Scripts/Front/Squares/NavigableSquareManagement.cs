using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.Squares
{
    public class NavigableSquareManagement : SquareManagement
    {     
        protected override bool navigable => true;
    }
}
