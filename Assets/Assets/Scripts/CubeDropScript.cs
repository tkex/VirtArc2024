using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enable the spawning of cubes -- either all at once or sequentially after a set countdown.
/// </summary>
public class CubeDropScript : MonoBehaviour
{
    [Tooltip("Array of cube prefabs to be spawned.")]
    public GameObject[] cubePrefabs;

    [Tooltip("Delay (in seconds) between the spawning of each cube. Only when not spawning all at once.")]
    public float spawnDelay = 1.0f;

    [Tooltip("Determines whether all cubes are spawned at once or sequentially.")]
    public bool spawnAllAtOnce = false;

    [Tooltip("Countdown duration (in seconds) before spawning begins.")]
    [Range(1, 10)]
    public int countdownDuration = 5;

    void Start()
    {
        StartCoroutine(CountdownAndSpawnCubes());
    }

    IEnumerator CountdownAndSpawnCubes()
    {
        // Countdown before spawning cubes.
        while (countdownDuration > 0)
        {
            Debug.Log("Countdown until cubes drop: " + countdownDuration);
            yield return new WaitForSeconds(1f);
            countdownDuration--;
        }

        // Spawn cubes after countdown.
        if (spawnAllAtOnce)
        {
            SpawnAllCubes();
        }
        else
        {
            StartCoroutine(SpawnCubesSequentially());
        }
    }

    void SpawnAllCubes()
    {
        foreach (GameObject cubePrefab in cubePrefabs)
        {
            Instantiate(cubePrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator SpawnCubesSequentially()
    {
        foreach (GameObject cubePrefab in cubePrefabs)
        {
            Instantiate(cubePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
