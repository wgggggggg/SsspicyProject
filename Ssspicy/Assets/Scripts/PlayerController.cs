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
    private Vector2 nowDir;
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
        //玩家能否前进一步(排除地面层)
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)dir * 0.3f, dir, 0.7f, otherLayer);
        if (!hit)
        {
            Move(dir);
        } else { //判断遇到的这个Body是不是最后一个Body
            if (hit.collider.GetComponent<Body>() != null)
            {
                if(bodyNum(hit) != 1 && isLastBody(hit))
                {
                    Move(dir);
                }
                //是食物的话就调用食物的MoveOrEaten
            } else if (hit.collider.GetComponent<Food>() != null)
            {   
                //吃东西或者推东西的时候伴随的移动在Food里调用
                hit.collider.GetComponent<Food>().MoveOrEaten(dir);
                //是其它Movable就调用一般的MoveIfCan
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
        //先移动身体，后移动头部
        PlayerBody.GetComponent<PlayerBodyController>().moveBody(dir);
        transform.Translate(dir);
        moveAnimDetect(dir);
        nowDir = dir;
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
        dieOrPassDetect = false; //飞行的时候关闭死亡和过关检测
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
                StartCoroutine(Fall());
            }
            if (shouldPass())
            {
                StartCoroutine(enterHoleAnimAndResult(nowDir));
            }
            if (shouldDie())
            {
                StartCoroutine(enterHoleAnimAndResult(nowDir));
            }
        }
    } //之后还需要加上洞口等

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

    void Die()
    {
        PlayerController.pausePlayerControl(true);
        LevelControl.DieScene();
    }
    
    void Pass()
    {
        LevelControl.startNextLevel();
    }

    IEnumerator enterHoleAnimAndResult(Vector2 dir)
    {
        pausePlayerControl(true);
        RaycastHit2D hitBunker = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, bunkerLayer);
        RaycastHit2D hitHole = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, holeLayer);
        GameObject hole;
        string result;
        if (hitBunker)
        {
            hole = hitBunker.transform.gameObject;
            result = "die";
        } else
        {
            hole = hitHole.transform.gameObject;
            result = "pass";
            hole.GetComponent<Hole>().HoleShapeChange(dir);
        }
        Vector2 holePosition = hole.transform.position;
        int bodyNum = PlayerBody.transform.childCount;
        for (int i = bodyNum - 1; i >= 0; i--)
        {
            for (int j = bodyNum - 1; j >= 1; j--)
            {
                Transform nowBody = PlayerBody.transform.GetChild(j);
                Transform nextBody = PlayerBody.transform.GetChild(j - 1);
                nowBody.position = nextBody.position;
            }
            if (i == bodyNum - 1)
            {
                PlayerBody.transform.GetChild(0).position = transform.position;
            }
            yield return new WaitForSeconds(0.05f);
        }

        if (result == "die")
        {
            Die();
        } else if (result == "pass")
        {
            Pass();
        }
        pausePlayerControl(false);
    }

    IEnumerator Fall()
    {
        pausePlayerControl(true);
        yield return new WaitForSeconds(0.5f);
        LinkedList<GameObject> dropList = new LinkedList<GameObject>();
        dropList.AddLast(gameObject);
        for (int i = 0; i < PlayerBody.transform.childCount; i++)
        {
            dropList.AddLast(PlayerBody.transform.GetChild(i).gameObject);
        }
        int dropTimes = 0;
        while (dropTimes < 100)
        {
            dropTimes++;
            foreach (GameObject gb in dropList)
            {
                gb.transform.Translate(40.0f * Vector2.down * Time.deltaTime);
            }
            yield return new WaitForSeconds(0.01f);
        }
        Pass();
        pausePlayerControl(false);
    }

}
