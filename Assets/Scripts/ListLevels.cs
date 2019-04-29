using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListLevels : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i <= PlayerPrefs.GetInt("levelCompletion"))
            {
                transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
        }
    }
}
