using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedSceneController : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI scoreTExt;

    private void Start()
    {
        int highscore = PlayerPrefs.GetInt("HIGHSCORE", 0);
        int score = PlayerPrefs.GetInt("SCORE", 0);
        highScoreText.SetText("HIGHSCORE:" + highscore.ToString().PadLeft(6, '0'));
        scoreTExt.SetText("YOUR SCORE:" + score.ToString().PadLeft(6, '0'));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            LoadTitleScene();
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadSceneAsync("TitleScreenController");
    }
}