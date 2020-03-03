using Tmiq2.Input;
using UnityEngine;

namespace Tmiq2.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {

        [Tooltip("Sensitivity multiplier for moving the camera around")]
        public float lookSensitivity = 0.7f;

        [Tooltip("Reference to the inpute actions scheme")]
        public MainActions inputActions;

        PlayerCharacterController m_PlayerCharacterController;

        private void Awake()
        {
            inputActions = new MainActions();
        }

        private void OnEnable()
        {
            inputActions.Gameplay.Enable();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_PlayerCharacterController = GetComponent<PlayerCharacterController>();
            DebugUtility.HandleErrorIfNullGetComponent<PlayerCharacterController,
                PlayerInputHandler>(m_PlayerCharacterController, this, gameObject);
           

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            inputActions.Gameplay.Disable();
        }

        public bool CanProcessInput()
        {
            return Cursor.lockState == CursorLockMode.Locked;
        }

        public Vector3 GetMoveInput()
        {
            if (CanProcessInput())
            {
                Vector2 moveInput = inputActions.Gameplay.Move.ReadValue<Vector2>();
                Debug.Log("moveInput" + moveInput);

                var movement = new Vector3()
                {
                    x = moveInput.x,
                    y = 0f,
                    z = moveInput.y
                };
                // constrain move input to a maximum magnitude of 1,
                // otherwise diagonal movement might exceed the max move speed defined
                movement = Vector3.ClampMagnitude(movement, 1);

                return movement;
            }

            return Vector3.zero;
        }

        public bool GetSprintInputHeld()
        {
            //if (CanProcessInput())
            //{
            //    return inputActions.Gameplay.Dash.triggered;
            //}

            return false;
        }

        public float GetLookInputsHorizontal()
        {
            return inputActions.Gameplay.Look.ReadValue<Vector2>().x * lookSensitivity * 0.01f * Time.deltaTime;
        }

        public float GetLookInputsVertical()
        {
            return -1 * inputActions.Gameplay.Look.ReadValue<Vector2>().y * lookSensitivity * 0.01f * Time.deltaTime;
        }

    }
}
