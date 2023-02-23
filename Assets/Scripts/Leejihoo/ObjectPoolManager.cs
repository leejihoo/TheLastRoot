using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
//using UnityEditor.Timeline.Actions;
using UnityEngine.UIElements;
using File = System.IO.File;
using UnityEngine.UI;

namespace Leejihoo
{
    public class ObjectPoolManager : MonoBehaviour
    {
        enum InteractiveType
        {
            ObstacleType0,
            ObstacleType1,
            ObstacleType2,
            Food,
            Ox
        }
        
        // 여유되면 resource.load로 코드 변경하기
        [SerializeField] private GameObject obstacleType0Prefab;
        [SerializeField] private GameObject obstacleType1Prefab;
        [SerializeField] private GameObject obstacleType2Prefab;
        [SerializeField] private GameObject foodPrefab;
        [SerializeField] private GameObject oxPrefab;
        
        public GameObject _obstacleType0Pool;
        public GameObject _obstacleType1Pool;
        public GameObject _obstacleType2Pool;
        public GameObject _foodPool; 
        public GameObject _oxPool; 
        
        public GameObject _obstacleType0Start; 
        public GameObject _obstacleType1Start;
        public GameObject _obstacleType2Start;
        public GameObject _foodStart;
        public GameObject _oxStart;
        
        public Vector3 _obstacleType0EndPos;
        public Vector3 _obstacleType1EndPos;
        public Vector3 _obstacleType2EndPos;
        public Vector3 _foodEndPos;
        public Vector3 _oxEndPos;

        public int currentStage;
        public JArray _interativeObjectData;
        
        private void Awake()
        {
            
            _obstacleType0Pool = GameObject.Find("ObstacleType0Pool");
            _obstacleType1Pool = GameObject.Find("ObstacleType1Pool");
            _obstacleType2Pool = GameObject.Find("ObstacleType2Pool");
            _foodPool = GameObject.Find("FoodPool");
            _oxPool = GameObject.Find("OxPool");
            
            _obstacleType0Start = GameObject.Find("ObstacleType0Start");
            _obstacleType1Start = GameObject.Find("ObstacleType1Start");
            _obstacleType2Start = GameObject.Find("ObstacleType2Start");
            _foodStart = GameObject.Find("FoodStart");
            _oxStart = GameObject.Find("OxStart");
            
            _obstacleType0EndPos = GameObject.Find("ObstacleType0EndPos").transform.position;
            _obstacleType1EndPos = GameObject.Find("ObstacleType1EndPos").transform.position;
            _obstacleType2EndPos = GameObject.Find("ObstacleType2EndPos").transform.position;
            _foodEndPos = GameObject.Find("FoodEndPos").transform.position;
            _oxEndPos = GameObject.Find("OxEndPos").transform.position;

            //currentStage = PlayerPrefs.GetInt("stage");

            var currentStageName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            CreateInterativeObjectData($"{currentStageName}");
            //CreateInterativeObjectData($"Assets/Resources/JsonData/Stage{currentStage}.txt");
            //string loadJson = File.ReadAllText($"Assets/Resources/JsonData/'시트1'.txt");
            //var JObject = Newtonsoft.Json.Linq.JObject.Parse(loadJson);
            //interativeObjectData = JArray.Parse(loadJson);
            
            Debug.Log(_interativeObjectData.ToString());
            Debug.Log(_interativeObjectData[0]["TimeToCreate"]);
            
        }

        public void CreateInterativeObjectData(string dataPath)
        {
            //string loadData = File.ReadAllText(dataPath);
            TextAsset textAsset = (TextAsset)Resources.Load(dataPath);
            string loadData = textAsset.text;
            _interativeObjectData = JArray.Parse(loadData);
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Flow());
            
        }

        IEnumerator Flow()
        {
            float currentTime = 0f;
            
            foreach (var jToken in _interativeObjectData)
            {
                yield return new WaitForSeconds(Convert.ToSingle((int)jToken["TimeToCreate"]-currentTime));
                currentTime = (int)jToken["TimeToCreate"];
                
                switch ((int)jToken["InteractiveType"])
                {
                    case (int)InteractiveType.ObstacleType0:
                        SetObjectPosition(_obstacleType0Start, _obstacleType0EndPos, _obstacleType0Pool, obstacleType0Prefab, (int)jToken["TimeToArrive"], (int)jToken["AddedHeight"]);
                        break;
                    case (int)InteractiveType.ObstacleType1:
                        SetObjectPosition(_obstacleType1Start, _obstacleType1EndPos, _obstacleType1Pool, obstacleType1Prefab, (int)jToken["TimeToArrive"], (int)jToken["AddedHeight"]);
                        break;
                    case (int)InteractiveType.ObstacleType2:
                        SetObjectPosition(_obstacleType2Start, _obstacleType2EndPos, _obstacleType2Pool, obstacleType2Prefab, (int)jToken["TimeToArrive"], (int)jToken["AddedHeight"]);
                        break;
                    case (int)InteractiveType.Food:
                        SetObjectPosition(_foodStart, _foodEndPos, _foodPool, foodPrefab, (int)jToken["TimeToArrive"], (int)jToken["AddedHeight"]);
                        break;
                    case (int)InteractiveType.Ox:
                        SetObjectPosition(_oxStart, _oxEndPos, _oxPool, oxPrefab, (int)jToken["TimeToArrive"], (int)jToken["AddedHeight"]);
                        break;
                }
            }
            
            var currentStageName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            yield return new WaitForSeconds(8f);
            PlayerPrefs.SetInt("stage", currentStage+1);

            if (currentStageName == "Stage0")
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1");
            }
            else
            {
                GameObject.Find("Player").transform.DOMove(GameObject.Find("Laboratory").transform.position, 3);
                yield return new WaitForSeconds(4f);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Stage2");
            }

        }

        // Pool에서 오브젝트를 배치 후 이동 & Pool로 위치 리셋
        public void SetObjectPosition(GameObject startPosition, Vector3 endPosition, GameObject interactiveGameObjectPool, GameObject prefab, float duration, float high = 0f)
        {
            if (interactiveGameObjectPool.transform.childCount == 0)
            {
               GameObject.Instantiate(prefab,interactiveGameObjectPool.transform);
            }

            var interactiveGameObject = interactiveGameObjectPool.transform.GetChild(0);
            
            interactiveGameObject.position = startPosition.transform.position + new Vector3(0, high, 0);
            interactiveGameObject.parent = startPosition.transform;
            
            interactiveGameObject.DOMove(endPosition + new Vector3(0, high, 0), duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                interactiveGameObject.gameObject.SetActive(true);
                interactiveGameObject.transform.position = interactiveGameObjectPool.transform.position;
                interactiveGameObject.parent = interactiveGameObjectPool.transform;
            });
        }
        
    }
}
 