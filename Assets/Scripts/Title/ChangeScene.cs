using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public GameObject other;
    public Image fadeOut;
    private bool isLoader=true;


    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, other.transform.position) < 2.0f)
        {
            if(isLoader)
            {
                StartCoroutine(Fade());
                isLoader = false;
            }
        }
    }

    IEnumerator Fade()
    {
        float fadeCount = 0; // 첫 알파값
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeOut.color = new Color(0, 0, 0, fadeCount);
        }

        SceneManager.LoadScene("ExplainScene");
    }
}