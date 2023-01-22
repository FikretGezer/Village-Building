using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNatureElements : MonoBehaviour
{
    [SerializeField] GameObject treePrefab;
    [SerializeField] GameObject rockPrefab;
    [SerializeField] Terrain terrain;
    [SerializeField] LayerMask ground;
    [SerializeField] float treeSpawnCount; // How many tree object is gonna spawn?
    [SerializeField] float rockSpawnCount; // How many rock object is gonna spawn?

    float randomX,randomZ;
    float terrainSize,limitSize;//This values for terrain measurements. For example terrainsize 200, and limit size 5, trees gonna be instantiated in 195*195
    private void Start()
    {
        terrainSize = BuildingManager.planeSize;
        limitSize = BuildingManager.restrictValue;
        SpawnObject(treeSpawnCount,0,treePrefab);
        SpawnObject(rockSpawnCount,0,rockPrefab);
    }
    //spawn ederken orada obje var mý kontrol et
    void SpawnObject(float treeSpawnCount,float spawnedObjeCount,GameObject prefab)
    {
        GameObject spawnObjectClone;
        Collider[] checkAroundSpawnObject;
        while(spawnedObjeCount < treeSpawnCount)
        {
            randomX = Random.Range(limitSize, terrainSize-limitSize);
            randomZ = Random.Range(limitSize, terrainSize-limitSize);
            checkAroundSpawnObject = Physics.OverlapSphere(new Vector3(randomX, 3, randomZ),2f);
            if(checkAroundSpawnObject.Length!=0)
            {
                continue;
            }
            else
            {
                if (terrain.terrainData.GetHeight((int)randomX, (int)randomZ) <= 0)
                {
                    spawnObjectClone = Instantiate(prefab, new Vector3(randomX, terrain.terrainData.GetHeight((int)randomX, (int)randomZ), randomZ), Quaternion.identity);
                    spawnObjectClone.transform.rotation = Quaternion.FromToRotation(Vector3.forward, spawnObjectClone.transform.position.normalized);
                    BoxSelectionScript.objs.Add(spawnObjectClone);
                    spawnedObjeCount++;
                }
            }                         
        }       
    }
}
