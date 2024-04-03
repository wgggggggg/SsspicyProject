using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : Food
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
        Destroy(gameObject);
        PlayerBody.GetComponent<PlayerBodyController>().addBodyBeforeMove(dir);
        Player.GetComponent<PlayerController>().Move(dir);
    }
    



}
