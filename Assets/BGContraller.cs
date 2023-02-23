using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGContraller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TimeStoryStart()
    {
        StartCoroutine("TimerStory");
    }

    IEnumerator TimerStory()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("GoToHome").SetActive(false);
        //GameObject.Find("SpaceShip").SetActive(false);
        yield return new WaitForSeconds(7f);
        GameObject.Find("Canvas").transform.GetChild(7).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
//
// #else
        Application.Quit();
// #endif
    }
}
