using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Sprite[] firePictures;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            GetComponent<SpriteRenderer>().sprite = firePictures[2];
        } else if (dir == Vector2.down)
        {
            GetComponent<SpriteRenderer>().sprite = firePictures[0];
        } else if (dir == Vector2.left)
        {
            GetComponent<SpriteRenderer>().sprite = firePictures[1];
        } else if (dir == Vector2.right)
        {
            GetComponent<SpriteRenderer>().sprite = firePictures[3];
        }
    }
}
