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
        if (!IsMoveOrEaten(dir)) //即需要被吃
        {
            Eaten(dir);
        } else
        {
            Player.GetComponent<PlayerController>().Move(dir);//食物不需要被吃的时候即玩家推动了食物,玩家也需要移动
        }
    }
    public bool IsMoveOrEaten(Vector2 dir)
    {
        //true代表Move，false代表Eaten,如果最终是true就顺便移动了
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);

        if (!hit)
        {
            Move(dir);
            return true;
        }
        else
        {
            if (hit.collider.GetComponent<Food>() != null) //如果下一个是Food
            {
                bool afterFoodMoveOrEaten = hit.collider.GetComponent<Food>().IsMoveOrEaten(dir);
                if (afterFoodMoveOrEaten) //如果之后的Food都是移动，我也可以移动
                {
                    Move(dir);
                }
                return afterFoodMoveOrEaten; //不管是否能移动都需要告诉上一个Food现在的情况
            }
            else if (hit.collider.GetComponent<Wall>() != null) //如果下一个是墙，即我不能移动了
            {
                return false;
            }
            return false; //如果是其他的，目前返回false，根据开发进度修改
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
