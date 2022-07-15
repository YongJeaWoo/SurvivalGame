using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : EnemyMissileTrigger
{
    Rigidbody rigid;
    float angularPower = 2;
    float scaleValue = 0.08f;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        // StartCoroutine(GainPowerTime());
        StartCoroutine(GainPower());
    }


    IEnumerator GainPower()
    {
        while (scaleValue <= 5f)
        {
            angularPower += 1f * Time.deltaTime;
            scaleValue += 2f * Time.deltaTime;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}
