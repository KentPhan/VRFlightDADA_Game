using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Game Manager
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance = null;
        public GameObject player;
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
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (SceneManager.GetActiveScene().name == "HiteshTestStartScene")
                {
                    Debug.Log("yeah you are");
                    SceneManager.LoadScene("HiteshTestScene");
                    player.GetComponent<Assets.Scripts.Entities.PlayerEntity>().startMovement = true;

                }
            }
            

        }
    }
}


