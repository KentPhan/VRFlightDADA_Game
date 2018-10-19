using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// UIManager
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class UIManager : MonoBehaviour
    {

        public static UIManager Instance = null;

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

        }


    }
}


