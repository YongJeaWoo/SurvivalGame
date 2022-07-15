using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogOpenTrigger : MonoBehaviour
{
    public Dialogue info;

    public void Trigger()
    {
        var system = FindObjectOfType<DialogOpen>();
        system.Begin(info);
        gameObject.SetActive(false);
    }
}
