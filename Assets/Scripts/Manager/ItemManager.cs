using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] items;
    public Transform[] itemZones;
    public List<int> itemList;
    public List<int> checkList;
    public int itemRespawn = 8;

    public int ranZone = -1;

    // 싱글톤
    private static ItemManager instance = null;

    public static ItemManager Instance
    {
        get
        {
            if (null == instance)
                instance = new ItemManager();
            return instance;
        }
    }


    private void Awake()
    {
        // 싱글톤
        if (null == instance)
            instance = this;

        itemList = new List<int>();
        checkList = new List<int>();

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(8f);

        while(!SpawnManager.Instance.bossAppear)
        {
            int ranItem = Random.Range(0, 3);
            itemList.Add(ranItem);
            bool isTrue = true;

            while (isTrue)
            {
                ranZone = Random.Range(0, itemRespawn);
                isTrue = false;
                foreach (var item in checkList)
                {
                    if (ranZone == item)
                        isTrue = true;
                }
            }
            GameObject instantItem = Instantiate(items[itemList[0]], itemZones[ranZone].position, itemZones[ranZone].rotation);
            instantItem.GetComponent<Item>().index = ranZone;
            checkList.Add(ranZone);

            itemList.RemoveAt(0);
            if (checkList.Count == itemRespawn)
            {
                StartCoroutine(Check());
                yield break;
            }
            yield return new WaitForSeconds(10f);
        }
    }

    IEnumerator Check()
    {
        while (true)
        {
            if (checkList.Count < itemRespawn)
            {
                StartCoroutine(Spawn());

                yield break;
            }
            yield return null;
        }
    }
}
