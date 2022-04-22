namespace Colfront.GamePlay
{
    public class PreviousPageButton : ShipScreenButton
    {
        protected override void DoSomethingOnClick()
        {
            shipScreenManager.PrevisousPage();
        }
    }
}