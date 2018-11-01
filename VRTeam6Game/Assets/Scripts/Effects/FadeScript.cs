using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Managers;

public class FadeScript : MonoBehaviour {
    private Image fadeImage;
   
     void Awake()
    {
        fadeImage = GetComponent<Image>();
        
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
            setScreenFade();
       
	}
    public void setScreenFade()
    {
        var tempcolor = fadeImage.color;
        tempcolor.a = 0.01f;
        fadeImage.color = tempcolor;

        fadeImage.CrossFadeAlpha(255f, 1f, false);
        Invoke("stopScreenFade", 1f);
    }
    void stopScreenFade()
    {
        GameManager.Instance.changeScene();
        fadeImage.CrossFadeAlpha(0f, 1f, false);
    }
}
