using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fly : MonoBehaviour
{
    public Vector2 flyDirection; // ���з���
    private float speed = 5; // �����ٶ�
    public GameObject PlayerBody;
    public GameObject Player;
    public LayerMask FlyDetectLayer; //������������Դ�  Ҳ������ɳ�Ӷ���
    public GameObject firePrefab;
    public GameObject fire;
    private int maxDist = 25;
    private Vector2 startPosition;
    bool start = false;
    LinkedList<GameObject> objectList;
    Queue<GameObject> toAddQueue;
    HashSet<GameObject> added;
    public void FlyStart(Vector2 dir)
    {
        PlayerController.pausePlayerControl(true);
        StartCoroutine(PauseForSeconds(0.5f));
        Player.GetComponent<Animator>().SetBool("flyStop", false);
        Player.GetComponent<Animator>().SetBool("spicyEat", true);
        Player.GetComponent<Animator>().SetBool("startMove", false);
        fire = Instantiate(firePrefab, transform);
        fire.transform.position = transform.position + (Vector3)dir * -1.75f;
        fire.GetComponent<Fire>().ChangeSprite(dir);
        flyDirection = dir;
        start = true;
        Start();
    }
    void Start()
    {
        objectList = new LinkedList<GameObject> ();
        toAddQueue = new Queue<GameObject> ();
        added = new HashSet<GameObject> ();
        startPosition = transform.position;
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
                if ((startPosition - (Vector2)transform.position).magnitude > maxDist)
                {
                    LevelControl.DieScene();
                }
                //�������Movable�ʹ���һ����(�������),���Ҽ���HashSet,��ֹ�ظ�����objectList
                if (hit.collider != null && hit.collider.GetComponent<Movable>() != null && !added.Contains(hit.collider.gameObject))
                {
                    toAddQueue.Enqueue(hit.collider.gameObject);
                    added.Add(hit.collider.gameObject);
                }
                if (hit.collider != null && hit.collider.GetComponent<Movable>() == null) //���������������ͣ����
                {
                    flyStopFunction();
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

    void flyStopFunction()
    {
        Player.GetComponent<Animator>().SetBool("flyStop", true);
        Player.GetComponent<Animator>().SetBool("spicyEat", false);
        start = false;
        Player.GetComponent<PlayerController>().startDieOrPassDetect(); //�����������
        PlayerController.pausePlayerControl(false);
        Destroy(fire);
    }

    IEnumerator PauseForSeconds(float seconds)
    {
        Time.timeScale = 0f; // ʱ����ͣ
        yield return new WaitForSecondsRealtime(seconds); // �ȴ�ʵʱʱ��
        Time.timeScale = 1f; // �ָ�ʱ��
    }
}
