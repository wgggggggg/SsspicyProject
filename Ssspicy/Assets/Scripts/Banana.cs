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
        //�����㽶���ܲ����ƶ����Լ���ǽ��ʱ��ᱻ�Ե���������ӳ���
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
