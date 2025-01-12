using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public int maxObjectCount = 200; // 최대 허용 오브젝트 수
    private int currentObjectCount = 0; // 현재 오브젝트 수

    private List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    private void Update()
    {
        Debug.Log(currentObjectCount);
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        // 비활성화된 오브젝트를 풀에서 찾기
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;
            }
        }

        // 현재 오브젝트 수가 최대치를 넘는지 확인
        if (currentObjectCount >= maxObjectCount)
        {
            // 최대치를 넘으면 데미지 처리
            ApplyDamage();
            return null; // 오브젝트를 반환하지 않음
        }

        // 새로운 오브젝트 생성 및 풀에 추가
        select = Instantiate(prefabs[index], transform);
        pools[index].Add(select);
        currentObjectCount++; // 현재 오브젝트 수 증가

        return select;
    }

    // 오브젝트를 반환하는 메서드
    public void ReturnToPool(GameObject obj, int index)
    {
        obj.SetActive(false);
        currentObjectCount--; // 현재 오브젝트 수 감소
    }

    // 데미지 처리 메서드
    private void ApplyDamage()
    {
        // 데미지 처리 로직 구현
        Debug.Log("최대 오브젝트 수를 초과하여 데미지를 받습니다.");
        // 예: 플레이어의 체력을 감소시키는 로직 등
    }
}