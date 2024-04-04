using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pepper : Food
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FallifShould();
    }

    public override void Eaten(Vector2 dir)
    {
        Player.GetComponent<PlayerController>().Move(dir);
        FlyPlayer(-dir);
        FireIfCan(dir);
        Destroy(gameObject);
    }

    void FlyPlayer(Vector2 dir)
    {
        Player.GetComponent<PlayerController>().Fly(dir);
    }

    void FireIfCan(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f, otherLayer);
        if (hit && (hit.collider.GetComponent<Ice>() != null || hit.collider.GetComponent<Wood>() != null)) 
        {
            Destroy(hit.collider.gameObject);
        }
    }
}
