using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 8f;

    public GameManager manager;
    public TalkManager talkManager;

    float hAxis;
    float vAxis;

    bool walkDown;
    bool jumpDown;
    bool dodgeDown;
    bool grenadeDown;

    bool interAction;

    bool isJump;
    bool isDodge;
    bool isDead;

    bool isBorder;

    // 무기 스왑
    bool isSwap;
    bool swap1;
    bool swap2;
    bool swap3;

    // 공격
    bool fireDown;
    bool isFire = true;
    float fireDelay;

    // 장전
    bool ReloadDown;
    bool isReload = false;

    // 무적
    bool isDamage;

    bool isMove = false;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    GameObject scanObject;

    GameObject objs;
    public Weapon equipWeapons;
    MeshRenderer[] meshRender;

    int equipWeaponIndex = -1;

    // 사운드
    [Header("Sounds Info")]
    public AudioSource tileSound;
    public AudioSource grassSound;
    public AudioSource jumpSound;
    public AudioSource dodgeSound;
    public AudioSource reloadSound;

    public int ammo;
    public int maxAmmo;
    public int hp;
    public int maxHp;
    public int hasGrenades;
    public int maxHasGrenades;

    [Header("Weapons Info")]
    public GameObject[] weapons;
    public bool[] hasWeapons;

    [Header("Grenade Info")]
    public GameObject[] grenades;
    public GameObject grenadeObj;
    public Transform grenadePos;

    public Camera cam;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshRender = GetComponentsInChildren<MeshRenderer>();
    }

    private void Update()
    {
        InputGet();
        Move();
        if (!isJump)
            Jump();
        Attack();
        Grenade();
        Reload();
        Dodge();
        InterAction();
        Swap();
    }

    // 플레이어 충돌 수정 

    // 충돌 회전 방지
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    // 벽 충돌 뚫기 방지
    void StopWall()
    {
        Vector3 rayVec = transform.position;
        rayVec.y += 0.5f;
        Debug.DrawRay(rayVec, transform.forward * 1f, Color.green);
        isBorder = Physics.Raycast(rayVec, transform.forward, 1f, LayerMask.GetMask("Border", "Objects", "NPC"));
    }

    void Scanning()
    {
        RaycastHit rayHit;
        Vector3 searchVec = transform.position;
        searchVec.y += 0.3f;
        Debug.DrawRay(searchVec, transform.forward * 1.5f, Color.red);
        bool check = Physics.Raycast(searchVec, transform.forward, out rayHit, 1.5f, LayerMask.GetMask("NPC", "EnemyBossDead", "Border"));

        if (check)
        {
            manager.Icon.SetActive(true);

            if (GameManager.Instance.isAction)
            {
                manager.Icon.SetActive(false);
            }

            //GameObject obj = manager.Icon.transform.GetChild(0).GetChild(0).gameObject;
            //obj.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.interName;
            scanObject = rayHit.collider.gameObject;
            ObjData data = scanObject.GetComponent<ObjData>();
            if (data.isNpc)
                GameManager.Instance.interName.text = data.npcName;
            else
                GameManager.Instance.interName.text = "살펴보기";
        }
        else
        {
            manager.Icon.SetActive(false);
            scanObject = null;
        }
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopWall();
        // Scanning();
    }

    private void LateUpdate()
    {
        Scanning();
    }

    // 충돌 수정 끝

    void InputGet()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        fireDown = Input.GetButton("Fire1");
        grenadeDown = Input.GetButtonDown("Fire2");
        ReloadDown = Input.GetButtonDown("Reload");
        dodgeDown = Input.GetButtonDown("Dodge");
        interAction = Input.GetButtonDown("InterAction");
        swap1 = Input.GetButtonDown("Swap1");
        swap2 = Input.GetButtonDown("Swap2");
        swap3 = Input.GetButtonDown("Swap3");
    }

    void Swap()
    {
        if (swap1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (swap2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (swap3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        
        if (swap1) weaponIndex = 0;
        if (swap2) weaponIndex = 1;
        if (swap3) weaponIndex = 2;

        if ((swap1 || swap2 || swap3) && !isJump && !isDodge && !isSwap)
        {
            if (equipWeapons != null)
                equipWeapons.gameObject.SetActive(false);         

            equipWeaponIndex = weaponIndex;
            equipWeapons = weapons[weaponIndex].GetComponent<Weapon>();       // 해당 무기 가지기
            equipWeapons.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapTime", 0.4f);
        }
    }

    void SwapTime()
    {
        isSwap = false;
    }

    void Move()
    {
        // 움직이기
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        moveVec = Quaternion.Euler(0, cam.transform.rotation.eulerAngles.y, 0) * moveVec;

        // 회피하면서 방향 전환?
        if (isDodge)
            moveVec = dodgeVec;

        // 움직임 제한
        if (isSwap || !isFire || isDead || isReload || manager.isAction)
        {
            moveVec = Vector3.zero;
        }

        if (moveVec.sqrMagnitude < 0.01f)
            isMove = false;
        else
            isMove = true;

        if (!isBorder)
            transform.position += moveVec * moveSpeed * (walkDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", walkDown);

        // 회전
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jumpDown && !isDodge && !isReload && !isDead && (!manager.isAction))
        {
            // 즉각 반응
            isJump = true;
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            rigid.angularVelocity = Vector3.zero;       // 관성 움직임 없애기
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            jumpSound.Play();
        }
    }

    void Attack()
    {
        if (equipWeapons == null)
            return;

        if (isReload)
            return;

        fireDelay += Time.deltaTime;
        isFire = equipWeapons.rate < fireDelay;

        if (fireDown && isFire && !isDodge && !isSwap && !isDead && !(manager.isAction))
        {
            equipWeapons.Use();
            anim.SetTrigger(equipWeapons.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }
    
    void Grenade()
    {
        if (hasGrenades == 0)
            return;
        if (grenadeDown && !isSwap && !isDodge && !isReload && !isDead && !(manager.isAction))
        {
            Debug.Log("던짐");
            GameObject instantGrenade = Instantiate(grenadeObj, grenadePos.position, grenadePos.rotation);
            Rigidbody grenadeRigid = instantGrenade.GetComponent<Rigidbody>();

            Vector3 grenadeVec = grenadePos.forward * 10f + Vector3.up * 5f;

            grenadeRigid.AddForce(grenadeVec, ForceMode.Impulse);
            grenadeRigid.AddTorque(Vector3.back * 20f, ForceMode.Impulse);

            hasGrenades--;
            grenades[hasGrenades].SetActive(false);
        }
    }

    void Reload()
    {
        if (equipWeapons == null)
            return;

        if (equipWeapons.type == Weapon.Type.Melee)
            return;

        if (ammo == 0)
            return;

        if (equipWeapons.curAmmo == equipWeapons.maxAmmo)
            return;

        if (ReloadDown && !isReload && !isJump && !isDodge && !isSwap && isFire && !isDead && !(manager.isAction))
        {
            isReload = true;
            reloadSound.Play();
            anim.SetTrigger("doReload");

            Invoke("Reloading", 2.5f);
        }
    }

    void Reloading()
    {
        isReload = false;
        int reAmmo = ammo + equipWeapons.curAmmo < equipWeapons.maxAmmo ? ammo : equipWeapons.maxAmmo - equipWeapons.curAmmo;

        equipWeapons.curAmmo += reAmmo;
        ammo -= reAmmo;
    }

    void Dodge()
    {
        if (dodgeDown && !isJump && moveVec != Vector3.zero && !isDodge && !isDead && !(manager.isAction))
        {
            dodgeVec = moveVec;

            moveSpeed *= 2f;
            anim.SetTrigger("doDodge");
            isDodge = true;
            dodgeSound.Play();

            // 시간차 호출
            Invoke("DodgeTime", 0.7f);
        }
    }

    void DodgeTime()
    {
        moveSpeed *= 0.5f;
        isDodge = false;
    }

    void InterAction()
    {
        if (interAction && scanObject != null)
        {
            manager.Action(scanObject);
        }
    }

    private float lastStepSound = 0;
    private void OnCollisionStay(Collision collision)
    {
        lastStepSound += Time.fixedDeltaTime;
        if (lastStepSound < 0.4f)
            return;

        isJump = false;
        anim.SetBool("isJump", false);

        lastStepSound = 0;

      

        // 움직임
        if (collision.gameObject.CompareTag("Tile") && isMove && !isDodge)
        {
            tileSound.Play();
        }

        if (collision.gameObject.CompareTag("Grass") && isMove && !isDodge)
        {
            grassSound.Play();
        }

    }

    //IEnumerator Tile()
    //{
    //    while(true)
    //    {
    //        if (isMove && !isJump && !isDodge)
    //        {
    //            Debug.Log("타일 발자국");
    //            tileSound.Play();
    //        }
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}

    //IEnumerator Grass()
    //{
    //    while (true)
    //    {
    //        if (isMove && !isJump && !isDodge)
    //        {
    //            grassSound.Play();
    //        }
    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapons"))
        {
            Item item = other.GetComponent<Item>();
            int weaponIndex = item.value;
            hasWeapons[weaponIndex] = true;

            Destroy(other.gameObject);
        }

        else if (other.CompareTag("Items"))
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Grenade:
                    {
                        if (hasGrenades == 4)
                            grenades[hasGrenades - 1].SetActive(true);

                        else
                            grenades[hasGrenades].SetActive(true);

                        hasGrenades += item.value;
                        if (hasGrenades > maxHasGrenades)
                            hasGrenades = maxHasGrenades;
                        ItemManager.Instance.checkList.Remove(item.index);
                    }
                    break;
                case Item.Type.Hp:
                    hp += item.value;
                    if (hp > maxHp)
                        hp = maxHp;
                    ItemManager.Instance.checkList.Remove(item.index);
                    break;
                case Item.Type.Ammunition:
                    ammo += item.value;
                    if (ammo > maxAmmo)
                        ammo = maxAmmo;
                    ItemManager.Instance.checkList.Remove(item.index);
                    break;
            }
            Destroy(other.gameObject);
        }

        else if (other.CompareTag("EnemyMelee"))
        {
            if (!isDamage)
            {
                EnemyCol enemyCol = other.GetComponent<EnemyCol>();
                hp -= enemyCol.damage;
                StartCoroutine(OnDamage(false));
            }
        }

        else if (other.CompareTag("EnemyBullet"))
        {
            if (!isDamage)
            {
                EnemyMissileTrigger enemyMissile = other.GetComponent<EnemyMissileTrigger>();
                hp -= enemyMissile.damage;

                bool isBossAtk = other.name == "BossMelee";
                StartCoroutine(OnDamage(isBossAtk));
            }
        }

        // 굿 엔딩 분기점
        else if (other.CompareTag("Victory"))
        {
            // 보스를 안 잡고 갈 경우
            if (QuestManager.Instance.questId >= 20 && SpawnManager.Instance.bossAppear)
                GameManager.Instance.ChangeEndingScene("BossLiveScene");
            // 보스를 잡고 npc를 봤으면
            else if (QuestManager.Instance.questId >= 40 && !SpawnManager.Instance.bossAppear)
                GameManager.Instance.ChangeEndingScene("NPCSeeScene");
            // 보스를 잡고 npc를 안봤으면 열린 결말
            else if (QuestManager.Instance.questId >= 30 && !SpawnManager.Instance.bossAppear)
                GameManager.Instance.ChangeEndingScene("NPCNOTSeeScene");
        }
    }

    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        foreach(MeshRenderer mesh in meshRender)
        {
            mesh.material.color = Color.cyan;
        }
        if (isBossAtk)
            rigid.AddForce(-(transform.forward) * 25, ForceMode.Impulse);

        // 체력이 없으면 게임 오버
        if (hp <= 0 && !isDead)
        {
            hp = 0;
            OnDie();
        }

        yield return new WaitForSeconds(1f);

        isDamage = false;
        foreach (MeshRenderer mesh in meshRender)
        {
            mesh.material.color = Color.white;
        }

        if(isBossAtk)
            rigid.velocity = Vector3.zero;
    }

    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        Enemy enemieInfo = new Enemy();
        enemieInfo.TargetDead();

        GameManager.Instance.ChangeEndingScene("GameOverScene");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapons")
            objs = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapons")
            objs = null;
    }
}
