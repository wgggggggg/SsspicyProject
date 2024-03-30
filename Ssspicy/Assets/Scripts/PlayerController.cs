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
            Debug.Log("此时哥们掉落了");
        }
    }

    bool MoveOrEat(Vector2 dir)
    {   
        //True表示移动或者吃东西成功,False表示啥都没干
        //返回玩家能否前进一步(排除地面层)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);
        if (!hit)
        {
            Move(dir);
            return true;
        } else { //判断遇到的这个Body是不是最后一个Body
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
                //吃东西或者推东西的时候伴随的移动在Food里调用
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                return true;
            }
            return false; //目前遇到除了食物之外的物体都不可移动
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

    public void Fly(Vector2 dir)
    {
        GetComponent<Fly>().FlyStart(dir);
    } 
}
