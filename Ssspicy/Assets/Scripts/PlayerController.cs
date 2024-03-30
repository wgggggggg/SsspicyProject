using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    public GameObject PlayerBody;
    public LayerMask groundLayer;
    public LayerMask otherLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveDir = Vector2.right;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            moveDir = Vector2.up;
        } else if ( Input.GetKeyDown(KeyCode.DownArrow)) {
            moveDir = Vector2.down;
        } else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            moveDir = Vector2.left;
        }
        if (moveDir != Vector2.zero)
        {
            MoveOrEat(moveDir);
        }
        moveDir = Vector2.zero;
        if(shouldFall())
        {
            Debug.Log("��ʱ���ǵ�����");
        }
    }

    bool MoveOrEat(Vector2 dir)
    {   
        //True��ʾ�ƶ����߳Զ����ɹ�,False��ʾɶ��û��
        //��������ܷ�ǰ��һ��(�ų������)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);
        if (!hit)
        {
            Move(dir);
            return true;
        } else { //�ж����������Body�ǲ������һ��Body
            if (hit.collider.GetComponent<Body>() != null)
            {
                GameObject hitObject = hit.collider.gameObject;
                Transform bodyParent = hit.collider.transform.parent;
                int childCount = bodyParent.childCount;
                int indexOfHitObject = hitObject.transform.GetSiblingIndex();
                bool isLastChild = (indexOfHitObject == childCount - 1);
                if (isLastChild)
                {
                    Move(dir);
                    return true;
                } else
                {
                    return false;
                }
            }
            if (hit.collider.GetComponent<Food>() != null)
            {   
                //�Զ��������ƶ�����ʱ�������ƶ���Food�����
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                return true;
            }
            return false; //Ŀǰ��������ʳ��֮������嶼�����ƶ�
        }
    }

    public void Move(Vector2 dir)
    {
        //���ƶ����壬���ƶ�ͷ��
        PlayerBody.GetComponent<PlayerBodyController>().moveBody();
        transform.Translate(dir);
    }

    bool InGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    bool shouldFall()
    {
        if (!InGround() && PlayerBody.GetComponent<PlayerBodyController>().BodyOutGround())
        {
            return true;
        }
        return false;
    }

    public void Fly(Vector2 dir)
    {
        GetComponent<Fly>().FlyStart(dir);
    } 
}
