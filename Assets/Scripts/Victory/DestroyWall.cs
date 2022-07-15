using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    private void Update()
    {
        var pos = SpawnManager.Instance.finalBoss.transform.position;
        var getPos = SpawnManager.Instance.bossZone.transform.position;
        var bossTrue = SpawnManager.Instance.bossAppear;

        if (bossTrue && pos != getPos)
            Destroy(gameObject);
    }
}
