namespace Colfront.GamePlay
{
    public class NextPageButton : ShipScreenButton
    {
        protected override void DoSomethingOnClick()
        {
            shipScreenManager.NextPage();
        }
    }
}