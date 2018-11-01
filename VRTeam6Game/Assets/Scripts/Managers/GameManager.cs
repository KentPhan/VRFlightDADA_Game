using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Game Manager
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class GameManager : MonoBehaviour
    {
        private Image fadeInImage;
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

            fadeInImage = player.GetComponentInChildren<Image>();
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
            

        }
        public void changeScene()
        {
            
                if (SceneManager.GetActiveScene().name == "Intro")
                {
                    Debug.Log("yeah you are");
                    //fadeInImage.GetComponent<FadeScript>().setScreenFade();
                    SceneManager.LoadScene("ExplorationScene");
                    //player.GetComponent<Assets.Scripts.Entities.PlayerEntity>().startMovement = true;

                }
                else if (SceneManager.GetActiveScene().name == "ExplorationScene")
                {
                    //fadeInImage.GetComponent<FadeScript>().setScreenFade();
                    SceneManager.LoadScene("Intro");

                }
            
        }
        
    }
}


