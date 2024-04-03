using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyController : MonoBehaviour
{
    public GameObject Player;
    public GameObject bodyPrefab; //body用的Object
    public LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addBodyBeforeMove(Vector2 dir)
    {
        GameObject newBody = Instantiate(bodyPrefab, transform);
        // newBody.transform.position = transform.GetChild(transform.childCount - 1).position; //任何位置都可以 在move后都会更新
        newBody.GetComponent<BodyShapeChange>().nowDir = dir;
        //newBody.GetComponent<SpriteRenderer>().sprite = transform.GetChild(transform.childCount - 1).GetComponent<SpriteRenderer>().sprite;
        if (transform.childCount == 1)
        {
            createTailBy(newBody.transform);
        }
    }


    public void moveBody(Vector2 dir)
    //移动头部前先移动身体部分的函数
    {
        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i > 0; i--)
            {
                Transform prevBody = transform.GetChild(i).transform;
                Transform nextBody = transform.GetChild(i - 1).transform;
                prevBody.transform.position = nextBody.transform.position;
                prevBody.GetComponent<BodyShapeChange>().nowDir = nextBody.GetComponent<BodyShapeChange>().nowDir;
                if (i == transform.childCount - 1)
                {
                    createTailBy(prevBody);
                } else
                {
                    prevBody.GetComponent<SpriteRenderer>().sprite = nextBody.GetComponent<SpriteRenderer>().sprite;
                }
            }

            Transform firstChild = transform.GetChild(0);
            firstChild.position = Player.transform.position;
            if (transform.childCount == 1)
            {
                createTailBy(firstChild, dir);
            } else
            {
                reShapeFirstBody(firstChild, dir);
            }
            firstChild.GetComponent<BodyShapeChange>().nowDir = dir;
        }
    }

    public bool BodyOutGround()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform nowChild = transform.GetChild(i);
            RaycastHit2D hit = Physics2D.Raycast(nowChild.transform.position, Vector2.down, 0.1f, groundLayer);
            if (hit.collider != null)
            {
                return false;
            }
        }
        return true;
    }

    public int BodyNum()
    {
        return transform.childCount;
    }

    void createTailBy(Transform newBody)
    {
        Vector2 dir = newBody.GetComponent<BodyShapeChange>().nowDir;
        createTailBy(newBody, dir);

    }

    void createTailBy(Transform newBody, Vector2 dir)
    {
        string name = "";
        if (dir == Vector2.up)
        {
            name = "upTail";
        }
        else if (dir == Vector2.down)
        {
            name = "downTail";
        }
        else if (dir == Vector2.left)
        {
            name = "leftTail";
        }
        else if (dir == Vector2.right)
        {
            name = "rightTail";
        }
        newBody.GetComponent<BodyShapeChange>().ChangeSprite(name);
    }

    void reShapeFirstBody(Transform firstChild, Vector2 dir)
    {
        Vector2 firstChildDir = firstChild.GetComponent<BodyShapeChange>().nowDir;
        if (firstChildDir == Vector2.up)
        {
            if (dir == Vector2.left)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("rightDownBody");
            }
            else if (dir == Vector2.right)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("leftDownBody");
            }
            else
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("upDownBody");
            }
        }
        else if (firstChildDir == Vector2.down)
        {
            if (dir == Vector2.left)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("rightUpBody");
            }
            else if (dir == Vector2.right)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("leftUpBody");
            }
            else
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("upDownBody");
            }
        }
        else if (firstChildDir == Vector2.left)
        {
            if (dir == Vector2.up)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("leftUpBody");
            }
            else if (dir == Vector2.down)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("leftDownBody");
            }
            else
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("leftRightBody");
            }
        }
        else if (firstChildDir == Vector2.right)
        {
            if (dir == Vector2.up)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("rightUpBody");
            }
            else if (dir == Vector2.down)
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("rightDownBody");
            }
            else
            {
                firstChild.GetComponent<BodyShapeChange>().ChangeSprite("leftRightBody");
            }
        }
    }
}
