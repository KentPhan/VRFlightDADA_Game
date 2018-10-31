using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Sound Manager
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class SoundManager : MonoBehaviour
    {

        public static SoundManager Instance = null;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;

            else if (Instance != null)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {

        }
        
        // Update is called once per frame
        void Update()
        {
            var audioClipSpeed = 10.0;

            var p = GetComponent<Rigidbody>().velocity.magnitude / audioClipSpeed;
            GetComponent<AudioSource>().pitch = Mathf.Clamp((float)p, (float)0.1, (float)4.0); // p is clamped to same values
        }
    }
}

