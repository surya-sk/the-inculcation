using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Input;

namespace Player
{
    public class ThirdPersonCharacterController : MonoBehaviour
    {
        public float playerSpeed = 4.0f;
        public float rotationSpeed = 8.0f;

        public AudioSource footstepSound;  

        private float gravityValue = -9.81f;

        private Vector2 inputDirection = Vector2.zero;
        private Vector3 moveAngle, playerVelocity = Vector3.zero;

        private Animator animator;
        private CharacterController controller;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
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

            // Move character in direction of moveAngle, multiply by deltaTime for time-dependency, along with playerSpeed
            controller.Move(moveAngle * Time.deltaTime * playerSpeed);

            // If player is moving, calculate the rotation needed to face that direction, then smoothly rotate using lerp
            if (inputDirection != Vector2.zero)
            {
                if (!footstepSound.isPlaying)
                {
                    footstepSound.Play();
                }
                float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
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
    }
}
