﻿using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.Entities
{
    public enum ControllerState
    {
        Keyboard,
        VR
    }

    /// <summary>
    /// Player Entity
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerEntity : MonoBehaviour
    {
        public float MaxFlapForce = 10.0f;
        public float ConstantForwardSpeed = 10.0f;


        public ControllerState CurrentControllerState = ControllerState.Keyboard;
        [Header("Keyboard")]
        public float KeyboardRotationSpeed = 100.0f;
        [Header("VR")]
        public float MinimumArmSwitchDistance = 2.0f; // minimum distance required to switch direction and have an effect



        // Arm Detection
        private Vector3 m_AnchorPositionLeft;
        private Vector3 m_AnchorPositionRight;
        private Vector3 m_PreviousLeftPosition;
        private Vector3 m_PreviousRightPosition;



        private bool m_LeftThresholdReached = false;
        private bool m_RightThresholdReached = false;


        // Controller State
        private Rigidbody m_RigidBody;

        // Use this for initialization
        void Start()
        {
            this.m_RigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {



            // Doesn't work right now
            if (this.CurrentControllerState == ControllerState.Keyboard)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    transform.Rotate(Vector3.up, KeyboardRotationSpeed * Time.deltaTime, Space.World);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    transform.Rotate(Vector3.up, -1 * KeyboardRotationSpeed * Time.deltaTime, Space.World);
                }

                if (Input.GetButtonDown("Flap"))
                {
                    this.m_RigidBody.AddForce(Vector3.up * this.MaxFlapForce, ForceMode.Impulse);

                }
            }
            else
            {

                Vector3 l_leftPosition = InputTracking.GetLocalPosition((XRNode.LeftHand));
                Vector3 l_rightPosition = InputTracking.GetLocalPosition((XRNode.RightHand));


                if (m_AnchorPositionLeft == null)
                {
                    m_AnchorPositionLeft = l_leftPosition;
                    m_PreviousLeftPosition = l_leftPosition;
                }

                if (m_AnchorPositionRight == null)
                {
                    m_AnchorPositionRight = l_rightPosition;
                    m_PreviousRightPosition = l_rightPosition;
                }


                // Left Arm
                Vector3 lDirection = l_leftPosition - m_PreviousLeftPosition;


                // If going up, update left position higher
                if (Vector3.Dot(lDirection, Vector3.up) > 0)
                {

                    if (m_LeftThresholdReached)
                    {
                        ApplyLift();
                        m_AnchorPositionLeft = l_leftPosition;
                        m_LeftThresholdReached = false;
                    }
                    else
                    {
                        m_AnchorPositionLeft = l_leftPosition;
                    }
                }
                // If going down
                else
                {
                    //Debug.Log("Going Down Bitch" + direction.magnitude);
                    Vector3 distanceFromAnchor = m_AnchorPositionLeft - l_leftPosition;
                    Debug.Log(m_AnchorPositionLeft + " " + l_leftPosition + " Distance From Anchor" + distanceFromAnchor.magnitude);
                    if (distanceFromAnchor.magnitude > MinimumArmSwitchDistance)
                    {
                        m_LeftThresholdReached = true;
                    }
                }
                m_PreviousLeftPosition = l_leftPosition;



                // Right Arm
                Vector3 rDirection = l_leftPosition - m_PreviousRightPosition;
                if (Vector3.Dot(rDirection, Vector3.up) > 0)
                {

                    if (m_RightThresholdReached)
                    {
                        ApplyLift();
                        m_AnchorPositionRight = l_rightPosition;
                        m_RightThresholdReached = false;
                    }
                    else
                    {
                        m_AnchorPositionRight = l_rightPosition;
                    }
                }
                // If going down
                else
                {
                    Vector3 distanceFromAnchor = m_AnchorPositionRight - l_rightPosition;
                    Debug.Log(m_AnchorPositionRight + " " + l_rightPosition + " Distance From Anchor" + distanceFromAnchor.magnitude);
                    if (distanceFromAnchor.magnitude > MinimumArmSwitchDistance)
                    {
                        m_RightThresholdReached = true;
                    }
                }
                m_PreviousRightPosition = l_rightPosition;

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyLift();
            }

            this.m_RigidBody.MovePosition(transform.position + transform.forward * ConstantForwardSpeed * Time.deltaTime);
        }

<<<<<<< HEAD
       
=======
        public void ApplyLift()
        {
            this.m_RigidBody.AddForce(Vector3.up * this.MaxFlapForce, ForceMode.Impulse);
        }
>>>>>>> f360900e4fab9ac112c6ca06d5ca91b715809096


    }

}

