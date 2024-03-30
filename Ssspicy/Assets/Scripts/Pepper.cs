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
        
    }

    public override void Eaten(Vector2 dir)
    {
        Player.GetComponent<PlayerController>().Move(dir);
        FlyPlayer(-dir);
        Destroy(gameObject);
    }

    void FlyPlayer(Vector2 dir)
    {
        Player.GetComponent<PlayerController>().Fly(dir);
    }
}
