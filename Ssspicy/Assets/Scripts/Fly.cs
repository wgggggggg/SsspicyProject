using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Vector2 flyDirection; // ���з���
    public float speed; // �����ٶ�
    public GameObject PlayerBody;
    public GameObject Player;
    public LayerMask FlyDetectLayer; //������������Դ�  Ҳ������ɳ�Ӷ���
    public bool start = false;
    LinkedList<GameObject> objectList;
    Queue<GameObject> toAddQueue;
    HashSet<GameObject> added;
    public void FlyStart(Vector2 dir)
    {
        flyDirection = dir;
        start = true;
        Start();
    }
    void Start()
    {
        objectList = new LinkedList<GameObject> ();
        toAddQueue = new Queue<GameObject> ();
        added = new HashSet<GameObject> ();
        objectList.AddLast(Player);
        for (int i = 0; i < PlayerBody.transform.childCount; i++)
        {
            objectList.AddLast(PlayerBody.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if (start)
        {
            foreach(GameObject gb in objectList)
            {
                //�ƶ��ö���
                gb.transform.Translate(flyDirection * Time.deltaTime * speed);
                // ����һ�����ߣ��������������ĽӴ�
                RaycastHit2D hit = Physics2D.Raycast(gb.transform.position + (Vector3)flyDirection * 0.5f, flyDirection, 0.25f, FlyDetectLayer);
                //�������Movable�ʹ���һ����(�������),���Ҽ���HashSet,��ֹ�ظ�����objectList
                if (hit.collider != null && hit.collider.GetComponent<Movable>() != null && !added.Contains(hit.collider.gameObject))
                {
                    toAddQueue.Enqueue(hit.collider.gameObject);
                    added.Add(hit.collider.gameObject);
                }
                if (hit.collider != null && hit.collider.GetComponent<Movable>() == null) //���������������ͣ����
                {
                    start = false;
                    Player.GetComponent<PlayerController>().startDieOrPassDetect(); //�����������
                    break; // ֹͣ��������������ײ
                }
            }
            //��Ҫһ���ߵ�Movable�Ӷ��д���
            while (toAddQueue.Count > 0)
            {
                objectList.AddLast((GameObject)toAddQueue.Dequeue());
            }
        }
    }
}
