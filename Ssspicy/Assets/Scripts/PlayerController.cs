using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir;
    public GameObject PlayerBody;
    public LayerMask groundLayer;
    public LayerMask bunkerLayer;
    public LayerMask holeLayer;
    public LayerMask otherLayer;
    private bool dieOrPassDetect;
    public static bool shouldPausePlayerControl;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        dieOrPassDetect = true;
        shouldPausePlayerControl = true;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldPausePlayerControl)
        {
            return;
        }
        moveDetect();
        dieOrPassDetectIfStart();
        bodyNumDetect();
    }

    void moveDetect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDir = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDir = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir = Vector2.left;
        }
        if (moveDir != Vector2.zero)
        {
            MoveOrEat(moveDir);
        }
        moveDir = Vector2.zero;
        //animator.SetBool("startMove", false);
    }

    void MoveOrEat(Vector2 dir)
    {   
        //����ܷ�ǰ��һ��(�ų������)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.3f, dir, 0.7f, otherLayer);
        if (!hit)
        {
            Move(dir);
        } else { //�ж����������Body�ǲ������һ��Body
            if (hit.collider.GetComponent<Body>() != null)
            {
                if(bodyNum(hit) != 1 && isLastBody(hit))
                {
                    Move(dir);
                }
                //��ʳ��Ļ��͵���ʳ���MoveOrEaten
            } else if (hit.collider.GetComponent<Food>() != null)
            {   
                //�Զ��������ƶ�����ʱ�������ƶ���Food�����
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                //������Movable�͵���һ���MoveIfCan
            } else if (hit.collider.GetComponent<Movable>() != null)
            {
                if (hit.collider.GetComponent<Movable>().MoveIfCan(dir))
                {
                    Move(dir);
                }
            }
        }
    }

    public void Move(Vector2 dir)
    {
        //���ƶ����壬���ƶ�ͷ��
        PlayerBody.GetComponent<PlayerBodyController>().moveBody(dir);
        transform.Translate(dir);
        moveAnimDetect(dir);
    }

    bool InGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    bool shouldFall()
    {
        if (!InGround() && PlayerBody.GetComponent<PlayerBodyController>().BodyOutGround())
        {
            return true;
        }
        return false;
    }

    bool shouldDie()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, bunkerLayer);
        if (hit) { return true; }
        return false;
    }

    bool shouldPass()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, holeLayer);
        if (hit && hit.collider.GetComponent<Hole>().IsOpen())
        {
            return true;
        }
        return false;
    }

    public void Fly(Vector2 dir)
    {
        GetComponent<Fly>().FlyStart(dir);
        dieOrPassDetect = false; //���е�ʱ��ر������͹��ؼ��
    } 

    public void startDieOrPassDetect()
    {
        dieOrPassDetect = true;
    }

    private void dieOrPassDetectIfStart()
    {
        if (dieOrPassDetect)
        {
            if (shouldFall())
            {
                StartCoroutine(Die());
            }
            if (shouldPass())
            {
                LevelControl.startNextLevel();
            }
            if (shouldDie())
            {
                StartCoroutine(Die());
            }
        }
    } //֮����Ҫ���϶��ڵ�

    bool isLastBody(RaycastHit2D hit)
    {
        GameObject hitObject = hit.collider.gameObject;
        Transform bodyParent = hit.collider.transform.parent;
        int childCount = bodyParent.childCount;
        int indexOfHitObject = hitObject.transform.GetSiblingIndex();
        return (indexOfHitObject == childCount - 1);
    }

    int bodyNum(RaycastHit2D hit)
    {
        Transform bodyParent = hit.collider.transform.parent;
        int childCount = bodyParent.childCount;
        return childCount;
    }

    public static void pausePlayerControl(bool pause)
    {
        if (pause)
        {
            shouldPausePlayerControl = true;
        } else
        {
            shouldPausePlayerControl = false;
        }
    }

    void bodyNumDetect()
    {
        int bodyNum = PlayerBody.GetComponent<PlayerBodyController>().BodyNum();
        if (bodyNum == 0)
        {
            animator.SetBool("headOnly", true);
        } else
        {
            animator.SetBool("headOnly", false);
        }
    }

    public void moveAnimDetect(Vector2 dir)
    {
        if (dir == Vector2.up)
        {
            animator.SetInteger("Direction", 1);
        } else if (dir == Vector2.right)
        {
            animator.SetInteger("Direction", 2);
        } else if (dir == Vector2.down)
        {
            animator.SetInteger("Direction", 3);
        } else if (dir == Vector2.left)
        {
            animator.SetInteger("Direction", 4);
        }
        animator.SetBool("startMove", true);
    }

    IEnumerator Die()
    {
        PlayerController.pausePlayerControl(true);
        yield return new WaitForSeconds(0.5f);
        LevelControl.DieScene();
    }

}
