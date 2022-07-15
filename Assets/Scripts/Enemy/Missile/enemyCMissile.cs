using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCMissile : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.right * 80f * Time.deltaTime);
    }
}
