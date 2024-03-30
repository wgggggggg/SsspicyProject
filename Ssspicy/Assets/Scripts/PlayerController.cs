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
            if (canMove(moveDir))
            {
                move(moveDir);
            }
        }
        moveDir = Vector2.zero;
        if(shouldFall())
        {
            Debug.Log("此时哥们掉落了");
        }
    }

    bool canMove(Vector2 dir)
    {
        //返回玩家能否前进一步(排除地面层)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);

        if (!hit)
        {
            return true;
        } else {
            if (hit.collider.GetComponent<Food>() != null)
            {
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                return true;
            }
            return false; //目前遇到除了食物之外的物体都不可移动
        }
    }

    void move(Vector2 dir)
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

    } 
}
