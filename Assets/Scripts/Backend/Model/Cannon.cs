namespace Assets.Scripts.Model
{
    /// <summary>
    /// le canon de navire
    /// </summary>
    public class Cannon
    {
        /// <summary>
        /// le nom du canon
        /// </summary>
        public string name { get; set; }
        
        /// <summary>
        /// puissance du canon
        /// </summary>
        public int power { get; set; }

        /// <summary>
        /// portée brute du canon (peut-être modifiée par le type de munition et le type de poudre)
        /// </summary>
        public int range { get; set; }

        /// <summary>
        /// la dispersion en mètre pour 100m de portée
        /// </summary>
        public float dispersion { get; set; }
    }
}