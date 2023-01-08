using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public CinemachineConfiner Confiner;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreMoneyText;
    public TextMeshProUGUI timeOverText;
    public Animator timeOverAnimator;

    public GameObject levelFinishedPanel;
    public Animator levelFinishedAnimator;

    public Text tomatoeCountText;
    public Text dopeCountText;
    public Text strawberryCountText;
    public Text eggplantCountText;

    public GameObject messagePanelGameObject;
    public Text messageText;

    private LevelSettings _levelSettings;

    // ToDo: decide if i really need this later 
    private int seededHoles = 0;
    private int holesCount = 0;

    private uint harvestedTomato;
    private uint harvestedDope;
    private uint harvestedStrawberry;
    private uint harvestedEggplant;

    private int collectedCollectables = 0;
    private int maxCollectableCount = 0;

    private int scoreMoney = 0;

    private int secondsLeft;

    // Start is called before the first frame update
    void Start()
    {
        _levelSettings = FindObjectOfType<LevelSettings>();
        if (_levelSettings.gameObject.GetComponent<CompositeCollider2D>() != null)
        {
            Confiner.m_BoundingShape2D = _levelSettings.gameObject.GetComponent<CompositeCollider2D>();
        }

        if (_levelSettings.time > 0)
        {
            secondsLeft = _levelSettings.time;
            Invoke(nameof(StartTimer), 2);
        }

        scoreMoney = PlayerPrefs.GetInt("SCORE", 0);
        UpdateScoreLabel();
        UpdateCollectableCountUI();
    }

    public void StartTimer()
    {
        timerText.gameObject.SetActive(true);
        InvokeRepeating(nameof(UpdateTimer), 0, 1);
    }

    public void UpdateScoreLabel()
    {
        scoreMoneyText.SetText("$ " + scoreMoney.ToString().PadLeft(6, '0'));
    }

    public void UpdateTimer()
    {
        if (secondsLeft > 0)
        {
            secondsLeft--;
            int seconds = Mathf.FloorToInt(secondsLeft % 60);
            timerText.SetText(seconds.ToString("00"));
        }
        else
        {
            timerText.gameObject.SetActive(false);
            CancelInvoke(nameof(UpdateTimer));
            GameOver();
        }
    }

    public void GameOver()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<TopDownMovement>().DisableMovement();
        timeOverText.gameObject.SetActive(true);
        timeOverAnimator.Play("TimeIsUpAnimation");
        PlayerPrefs.SetInt("SCORE",scoreMoney);
        PlayerPrefs.Save();
        Invoke(nameof(RestartLevel), 2);
    }

    public void LevelFinished()
    {
        // ToDo: maybe have some "party" animation for the Player or something
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<SpriteRenderer>().enabled = false;
        player.GetComponent<TopDownMovement>().DisableMovement();
        CancelInvoke(nameof(UpdateTimer));
        timerText.gameObject.SetActive(false);
        PlayerPrefs.SetInt("SCORE",scoreMoney);
        PlayerPrefs.Save();
        // Some nice "Finished Level" Panel
        levelFinishedPanel.SetActive(true);
        levelFinishedAnimator.Play("LevelFinishedAnimation");
        
        Invoke(nameof(LoadNextLevelOrFinish), 2);
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevelOrFinish()
    {
        if (_levelSettings.isLastLevel)
        {
            int highscore = PlayerPrefs.GetInt("HIGHSCORE");
            if (scoreMoney > highscore)
            {
                PlayerPrefs.SetInt("HIGHSCORE",scoreMoney);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene("FinishedScene");
        }
        else
        {
            int nextLevelNum = _levelSettings.levelNumber + 1;
            SceneManager.LoadScene("Level" + nextLevelNum);
        }
    }


    public void UpdateScore(int scoreToGain)
    {
        scoreMoney += scoreToGain;
        UpdateScoreLabel();
    }

    public void IncreaseHarvestCollectableCount(Collectable.COLLECTABLE_TYPE collectableCollectableType)
    {
        Debug.Log($"INCREASE AMOUNT FOR {collectableCollectableType}");
        switch (collectableCollectableType)
        {
            case Collectable.COLLECTABLE_TYPE.TOMATOE:
                harvestedTomato++;
                break;
            case Collectable.COLLECTABLE_TYPE.DOPE:
                harvestedDope++;
                break;
            case Collectable.COLLECTABLE_TYPE.STRAWBERRY:
                harvestedStrawberry++;
                break;
            case Collectable.COLLECTABLE_TYPE.EGGPLANT:
                harvestedEggplant++;
                break;
        }

        UpdateCollectableCountUI();
        CheckIfLevelIsFinished();
    }

    private void CheckIfLevelIsFinished()
    {
        bool isLevelFinished = harvestedTomato >= _levelSettings.tomatoRequired &&
                                harvestedDope >= _levelSettings.dopeRequired &&
                                harvestedStrawberry >= _levelSettings.strawberryRequired &&
                                harvestedEggplant >= _levelSettings.eggplantRequired;
        
        if (isLevelFinished)
        {
            Debug.Log("LevelFinished");
            LevelFinished();
        }
    }

    private void UpdateCollectableCountUI()
    {
        tomatoeCountText.text = $"{harvestedTomato}/{_levelSettings.tomatoRequired}";
        dopeCountText.text = $"{harvestedDope}/{_levelSettings.dopeRequired}";
        strawberryCountText.text = $"{harvestedStrawberry}/{_levelSettings.strawberryRequired}";
        eggplantCountText.text = $"{harvestedEggplant}/{_levelSettings.eggplantRequired}";
    }

    public void ShowMessageBox(string text)
    {
        messageText.text = text;
        messagePanelGameObject.SetActive(true);
    }

    public void HideMessageBox()
    {
        messageText.text = "";
        messagePanelGameObject.SetActive(false);
    }
}