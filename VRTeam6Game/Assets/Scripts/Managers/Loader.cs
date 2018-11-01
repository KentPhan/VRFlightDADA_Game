using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// For Loading Instances 
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Loader : MonoBehaviour
    {

        public GameObject gameManager;
        public GameObject soundManager;
        public GameObject uIManager;

        public void Awake()
        {
            if (GameManager.Instance == null)
            {
                Instantiate(gameManager);
            }

            if (SoundManager.Instance == null)
            {
                Instantiate(soundManager);
            }

            //if (UIManager.Instance == null)
            //{
            //    Instantiate(uIManager);
            //}
        }
    }
}