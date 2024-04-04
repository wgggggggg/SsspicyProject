using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public GameObject Food;
    public Sprite[] HoleShapes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Animator>().SetBool("HoleOpen", IsOpen());
    }

    public bool IsOpen()
    {
        return !Food.GetComponent<Food>().IsFoodExist();
    }

    public void HoleShapeChange(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            GetComponent<SpriteRenderer>().sprite = HoleShapes[0];
        } else if (dir == Vector2.right)
        {
            GetComponent<SpriteRenderer>().sprite = HoleShapes[1];
        } else if (dir == Vector2.down)
        {
            GetComponent<SpriteRenderer>().sprite = HoleShapes[2];
        } else if (dir == Vector2.left)
        {
            GetComponent<SpriteRenderer>().sprite = HoleShapes[3];
        }
    }
}
