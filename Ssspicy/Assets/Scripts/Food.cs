using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Movable
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void MoveOrEaten(Vector2 dir)
    {
        if (!MoveIfCan(dir)) //����Ҫ����
        {
            Player.GetComponent<PlayerController>().moveAnimDetect(dir); //��ת��...
            Eaten(dir);
        } else
        {
            Player.GetComponent<PlayerController>().Move(dir);//ʳ�ﲻ��Ҫ���Ե�ʱ������ƶ���ʳ��,���Ҳ��Ҫ�ƶ�
        }
    }

    public virtual void Eaten(Vector2 dir)
    {
        Destroy(gameObject);
    }

    public bool IsFoodExist()
    {
        if (transform.childCount != 0)
        {
            return true;
        }
        return false;
    }

    public override void gameStopIfShould()
    {
        LevelControl.DieScene();
    }

    
}
