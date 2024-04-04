using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{
    private static int levelSelectScene = 7;
    private static int passScene = 6;
    private static int dieScene = 8;
    public static void startLevel(int i)
    {
        SceneManager.LoadScene(i);
        Time.timeScale = 1.0f;
    }
    public static void startNextLevel() //后续需要判定一下最后一关了怎么样
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1.0f;
    }

    public static void LevelSelect()
    {
        SceneManager.LoadScene(levelSelectScene);
    }

    public static void PassScene()
    {
        SceneManager.LoadScene(passScene);
    }

    public static void DieScene()
    {
        SceneManager.LoadScene(dieScene);
    }
    public static void Title()
    {
        startLevel(0);
        Time.timeScale = 1.0f;
    }

    public static void Restart()
    {
        startLevel(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public static void Exit()
    {
        Application.Quit();
    }

}
