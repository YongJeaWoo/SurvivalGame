using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool isLoader = true;

    [Header ("Fade Effect && UI")]
    public Image fadeOut;
    public GameObject fadeOutGo;
    public GameObject inGameInfo;
    public Image firstDis;

    [Header("InGame Units")]
    public AudioSource backgroundSound;

    [Header("Player Info")]
    public Player player;
    public Text playerHpTxt;
    public Text playerAmmoTxt;

    [Header("Inven Window")]
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;

    [Header ("Conversation Info")]
    public Text nameText;
    public Text descriptionText;
    public GameObject scanObject;
    public Animator anim;

    [Header("Info UI Off")]
    public GameObject Icon;
    public TextMeshProUGUI interName;
    public GameObject uiOff;
    public bool isAction;
    public int talkIndex;

    [Header("Another Manager")]
    public TalkManager talkManager;

    // 싱글톤
    private static GameManager instance = null;

    private void Start()
    {
        QuestManager.Instance.CheckQuest();
    }

    private void Awake()
    {
        if (null == instance)
            instance = this;

        backgroundSound.Play();
        StartCoroutine(ShowDisplayInfo());
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
                instance = new GameManager();
            return instance;
        }
    }

    IEnumerator ShowDisplayInfo()
    {
        yield return new WaitForSeconds(1f);
        firstDis.gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        firstDis.gameObject.SetActive(false);
        inGameInfo.gameObject.SetActive(true);

        yield break;
    }

    private void LateUpdate()
    {
        playerHpTxt.text = player.hp.ToString();

        if (player.equipWeapons == null)
            playerAmmoTxt.text = " - / " + player.ammo;
        else if (player.equipWeapons.type == Weapon.Type.Melee)
            playerAmmoTxt.text = " - / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapons.curAmmo + " / " + player.ammo;

        // 무기를 가지고 있는지 없는지 알파값으로 표시
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenades > 0 ? 1 : 0);
    }

    public void ChangeEndingScene(string sceneName)
    {
        if (isLoader)
        {
            StartCoroutine(Changing(sceneName));
            isLoader = false;
        }
    }

    IEnumerator Changing(string sceneName)
    {
        fadeOut.gameObject.SetActive(true);
        fadeOutGo.gameObject.SetActive(true);
        uiOff.SetActive(false);
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeOut.color = new Color(0, 0, 0, fadeCount);
        }

        SceneManager.LoadScene(sceneName);
    }    

    // 대화창 표시
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData);
    }

    void Talk(ObjData objData)
    {
        int id = objData.id;
        bool isNpc = objData.isNpc;
        string npcName = objData.npcName;

        int questIndex = QuestManager.Instance.GetQuestIndex(id);
        string talkData = talkManager.GetTalk(id + questIndex, talkIndex);

        // 대화가 더 이상 없다면
        if (talkData == null)
        {
            isAction = false;
            uiOff.SetActive(true);
            anim.SetBool("isOpen", false);
            talkIndex = 0;      // 대화 초기화
            QuestManager.Instance.CheckQuest(id);
            Time.timeScale = 1;
            return;
        }

        if (isNpc)
        {
            nameText.text = npcName;
            descriptionText.text = talkData;
        }
        else
        {
            nameText.text = "";
            descriptionText.text = talkData;
        }

        Time.timeScale = 0;
        isAction = true;
        talkIndex++;            // 대화 더 있으면 계속 증가
        uiOff.SetActive(false);
        anim.SetBool("isOpen", true);
    }
}
