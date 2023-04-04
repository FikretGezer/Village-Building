using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] GameObject toolsCharacterUsed;
    [SerializeField] GameObject market;

    NavMeshAgent agent;
    Animator agentAnimator;

    bool isChopping;
    bool isMining;
    bool isAgentOnDestination;
    GameObject pickAxe, axe;

    public int woodCount, rockCount;

    GameObject currentObject;//Is it tree or rock

    public static BoxSelectionScript boxSelectionScript;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();

        boxSelectionScript = FindObjectOfType<BoxSelectionScript>();

        woodCount = rockCount = 0;

        if(toolsCharacterUsed!=null)
        {
            axe = toolsCharacterUsed.transform.GetChild(0).gameObject;
            pickAxe = toolsCharacterUsed.transform.GetChild(1).gameObject;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            SelectRandomSelectedObject();
        }

        if (!ReachedDestinationOrGaveUp(agent))
        {
            isChopping = false;
            isMining = false;

            agentAnimator.SetBool("isChopping", false);
            agentAnimator.SetBool("isMining", false);
            agentAnimator.SetBool("isWalking", true);

            axe.gameObject.SetActive(false);
            pickAxe.gameObject.SetActive(false);
        }
        else
        {
            agentAnimator.SetBool("isWalking", false);
            agentAnimator.SetBool("isChopping", isChopping);
            var isChop = agentAnimator.GetBool("isChopping");
            if (isChop)
            {                
                CountIncreaser(ref woodCount,nameof(woodCount));
            }
            else if(isMining)
            {
                agentAnimator.SetBool("isMining", true);
                CountIncreaser(ref rockCount, nameof(rockCount));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!agent.hasPath)
        {
            if (other.gameObject.tag == "tree")
            {
                Debug.Log("tree in");
                //ThingsToDoOnAction(other.gameObject, axe, out isChopping);

                axe.SetActive(true);
                transform.LookAt(other.gameObject.transform.localPosition);
                isChopping = true;
                agent.ResetPath();

                currentObject = other.gameObject;
            }
            if (other.gameObject.tag == "rock")
            {
                ThingsToDoOnAction(other.gameObject, pickAxe, out isMining);
                currentObject = other.gameObject;
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
                    isAgentOnDestination=true;
                    return true;
                }
            }
        }
        isAgentOnDestination = false;
        return false;
    }
    private void ThingsToDoOnAction(GameObject objectToLookAt, GameObject toolThatCharacterUse, out bool whatCharacterDoes )
    {
        toolThatCharacterUse.SetActive(true);
        transform.LookAt(objectToLookAt.transform.localPosition);
        whatCharacterDoes = true;
        agent.ResetPath();        
    }
    public void SelectRandomSelectedObject()
    {
        if(isAgentOnDestination)
        {
            int deger = BoxSelectionScript.chosenObject.Count;
            Debug.Log("Object Count : " + deger);
            if (deger > 0)
            {
                int random = Random.Range(0, deger);
                agent.SetDestination(BoxSelectionScript.chosenObject[random].transform.localPosition);
                BoxSelectionScript.chosenObject.Remove(BoxSelectionScript.chosenObject[random]);
            }
            //else
            //{
            //    agent.SetDestination(market.transform.localPosition);
            //}
        }

    }

    float timer = .05f;
    float timerPerMinute = .05f;
    int minute = 0;
    void CountIncreaser(ref int objectCount,string nameOfCount)
    {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            minute++;
            if(minute>2)
            {
                objectCount += 5;
                if(objectCount > 49)
                {
                    currentObject?.SetActive(false);
                    currentObject = null;
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
}

/*
 * Karakter se�ilen objeye hareket edecek.
 * Se�ilen obje toplanabilir, kesilebilir vb. bir nesne ise ona g�re ekipman se�ip animasyonu oynatacak.
 * - Bu durumda her bir nesneye yaln�zca bir ki�i d��ecek. (Bu de�i�tirilebilir.) 
 * - Her bir nesnenin belli bir can� olacak ve i�lem bitti�inde elde edilenler toplanma yerine b�rak�lacak. (Bu y�zden karakter �zerine g�stemelik toplanan eklenebilir ve b�ylece onu b�rakmadan di�er a�aca ge�emez.)
 * - E�er se�ilen yerde ki�i atanmam�� ki�iler varsa oraya gidecek ve yine ayn� i�lemi yapacak.
 * 
 * Yada baraka, ev, terzi, okul gibi bir yerse oraya girecek. Sahnede setactive false olacak ve ��k�ncada true olacak.
 * - Bu tarz yap�lara bir UI atanarak orada �rne�in 5 ki�i �al��abiliyorsa doluluk oran�n� g�sterecek.
 */
