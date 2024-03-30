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
        if (!MoveIfCan(dir)) //即需要被吃
        {
            Eaten(dir);
        } else
        {
            Player.GetComponent<PlayerController>().Move(dir);//食物不需要被吃的时候即玩家推动了食物,玩家也需要移动
        }
    }

    public virtual void Eaten(Vector2 dir)
    {
        Destroy(gameObject);
    }
 
}
