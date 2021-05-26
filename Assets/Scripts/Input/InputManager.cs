using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public delegate void PlayerMovementPerformed(Vector2 direction);
        public static event PlayerMovementPerformed OnPlayerMovementPerformed;

        public delegate void PlayerMovementCanceled();
        public static event PlayerMovementCanceled OnPlayerMovementCanceled;

        private InputMaster inputMaster;
        private void Awake()
        {
            inputMaster = new InputMaster();
            inputMaster.Player.Movement.performed += context => { if (OnPlayerMovementPerformed != null) OnPlayerMovementPerformed(context.ReadValue<Vector2>()); };
            inputMaster.Player.Movement.canceled += context => { if (OnPlayerMovementCanceled != null) OnPlayerMovementCanceled(); };
        }

        private void OnEnable()
        {
            inputMaster.Enable();
        }

        private void OnDisable()
        {
            inputMaster.Disable();
        }
    }
}
