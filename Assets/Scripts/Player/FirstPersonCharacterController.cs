using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CultGame.Player
{
    public class FirstPersonCharacterController : MonoBehaviour
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;
        public AudioSource walkSound;
        public AudioSource runSound;

        CharacterController characterController;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        [HideInInspector]
        public bool canMove = true;

        void Start()
        {
            characterController = GetComponent<CharacterController>();

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            // Press Left Shift to run
            bool isRunning = UnityEngine.Input.GetKey(KeyCode.LeftShift) || UnityEngine.Input.GetKey(KeyCode.JoystickButton8);
            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * UnityEngine.Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * UnityEngine.Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);
           moveDirection.y = movementDirectionY;

            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            if (moveDirection.z != 0)
            {
                if (isRunning)
                {
                    walkSound.Stop();
                    if (!runSound.isPlaying)
                    {
                        runSound.Play();
                    }
                }
                else
                {
                    runSound.Stop();
                    if (!walkSound.isPlaying)
                    {
                        walkSound.Play();
                    }
                }
            }
            else
            {
                walkSound.Stop();
                runSound.Stop();
            }

                // Player and Camera rotation
                if (canMove)
            {
                rotationX += -UnityEngine.Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, UnityEngine.Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
    }
}
