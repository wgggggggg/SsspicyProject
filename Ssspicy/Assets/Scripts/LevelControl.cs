using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    private int levelSelectScene = 6;
    private int passScene = 7;
    private int dieScene = 8;
    public void startLevel(int i)
    {
        SceneManager.LoadScene(i);
        Time.timeScale = 1.0f;
    }
    public void startNextLevel() //后续需要判定一下最后一关了怎么样
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public void PassScene()
    {
        SceneManager.LoadScene(passScene);
    }

    public void DieScene()
    {
        SceneManager.LoadScene(dieScene);
    }
    public void Title()
    {
        startLevel(0);
        Time.timeScale = 1.0f;
    }

    public void Restart()
    {
        startLevel(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void Exit()
    {
        Application.Quit();
    }

}
