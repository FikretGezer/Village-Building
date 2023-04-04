using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterMovement : MonoBehaviour
{
    [SerializeField] GameObject[] toolsCharacterUsed=new GameObject[2];
    GameObject axe, pickAxe;

    int isWalkingCache, isChoppingCache, isMiningCache;

    NavMeshAgent agent;
    Animator agentAnimator;

    int deger = 0;
    string nameOfSelectedObject = "";
    GameObject chosenObject = default;

    int woodCount, rockCount;
    bool isOnTheJob = false;

    bool isHeInTrigger;

    List<GameObject> allPlacedObject = new List<GameObject>();
    private void Awake()
    {       

        woodCount = rockCount = 0;

        agent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();

        axe = toolsCharacterUsed[0];
        pickAxe = toolsCharacterUsed[1];

        isWalkingCache = Animator.StringToHash("isWalking");
        isChoppingCache = Animator.StringToHash("isChopping");
        isMiningCache = Animator.StringToHash("isMining");
    }
    private void Update()
    {
        deger = BoxSelectionScript.chosenObject.Count;        

        if (deger>0 && !isOnTheJob)
        {
            SelectRandomSelectedObject();
            agentAnimator.SetBool(isWalkingCache, true);               
        }
     
        if(ReachedDestinationOrGaveUp(agent))
        {
            agentAnimator.SetBool(isWalkingCache, false);
            agentAnimator.SetBool(isMiningCache, false);
            agentAnimator.SetBool(isChoppingCache, false);


            if(nameOfSelectedObject=="Tree Model1")
            {
                axe?.SetActive(true);
                agent.transform.LookAt(chosenObject.transform);
                agentAnimator.SetBool(isChoppingCache, true);

                CountIncreaser(ref woodCount,axe,ref isChoppingCache, nameof(woodCount));
            }
            if (nameOfSelectedObject == "Rock Model1")
            {
                pickAxe?.SetActive(true);
                agent.transform.LookAt(chosenObject.transform);
                agentAnimator.SetBool(isMiningCache, true);

                CountIncreaser(ref rockCount, pickAxe, ref isMiningCache, nameof(rockCount));
            }
        }
    }
    private bool ReachedDestinationOrGaveUp(NavMeshAgent navMeshAgent)
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public void SelectRandomSelectedObject()
    {       
        if (ReachedDestinationOrGaveUp(agent))
        {
            if (deger > 0)
            {
                isOnTheJob = true;
                int random = Random.Range(0, deger);
                chosenObject = BoxSelectionScript.chosenObject[random];
                nameOfSelectedObject = chosenObject.name;
                agent.SetDestination(chosenObject.transform.localPosition);   
                
                BoxSelectionScript.chosenObject.Remove(BoxSelectionScript.chosenObject[random]);               
            }
            else
            {
                isOnTheJob = false;
                if (isHeInTrigger)
                {
                    //Karakterler evin altýndaki aðacý kestikten sonra harita rastgele bir yere gider
                    float enBuyukX, enKucukX, enBuyukZ, enKucukZ;

                    enBuyukX=enKucukX = BuildingManager.allPlacedObject[0].transform.position.x;

                    enBuyukZ=enKucukZ= BuildingManager.allPlacedObject[0].transform.position.z;

                    for (int i = 1; i < BuildingManager.allPlacedObject.Count; i++)
                    {
                        if(enBuyukX < BuildingManager.allPlacedObject[i].transform.position.x)
                        {
                            enBuyukX = BuildingManager.allPlacedObject[i].transform.position.x;
                        }
                        if (enKucukX > BuildingManager.allPlacedObject[i].transform.position.x)
                        {
                            enKucukX = BuildingManager.allPlacedObject[i].transform.position.x;
                        }

                        if (enBuyukZ < BuildingManager.allPlacedObject[i].transform.position.z)
                        {
                            enBuyukZ = BuildingManager.allPlacedObject[i].transform.position.z;
                        }
                        if (enKucukZ > BuildingManager.allPlacedObject[i].transform.position.z)
                        {
                            enKucukZ = BuildingManager.allPlacedObject[i].transform.position.z;
                        }
                    }
                    float randomX = Random.Range(enKucukX, enBuyukX);
                    float randomZ = Random.Range(enKucukZ, enBuyukZ);
                    agent.SetDestination(new Vector3(randomX, 0, randomZ));

                }
            }
        }

    }

    float timer = .5f;
    float timerPerMinute = .5f;
    int minute = 0;
    void CountIncreaser(ref int objectCount,GameObject usedTool/*axe or pickaxe*/,ref int animationCache, string nameOfCount)
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            minute++;
            if (minute > 2)
            {
                objectCount += 5;
                if (objectCount > 49)
                {
                    usedTool?.SetActive(false);
                    agentAnimator.SetBool(animationCache, false);
                    agentAnimator.SetBool(isWalkingCache, true);
                    nameOfSelectedObject = null;                   
                    chosenObject?.SetActive(false);

                    if (!SpawnNatureElements.DisabledGameObjects.Contains(chosenObject))
                    {
                        SpawnNatureElements.DisabledGameObjects.Add(chosenObject);
                    }

                    chosenObject = null;
                    UiManager.NameOfCountedObject = nameOfCount;
                    UiManager.OnResourceUpdate(objectCount);
                    objectCount = 0;
                    SelectRandomSelectedObject();
                }
                minute = 0;
            }
            timer = timerPerMinute;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="placeable")
        {
            isHeInTrigger = true;
        }
    }
}
//We can fix that is not cutting problem by detect if the ai goes to place that we want different way rather than detect collisions or triggers.
//Markete dönüldükten sonra yeni bi yere karakterleri yönlendirdiðinde karakterler birbirine çarpýþýyor ve bir yere gitmiyor.
