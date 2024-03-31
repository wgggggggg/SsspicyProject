using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    public GameObject PlayerBody;
    public LayerMask groundLayer;
    public LayerMask otherLayer;
    private bool dieOrPassDetect;
    // Start is called before the first frame update
    void Start()
    {
        dieOrPassDetect = true;
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
        dieOrPassDetectIfStart();
        moveDir = Vector2.zero;
    }

    void MoveOrEat(Vector2 dir)
    {   
        //��������ܷ�ǰ��һ��(�ų������)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);
        if (!hit)
        {
            Move(dir);
        } else { //�ж����������Body�ǲ������һ��Body
            if (hit.collider.GetComponent<Body>() != null)
            {
                if(bodyNum(hit) != 1 && isLastBody(hit))
                {
                    Move(dir);
                }
                //��ʳ��Ļ��͵���ʳ���MoveOrEaten
            } else if (hit.collider.GetComponent<Food>() != null)
            {   
                //�Զ��������ƶ�����ʱ�������ƶ���Food�����
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                //������Movable�͵���һ���MoveIfCan
            } else if (hit.collider.GetComponent<Movable>() != null)
            {
                if (hit.collider.GetComponent<Movable>().MoveIfCan(dir))
                {
                    Move(dir);
                }
            }
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
        dieOrPassDetect = false; //���е�ʱ��ر������͹��ؼ��
    } 

    public void startDieOrPassDetect()
    {
        dieOrPassDetect = true;
    }

    private void dieOrPassDetectIfStart()
    {
        if (dieOrPassDetect)
        {
            if (shouldFall())
            {
                Debug.Log("��ʱ���ǵ�����");
            }
        }
    } //֮����Ҫ���϶��ڵ�

    bool isLastBody(RaycastHit2D hit)
    {
        GameObject hitObject = hit.collider.gameObject;
        Transform bodyParent = hit.collider.transform.parent;
        int childCount = bodyParent.childCount;
        int indexOfHitObject = hitObject.transform.GetSiblingIndex();
        return (indexOfHitObject == childCount - 1);
    }

    int bodyNum(RaycastHit2D hit)
    {
        Transform bodyParent = hit.collider.transform.parent;
        int childCount = bodyParent.childCount;
        return childCount;
    }
}
