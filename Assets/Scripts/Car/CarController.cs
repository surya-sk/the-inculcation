using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CultGame.Saving;

namespace CultGame.Car
{
    public class CarController : MonoBehaviour, ISaveable
    {
        private float m_Horizontal;
        private float m_Vertical;
        private float steerAngle;

        public WheelCollider frontDriverW;
        public WheelCollider frontPassengerW;
        public WheelCollider rearDriverW;
        public WheelCollider rearPassengerW;

        public Transform frontDriverT;
        public Transform frontPassengerT;
        public Transform rearDriverT;
        public Transform rearPassengerT;

        public float maxSteerAngle = 30f;
        public float motorForce = 50f;

        public AudioSource engineRevSound;


        private void FixedUpdate()
        {
            GetInput();
            Steer();
            Accerlerate();
            UpdateWheelPoses();
        }

        private void OnDisable()
        {
            engineRevSound.Stop();
        }

        private void GetInput()
        {
            m_Horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
            m_Vertical = UnityEngine.Input.GetAxisRaw("Vertical");
            if(m_Vertical > 0 && !engineRevSound.isPlaying)
            {
                engineRevSound.Play();
            }
        }

        private void Steer()
        {
            steerAngle = maxSteerAngle * m_Horizontal;
            frontDriverW.steerAngle = steerAngle;
            frontPassengerW.steerAngle = steerAngle;
        }

        private void Accerlerate()
        {
            frontDriverW.motorTorque = m_Vertical * motorForce;
            frontPassengerW.motorTorque = m_Vertical * motorForce;
        }

        private void UpdateWheelPoses()
        {
            UpdateWheelPose(frontDriverW, frontDriverT);
            UpdateWheelPose(frontPassengerW, frontPassengerT);
            UpdateWheelPose(rearDriverW, rearDriverT);
            UpdateWheelPose(rearPassengerW, rearPassengerT);
        }

        private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
        {
            Vector3 position = _transform.position;
            Quaternion rotation = _transform.rotation;

            _collider.GetWorldPose(out position, out rotation);

            _transform.position = position;
            _transform.rotation = rotation;
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
