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
    public void startNextLevel() //������Ҫ�ж�һ�����һ������ô��
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
