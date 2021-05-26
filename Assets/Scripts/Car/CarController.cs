using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car
{
    public class CarController : MonoBehaviour
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


        private void FixedUpdate()
        {
            GetInput();
            Steer();
            Accerlerate();
            UpdateWheelPoses();
        }
        private void GetInput()
        {
            m_Horizontal = Input.GetAxisRaw("Horizontal");
            m_Vertical = Input.GetAxisRaw("Vertical");
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
    }
}
