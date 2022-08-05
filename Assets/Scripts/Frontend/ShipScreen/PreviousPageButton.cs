namespace Assets.Scripts.Front.ShipScreen
{
    public class PreviousPageButton : ShipScreenButton
    {
        protected override void DoSomethingOnClick()
        {
            shipScreenManager.PrevisousPage();
        }
    }
}