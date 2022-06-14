using Assets.Scripts.Model;

namespace Assets.Scripts.DTO
{
    public class MoveDTO
    {
        /// <summary>
        /// le navrie associé au mouvement
        /// </summary>
        public Ship ship { get; set; }

        /// <summary>
        /// le mouvement réellement effectué
        /// </summary>
        public Move move { get; set; }
    }
}
