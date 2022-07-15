using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Info")]
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    public int enemyACount;
    public int enemyBCount;
    public int enemyCCount;

    [Header("Unit Info")]
    public Player player;
    Boss boss;

    [Header("Enemy Spawn Info")]
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;

    [Header("Boss Spawn Info")]
    public Transform bossZone;
    public GameObject finalBoss;
    public RectTransform bossHpGroup;
    public RectTransform bossCurHp;
    public bool bossAppear = false;

    // 싱글톤
    private static SpawnManager instance = null;

    private void Awake()
    {
        // 싱글톤
        if (null == instance)
        {
            instance = this;
        }

        enemyList = new List<int>();
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    public static SpawnManager Instance
    {
        get
        {
            if (null == instance)
                instance = new SpawnManager();
            return instance;
        }
    }

    private void LateUpdate()
    {
        enemyATxt.text = enemyACount.ToString();
        enemyBTxt.text = enemyBCount.ToString();
        enemyCTxt.text = enemyCCount.ToString();

        HPBossBar();
    }

    IEnumerator Spawn()
    {
        while (!bossAppear)
        {
            int ran = Random.Range(0, 3);
            enemyList.Add(ran);

            switch (ran)
            {
                case 0:
                    enemyACount++;
                    break;
                case 1:
                    enemyBCount++;
                    break;
                case 2:
                    enemyCCount++;
                    break;
            }

            while (enemyList.Count > 0)
            {
                int ranZone = Random.Range(0, 3);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ranZone].position, enemyZones[ranZone].rotation);
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemyList.RemoveAt(0);

                yield return new WaitForSeconds(15f);
            }
        }
    }

    public void SpawnBoss()
    {
        if (bossAppear)
            return;

        bossAppear = true;

        // 생성
        GameObject instantBoss = Instantiate(finalBoss, bossZone.position, bossZone.rotation);
        instantBoss.transform.Rotate(Vector3.up * 180);
        boss = instantBoss.GetComponent<Boss>();
        boss.target = player.transform;
    }

    public void DeleteBoss()
    {
        bossAppear = false;
        boss = null;
    }

    public void HPBossBar()
    {
        if (null != boss)
        {
            bossHpGroup.anchoredPosition = Vector3.down;
            bossCurHp.localScale = new Vector3((float)boss.curHp / boss.maxHp, 1, 1);
        }
        else
        {
            bossHpGroup.anchoredPosition = Vector3.up * 180;
        }
    }
}
