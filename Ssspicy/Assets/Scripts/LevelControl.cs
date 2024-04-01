using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControl : MonoBehaviour
{

    public void startLevel(int i)
    {
        SceneManager.LoadScene(i);
    }
    public void startNextLevel() //后续需要判定一下最后一关了怎么样
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
