using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CultGame.Utils;
using CultGame.Player;
using CultGame.Saving;
using CultGame.Gameplay;

namespace CultGame.Enemy
{
    public class AI : MonoBehaviour, ISaveable
    {
        public Transform player;
        public AudioSource walkSound;
        public AudioSource RunSound;
        public float detectionRadius = 15f;
        public bool hasDetected = false;
        public Transform[] waypointArray;
        public GameOver gameOverRef;
        public bool shouldDetectPlayer = true;
        public float RunSpeed;
        public Transform WatchPoint;
        public ChaseTrigger ChaseTrigger;

        private bool m_ChaseStarted = false;
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
                    player.GetComponent<ThirdPersonCharacterController>().playerSpeed = 2.0f;
                    StartCoroutine(FollowWaypoint());
                    break;
                case "Pray":
                    StartCoroutine(Pray());
                    break;
                case "Relax":
                    StartCoroutine(Relax(random.Next(0,2)));
                    break;
                case "Float":
                    StartCoroutine(Float());
                    break;
                case "Stand":
                    animator.SetTrigger("Stand");
                    break;
                case "Laugh":
                    animator.SetTrigger("Laugh");
                    break;
                case "Dance":
                    animator.SetTrigger("Dance");
                    break;
                case "Piano":
                    animator.SetTrigger("Piano");
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                animator.SetTrigger("Stab");
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

            if(ChaseTrigger != null)
            {
                if(ChaseTrigger.PlayerHasCrossed && !m_ChaseStarted)
                {
                    StopAllCoroutines();
                    StartCoroutine(ChasePlayer());
                }
            }
        }

        /// <summary>
        /// Checks if the player is within detection range 
        /// </summary>
        private void CheckPlayerDetection()
        {
            if(shouldDetectPlayer)
            {
                distanceFromPlayer = Vector3.Distance(player.position, transform.position);
                if (distanceFromPlayer <= detectionRadius)
                {
                    reasonOfDeath = "You were detected".ToUpper();
                    hasDetected = true;
                }
            }
        }

        IEnumerator ChasePlayer()
        {
            m_ChaseStarted = true;
            while(gameObject.activeSelf)
            {
                distanceFromPlayer = Vector3.Distance(player.position, transform.position);
                if (distanceFromPlayer <= detectionRadius)
                {
                    navMeshAgent.speed = RunSpeed;
                    Run(player.position);
                }
                else
                {
                    navMeshAgent.speed = 3.0f;
                    detectionRadius = 10;
                    Vector3 randomLocation = new Vector3(WatchPoint.position.x + GetRandomFloat(), WatchPoint.position.y, WatchPoint.position.z + GetRandomFloat());
                    if(Vector3.Distance(randomLocation, transform.position) < 2.0)
                    {
                        Move(WatchPoint.position);
                    }
                    else
                    {
                        Move(randomLocation);
                    }
                }
                yield return new WaitForSeconds(0.5f);
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

                if(shouldDetectPlayer)
                    CheckPlayerDetection();

                yield return new WaitForSeconds(0.5f);
            }

        }

        /// <summary>
        /// Coroutine to have enemies pray
        /// </summary>
        /// <returns></returns>
        IEnumerator Pray()
        {
            animator.SetTrigger("Pray");
            while (gameObject.activeSelf)
            {
                if(shouldDetectPlayer)
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
                if(shouldDetectPlayer)
                    CheckPlayerDetection();
                yield return new WaitForSeconds(0.5f);
            }
        }

        IEnumerator Float()
        {
            animator.SetTrigger("Float");
            transform.position = new Vector3(transform.position.x, 4, transform.position.z);
            while(gameObject.activeSelf)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                yield return new WaitForSeconds(0.5f);
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
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

        /// <summary>
        /// Move the enemy to the given destination
        /// </summary>
        /// <param name="destination"></param>
        private void Run(Vector3 destination)
        {
            animator.SetTrigger("Run");
            navMeshAgent.SetDestination(destination);
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
        }

        private float GetRandomFloat()
        {
            System.Random random = new System.Random();
            double val = (random.NextDouble() * (20 - 2) + 2);
            return (float)val;
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
