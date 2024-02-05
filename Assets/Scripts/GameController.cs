using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //spawn balls

    public GameObject playerPrefab;
    public Transform _playerSpawnPosition;

    bool isBallSpawned=false;
    
    private void Start()
    {
        SpawnPlayerBall();
    }
    private void OnEnable()
    {
        BallController.onStickModeChanged += SpawnPlayerBall;
    }

    private void OnDisable()
    {
        BallController.onStickModeChanged -= SpawnPlayerBall;
    }
    
    public void SpawnPlayerBall()
    {
        StartCoroutine(SpawnAfterSomeTime());
    }

    IEnumerator SpawnAfterSomeTime()
    {
        if (!isBallSpawned)
        {
            yield return new WaitForSeconds(1);
            print("Spawn Ball");
            GameObject myPlayer = Instantiate(playerPrefab, _playerSpawnPosition.transform.position,_playerSpawnPosition.transform.rotation);
            isBallSpawned=true;
        }
        
    }
}
