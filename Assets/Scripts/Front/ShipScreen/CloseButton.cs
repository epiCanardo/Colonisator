namespace Assets.Scripts.Front.ShipScreen
{
    public class CloseButton : ShipScreenButton
    {
        /// <summary>
        ///  action de fermture du panneau
        /// </summary>
        protected override void DoSomethingOnClick()
        {
            shipScreenManager.Close();
        }
    }
}