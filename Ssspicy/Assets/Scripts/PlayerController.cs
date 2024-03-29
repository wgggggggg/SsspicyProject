using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    public GameObject PlayerBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveDir = Vector2.right;
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            moveDir = Vector2.up;
        } else if ( Input.GetKeyDown(KeyCode.DownArrow)) {
            moveDir = Vector2.down;
        } else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            moveDir = Vector2.left;
        }
        if (moveDir != Vector2.zero)
        {
            if (canMove(moveDir))
            {
                move(moveDir);
            }
        }
        moveDir = Vector2.zero;
    }

    bool canMove(Vector2 dir)
    {
        //��������ܷ�ǰ��һ��
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.5f, dir, 0.5f);

        if (!hit)
        {
            return true;
        } else {
            if (hit.collider.GetComponent<Banana>() != null)
            {
                return hit.collider.GetComponent<Banana>().move(dir);
            }
            return false;
        }
    }

    void move(Vector2 dir)
    {
        //���ƶ����壬���ƶ�ͷ��
        PlayerBody.GetComponent<PlayerBodyController>().moveBody();
        transform.Translate(dir);
    }
}
