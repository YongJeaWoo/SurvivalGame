using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCol : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            return;
    }
}
