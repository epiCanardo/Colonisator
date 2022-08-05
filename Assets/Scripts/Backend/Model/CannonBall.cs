namespace Assets.Scripts.Model
{
    /// <summary>
    /// le canon de navire
    /// </summary>
    public class CannonBall
    {
        /// <summary>
        /// poids en kg
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// le type de munition
        /// </summary>
        public string type { get; set; }
    }

    public enum AmmoType
    {
        REGULAR, // season lol. le boulet classique pour casser de la coque
        CHAINED, // deux boulets enchaînés pour casser du gréément
        SHIT // de la grenaille pour mutiler et tuer des matelots
    }
}