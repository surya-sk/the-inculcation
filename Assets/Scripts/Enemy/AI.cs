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
        public Transform WaitingPoint;
        public GameObject CameraMonitor;
        public SceneLoader SceneLoader;
        public Canvas BlackCanvas;
        public CameraSwitcher CameraSwitcher;
        public AudioSource PianoSound;
        public AudioSource LaughSound;

        private bool m_ChaseStarted = false;
        private bool m_StabPlayer = false;
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
                    player.GetComponent<ThirdPersonCharacterController>().walkSpeed = 2.0f;
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
                    LaughSound.Play();
                    break;
                case "Dance":
                    animator.SetTrigger("Dance");
                    break;
                case "Piano":
                    animator.SetTrigger("Piano");
                    PianoSound.Play();
                    break;
                case "Sit":
                    animator.SetTrigger("Sit");
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                m_StabPlayer = true;
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
            if(m_StabPlayer)
            {
                StartCoroutine(StabPlayer());
            }

            if(hasDetected)
            {
                FacePlayer();
                gameOverRef.EndGame(reasonOfDeath);
            }

            if(ChaseTrigger != null)
            {
                if(ChaseTrigger.PlayerHasCrossed && !m_ChaseStarted)
                {
                    if(gameObject.tag == "Sister")
                    {
                        navMeshAgent.SetDestination(WaitingPoint.position);
                    }
                    else
                    {
                        StopAllCoroutines();
                        StartCoroutine(ChasePlayer());
                    }
                }
            }
        }

        IEnumerator StabPlayer()
        {
            FacePlayer();
            DisablePlayerMovement();
            CameraSwitcher.UpdateCamera(1);
            CameraSwitcher.CanChange = false;
            animator.SetTrigger("Stab");
            BlackCanvas.enabled = true;
            yield return new WaitForSeconds(2f);
            SceneLoader.LoadScene((int)Scenes.GAME_OVER);
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
                    FacePlayer();
                    animator.SetTrigger("Idle");
                    walkSound.Stop();
                    navMeshAgent.isStopped = true;
                    DisablePlayerMovement();
                    hasDetected = true;
                }
            }
        }

        /// <summary>
        /// Looks in the player direction
        /// </summary>
        private void FacePlayer()
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        /// <summary>
        /// Chase the player and stab them if close
        /// </summary>
        /// <returns></returns>
        IEnumerator ChasePlayer()
        {
            m_ChaseStarted = true;
            if(PianoSound != null)
                PianoSound.Stop();
            if(LaughSound != null)
                LaughSound.Stop();
            Vector3 randomLocation = new Vector3(WatchPoint.position.x + GetRandomFloat(), WatchPoint.position.y, WatchPoint.position.z + GetRandomFloat());
            while (gameObject.activeSelf)
            {
                distanceFromPlayer = Vector3.Distance(player.position, transform.position);
                if (distanceFromPlayer <= navMeshAgent.stoppingDistance)
                {
                    reasonOfDeath = "You were caught".ToUpper();
                    StopAllCoroutines();
                    hasDetected = true;
                    DisablePlayerMovement();
                    CameraSwitcher.UpdateCamera(1);
                    FacePlayer();
                    navMeshAgent.speed = 0;
                    animator.SetTrigger("Stand");
                    animator.SetTrigger("Punch");
                    yield return new WaitForSeconds(3.0f);
                    BlackCanvas.enabled = true;
                }
                if (distanceFromPlayer <= detectionRadius)
                {
                    navMeshAgent.speed = RunSpeed;
                    Run(player.position);
                    if(RunSpeed > 16f)
                        RunSpeed -= 0.02f;
                }
                else
                {
                    navMeshAgent.speed = 4.0f;
                    detectionRadius = 20;
                    if(Vector3.Distance(randomLocation, transform.position) < 3.0)
                    {
                        new Vector3(WatchPoint.position.x + GetRandomFloat(), WatchPoint.position.y, WatchPoint.position.z + GetRandomFloat());
                    }
                    Move(randomLocation);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        /// <summary>
        /// Prevents the player from moving
        /// </summary>
        private void DisablePlayerMovement()
        {
            player.GetComponent<ThirdPersonCharacterController>().enabled = false;
            player.GetComponent<FirstPersonCharacterController>().enabled = false;
        }

        /// <summary>
        /// A co-routine to make the enemy follow given waypoints
        /// </summary>
        /// <returns></returns>
        IEnumerator FollowWaypoint()
        {
            Vector3 destination = waypoints.Dequeue().position;
            bool destinationReached = false;
            player.GetComponent<ThirdPersonCharacterController>().playerSpeed = 2.0f;
            player.GetComponent<FirstPersonCharacterController>().walkingSpeed = 2.0f;
            while(!destinationReached)
            {
                Move(destination);
                if (Vector3.Distance(transform.position, destination) < 3.0)
                {
                    try
                    {
                        destination = waypoints.Dequeue().position;
                    }
                    catch
                    {
                        Destroy(gameObject);
                    }
                }

                if (Vector3.Distance(transform.position, lastPoint.position) <= 2.0)
                {
                    animator.SetTrigger("Idle");
                    walkSound.Stop();
                    player.GetComponent<ThirdPersonCharacterController>().playerSpeed = 4.0f;
                    player.GetComponent<FirstPersonCharacterController>().walkingSpeed = 6.0f;
                    destinationReached = true;
                }

                if(shouldDetectPlayer)
                    CheckPlayerDetection();

                yield return new WaitForSeconds(0.5f);
            }
            gameObject.SetActive(false);
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
            RunSound.Stop();
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
            walkSound.Stop();
            if (!RunSound.isPlaying)
            {
                RunSound.Play();
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
