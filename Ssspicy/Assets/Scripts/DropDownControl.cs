using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownControl : MonoBehaviour
{
    public GameObject[] dropDownGameObjects;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDropDownAnimations());
        StartCoroutine(continuePlayerControl());
    }

    IEnumerator StartDropDownAnimations()
    {
        foreach (var gameObject in dropDownGameObjects)
        {
            PlayerController.pausePlayerControl(true);
            gameObject.GetComponent<DropDown>().dropStart();
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator continuePlayerControl()
    {

        yield return new WaitForSeconds(dropDownGameObjects.Length * 0.5f);
        PlayerController.pausePlayerControl(false);
    }
}
