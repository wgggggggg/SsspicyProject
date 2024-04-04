using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    Vector2 StartPosition;
    Vector2 TargetPosition;
    float dropSpeed = 30.0f;
    // Start is called before the first frame update

    bool start = false;

    public void dropStart(Vector2 startP, Vector2 targetP)
    {
        StartPosition = startP;
        TargetPosition = targetP;
        start = true;
        Start();
    }

    public void dropStart(Vector2 targetP)
    {
        StartPosition = transform.position;
        TargetPosition = targetP;
        start = true;
        Start();
    }
    void Start()
    {
        if (!start)
        {
            return;
        }
        transform.position = StartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            return;
        }
        if (TargetPosition.y < transform.position.y)
        {
            transform.Translate(Time.deltaTime * dropSpeed * Vector2.down);
        } else
        {
            start = false;
        }
    }
}
