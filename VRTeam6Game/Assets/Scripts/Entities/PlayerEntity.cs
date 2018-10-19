using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.Entities
{
    public enum ArmState
    {
        Up,
        Down,
        None
    }

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

        private ArmState m_CurrentLeftArmState = ArmState.Up;
        private ArmState m_CurrentRightArmState = ArmState.Up;

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
                Vector3 direction = l_leftPosition - m_PreviousLeftPosition;

                // If going up, update left position higher
                if (Vector3.Dot(direction, Vector3.up) > 0)
                {
                    //Debug.Log("Going Up Bitch" + m_AnchorPositionLeft);
                    if (m_LeftThresholdReached)
                    {
                        Debug.Log("WE GOING PLACES");
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
                    Debug.Log("Going Down Bitch" + direction.magnitude);
                    if (direction.magnitude > MinimumArmSwitchDistance)
                    {
                        m_LeftThresholdReached = true;
                    }
                }

                m_PreviousLeftPosition = l_leftPosition;







                //// Right Arm
                //if (m_CurrentRightArmState == ArmState.Up)
                //{

                //}
                //else
                //{

                //}
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyLift();
            }

            this.m_RigidBody.MovePosition(transform.position + transform.forward * ConstantForwardSpeed * Time.deltaTime);
        }

        public void ApplyLift()
        {
            this.m_RigidBody.AddForce(Vector3.up * this.MaxFlapForce, ForceMode.Impulse);
        }


    }

}

