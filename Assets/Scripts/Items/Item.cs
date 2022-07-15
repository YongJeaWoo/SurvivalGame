using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 무기, 코인, 탄, 체력
    public enum Type
    {
        Weapon,
        Grenade,
        Ammunition,
        Hp,
    }
    // 아이템 값과 그 변수
    public Type type;
    public int value;
    public int index;

    private void Update()
    {
        transform.Rotate(Vector3.up * 10 * Time.deltaTime);
    }
}
