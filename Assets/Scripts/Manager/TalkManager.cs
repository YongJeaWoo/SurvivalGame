using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkManager : MonoBehaviour
{
    // 대화 값 넣기
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();             // 빈 공간 생성
        GenerateData();
    }

    // 대화 시작
    void GenerateData()
    {
        // 일반 대화
        talkData.Add(100, new string[] { "무언가의 숨소리가 들린다.." });
        talkData.Add(200, new string[] { "이곳으로 지나갈 수 없어.." });

        talkData.Add(1000, new string[] {"빨리 돌아가야 해.."});
        talkData.Add(2000, new string[] {"훌쩍...", "언제 오는 거야.."});  // 동생


        // 퀘스트 대화
        talkData.Add(5000, new string[] {"괴물은 나에게 죽어 싸늘한 시체로 남아있다."});       // 보스

        talkData.Add(100 + 10, new string[] {"무언가가 막고 있어 갈 수가 없다."});

        talkData.Add(1000 + 20, new string[] {"으악! 깜짝이야.", "맞다. \n혹시 한 여자아이를 못 봤어?",
                                        "내 여동생인데 그만 헤어지게 되어버렸어..",
                                        "동생이 많이 아파서 뛰지도 못하는 상황인데..",
                                        "혹시 찾으면 나에게 찾아올 수 있을까?",
                                        "호텔을 기준으로 1시방향쪽에 우리집이 있어.",
                                        "아마 그 근처에 있을거야."});

        talkData.Add(1000 + 20 + 1, new string[] {"어서 가 줘.",
                                                  "난 여기서 무기를 좀 찾아봐야겠어."});

        talkData.Add(2000 + 20 + 1, new string[] {"지금이라도 찾으러 가 봐야 하나.."});

        talkData.Add(2000 + 30, new string[] {"그쪽은 누구야..?",
                                            "나를 찾고 있는 사람이 있다고?",
                                            "이 상황에 찾는 건 아마 내 오빠겠지?",
                                            "하지만 난 다리를 다쳐서...", 
                                            "거기까지 같이 가다가 괜히 발목만 잡을 거야.",
                                            "난 여기 있을 테니 \n우리 오빠를 데리고 같이 와 줬으면 해.",
                                            "맞다! 이 마을을 나갈 수 있는 유일한 길목에 큰 괴물이 있어..",
                                            "그 괴물을 '움직이게 하면' 그 길로 지나갈 수 있을 거 같아...",
                                            "그럼 부탁할게."});

        talkData.Add(1000 + 30 + 1, new string[] {"어서 가 줘.",
                                                  "난 여기서 무기를 좀 찾아봐야겠어."});

        talkData.Add(2000 + 30 + 1, new string[] { "난 어서 들어가봐야겠어. 내 걱정은 하지 마." });

        talkData.Add(1000 + 40, new string[] { "...",
                                                "그는 싸늘한 시체로 있다."});

        talkData.Add(2000 + 40, new string[] {"흑..흑.."});
        talkData.Add(2000 + 40 + 1, new string[] { "어째서.." });


        talkData.Add(1000 + 50, new string[] { "...",
                                                "그는 싸늘한 시체로 있다."});

        talkData.Add(2000 + 50, new string[] { "난.. 여기 남아있을게.. 그냥 가줘.." });
        talkData.Add(2000 + 50 + 1, new string[] { "제발 그냥 가.." });


        talkData.Add(1000 + 60, new string[] { "...",
                                                "그는 싸늘한 시체로 있다."});

        talkData.Add(2000 + 60, new string[] { "그 큰 괴물이 막고 있던 길은 뚫렸을거야..",
                                                "나는.. 잘 모르겠어.."});

        talkData.Add(2000 + 60 + 1, new string[] {"이제 더 이상 건들지 말아줘."});
        talkData.Add(2000 + 60 + 2, new string[] { "이제 더 이상 건들지 말아줘." });
        
        talkData.Add(1000 + 70, new string[] {"...",
                                                "그는 싸늘한 시체로 있다."});

        talkData.Add(2000 + 70, new string[] { "이제 더 이상 건들지 말아줘." });
    }

    // 대화를 받기
    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            // 퀘스트 진행 순서 대사가 없을 경우
            // 퀘스트 맨 처음 대사 가지고 오기
            // 아니면 기존 대사 가져오기
            if (!talkData.ContainsKey(id - id % 10))
                return GetTalk(id - id % 100, talkIndex);
            else
                return GetTalk(id - id % 10, talkIndex);
        }

        // 대화가 끝났나?
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }
}
