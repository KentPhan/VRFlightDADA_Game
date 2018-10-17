using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Game Manager
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance = null;

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


