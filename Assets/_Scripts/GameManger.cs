using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManger : MonoBehaviour
{
    public static GameManger instance;

    private int score;

    [SerializeField]
    private Text scoretxt;

    [SerializeField]
    private Image fader;
    // Start is called before the first frame update
    void Awake ()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
            Destroy(gameObject);
    }

  

    public void AddScore()
    {
        score++;
        scoretxt.text = score.ToString();
        
    }

    public void ResetScore()
    {
        score = 0;
        scoretxt.text = score.ToString();

        
    }

    public void ReloadScene()
    {
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 0);

        StopCoroutine("FadeReload");
        StartCoroutine("FadeReload");

    }

    IEnumerator FadeReload()
    {


        yield return new WaitForSeconds(1);

        fader.gameObject.SetActive(true);
        float starTime = Time.time;
        float duration = 0.2f;

        while (Time.time < starTime + duration)
        {
            float t = (Time.time - starTime) / duration;
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

           // after load scene come from black to white color
        starTime = Time.time;
    

        while (Time.time < starTime + duration)
        {
            float t = (Time.time - starTime) / duration;
            fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        fader.color = new Color(fader.color.r, fader.color.g, fader.color.b, 0);

    }
}
