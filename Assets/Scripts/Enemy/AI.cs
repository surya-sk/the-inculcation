using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CultGame.Utils;
using CultGame.Player;
using CultGame.Saving;

namespace CultGame.Enemy
{
    public class AI : MonoBehaviour, ISaveable
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
        readonly System.Random random = new System.Random();


        // Start is called before the first frame update
        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            switch(gameObject.tag)
            {
                case "Follow":
                    InitWaypointQueue();
                    player.GetComponent<ThirdPersonCharacterController>().playerSpeed = 3.0f;
                    StartCoroutine(FollowWaypoint());
                    break;
                case "Pray":
                    StartCoroutine(Pray());
                    break;
                case "Relax":
                    StartCoroutine(Relax(random.Next(3)));
                    break;
            }
        }

        /// <summary>
        /// Adds waypoints to the queue for convinience
        /// </summary>
        private void InitWaypointQueue()
        {
            waypoints = new Queue<Transform>();
            lastPoint = waypointArray[waypointArray.Length - 1];
            foreach (Transform t in waypointArray)
            {
                waypoints.Enqueue(t);
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

        /// <summary>
        /// Checks if the player is within detection range 
        /// </summary>
        private void CheckPlayerDetection()
        {
            distanceFromPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceFromPlayer <= detectionRadius)
            {
                reasonOfDeath = "You were detected".ToUpper();
                hasDetected = true;
            }
        }

        /// <summary>
        /// A co-routine to make the enemy follow given waypoints
        /// </summary>
        /// <returns></returns>
        IEnumerator FollowWaypoint()
        {
            Vector3 destination = waypoints.Dequeue().position;
            bool destinationReached = false;
            while(!destinationReached)
            {
                Move(destination);
                if (Vector3.Distance(transform.position, destination) < 3.0)
                {
                    destination = waypoints.Dequeue().position;
                }

                if (Vector3.Distance(transform.position, lastPoint.position) <= 2.0)
                {
                    animator.SetTrigger("Idle");
                    walkSound.Stop();
                    player.GetComponent<ThirdPersonCharacterController>().playerSpeed = 4.0f;
                    destinationReached = true;
                }

                CheckPlayerDetection();

                yield return new WaitForSeconds(0.5f);
            }

        }

        IEnumerator Pray()
        {
            animator.SetTrigger("Pray");
            while (gameObject.activeSelf)
            {
                CheckPlayerDetection();
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator Relax(int index)
        {
            string[] triggers = { "Relax", "Relax1", "Relax2" };
            animator.SetTrigger(triggers[index]);
            while (gameObject.activeSelf)
            {
                CheckPlayerDetection();
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

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            transform.position = position.ConvertToVector();
        }
    }
}
