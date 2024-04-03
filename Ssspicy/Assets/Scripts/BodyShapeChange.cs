using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyShapeChange : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] pictures;
    public Vector2 nowDir;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSprite(string name)
    {
        if (name == "upDownBody")
        {
            spriteRenderer.sprite = pictures[0];
        } else if (name == "rightTail")
        {
            spriteRenderer.sprite = pictures[1];
        } else if (name == "downTail")
        {
            spriteRenderer.sprite = pictures[2];
        } else if (name == "leftDownBody")
        {
            spriteRenderer.sprite = pictures[3];
        } else if (name == "leftTail")
        {
            spriteRenderer.sprite = pictures[4];
        } else if (name == "leftRightBody")
        {
            spriteRenderer.sprite = pictures[5];
        } else if (name == "rightDownBody")
        {
            spriteRenderer.sprite = pictures[6];
        } else if (name == "upTail")
        {
            spriteRenderer.sprite = pictures[7];
        } else if (name == "leftUpBody")
        {
            spriteRenderer.sprite = pictures[8];
        } else if (name == "rightUpBody")
        {
            spriteRenderer.sprite = pictures[9];
        }
    }
}
