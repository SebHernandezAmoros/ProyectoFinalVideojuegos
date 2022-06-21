using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float timeRemainingFlag = 10;
    public float timeRemaining = 10;
    // Start is called before the first frame update
    void Start()
    {
        CreateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = timeRemainingFlag;
            CreateEnemy();
        }
    }
    public void CreateEnemy()
    {
        var x = this.transform.position.x;
        var y = this.transform.position.y;
        var enemyGO = Instantiate(EnemyPrefab, new Vector2(x, y), Quaternion.identity) as GameObject;
        Destroy(enemyGO, timeRemainingFlag * 30);
    }
}
