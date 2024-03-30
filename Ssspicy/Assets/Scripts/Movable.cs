using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
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

    public void Move(Vector2 dir)
    {
        transform.Translate(dir);
    }

    public bool MoveIfCan(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);

        if (!hit)
        {
            Move(dir);
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Movable>() != null) //�����һ����Movable
            {
                bool afterMovableCanMove = hit.collider.GetComponent<Movable>().MoveIfCan(dir);
                if (afterMovableCanMove) //���֮���Movable�����ƶ�����Ҳ�����ƶ�
                {
                    Move(dir);
                }
                return afterMovableCanMove; //�����Ƿ����ƶ�����Ҫ������һ��Movable���ڵ����
            }
            else if (hit.collider.GetComponent<Obstacle>() != null) //�����һ����Obstacle�����Ҳ����ƶ���
            {
                return false;
            }
            return false; //����������ģ�Ŀǰ����false�����ݿ��������޸ģ�֮�����ɳ�Ӷ��ڵ���Ҫ�޸�LayerMask��������
        }
    }
}
