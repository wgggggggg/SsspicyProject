using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public GameObject PlayerBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool move(Vector2 dir)
    {
        //返回香蕉们能不能移动，以及靠墙的时候会被吃掉给玩家增加长度
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f);

        if (!hit)
        {
            transform.Translate(dir);
            return true;
        } 
        else 
        {
            if (hit.collider.GetComponent<Banana>() != null)
            {
                bool afterBananaCanMove = hit.collider.GetComponent<Banana>().move(dir);
                if (afterBananaCanMove)
                {
                    transform.Translate(dir);
                }
                return afterBananaCanMove;
            } else if (hit.collider.GetComponent<Wall>() != null)
            {
                Destroy(gameObject);
                PlayerBody.GetComponent<PlayerBodyController>().addBodyBeforeMove();
                return true;
            }
            return false;
        }
    }



}
