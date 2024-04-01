using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuList : MonoBehaviour
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
                Time.timeScale = 1f; //������������bgm�Ĳ�������ͣ
                PlayerController.pauseGame(false);
            }
        } else
        {
            if (Input.GetKeyDown (KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuOpening = true;
                Time.timeScale = 0f;
                PlayerController.pauseGame(true);
            }
        }
    }
}