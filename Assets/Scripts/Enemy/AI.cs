using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CultGame.Enemy
{
    public class AI : MonoBehaviour
    {
        public Transform player;
        public AudioSource walkSound;
        public float detectionRadius = 10f;
        public bool hasDetected = false;
        public Transform[] waypointArray;
        Queue<Transform> waypoints;
        Animator animator;
        NavMeshAgent navMeshAgent;
        float distanceFromPlayer;
        Transform lastPoint;

        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            if(gameObject.tag == "Follow")
            {
                waypoints = new Queue<Transform>();
                lastPoint = waypointArray[waypointArray.Length - 1];
                foreach (Transform t in waypointArray)
                {
                    waypoints.Enqueue(t);
                }
                StartCoroutine(FollowWaypoint());
            }
        }

        // Update is called once per frame
        void Update()
        {
            //distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            //if(!hasDetected)
            //{
            //    if(distanceFromPlayer <= detectionRadius)
            //    {
            //        hasDetected = true;
            //    }
            //}
        }

        private void FixedUpdate()
        {
            if(gameObject.tag == "Follow")
            {
                if(Vector3.Distance(transform.position, lastPoint.position) <= 3.0)
                {
                    animator.SetTrigger("Idle");
                    walkSound.Stop();
                }
            }
        }

        IEnumerator FollowWaypoint()
        {
            while(waypoints.Count != 0)
            {
                Move(waypoints.Dequeue().position);
                yield return new WaitForSeconds(0.5f);

                //if(distanceFromPlayer <= detectionRadius)
                //{
                //    hasDetected = true;
                //}
            }
            
        }

        private void Move(Vector3 destination)
        {
            animator.SetTrigger("Move");
            navMeshAgent.SetDestination(destination);
            if(!walkSound.isPlaying)
            {
                walkSound.Play();
            }
        }
    }
}
