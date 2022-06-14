using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Front.Paths
{
    public class PathManager : UnityEngine.MonoBehaviour
    {
        public GameObject GlobalPath;
        private List<GameObject> waypoints;

        //public GameObject startWayPoint;
        //public GameObject[] wayPoints;
        private int nextWayPointId;
        private Animator animator;
        private bool walking = true;
        private float walkingSpeed;

        //private GameObject NextWayPoint => waypoints[nextWayPointId];
        //private GameObject NextWayPointAdvanced
        //{
        //    get
        //    {                
        //        if (currentWayPoint.tag.Equals("EndWaypoint"))
        //            currentWayPoint = null;
                
        //        // on va chercher le prochain waypoint :
        //        // le plus proche qui n'est pas le waypoint en cours
        //        return GlobalPath.GetComponentsInChildren<Transform>().Where(x => x.gameObject != currentWayPoint && !x.tag.Equals("RootPath")).Select(x => x.gameObject).
        //           OrderBy(x => Vector3.Distance(transform.position, (x.transform.position))).First();
        //    }
        //}
            
        private string currentWayPoint;
        private GameObject nextWayPoint;

        private void Start()
        {
            //transform.DOMove(transform.position + Vector3.forward, 5);

            walkingSpeed = Random.Range(0.75f, 1.5f);

            animator = GetComponent<Animator>();
            animator.speed = walkingSpeed;
            animator.Play("Walking");

            nextWayPoint = GlobalPath.GetComponentsInChildren<Transform>().Where(x => !x.tag.Equals("RootPath")).Select(x => x.gameObject).
                   OrderBy(x => Vector3.Distance(transform.position, (x.transform.position))).First();

            currentWayPoint = nextWayPoint.transform.ToString();

            //waypoints = GlobalPath.GetComponentsInChildren<Transform>().Where(x => !x.tag.Equals("RootPath")).Select(x => x.gameObject).
            //   OrderBy(x => Vector3.Distance(transform.position, (x.transform.position))).ToList();

        }

        private void FixedUpdate()
        {
            if (walking)
            {
                var distanceToDestination = Vector3.Distance(transform.position, (nextWayPoint.transform.position));
                if (distanceToDestination > 0.1f)
                {
                    //transform.LookAt(NextWayPoint.transform);
                    transform.Translate(Vector3.forward * Time.fixedDeltaTime * walkingSpeed, Space.Self);

                    var targetRotation = Quaternion.LookRotation(nextWayPoint.transform.position - transform.position);
                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2*walkingSpeed * Time.deltaTime);
                }
                else
                {
                    var temp1 = GlobalPath.GetComponentsInChildren<Transform>().Where(x => !x.ToString().Equals(currentWayPoint.ToString()) &&
                      !x.ToString().Equals(nextWayPoint.transform.ToString()) &&
                      !x.tag.Equals("RootPath"));//;.Select(x => x.gameObject);

                     var temp2 = temp1.OrderBy(x => Vector3.Distance(transform.position, (x.transform.position))).First();
                    currentWayPoint = nextWayPoint.transform.ToString();
                    nextWayPoint = temp2.gameObject;

                    //currentWayPoint = waypoints[nextWayPointId];
                    //nextWayPointId++;
                    //currentWayPoint = NextWayPointAdvanced;
                    // si on est au bout des waypoints, on arrête
                    //if (nextWayPointId == waypoints.Count())
                    //if (currentWayPoint.tag.Equals("EndWaypoint"))
                    //{
                    //    animator.SetBool("walking", false);
                    //    walking = false;

                    //    StartCoroutine(WaitForNextWalk());
                    //}
                    //else
                    //    nextWayPoint = NextWayPointAdvanced;
                }
            }
        }

        private IEnumerator WaitForNextWalk()
        {
            yield return new WaitForSeconds(3);

            //waypoints = GlobalPath.GetComponentsInChildren<Transform>().Where(x => !x.tag.Equals("RootPath")).Select(x => x.gameObject).
            //                OrderBy(x => Vector3.Distance(transform.position, (x.transform.position))).ToList();
            //nextWayPointId = 0;

           // currentWayPoint = NextWayPointAdvanced;

            animator.Play("Walking");
            animator.SetBool("walking", true);
            walking = true;
        }
    }
}
