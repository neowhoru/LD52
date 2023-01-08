using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Fire1"))
        {
            LoadTitleScene();
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadSceneAsync("Level1");
    }
}
