using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Saving;

namespace CultGame.Car
{
    public class CarController : MonoBehaviour, ISaveable
    {
        public WheelCollider WheelFL;
        public WheelCollider WheelFR;
        public WheelCollider WheelRL;
        public WheelCollider WheelRR;
        public float MaxTorque = 500f;
        public float BrakeTorque = 1000f;
        // max wheel turn angle;
        public float MaxWheelTurnAngle = 30f; // degrees
        public Vector3 CenterOfMass = new Vector3(0f, 0f, 0f); // unchanged
        public Vector3 EulerTest;
        // acceleration increment counter
        private float m_TorquePower = 0f;
        // turn increment counter
        private float m_SteerAngle = 30f;

        void Start()
        {
            GetComponent<Rigidbody>().centerOfMass = CenterOfMass;
        }

        // Physics updates
        void FixedUpdate()
        {
            // CONTROLS - FORWARD & RearWARD
            if (UnityEngine.Input.GetKey(KeyCode.B) || UnityEngine.Input.GetKey(KeyCode.Joystick1Button1))
            {
                // BRAKE
                m_TorquePower = 0f;
                WheelRL.brakeTorque = BrakeTorque;
                WheelRR.brakeTorque = BrakeTorque;
            }
            else
            {
                // SPEED
                m_TorquePower = MaxTorque * Mathf.Clamp(UnityEngine.Input.GetAxis("Vertical"), -1, 1);
                WheelRL.brakeTorque = 0f;
                WheelRR.brakeTorque = 0f;
            }
            // Apply torque
            WheelRR.motorTorque = m_TorquePower;
            WheelRL.motorTorque = m_TorquePower;
            // CONTROLS - LEFT & RIGHT
            // apply steering to front wheels
            m_SteerAngle = MaxWheelTurnAngle * UnityEngine.Input.GetAxis("Horizontal");
            WheelFL.steerAngle = m_SteerAngle;
            WheelFR.steerAngle = m_SteerAngle;
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
