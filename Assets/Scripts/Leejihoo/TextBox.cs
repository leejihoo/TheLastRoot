using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    private Image image;
    private Text text;
    
// Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
        text = this.gameObject.transform.GetChild(0).GetComponent<Text>();
        StartCoroutine(FadeInOutImage(image));
        StartCoroutine(FadeInOutText(text));
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator FadeInOutImage(Image textBox)
    {
        Color clearImageColor = textBox.color;
        clearImageColor.a = 0;
        textBox.color = clearImageColor;
        for (float i = 1; i <= 20; i++)
        {
            clearImageColor.a = i / 20;
            textBox.color = clearImageColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        clearImageColor.a = 100;
        textBox.color = clearImageColor;

        yield return new WaitForSeconds(2f);

        for (float i = 20; i >= 1; i--)
        {
            clearImageColor.a = i / 20;
            textBox.color = clearImageColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        clearImageColor.a = 0;
        textBox.color = clearImageColor;      
    }
    IEnumerator FadeInOutText(Text textBox)
    {
        Color clearImageColor = textBox.color;
        clearImageColor.a = 0;
        textBox.color = clearImageColor;
        for (float i = 1; i <= 20; i++)
        {
            clearImageColor.a = i / 20;
            textBox.color = clearImageColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        clearImageColor.a = 100;
        textBox.color = clearImageColor;

        yield return new WaitForSeconds(2f);

        for (float i = 20; i >= 1; i--)
        {
            clearImageColor.a = i / 20;
            textBox.color = clearImageColor;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        clearImageColor.a = 0;
        textBox.color = clearImageColor;
    }
}
