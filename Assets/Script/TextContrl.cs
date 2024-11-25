using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextContrl : MonoBehaviour {

    public static TextContrl instance;

    public GameObject readyText;

    public GameObject gameOverText;
    private void Awake()
    {
        if (TextContrl.instance == null)
        {
            TextContrl.instance = this;
        }
    }

    void Start () {
        readyText.SetActive(false);

        
         gameOverText.SetActive(false);

        StartCoroutine(ShowReady());
      
	}
    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 3)
        {
            readyText.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            readyText.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            count++;
        }

    }

    public void ShowGameOver()
    {
        gameOverText.SetActive(true);
    }
    
    public void Restart()
    {
        gameOverText.SetActive(false);
        StartCoroutine(ShowReady());

    }


    void Update () {
		
	}
}
