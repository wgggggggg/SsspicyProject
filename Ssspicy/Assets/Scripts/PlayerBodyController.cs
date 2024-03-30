using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyController : MonoBehaviour
{
    public GameObject head;
    public GameObject bodyPrefab; //body�õ�Object
    public LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addBodyBeforeMove()
    {
        GameObject newBody = Instantiate(bodyPrefab, transform);
        newBody.transform.position = transform.GetChild(transform.childCount - 1).position; //�κ�λ�ö����� ��move�󶼻����
    }

    public void moveBody()
        //�ƶ�ͷ��ǰ���ƶ����岿�ֵĺ���
    {
        if (transform.childCount != 0)
        {
            for (int i = transform.childCount - 1; i > 0; i--)
            {
                transform.GetChild(i).transform.position = transform.GetChild(i - 1).transform.position;
            }
            transform.GetChild(0).transform.position = head.transform.position;
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
}
