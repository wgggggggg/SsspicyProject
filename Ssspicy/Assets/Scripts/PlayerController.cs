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
    public LayerMask bunkerLayer;
    public LayerMask holeLayer;
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
        //返回玩家能否前进一步(排除地面层)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);
        if (!hit)
        {
            Move(dir);
        } else { //判断遇到的这个Body是不是最后一个Body
            if (hit.collider.GetComponent<Body>() != null)
            {
                if(bodyNum(hit) != 1 && isLastBody(hit))
                {
                    Move(dir);
                }
                //是食物的话就调用食物的MoveOrEaten
            } else if (hit.collider.GetComponent<Food>() != null)
            {   
                //吃东西或者推东西的时候伴随的移动在Food里调用
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                //是其它Movable就调用一般的MoveIfCan
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
        //先移动身体，后移动头部
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

    bool shouldDie()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, bunkerLayer);
        if (hit) { return true; }
        return false;
    }

    bool shouldPass()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, holeLayer);
        if (hit && hit.collider.GetComponent<Hole>().IsOpen())
        {
            return true;
        }
        return false;
    }

    public void Fly(Vector2 dir)
    {
        GetComponent<Fly>().FlyStart(dir);
        dieOrPassDetect = false; //飞行的时候关闭死亡和过关检测
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
                Debug.Log("此时哥们掉落了");
            }
            if (shouldPass())
            {
                Debug.Log("此时哥们过关了");
            }
            if (shouldDie())
            {
                Debug.Log("此时哥们踩到沙坑了");
            }
        }
    } //之后还需要加上洞口等

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
