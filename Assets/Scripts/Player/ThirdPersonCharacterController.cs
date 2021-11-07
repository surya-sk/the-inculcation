using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Input;
using CultGame.Saving;
using System;

namespace CultGame.Player
{
    public class ThirdPersonCharacterController : MonoBehaviour, ISaveable
    {
        public float playerSpeed = 0f;
        public float walkSpeed = 4.0f;
        public float runSpeed = 10.0f;
        public float rotationSpeed = 8.0f;
        public AudioSource walkSound;
        public AudioSource runSound;
        public RuntimeAnimatorController defaultAnimController;
        public RuntimeAnimatorController crouchAnimController;

        // bad idea to mess with this
        private float gravityValue = -9.81f;

        private Vector2 inputDirection = Vector2.zero;
        private Vector3 moveAngle, playerVelocity = Vector3.zero;
        private bool isCrouched;
        private bool isRunning;

        private Animator animator;
        private CharacterController controller;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            playerSpeed = walkSpeed;
            isCrouched = false;
            isRunning = false;
        }

        void Update()
        {
            // Set moveAngle to match input directions
            moveAngle = new Vector3(inputDirection.x, 0, inputDirection.y);
            moveAngle = Camera.main.transform.forward * moveAngle.z + Camera.main.transform.right * moveAngle.x;
            moveAngle.y = 0f;

            // Set y velocity and move for gravity (add y velocity in future to add jumping)
            playerVelocity.y += gravityValue;
            controller.Move(playerVelocity * Time.deltaTime);

            if(UnityEngine.Input.GetKeyDown(KeyCode.C) && !isRunning)
            {
                isCrouched = !isCrouched;
            }

            if(UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.JoystickButton8))
            {
                isRunning = true;
            }
            else
            {
                isRunning = false;
            }

            //if(isCrouched)
            //{
            //    animator.SetBool("Sneak", true);
            //    walkSound.volume = 0.06f;
            //}
            //else
            //{
            //    animator.SetBool("Sneak", false);
            //    walkSound.volume = 0.15f;
            //}

            // Move character in direction of moveAngle, multiply by deltaTime for time-dependency, along with playerSpeed
            controller.Move(moveAngle * Time.deltaTime * playerSpeed);

            // If player is moving, calculate the rotation needed to face that direction, then smoothly rotate using lerp
            if (inputDirection != Vector2.zero)
            {
                if(isRunning)
                {
                    animator.SetBool("Run", true);
                    playerSpeed = runSpeed;
                    walkSound.Stop();
                    if (!runSound.isPlaying)
                    {
                        runSound.Play();
                    }
                }
                else
                {
                    animator.SetBool("Run", false);
                    playerSpeed = walkSpeed;
                    runSound.Stop();
                    if (!walkSound.isPlaying)
                    {
                        walkSound.Play();
                    }
                }

                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                isCrouched = false;
                runSound.Stop();
                walkSound.Stop();
            }
        }

        private void OnEnable()
        {
            InputManager.OnPlayerMovementPerformed += OnPlayerMovementPerformed;
            InputManager.OnPlayerMovementCanceled += OnPlayerMovementCanceled;
        }

        private void OnDisable()
        {
            InputManager.OnPlayerMovementPerformed -= OnPlayerMovementPerformed;
            InputManager.OnPlayerMovementCanceled -= OnPlayerMovementCanceled;
        }

        private void OnPlayerMovementPerformed(Vector2 direction)
        {
            inputDirection = direction;
            animator.SetBool("Walk", true);
        }

        private void OnPlayerMovementCanceled()
        {
            inputDirection = Vector2.zero;
            animator.SetBool("Walk", false);
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

