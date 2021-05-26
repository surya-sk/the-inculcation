using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Input;

namespace Player
{
    public class ThirdPersonCharacterController : MonoBehaviour
    {
        [SerializeField]
        private float playerSpeed = 4.0f;

        [SerializeField]
        private float rotationSpeed = 8.0f;

        private float gravityValue = -9.81f;

        private Vector2 inputDirection = Vector2.zero;
        private Vector3 moveAngle, playerVelocity = Vector3.zero;
        private CharacterController controller;

        void Start()
        {
            controller = GetComponent<CharacterController>();
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
        }

        private void OnPlayerMovementCanceled()
        {
            inputDirection = Vector2.zero;
        }
    }
}

