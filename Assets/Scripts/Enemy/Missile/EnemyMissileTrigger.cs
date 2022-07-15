using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissileTrigger : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
        else if (other.gameObject.CompareTag("Objects"))
            Destroy(gameObject);
    }
}
