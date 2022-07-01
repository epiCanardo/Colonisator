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

        public float[,] GetGroundCoordinates()
        {
            //return m_terrain.terrainData.GetHeights(0, 0, 100, 100);
            return m_terrain.terrainData.GetHeights(0, 0, m_terrain.terrainData.heightmapResolution, m_terrain.terrainData.heightmapResolution);
        }
    }
}