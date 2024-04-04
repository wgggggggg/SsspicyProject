using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    public GameObject PlayerBody;
    public LayerMask otherLayer;
    public LayerMask groundLayer;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.3f, dir, 0.7f, otherLayer);

        if (!hit)
        {
            Move(dir);
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Movable>() != null) //如果下一个是Movable
            {
                bool afterMovableCanMove = hit.collider.GetComponent<Movable>().MoveIfCan(dir);
                if (afterMovableCanMove) //如果之后的Movable都是移动，我也可以移动
                {
                    Move(dir);
                }
                return afterMovableCanMove; //不管是否能移动都需要告诉上一个Movable现在的情况
            }
            else if (hit.collider.GetComponent<Obstacle>() != null) //如果下一个是Obstacle，即我不能移动了
            {
                return false;
            }
            return false; //如果是其他的，目前返回false，根据开发进度修改，之后加入沙坑洞口等需要修改LayerMask或者这里
        }
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

    IEnumerator Fall()
    {

        yield return new WaitForSeconds(0.5f);
        GetComponent<DropDown>().dropStart((Vector2)transform.position + Vector2.down * 24);
        yield return new WaitForSeconds(1.5f);
        gameStopIfShould();
        Destroy(gameObject);
    }

    public void FallifShould()
    {
        if (!InGround() && !PlayerController.shouldPausePlayerControl)
        {
            StartCoroutine(Fall());
        }
    }

    public virtual void gameStopIfShould(){}
}
