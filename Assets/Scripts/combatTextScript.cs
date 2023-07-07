using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combatTextScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        text=GetComponent<TMPro.TextMeshProUGUI>();
        StartCoroutine("shrink");
    }

    IEnumerator shrink()
    {
        text.fontSize = text.fontSize - 0.001f;
        text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - 0.01f);
        yield return new WaitForSeconds(0.01f);
        if(text.color.a<=0)
        {
            Destroy(transform.parent.gameObject);
        }
        StartCoroutine(shrink());
    }
}
