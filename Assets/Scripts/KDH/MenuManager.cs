using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] GameObject storyBG1;
    [SerializeField] GameObject storyBG2;

    float alpha = 1;
    private bool _isMaxValue = true;
    private bool _isFadeStart = false;
    private bool _isRotationStart = true;
    
    public void LogoStartButton()
    {
        PlayerPrefs.SetInt("stage", 0);
        logo.SetActive(false);
        StartCoroutine(TimerStory());
        
    }
    public void PlayButton()
    {
        SceneManager.LoadScene("Stage0");
    }

    IEnumerator TimerStory()
    {
        yield return new WaitForSeconds(3f);
        storyBG1.SetActive(false);
    }

    private void Update()
    {
        var logoStartColor = logo.transform.GetChild(0).GetComponent<Image>().color;
        
        // 전체배경화면 깜빡거림
        if (!_isFadeStart)
        {
            _isFadeStart = true;
            StartCoroutine(FadeOut(0.6f));
        }
        
        // 게임 시작 버튼 깜빡꺼림
        if(logoStartColor.a >= 0.5 && _isMaxValue)
        {
            alpha -= Time.deltaTime;
            logo.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, alpha);
        }
        else 
        {
            _isMaxValue = false;
            alpha += Time.deltaTime;
            logo.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, alpha);
            if(alpha >= 1)
            {
                _isMaxValue = true;
            }
        }

        if (_isRotationStart)
        {
            _isRotationStart = false;
            storyBG2.transform.GetChild(0).DORotate(new Vector3(0, 0, 1f), 0.5f).OnComplete((() =>
            {
                storyBG2.transform.GetChild(0).DORotate(new Vector3(0, 0, -1f), 0.5f).OnComplete((() =>
                {
                    _isRotationStart = true;
                }));
            }));    
        }
        
        //종이 흔들림
        if (storyBG2.transform.GetChild(0).rotation.z == -2f)
        {
            storyBG2.transform.GetChild(0).DORotate(new Vector3(0, 0, 2), 2);
        }
        else if (storyBG2.transform.GetChild(0).rotation.z == 2f)
        {
            storyBG2.transform.GetChild(0).DORotate(new Vector3(0, 0, -2), 2);
        }
       
    }
    
    IEnumerator FadeOut(float boundary)
    {
        Color c = logo.GetComponent<Image>().color;
        for (float alphaValue = 1f; alphaValue >= boundary; alphaValue -= 0.01f)
        {
            c.a = alphaValue;
            logo.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.1f);
        }
    
        StartCoroutine(FadeIn(boundary));
        //storyBG1.SetActive(false);
    }
    
    IEnumerator FadeIn(float boundary)
    {
        Color c = logo.GetComponent<Image>().color;
        for (float alphaValue = boundary; alphaValue <= 1f; alphaValue += 0.01f)
        {
            c.a = alphaValue;
            logo.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.1f);
        }
        
        _isFadeStart = false;
    }
}
