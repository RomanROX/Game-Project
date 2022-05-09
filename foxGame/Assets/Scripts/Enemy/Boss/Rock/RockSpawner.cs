using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] GameObject rock;
    [SerializeField] List<Sprite> rockSprites;
    [SerializeField] float delayBetweenRocks;
    [SerializeField] Transform leftSpawnBorder;
    [SerializeField] Transform rightSpawnBorder;


    private void Start()
    {
        StartCoroutine(SpawnRock());
    }

    IEnumerator SpawnRock()
    {
        GameObject obj = rock;
        obj.GetComponent<SpriteRenderer>().sprite = GetRandomRockSprite();
        Instantiate(obj, GetSpawnPoint(), Quaternion.identity);

        yield return new WaitForSeconds(delayBetweenRocks);

        StartCoroutine(SpawnRock());
    }

    Sprite GetRandomRockSprite()
    {
        int num = Random.Range(0, rockSprites.Count);
        return rockSprites[num];
    }

    Vector2 GetSpawnPoint()
    {
        float x = Random.Range(leftSpawnBorder.position.x, rightSpawnBorder.position.x);
        float y = Random.Range(leftSpawnBorder.position.y, rightSpawnBorder.position.y);
        Vector2 spawn = new Vector2(x, y);
        return spawn;
    }
}
