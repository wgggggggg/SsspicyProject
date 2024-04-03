using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownControl : MonoBehaviour
{
    public GameObject[] dropDownGameObjects;
    private Queue<Vector2> targetPositions;
    // Start is called before the first frame update
    void Start()
    {
        targetPositions = new Queue<Vector2>();
        PlayerController.pausePlayerControl(true);
        Init();
        StartCoroutine(StartDropDownAnimations());
        StartCoroutine(continuePlayerControl());
    }

    IEnumerator StartDropDownAnimations()
    {
        foreach (var gameObject in dropDownGameObjects)
        {
            PlayerController.pausePlayerControl(true);
            gameObject.GetComponent<DropDown>().dropStart(targetPositions.Peek());
            targetPositions.Dequeue();
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator continuePlayerControl()
    {

        yield return new WaitForSeconds(dropDownGameObjects.Length * 0.1f);
        PlayerController.pausePlayerControl(false);
    }

    void Init()
    {
        foreach (var gameObject in dropDownGameObjects)
        {
            targetPositions.Enqueue(gameObject.transform.position);
            gameObject.transform.Translate(Vector2.up * 24);
        }
    }
}
