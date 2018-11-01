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
            //fadeInImage = player
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (SceneManager.GetActiveScene().name == "HiteshTestStartScene")
                {
                    Debug.Log("yeah you are");
                    SceneManager.LoadScene("HiteshTestScene");
                    //player.GetComponent<Assets.Scripts.Entities.PlayerEntity>().startMovement = true;

                }
                else if(SceneManager.GetActiveScene().name == "HiteshTestScene")
                {
                    SceneManager.LoadScene("HiteshTestStartScene");

                }
            }
            

        }
        void setScreenFade()
        {
            var tempcolor = fadeInImage.color;
            tempcolor.a = 0.01f;
            fadeInImage.color = tempcolor;

            fadeInImage.CrossFadeAlpha(255f, 1f, false);
            Invoke("stopScreenFade", 1f);
        }
        void stopScreenFade()
        {
            fadeInImage.CrossFadeAlpha(0f, 1f, false);
        }
    }
}


