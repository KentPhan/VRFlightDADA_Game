using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.Entities
{
    public enum ControllerState
    {
        VR_Flap_Turn,
        VR_Head_Turn
    }

    public enum PlayerState
    {
        IN_UI,
        GAMEPLAY
    }

    /// <summary>
    /// Player Entity
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerEntity : MonoBehaviour
    {


        public PlayerState CurrentState = PlayerState.IN_UI;
        [Header("Arm Flapping measurements of input detection")]
        public float MaxFlapForce = 10.0f;
        public float MinimumArmSwitchDistance = 2.0f; // minimum distance required to switch direction and have an effect
        public float MinimumArmFlapDistance = 4.0f;

        public ControllerState CurrentControllerState = ControllerState.VR_Flap_Turn;
        [Header("VR Flap Turn")]

        public float MaxAngularFlapForce = 10.0f;
        public float RotationalDrag = 0.9f;
        public float MinimumDragForce = 1.0f;


        private Vector3 m_rotationalAcceleration;
        private Vector3 m_rotationalVelocity;
        private float MawShittyAngle;



        [Header("VR Head Turn")]
        public float MinimumForwardSpeed = 1.0f;
        [Range(0.0f, 1.0f)]
        public float TriggerThreshold = 0.3f;
        public float MaxAcceleration = 20.0f;
        public float MaxSpeed = 20.0f;
        public float MaxParticleCount = 100.0f;

        // Rotation Detection
        private Quaternion m_RotationalDestination;


        // Arm Detection
        private Vector3 m_AnchorPositionLeft;
        private Vector3 m_AnchorPositionRight;
        private Vector3 m_PreviousLeftPosition;
        private Vector3 m_PreviousRightPosition;
        private bool m_LeftThresholdReached = false;
        private bool m_RightThresholdReached = false;

        // Components
        private Rigidbody m_RigidBody;
        private Camera m_Camera;
        private ParticleSystem m_Particles;

        //Test Variables - Hitesh
        public bool startMovement = false;
        private float ConstantForwardSpeed = 10f;


        public void OnCollisionEnter()
        {
            this.m_RigidBody.angularVelocity = Vector3.zero;
        }

        public void OnCollisionStay()
        {
            this.m_RigidBody.angularVelocity = Vector3.zero;
        }

        // Use this for initialization
        void Start()
        {
            this.m_RigidBody = GetComponent<Rigidbody>();
            this.m_Camera = GetComponentInChildren<Camera>();
            this.m_Particles = GetComponentInChildren<ParticleSystem>();

            if (this.CurrentState == PlayerState.IN_UI)
            {
                SwitchToUIState();
            }
            else
            {
                SwitchToGamePlayState();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.CurrentState == PlayerState.IN_UI)
            {


                //Used for checking collision -Hitesh
                if (startMovement)
                    this.m_RigidBody.MovePosition(transform.position + m_Camera.transform.forward * ConstantForwardSpeed * Time.deltaTime);

                //Used for checking collision - Hitesh
                //this.m_RigidBody.MovePosition(transform.position +
                //                                 m_Camera.transform.forward * ConstantForwardSpeed * Time.deltaTime);


                return;
            }

            DetectSwing();
            DetectAcceleration();


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ApplyLift();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ApplyRotateRightAcceleration();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ApplyRotateLeftAcceleration();
            }

            DetectRotations();

            // limit max speed
            this.m_RigidBody.velocity = Vector3.ClampMagnitude(m_RigidBody.velocity, MaxSpeed);

            // Particle effects emmit relative to velocity
            ParticleSystem.EmissionModule module = this.m_Particles.emission;
            module.rateOverTime = MaxParticleCount * (this.m_RigidBody.velocity.magnitude / MaxSpeed);
        }

        public void DetectSwing()
        {
            // Swing Detection ------------------------------------
            Vector3 l_leftPosition = InputTracking.GetLocalPosition((XRNode.LeftHand));
            Vector3 l_rightPosition = InputTracking.GetLocalPosition((XRNode.RightHand));

            // Get direction of controllers moving
            Vector3 lDirection = l_leftPosition - m_PreviousLeftPosition;
            Vector3 rDirection = l_rightPosition - m_PreviousRightPosition;

            // Get distance from anchor
            float l_LdistanceFromAnchor = (m_AnchorPositionLeft - l_leftPosition).magnitude;
            float l_RdistanceFromAnchor = (m_AnchorPositionRight - l_rightPosition).magnitude;

            // Left Controller. If moving up: update anchor, If moving down: check it breaks the switch direction threshold
            if (Vector3.Dot(lDirection, Vector3.up) > 0)
                m_AnchorPositionLeft = l_leftPosition;
            else
                m_LeftThresholdReached = (l_LdistanceFromAnchor > MinimumArmSwitchDistance);

            // Right Controller. If moving up: update anchor, If moving down: check it breaks the switch direction threshold
            if (Vector3.Dot(rDirection, Vector3.up) > 0)
                m_AnchorPositionRight = l_rightPosition;
            else
                m_RightThresholdReached = (l_RdistanceFromAnchor > MinimumArmSwitchDistance);


            // If either arm 
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
                    if (this.CurrentControllerState == ControllerState.VR_Flap_Turn)
                        ApplyRotateLeftAcceleration();
                    m_LeftThresholdReached = false;
                    m_AnchorPositionLeft = l_leftPosition;
                }
                //check if one arm is flapping and that is right
                else if (m_RightThresholdReached && !m_LeftThresholdReached)
                {
                    if (this.CurrentControllerState == ControllerState.VR_Flap_Turn)
                        ApplyRotateRightAcceleration();
                    m_RightThresholdReached = false;
                    m_AnchorPositionRight = l_rightPosition;
                }

            }

            // Update Next positions to calculate with
            m_PreviousLeftPosition = l_leftPosition;
            m_PreviousRightPosition = l_rightPosition;
        }


        public void DetectAcceleration()
        {
            float l_leftTriggerAxis = Input.GetAxis("CONTROLLER_LEFT_TRIGGER");
            float l_rightTriggerAxis = Input.GetAxis("CONTROLLER_RIGHT_TRIGGER");


            // If triggers are pressed, accelerate relative to max acceleration and how depressed the triggers are
            if (l_leftTriggerAxis >= TriggerThreshold && l_rightTriggerAxis >= TriggerThreshold)
            {
                float acceleration = MaxAcceleration * ((l_leftTriggerAxis + l_rightTriggerAxis) / 2.0f);

                // Accelerate in boat direction
                if (CurrentControllerState == ControllerState.VR_Flap_Turn)
                {
                    this.m_RigidBody.AddForce(new Vector3(this.m_RigidBody.transform.forward.x, 0.0f, this.m_RigidBody.transform.forward.z) // Direction on x,z plane
                                              * acceleration // Speed
                                              * Time.deltaTime, ForceMode.Acceleration);
                }

                // Accelerate in head direction
                else if (CurrentControllerState == ControllerState.VR_Head_Turn)
                {
                    this.m_RigidBody.AddForce(new Vector3(m_Camera.transform.forward.x, 0.0f, m_Camera.transform.forward.z) // Direction on x,z plane
                                              * acceleration // Speed
                                              * Time.deltaTime, ForceMode.Acceleration);
                }


            }
            // If triggers are not pressed, set minimum velocity on x and z plane forward if not going too fast already
            else
            {
                // If going slow then minimum speed
                if (new Vector3(m_RigidBody.velocity.x, 0.0f, m_RigidBody.velocity.z).magnitude < MinimumForwardSpeed)
                {

                    // Minimum constant velocity in boat direction
                    if (CurrentControllerState == ControllerState.VR_Flap_Turn)
                    {
                        this.m_RigidBody.velocity = new Vector3(this.m_RigidBody.transform.forward.normalized.x * MinimumForwardSpeed, this.m_RigidBody.velocity.y, this.m_RigidBody.transform.forward.normalized.z * MinimumForwardSpeed);
                    }

                    // Minimum constant velocity in head direction
                    else if (CurrentControllerState == ControllerState.VR_Head_Turn)
                    {
                        this.m_RigidBody.velocity = new Vector3(m_Camera.transform.forward.normalized.x * MinimumForwardSpeed, this.m_RigidBody.velocity.y, m_Camera.transform.forward.normalized.z * MinimumForwardSpeed);
                    }
                }
            }
        }

        public void DetectRotations()
        {
            // apply drag to rotational acceleration
            if (m_rotationalVelocity.y > 0)
            {
                float l_dragForce = -1 * (RotationalDrag * m_rotationalVelocity.y * m_rotationalVelocity.y);

                l_dragForce = (l_dragForce > -1 * MinimumDragForce) ? -MinimumDragForce : l_dragForce;

                // Apply force to Acceleration
                m_rotationalAcceleration.y += l_dragForce * Time.deltaTime;

                // apply acceleration to velocity
                m_rotationalVelocity.y += (m_rotationalAcceleration.y * Time.deltaTime);

                if (m_rotationalVelocity.y <= 0)
                {
                    Debug.Log("Hit 0 From right");
                    m_rotationalVelocity.y = 0.0f;
                    m_rotationalAcceleration.y = 0.0f;
                }

            }
            else if (m_rotationalVelocity.y < 0)
            {
                float l_dragForce = -1 * (RotationalDrag * -1 * (m_rotationalVelocity.y * m_rotationalVelocity.y));

                l_dragForce = (l_dragForce < MinimumDragForce) ? MinimumDragForce : l_dragForce;


                // Apply force to Acceleration
                m_rotationalAcceleration.y += l_dragForce * Time.deltaTime;

                // apply acceleration to velocity
                m_rotationalVelocity.y += (m_rotationalAcceleration.y * Time.deltaTime);


                if (m_rotationalVelocity.y >= 0)
                {
                    Debug.Log("Hit 0 From left");
                    m_rotationalVelocity.y = 0.0f;
                    m_rotationalAcceleration.y = 0.0f;
                }

            }
            else
            {
                // apply acceleration to velocity
                m_rotationalVelocity.y += (m_rotationalAcceleration.y * Time.deltaTime);
            }

            transform.rotation = Quaternion.Euler(0.0f, transform.rotation.eulerAngles.y + (m_rotationalVelocity.y * Time.deltaTime), 0.0f);
        }

        public bool HandleRotationalAcceleration()
        {
            return false;
        }


        public void ApplyLift()
        {
            this.m_RigidBody.AddForce(Vector3.up * MaxFlapForce, ForceMode.Impulse);
        }

        public void ApplyRotateRightAcceleration()
        {
            Debug.Log("Accelerate Right Before:" + m_rotationalAcceleration.y);

            m_rotationalAcceleration.y += MaxAngularFlapForce;
            Debug.Log("Accelerate Right After :" + m_rotationalAcceleration.y);
            //m_rotationalVelocity.y = degreeOrRotation;
            //m_RotationalDestination = transform.rotation * Quaternion.Quaternion.Euler(0, degreeOrRotation, 0);
            //transform.Rotate(0, rotateValue * Time.deltaTime, 0, Space.World);
        }

        public void ApplyRotateLeftAcceleration()
        {
            Debug.Log("Accelerate Left Before:" + m_rotationalAcceleration.y);

            m_rotationalAcceleration.y -= MaxAngularFlapForce;
            Debug.Log("Accelerate Left After :" + m_rotationalAcceleration.y);
            //m_accelerateRotationLeft = true;
            //m_rotationalVelocity. += degreeOrRotation;
            //m_RotationalDestination = transform.rotation * Quaternion.Euler(0, -degreeOrRotation, 0);
            //transform.Rotate(0, -1 * rotateValue * Time.deltaTime, 0, Space.World);
        }



        public void SwitchToUIState()
        {
            this.m_Particles.gameObject.SetActive(false);
            this.m_RigidBody.useGravity = false;
        }

        public void SwitchToGamePlayState()
        {
            this.m_Particles.gameObject.SetActive(true);
            this.m_RigidBody.useGravity = true;
        }

        public void setPosition(Vector3 lastCheckpointPosition)
        {
            // Debug.Log("changing position from "+ transform.position);
            transform.position = lastCheckpointPosition;
            //Debug.Log("changed position to " + transform.position);
        }

    }

}

