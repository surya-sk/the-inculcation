using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CultGame.Utils;

namespace CultGame.Enemy
{
    public class AI : MonoBehaviour
    {
        public Transform player;
        public AudioSource walkSound;
        public float detectionRadius = 15f;
        public bool hasDetected = false;
        public Transform[] waypointArray;
        public GameOver gameOverRef;

        string reasonOfDeath;
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
            if(hasDetected)
            {
                gameOverRef.EndGame(reasonOfDeath);
            }
        }

        private void FixedUpdate()
        {
            // Check if enemy reached final destination
            if(gameObject.tag == "Follow")
            {
                distanceFromPlayer = Vector3.Distance(player.position, transform.position);
                if (Vector3.Distance(transform.position, lastPoint.position) <= 3.0)
                {
                    animator.SetTrigger("Idle");
                    walkSound.Stop();
                }
                if (distanceFromPlayer <= detectionRadius)
                {
                    reasonOfDeath = "You were detected".ToUpper();
                    hasDetected = true;
                }
            }
        }

        /// <summary>
        /// A co-routine to make the enemy follow given waypoints
        /// </summary>
        /// <returns></returns>
        IEnumerator FollowWaypoint()
        {
            while(waypoints.Count != 0)
            {
                Move(waypoints.Dequeue().position);
                yield return new WaitForSeconds(0.5f);
            }
            
        }

        /// <summary>
        /// Move the enemy to the given destination
        /// </summary>
        /// <param name="destination"></param>
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
