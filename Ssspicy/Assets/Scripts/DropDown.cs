using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    Vector2 nowPosition;
    // Start is called before the first frame update

    bool start = false;

    public void dropStart(Vector2 position)
    {
        nowPosition = position;
        start = true;
        Start();
    }
    void Start()
    {
        if (!start)
        {
            return;
        }
        transform.Translate(Time.deltaTime * 24.0f * Vector2.down);
    }

    // Update is called once per frame
    void Update()
    {
        if (!start)
        {
            return;
        }
        if (nowPosition.y < transform.position.y)
        {
            transform.Translate(Time.deltaTime * 24.0f * Vector2.down);
        } else
        {
            start = false;
        }
    }
}
