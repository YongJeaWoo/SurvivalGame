using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type
    {
        A,
        B,
        C,
        Boss,
    };

    public Type enemyType;

    public int maxHp;
    public int curHp;

    public bool isChase;
    public bool isAttack;
    public bool isDead;

    public BoxCollider attackArea;
    public GameObject bullet;

    public Transform target;

    float missileSpeed = 25f;

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer[] mesh;
    public NavMeshAgent nav;
    public Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mesh = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (enemyType != Type.Boss)
            Invoke("ChaseStart", 2f);
    }

    private void Update()
    {
        if (nav.enabled && enemyType != Type.Boss)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    private void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }

    void Targeting()
    {
        if (!isDead && enemyType != Type.Boss)
        {
            float targetRadius = 0f;
            float targetRange = 0f;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 0.8f;
                    targetRange = 1f;
                    break;
                case Type.B:
                    targetRadius = 0.5f;
                    targetRange = 4f;
                    break;
                case Type.C:
                    targetRadius = 0.3f;
                    targetRange = 25f;
                    break;
            }



            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 
                                                        targetRadius, 
                                                        transform.forward, 
                                                        targetRange, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.78f);
                attackArea.enabled = true;

                yield return new WaitForSeconds(1.1f);
                attackArea.enabled = false;

                yield return new WaitForSeconds(1f);

                break;
            case Type.B:
                yield return new WaitForSeconds(0.5f);

                rigid.AddForce(transform.forward * 20f, ForceMode.Impulse);
                attackArea.enabled = true;

                yield return new WaitForSeconds(0.45f);
                rigid.velocity = Vector3.zero;
                attackArea.enabled = false;

                yield return new WaitForSeconds(3f);
                break;
            case Type C:
                yield return new WaitForSeconds(0.3f);

                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * missileSpeed;
                    
                yield return new WaitForSeconds(2f);
                break;

        }

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Melee"))
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(Ondamage(reactVec, false));
        }
        else if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(Ondamage(reactVec, false));
        }
    }

    IEnumerator Ondamage(Vector3 reactVec, bool grenadeReact)
    {
        foreach(MeshRenderer meshs in mesh)
        {
            meshs.material.color = Color.red;
        }
        yield return new WaitForSeconds(0.1f);

        if (curHp > 0)
        {
            foreach (MeshRenderer meshs in mesh)
            {
                meshs.material.color = Color.white;
            }
        }
        else
        {
            curHp = 0;

            SpawnManager.Instance.bossAppear = false;

            foreach (MeshRenderer meshs in mesh)
            {
                meshs.material.color = Color.grey;
            }


            if (enemyType == Type.Boss)
                gameObject.layer = 22;
            else
                gameObject.layer = 21;

            anim.SetTrigger("doDie");
            isDead = true;
            isChase = false;
            nav.enabled = false;
            

            switch (enemyType)
            {
                case Type.A:
                    //if (SpawnManager.Instance.enemyACount < 0)
                    //    SpawnManager.Instance.enemyACount = 0;
                    SpawnManager.Instance.enemyACount--;
                    break;

                case Type.B:
                    //if (SpawnManager.Instance.enemyBCount < 0)
                    //    SpawnManager.Instance.enemyBCount = 0;
                    SpawnManager.Instance.enemyBCount--;
                    break;

                case Type.C:
                    //if (SpawnManager.Instance.enemyCCount < 0)
                    //    SpawnManager.Instance.enemyCCount = 0;
                    SpawnManager.Instance.enemyCCount--;
                    break;
            }

            // 넉백
            if (grenadeReact)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.freezeRotation = false;
                //rigid.AddForce(reactVec * Random.Range(30, 50), ForceMode.Impulse);
                //rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
                rigid.AddExplosionForce(Random.Range(30, 60), reactVec, 10, 10, ForceMode.Impulse);
            }
            else
            {
                reactVec = -reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 8f, ForceMode.Impulse);
            }

            if (enemyType != Type.Boss)
            {
                Destroy(gameObject, 2f);
            }                
        }
    }

    public void HitByGrenade(Vector3 explosion)
    {
        curHp -= 100;
        Vector3 reactVec = transform.position - explosion;
        StartCoroutine(Ondamage(reactVec, true));
    }

    public void TargetDead()
    {
        isAttack = false;
        isChase = false;
    }
}
