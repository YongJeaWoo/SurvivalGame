using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questOrder;      // 퀘스트 대화 순서

    public GameObject lunaNPC;

    Dictionary<int, QuestData> questList;

    // 싱글톤
    private static QuestManager instance = null;
    public static QuestManager Instance
    {
        get
        {
            if (null == instance)
                instance = new QuestManager();
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }

        questList = new Dictionary<int, QuestData>();   // 생성
        GenerateData();
    }

    void GenerateData()
    {
        questList.Add(10, new QuestData("이곳을 지나갈 방법을 알 사람을 찾아보자",
                                        new int[] { 100 }));

        questList.Add(20, new QuestData("동생을 찾아보자", 
                                        new int[] {1000, 2000}));

        questList.Add(30, new QuestData("보스를 잡자",
                                        new int[] {2000, 5000}));

        questList.Add(40, new QuestData("퀘스트 클리어",
                                        new int[] {2000}));

        questList.Add(50, new QuestData("NPC와의 대화",
                                        new int[] {2000, 1000}));

        questList.Add(60, new QuestData("엔딩 분기 시작",
                                        new int[] {2000, 2000, 2000}));

        questList.Add(70, new QuestData("엔딩을 향해서",
                                        new int[] {0}));
    }

    public int GetQuestIndex(int id)
    {
        return questId + questOrder;
    }

    public string CheckQuest(int id)
    {
        if (id == questList[questId].npcId[questOrder])
        {
            questOrder++;
        }

        if (questOrder == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        ControlObject();

        return questList[questId].questName;
    }

    public string CheckQuest()
    {
        return questList[questId].questName;
    }

    void NextQuest()
    {
        questId += 10;
        questOrder = 0;
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                break;
            case 20:
                break;
            case 30:
                {
                    if (questOrder == 1)    // 진행도
                    {
                        SpawnManager.Instance.SpawnBoss();
                    }
                }
                break;
            case 40:
                SpawnManager.Instance.DeleteBoss();
                lunaNPC.transform.position = new Vector3(-27, 0.15f, -31);
                lunaNPC.transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case 50:
                break;
            case 60:
                break;
            case 70:
                break;
        }
    }
}
