using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public PlayerController playerController;
    public List<UnitController> enemyPrefabList = new();

    private void Start()
    {
        StartCoroutine(CreateEnemy());
    }

    private void Update()
    {

    }

    private IEnumerator CreateEnemy()
    {
        while (true)
        {
            if (enemyPrefabList.Count > 0)
            {
                UnitController enemyController = Instantiate(enemyPrefabList[Random.Range(0, enemyPrefabList.Count)]);
                float randomRadian = Random.Range(0f, Mathf.PI * 2);
                enemyController.transform.position = playerController.transform.position + Mathf.Cos(randomRadian) * 5f * Vector3.right + Mathf.Sin(randomRadian) * 5f * Vector3.up;
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
