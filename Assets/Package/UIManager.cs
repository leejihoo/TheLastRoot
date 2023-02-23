using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    
    [Header("Player")]
    [SerializeField] Image hpBar;
    [SerializeField] Image oxBar;
    [SerializeField] PlayerController player;

    [Header("GameOver")]
    [SerializeField] GameObject gameOverUI;

    [SerializeField] private GameObject pauseUI;
    
    
    [System.Serializable]
    public class StoryPack
    {
        public  float popUpTime;
        public GameObject storyUI;

        public IEnumerator StoryPop()
        {
            yield return new WaitForSeconds(popUpTime);
            storyUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    [Header("Story")]
    [SerializeField] List<StoryPack> storyPacks;
    // Start is called before the first frame update
    void Start()
    {
        if(storyPacks.Count != 0)
        {
            for (int i = 0; i < storyPacks.Count; i++)
            {
                StartCoroutine(storyPacks[i].StoryPop());
            }
        }
        

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        hpBar = GameObject.Find("HpBar").GetComponent<Image>();
        oxBar = GameObject.Find("OxBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float hpRatio = player.Hp / 100f;
        this.hpBar.fillAmount = hpRatio;
        float oxRatio = player.Ox / 100f;
        this.oxBar.fillAmount = oxRatio;

        GameOver();
    }
    void GameOver()
    {
        if(player.isDie == true)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Pause()
    {
        if (!player.isDie)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void Restart()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }

    public void BackBtn(GameObject Parent)
    {
        Parent.SetActive(false);
        Time.timeScale = 1;
    }
}
