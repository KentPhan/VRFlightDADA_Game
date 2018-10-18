using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.Entities
{
    /// <summary>
    /// Player Entity
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerEntity : MonoBehaviour
    {
        public float MaxFlapForce = 10.0f;
        public float ConstantForwardSpeed = 10.0f;





        private Rigidbody m_RigidBody;

        // Use this for initialization
        void Start()
        {

            this.m_RigidBody = GetComponent<Rigidbody>();

        }

        // Update is called once per frame
        void Update()
        {

            string[] test = UnityEngine.Input.GetJoystickNames();

            Vector3 l_leftPosition = InputTracking.GetLocalPosition((XRNode.LeftHand));

            Vector3 l_rightPosition = InputTracking.GetLocalPosition((XRNode.RightHand));

            Debug.Log(l_leftPosition);
            Debug.Log(l_rightPosition);

            if (Input.GetButtonDown("Flap"))
            {
                this.m_RigidBody.AddForce(Vector3.up * this.MaxFlapForce, ForceMode.Impulse);

            }

            this.m_RigidBody.MovePosition(transform.position + transform.forward * ConstantForwardSpeed * Time.deltaTime);
        }

        public void MovePlayer(float flapForce)
        {
            this.m_RigidBody.AddForce(Vector3.up * this.MaxFlapForce, ForceMode.Impulse);
        }


    }

}

