using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public GameObject PlayerBody;
    public LayerMask otherLayer;
    public GameObject Player;
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
        if (!IsMoveOrEaten(dir)) //����Ҫ����
        {
            Eaten(dir);
        } else
        {
            Player.GetComponent<PlayerController>().Move(dir);//ʳ�ﲻ��Ҫ���Ե�ʱ������ƶ���ʳ��,���Ҳ��Ҫ�ƶ�
        }
    }
    public bool IsMoveOrEaten(Vector2 dir)
    {
        //true����Move��false����Eaten,���������true��˳���ƶ���
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);

        if (!hit)
        {
            Move(dir);
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Food>() != null) //�����һ����Food
            {
                bool afterFoodMoveOrEaten = hit.collider.GetComponent<Food>().IsMoveOrEaten(dir);
                if (afterFoodMoveOrEaten) //���֮���Food�����ƶ�����Ҳ�����ƶ�
                {
                    Move(dir);
                }
                return afterFoodMoveOrEaten; //�����Ƿ����ƶ�����Ҫ������һ��Food���ڵ����
            }
            else if (hit.collider.GetComponent<Wall>() != null) //�����һ����ǽ�����Ҳ����ƶ���
            {
                return false;
            }
            return false; //����������ģ�Ŀǰ����false�����ݿ��������޸�
        }
    }
    void Move(Vector2 dir)
    {
        transform.Translate(dir);
    }

    public virtual void Eaten(Vector2 dir)
    {
        Destroy(gameObject);
    }
 
}
