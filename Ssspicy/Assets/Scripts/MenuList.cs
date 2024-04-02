using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuList : LevelControl
{
    private bool menuOpening;
    public GameObject menuList;
    // Start is called before the first frame update
    void Start()
    {
        menuOpening = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuOpening)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(false);
                menuOpening = false;
                Time.timeScale = 1f; //后续可以添加bgm的播放与暂停
                PlayerController.pausePlayerControl(false);
            }
        } else
        {
            if (Input.GetKeyDown (KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuOpening = true;
                Time.timeScale = 0f;
                PlayerController.pausePlayerControl(true);
            }
        }
    }

    public void Play()
    {
        menuList.SetActive(false);
        menuOpening = false;
        Time.timeScale = 1f; //后续可以添加bgm的播放与暂停
        PlayerController.pausePlayerControl(false);
    }
}
