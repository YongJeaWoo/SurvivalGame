using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;
    public AudioSource explosionSound;

    private void Start()
    {
        StartCoroutine(Throw());
    }
    
    IEnumerator Throw()
    {
        yield return new WaitForSeconds(4.5f);
        
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        meshObj.SetActive(false);
        effectObj.SetActive(true);
        explosionSound.Play();

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 
                                                    8, 
                                                    Vector3.up, 
                                                    0f, 
                                                    LayerMask.GetMask("Enemy"));

        foreach(RaycastHit hit in rayHits)
        {
            hit.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 2f);
    }
}
