using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEndingTrigger : MonoBehaviour
{
    public Dialogue info;

    public void Trigger()
    {
        var system = FindObjectOfType<DialogEnding>();
        system.Begin(info);
        gameObject.SetActive(false);
    }
}
