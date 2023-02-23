using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace Leejihoo
{
    public class SpaceShipContraller : MonoBehaviour
    {
        public GameObject spaceShip;
        public GameObject endPoint;
        
        public void ContralSpaceShip()
        {
            spaceShip.transform.DOMove(endPoint.transform.position, 3f).OnComplete((() =>
            {
                spaceShip.SetActive(false);
            }));
            ContralLight();
        }

        public void ContralLight()
        {
            spaceShip.transform.GetChild(0).GetComponent<Image>().DOFade(0.5f, 1f).SetEase(Ease.Linear).OnComplete((() =>
            {
                spaceShip.transform.GetChild(0).GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear).OnComplete((() =>
                {
                    ContralLight();
                }));
            }));
            
        }
    }
}
