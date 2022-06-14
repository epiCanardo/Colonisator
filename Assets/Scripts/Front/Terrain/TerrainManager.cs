namespace Assets.Scripts.Front.Terrain
{

    public class TerrainManager : UnityEngine.MonoBehaviour
    {

        public UnityEngine.Terrain m_terrain;  //reference to your terrain
        public float DrawDistance; // how far you want to be able to see the grass

        // Use this for initialization
        void Start()
        {
            m_terrain.detailObjectDistance = DrawDistance;

        }
    }
}