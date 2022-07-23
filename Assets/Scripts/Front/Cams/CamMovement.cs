using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.ModsDTO;
using UnityEngine;

namespace Assets.Scripts.Front.Cams
{
    public class CamMovement : UnityEngine.MonoBehaviour
    {
        [SerializeField] float camspeed = 1500;
        [SerializeField] float zoomSpeed = 10;

        private int defaultCullingMask;
        bool hasZoomChanged;
        private int zoomLevel = 0;

        // Start is called before the first frame update
        void Start()
        {
            defaultCullingMask = Camera.main.cullingMask;
        }

        void Update()
        {
            HandleMovement();
            HandleZoom();
        }

        private void HandleMovement()
        {
            var hMovement = Input.GetAxis("Horizontal");
            var vMovement = Input.GetAxis("Vertical");
            bool isMouseButtonPressed = Input.GetMouseButton(2);            

            if (isMouseButtonPressed)
            {
                hMovement = ModManager.Instance.GetMouseAxis() * MouseSensitivityFactor * Input.GetAxis("Mouse X");
                vMovement = ModManager.Instance.GetMouseAxis() * MouseSensitivityFactor * Input.GetAxis("Mouse Y");
            }

            bool movement = hMovement != 0 || vMovement != 0;
            if (movement)
                transform.Translate(hMovement * Time.deltaTime * camspeed, 0, vMovement * Time.deltaTime * camspeed, Space.World);
        }

        private void HandleZoom()
        {
            float wheel = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetKeyDown(KeyCode.PageDown) || wheel < 0)
            {
                zoomLevel--;
                zoomLevel = Mathf.Clamp(zoomLevel, -3, 3);
                hasZoomChanged = true;
            }
            else if (Input.GetKeyDown(KeyCode.PageUp) || wheel > 0)
            {
                zoomLevel++;
                zoomLevel = Mathf.Clamp(zoomLevel, -3, 3);
                hasZoomChanged = true;
            }

            if (!hasZoomChanged)
                return;

            if (zoomLevel < 0)
                SetCamToMapLevel();
            else
                SetCamToActionLevel();
        }

        /// <summary>
        /// calcul de l'ajustement de la sensibilité de la souris, selon le zoom
        /// si en vue map : sensibilité * 2
        /// </summary>
        private float MouseSensitivityFactor => (zoomLevel < 0)
                ? ModManager.Instance.GetMouseSensibility() * 2
                : ModManager.Instance.GetMouseSensibility();

        private void SetCamTransform(float positionyAxis, float rotationxAxis)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, positionyAxis, transform.position.z);
            Vector3 targetRotation = new Vector3(rotationxAxis, 0, 0);

            transform.position = targetPosition;
            transform.eulerAngles = targetRotation;
        }

        public void SetCamToActionLevel()
        {
            if (zoomLevel < 0) zoomLevel = 0;
            SetCamTransform(300 - zoomLevel * 50, 70 - 10 * zoomLevel);
            Camera.main.cullingMask = defaultCullingMask;
            GameManager.Instance.ToggleSquares(false);
            hasZoomChanged = false;
        }

        public void SetCamToMapLevel()
        {
            if (zoomLevel > -1) zoomLevel = -1;
            SetCamTransform(700 - zoomLevel * 400, 90);
            Camera.main.cullingMask = LayerMask.GetMask("Minimap");
            GameManager.Instance.ToggleSquares(true);
            hasZoomChanged = false;
        }
    }
}