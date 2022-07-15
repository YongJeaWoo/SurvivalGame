using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogOpen : MonoBehaviour
{
    public Text nameTxt;
    public Text descriptionTxt;

    Queue<string> descriptions = new Queue<string> ();

    public Animator anim;
    public Transform panel;
    public Image image;

    public void Begin(Dialogue info)
    {
        anim.SetBool("isOpen", true);
        descriptions.Clear();

        nameTxt.text = info.name;

        foreach (var description in info.descriptions)
        {
            descriptions.Enqueue(description);
        }

        Next();
    }

    public void Next()
    {
        if (descriptions.Count == 0)
        {
            End();
            return;
        }

        //image.color = new Color(0,0,0,0)

        descriptionTxt.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypingDescription(descriptions.Dequeue()));
    }

    IEnumerator TypingDescription(string description)
    {
        foreach (var letter in description)
        {
            descriptionTxt.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void End()
    {
        anim.SetBool("isOpen", false);
        nameTxt.text = string.Empty;
        descriptionTxt.text = string.Empty;
        Invoke("EndingScn", 2f);        
    }

    void EndingScn()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
