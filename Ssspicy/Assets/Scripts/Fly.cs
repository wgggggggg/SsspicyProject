using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Vector2 flyDirection; // 飞行方向
    public float speed; // 飞行速度
    public GameObject PlayerBody;
    public GameObject Player;
    public LayerMask FlyDetectLayer; //不包括身体和脑袋  也不包括沙坑洞口
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
                //移动该对象
                gb.transform.Translate(flyDirection * Time.deltaTime * speed);
                // 发射一条射线，检测与其他物体的接触
                RaycastHit2D hit = Physics2D.Raycast(gb.transform.position + (Vector3)flyDirection * 0.5f, flyDirection, 0.25f, FlyDetectLayer);
                //如果碰到Movable就带着一起走(先入队列),并且加入HashSet,防止重复进入objectList
                if (hit.collider != null && hit.collider.GetComponent<Movable>() != null && !added.Contains(hit.collider.gameObject))
                {
                    toAddQueue.Enqueue(hit.collider.gameObject);
                    added.Add(hit.collider.gameObject);
                }
                if (hit.collider != null && hit.collider.GetComponent<Movable>() == null) //碰到了其他物体就停下来
                {
                    start = false;
                    Player.GetComponent<PlayerController>().startDieOrPassDetect(); //启动死亡检测
                    break; // 停止检测其他物体的碰撞
                }
            }
            //将要一起走的Movable从队列带走
            while (toAddQueue.Count > 0)
            {
                objectList.AddLast((GameObject)toAddQueue.Dequeue());
            }
        }
    }
}
