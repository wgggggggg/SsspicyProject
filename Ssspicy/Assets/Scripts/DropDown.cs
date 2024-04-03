using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDown : MonoBehaviour
{
    Vector2 nowPosition;

    // Start is called before the first frame update

    bool start = false;

    public void dropStart()
    {
        start = true;
    }
    void Start()
    {
        if (!start)
        {
            return;
        }
        nowPosition = transform.position;
        transform.position += (Vector3)Vector2.up * 15.0f;
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
            transform.position += Time.deltaTime * 24.0f * (Vector3)Vector2.down;
        } else
        {
            start = false;
        }
    }
}
