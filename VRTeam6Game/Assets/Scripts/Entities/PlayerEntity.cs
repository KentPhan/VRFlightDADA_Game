using UnityEngine;
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
        public float rotateValue = 500f;

        public ControllerState CurrentControllerState = ControllerState.Keyboard;
        [Header("Keyboard")]
        public float KeyboardRotationSpeed = 100.0f;
        [Header("VR")]
        public float MinimumArmSwitchDistance = 2.0f; // minimum distance required to switch direction and have an effect

        public float MinimumArmFlapDistance = 4.0f;



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
                // Right Arm
                Vector3 rDirection = l_rightPosition - m_PreviousRightPosition;

                float l_LdistanceFromAnchor = (m_AnchorPositionLeft - l_leftPosition).magnitude;
                float l_RdistanceFromAnchor = (m_AnchorPositionRight - l_rightPosition).magnitude;


                Debug.Log(l_leftPosition);

                // Left Arm Threshold Check
                if (Vector3.Dot(lDirection, Vector3.up) > 0)
                {
                    m_AnchorPositionLeft = l_leftPosition;
                }
                else
                {
                    m_LeftThresholdReached = (l_LdistanceFromAnchor > MinimumArmSwitchDistance);
                }

                // Right Arm Threshold Check
                if (Vector3.Dot(rDirection, Vector3.up) > 0)
                {
                    m_AnchorPositionRight = l_rightPosition;
                }
                else
                {
                    m_RightThresholdReached = (l_RdistanceFromAnchor > MinimumArmSwitchDistance);
                }


                //check if both arms have reached threshold
                if (l_LdistanceFromAnchor > MinimumArmFlapDistance || l_RdistanceFromAnchor > MinimumArmFlapDistance)
                {

                    if (m_RightThresholdReached && m_LeftThresholdReached)
                    {
                        ApplyLift();
                        m_LeftThresholdReached = false;
                        m_RightThresholdReached = false;

                        m_AnchorPositionLeft = l_leftPosition;
                        m_AnchorPositionRight = l_rightPosition;
                    }
                    //check if one arm is flapping and that is left
                    else if (m_LeftThresholdReached && !m_RightThresholdReached)
                    {
                        RotateLeft();
                        m_LeftThresholdReached = false;
                        m_AnchorPositionLeft = l_leftPosition;
                    }
                    //check if one arm is flapping and that is right
                    else if (m_RightThresholdReached && !m_LeftThresholdReached)
                    {

                        RotateRight();
                        m_RightThresholdReached = false;
                        m_AnchorPositionRight = l_rightPosition;
                    }

                }


                //if (m_RightThresholdReached && m_LeftThresholdReached)
                //{
                //    ApplyLift();
                //    m_LeftThresholdReached = false;
                //    m_RightThresholdReached = false;
                //    //m_AnchorPositionRight = l_rightPosition;
                //    //m_RightThresholdReached = false;
                //}
                ////check if one arm is flapping and that is left
                //else if (m_LeftThresholdReached && !m_RightThresholdReached)
                //{
                //    RotateLeft();
                //    m_AnchorPositionLeft = l_leftPosition;
                //    m_LeftThresholdReached = false;
                //}
                ////check if one arm is flapping and that is right
                //else if (m_RightThresholdReached && !m_LeftThresholdReached)
                //{

                //    RotateRight();
                //    m_AnchorPositionRight = l_rightPosition;
                //    m_RightThresholdReached = false;
                //}

                //m_LeftThresholdReached = false;
                //m_RightThresholdReached = false;




                // Update Next positions to calculate with
                m_PreviousLeftPosition = l_leftPosition;
                m_PreviousRightPosition = l_rightPosition;

            }
            // Move Forward with constant speed
            this.m_RigidBody.MovePosition(transform.position +
                                              transform.forward * ConstantForwardSpeed * Time.deltaTime);


        }

        public void ApplyLift()
        {
            Debug.Log("Going up");
            this.m_RigidBody.useGravity = true;
            this.m_RigidBody.AddForce(Vector3.up * this.MaxFlapForce, ForceMode.Impulse);
        }

        public void RotateRight()
        {
            Debug.Log("Rotating player Right" + transform.localRotation);
            transform.Rotate(0, rotateValue * Time.deltaTime, 0, Space.World);
        }

        public void RotateLeft()
        {
            Debug.Log("Rotating player left" + transform.localRotation);
            transform.Rotate(0, -1 * rotateValue * Time.deltaTime, 0, Space.World);
        }



    }

}

