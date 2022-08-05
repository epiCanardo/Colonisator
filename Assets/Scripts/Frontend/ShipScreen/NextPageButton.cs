namespace Assets.Scripts.Front.ShipScreen
{
    public class NextPageButton : ShipScreenButton
    {
        protected override void DoSomethingOnClick()
        {
            shipScreenManager.NextPage();
        }
    }
}