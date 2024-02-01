using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public GameObject[] portals; // Portal 오브젝트 배열

    void Start()
    {
        ActivateRandomPortal();
    }

    void ActivateRandomPortal()
    {
        // 모든 Portal을 비활성화
        foreach (var portal in portals)
        {
            portal.SetActive(false);
        }

        // 랜덤으로 Portal 하나를 활성화
        int randomIndex = Random.Range(0, portals.Length);
        portals[randomIndex].SetActive(true);
    }
}
