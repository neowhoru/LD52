using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private int highScore = 0;
    
    void Start()
    {
        PlayerPrefs.SetInt("SCORE",0);
        highScore = PlayerPrefs.GetInt("HIGHSCORE", 0);
        highScoreText.SetText($"HIGHSCORE:{highScore.ToString().PadLeft(7,'0')}");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            LoadGameScene();
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync("Tutorial");
    }
}
