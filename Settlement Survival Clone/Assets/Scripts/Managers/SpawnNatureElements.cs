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

    public static List<GameObject> DisabledGameObjects = new List<GameObject>();
    private void Start()
    {
        terrainSize = BuildingManager.planeSize;
        limitSize = BuildingManager.restrictValue;
        //terrainSize = 50;
        //limitSize = 5;
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
                if (terrain.terrainData.GetHeight((int)randomX, (int)randomZ) <= 0)//If height of the random position is greater than 0, object is not gonna spawn. 
                {
                    spawnObjectClone = Instantiate(prefab, new Vector3(randomX, terrain.terrainData.GetHeight((int)randomX, (int)randomZ), randomZ), Quaternion.identity);
                    spawnObjectClone.transform.rotation = Quaternion.FromToRotation(Vector3.forward, spawnObjectClone.transform.position.normalized);
                    spawnObjectClone.name = prefab.name;
                    BoxSelectionScript.objs.Add(spawnObjectClone);
                    spawnedObjeCount++;
                }
                else
                    continue;
            }                         
        }       
    }
    private void Update()
    {
        if(DisabledGameObjects.Count>0)
        {
            Counter();
        }
    }
    float timer = .5f;
    float timerPerMinute = .5f;
    int minute = 0;
    GameObject randomObject=null;
    float randomObjectMaxSizeOneAxis;
    void Counter()
    {
       
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            minute++;
            if (minute > 20)
            {
                if (randomObject==null)
                {
                    randomObject = DisabledGameObjects[Random.Range(0, DisabledGameObjects.Count)];
                    randomObjectMaxSizeOneAxis = randomObject.transform.localScale.x;
                    randomObject.transform.localScale = new Vector3(0, 0, 0);
                }
                else
                {
                    if (randomObject.transform.localScale.x<randomObjectMaxSizeOneAxis)
                    {
                        if(!randomObject.activeSelf)
                            randomObject?.SetActive(true);
                        randomObject.transform.localScale += new Vector3(.1f, .1f, .1f);
                    }
                    else
                    {
                        DisabledGameObjects.Remove(randomObject);
                        randomObject = null;
                    }
                }                    
                
                minute = 0;
            }
            timer = timerPerMinute;
        }
    }
}
