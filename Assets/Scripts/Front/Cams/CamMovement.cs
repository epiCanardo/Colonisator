using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.Terrain;
using Assets.Scripts.ModsDTO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Front.Cams
{
    public class CamMovement : UnityEngine.MonoBehaviour
    {
        [SerializeField] float camspeed = 1500;
        [SerializeField] float zommspeed = 20;

        public UnityEngine.Terrain sampleTerrain;
        public GameObject mapPointsParent;

        private int defaultCullingMask;
        bool hasZoomChanged;
        public int zoomLevel = 0;

        // Start is called before the first frame update
        void Start()
        {
            LineRenderer line = GetComponent<LineRenderer>();
            Vector3[] aboveSeaLevelPoints = new Vector3[100000];
            int nbPoints = 0;
            defaultCullingMask = Camera.main.cullingMask;
            var test = sampleTerrain.GetComponent<TerrainManager>().GetGroundCoordinates();
            for (int x = 0; x < sampleTerrain.terrainData.heightmapResolution; x += 3)
            {
                for (int y = 0; y < sampleTerrain.terrainData.heightmapResolution; y += 3)
                {
                    if (test[x, y] > 0)
                    {
                        //aboveSeaLevelPoints[nbPoints] = new Vector3(x, 0, y);
                        //nbPoints++;
                        var sphere = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        sphere.transform.localScale /= 10;
                        var instance = Instantiate(sphere, new Vector3(x - 2509.5f, 0, y + 5816.6f), Quaternion.identity, mapPointsParent.transform);

                        if (test[x, y] > 0.04)
                            instance.GetComponent<MeshRenderer>().material.color = Color.black;
                        else if (test[x, y] > 0.03)
                            instance.GetComponent<MeshRenderer>().material.color = Color.green;
                        else if (test[x, y] > 0.02)
                            instance.GetComponent<MeshRenderer>().material.color = Color.yellow;
                        else if (test[x, y] > 0.01)
                            instance.GetComponent<MeshRenderer>().material.color = Color.grey;
                        else if (test[x, y] > 0)
                            instance.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                }
            }

            //Vector3[] finalArray = new Vector3[nbPoints];
            //int count = 0;
            //for (int i = 0; i < aboveSeaLevelPoints.Length; i++)
            //{
            //    if (aboveSeaLevelPoints[i].sqrMagnitude > 0)
            //    {
            //        //finalArray[count] = aboveSeaLevelPoints[i];
            //        //count++;
            //        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), aboveSeaLevelPoints[i], Quaternion.identity);
            //    }
            //}

            //line.positionCount = nbPoints;
            //line.SetPositions(aboveSeaLevelPoints);
        }

        void Update()
        {
            var hMovement = Input.GetAxis("Horizontal");
            var vMovement = Input.GetAxis("Vertical");
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            bool isMouseButtonPressed = Input.GetMouseButton(2);

            if (isMouseButtonPressed)
            {                
                hMovement = ModManager.Instance.GetMouseAxis() * ModManager.Instance.GetMouseSensibility() * Input.GetAxis("Mouse X");
                vMovement = ModManager.Instance.GetMouseAxis() * ModManager.Instance.GetMouseSensibility() * Input.GetAxis("Mouse Y");
            }

            if (Input.GetKeyDown(KeyCode.PageDown) || wheel < 0)
            {
                zoomLevel -= 1;
                zoomLevel = Mathf.Clamp(zoomLevel, -3, 3);
                hasZoomChanged = true;
            }
            else if (Input.GetKeyDown(KeyCode.PageUp) || wheel > 0)
            {
                zoomLevel += 1;
                zoomLevel = Mathf.Clamp(zoomLevel, -3, 3);
                hasZoomChanged = true;
            }

            transform.Translate(hMovement * Time.deltaTime * camspeed, 0, vMovement * Time.deltaTime * camspeed, Space.World);

            if (!hasZoomChanged)
                return;

            if (zoomLevel < 0)
                SetCamToMapLevel();
            else
                SetCamToActionLevel();            
            
        }
        private void SetCamTransform()
        {
            transform.position = new Vector3(transform.position.x, 300 - zoomLevel * 50, transform.position.z);
            transform.eulerAngles = new Vector3(70 - 10 * zoomLevel, 0, 0);
        }

        public void SetCamToActionLevel()
        {
            if (zoomLevel < 0) zoomLevel = 0;
            SetCamTransform();
            Camera.main.cullingMask = defaultCullingMask;
            GameManager.Instance.ToggleSquares(false);
            hasZoomChanged = false;
        }

        public void SetCamToMapLevel()
        {
            if (zoomLevel > -1) zoomLevel = -1;
            transform.position = new Vector3(transform.position.x, 700 - zoomLevel * 400, transform.position.z);
            transform.eulerAngles = new Vector3(90, 0, 0);
            Camera.main.cullingMask = LayerMask.GetMask("Minimap");
            GameManager.Instance.ToggleSquares(true);
            hasZoomChanged = false;
        }
    }
}