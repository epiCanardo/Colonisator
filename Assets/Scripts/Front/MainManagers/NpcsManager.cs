using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{

    public class NpcsManager : UnityEngine.MonoBehaviour
    {
        [Header("Les capitaines")]
        public List<GameObject> CaptainPrefabs;
        public List<Material> CaptainMaterials;

        [Header("Les matelots")]
        public List<GameObject> SailorPrefabs;
        public List<Material> SailorMaterials;

        public static NpcsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public GameObject InstanciateNPC(NpcType type, GameObject spot, GameObject parent)
        {
            GameObject npcToInstanciate;
            GameObject instance;

            switch (type)
            {
                case NpcType.Captain:
                    npcToInstanciate = CaptainPrefabs[Random.Range(0, CaptainPrefabs.Count)];
                    instance = Instantiate(npcToInstanciate, spot.transform.position, spot.transform.rotation, parent.transform);
                    instance.GetComponentInChildren<SkinnedMeshRenderer>(false).material = CaptainMaterials[Random.Range(0, CaptainMaterials.Count)];
                    instance.transform.localScale = new Vector3(1f / parent.transform.localScale.x, 1f / parent.transform.localScale.y, 1f / parent.transform.localScale.z);
                    return instance;
                case NpcType.Sailor:
                    npcToInstanciate = SailorPrefabs[Random.Range(0, SailorPrefabs.Count)];
                    instance = Instantiate(npcToInstanciate, spot.transform.position, spot.transform.rotation, parent.transform);
                    instance.GetComponentInChildren<SkinnedMeshRenderer>(false).material = SailorMaterials[Random.Range(0, SailorMaterials.Count)];
                    return instance;
                  default:
                    return null;
            }
        }
    }

    public enum NpcType
    {
        Captain,
        Sailor
    }
}