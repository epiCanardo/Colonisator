using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.CombatScene
{
    class PlayerShipManager : MonoBehaviour
    {
        [Header("Canons")]
        public List<GameObject> cannons;

        [Header("Voiles")]
        public List<AudioClip> sailsHostingSounds;
        public GameObject mainSails1;
        public GameObject mainSails2;

        public AudioClip fireOrder;

        private AudioSource audioSource;
        private LineRenderer line;

        private ShipSails actualShipSpeed = ShipSails.Stop;
        private ShipMotion actualShipMotion = ShipMotion.None;

        private float speed = 0f;
        private float maxSpeed = .1f;
        private float acceleration = .02f;
        private float slowingrate = -.02f;
        private float turnspeed = 2f;

        private Mesh mesh;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();

            mainSails1.transform.localScale = new Vector3(1, 0, 1);
            mainSails2.transform.localScale = new Vector3(1, 0, 1);

            line = GetComponent<LineRenderer>();
            line.positionCount = 2;

            mesh = new Mesh();
            //GetComponent<MeshFilter>().mesh = mesh;

            mesh.vertices = new Vector3[]
{
                cannons.First().transform.position,
                cannons.First().GetComponent<CannonManager>().maxScope.transform.position,
                cannons.Last().transform.position,
                cannons.Last().GetComponent<CannonManager>().maxScope.transform.position
};
            mesh.triangles = new int[] {
                    0, 1, 2,
                    2, 1, 3
                };

            mesh.RecalculateNormals();
        }
        private void OnGUI()
        {
            if (Input.GetKey(KeyCode.LeftControl))
                DrawText();
        }

        private void DrawCircle(LineRenderer line, float radius, float lineWidth)
        {
            var segments = 360;
            line.useWorldSpace = false;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.positionCount = segments + 1;

            var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
            var points = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            line.SetPositions(points);
        }

        private void DrawText()
        {
            var pos = Camera.main.WorldToScreenPoint((transform.position + CombatManager.Instance.ennemyShip.transform.position) / 2);
            string text = $"Dictance : { Vector3.Distance(CombatManager.Instance.ennemyShip.transform.position, transform.position).ToString()} m";
            var textSize = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(pos.x, Screen.height - pos.y, textSize.x, textSize.y), text);
        }

        private void Update()
        {
            //var leftCannonLine = cannons.First().GetComponent<LineRenderer>();
            //var rightCannonLine = cannons.Last().GetComponent<LineRenderer>();

            if (Input.GetKey(KeyCode.LeftControl))
            {
                // affichage de la distance entre les deux navires
                //line.useWorldSpace = false;
                line.positionCount = 2;
                line.SetPosition(0, transform.position);
                line.SetPosition(1, CombatManager.Instance.ennemyShip.transform.position);

                //// affichage de la portée des canons
                //// canon le plus à gauche                
                //leftCannonLine.positionCount = 2;
                //leftCannonLine.SetPosition(0, cannons.First().transform.position);
                //leftCannonLine.SetPosition(1, cannons.First().GetComponent<CannonManager>().maxScope.transform.position);

                //// canon le plus à droite                
                //rightCannonLine.positionCount = 2;
                //rightCannonLine.SetPosition(0, cannons.Last().transform.position);
                //rightCannonLine.SetPosition(1, cannons.Last().GetComponent<CannonManager>().maxScope.transform.position);

                // dessin à l'intérieur du rectangle dessiné par les deux lines
                //mesh.RecalculateNormals();

                GetComponent<MeshFilter>().mesh = mesh;
            }

            else
            {
                line.positionCount = 0;
                //leftCannonLine.positionCount = 0;
                //rightCannonLine.positionCount = 0;
                GetComponent<MeshFilter>().mesh = null;
            }

            //textZone.transform.rotation = Quaternion.identity;
            //healthBar.gameObject.transform.rotation = Quaternion.identity;
            //text.transform.position = placeHolder.transform.position; //Camera.main.WorldToScreenPoint(transform.position);

            // On envoie une bordée sur le bord actif !
            if (Input.GetKeyDown(KeyCode.Space))
            {                
                StartCoroutine(FireBattery(0f));                

                //// une deuxième allez ^^
                //foreach (var cannon in cannons)
                //{
                //    float delay = (float)rnd.NextDouble();
                //    StartCoroutine(BeginFire(delay, cannon));
                //}

                //// une deuxième allez ^^
                //foreach (var cannon in cannons)
                //{
                //    float delay = (float)rnd.NextDouble();
                //    StartCoroutine(BeginFire(delay, cannon));
                //}
            }

            // On hisse les voiles pour avancer
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                switch (actualShipSpeed)
                {
                    case ShipSails.Stop:
                        audioSource.PlayOneShot(sailsHostingSounds[CombatManager.Instance.NextInt(0, 2)]);
                        actualShipSpeed = ShipSails.Half;
                        actualShipMotion = ShipMotion.Accelerate;
                        break;
                    case ShipSails.Half:                        
                        break;
                    case ShipSails.Full:
                        break;
                    default:
                        break;
                }
            }

            // On baisse les voiles pour ralentir
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                switch (actualShipSpeed)
                {
                    case ShipSails.Stop:
                        break;
                    case ShipSails.Half:
                        audioSource.PlayOneShot(sailsHostingSounds[CombatManager.Instance.NextInt(0,2)]);
                        actualShipSpeed = ShipSails.Stop;
                        actualShipMotion = ShipMotion.Slow;
                        break;
                    case ShipSails.Full:
                        break;
                    default:
                        break;
                }
            }

            // Gauche => rotation antihoraire
            if (speed > 0 && Input.GetKey(KeyCode.LeftArrow))
            {
                transform.rotation = Quaternion.AngleAxis(-turnspeed * Time.deltaTime, Vector3.up) * transform.rotation;
                Debug.Log($"Rotation : {transform.localRotation.y}");
            }

            // Droite => rotation horaire
            if (speed > 0 && Input.GetKey(KeyCode.RightArrow))
            {
                transform.rotation = Quaternion.AngleAxis(turnspeed * Time.deltaTime, Vector3.up) * transform.rotation;
                Debug.Log($"Rotation : {transform.localRotation.y}");
            }

            // Gestion de la voilure
            switch (actualShipSpeed)
            {
                case ShipSails.Stop:
                    mainSails1.transform.localScale = new Vector3(1, 0, 1);
                    mainSails2.transform.localScale = new Vector3(1, 0, 1);
                    break;
                case ShipSails.Half:
                    mainSails1.transform.localScale = new Vector3(1, 1, 1);
                    mainSails2.transform.localScale = new Vector3(1, 1, 1);
                    break;
                case ShipSails.Full:
                    break;
                default:
                    break;
            }

            // Gestion de la vitesse
            switch (actualShipMotion)
            {
                case ShipMotion.None:
                    break;
                case ShipMotion.Accelerate:
                    speed = Math.Min(maxSpeed, speed + (acceleration * Time.deltaTime));
                    if (speed >= maxSpeed - acceleration) { speed = maxSpeed; actualShipMotion = ShipMotion.None; }
                    break;
                case ShipMotion.Slow:
                    speed = Math.Max(0, speed + (slowingrate * Time.deltaTime));
                    if (speed <= acceleration) { speed = 0f; actualShipMotion = ShipMotion.None; }
                    break;
                default:
                    break;
            }
            transform.Translate(Vector3.forward * speed, Space.Self);

            //transform.RotateAround(ennmyShip.transform.position, Vector3.down, 5 * Time.deltaTime);
        }

        IEnumerator FireBattery(float delay)
        {
            yield return new WaitForSeconds(delay);
            foreach (var cannon in cannons)
            {
                float uniTdelay = (float)CombatManager.Instance.NextDouble * 2;
                StartCoroutine(BeginCannon(uniTdelay, cannon));
            }
        }

        IEnumerator BeginCannon(float delay, GameObject cannon)
        {
            yield return new WaitForSeconds(delay);
            audioSource.PlayOneShot(CombatManager.Instance.fireSounds[6]);
            cannon.GetComponent<CannonManager>().FireBall();
        }
    }

    enum ShipSails
    {
        Stop,
        Half,
        Full
    }

    enum ShipMotion
    {
        None,
        Accelerate,
        Slow
    }
}
