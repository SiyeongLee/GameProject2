using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI 요소들")]
    public GameObject questUI;
    public Text questTitleText;
    public Text questDescriptionText;
    public Text questProgressText;
    public Button completeButton;

    [Header("퀘스트 목록")]
    public QuestData[] availableQuests;

    private QuestData currentQuest;
    private int currentQuestIndex = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (availableQuests.Length > 0)
        {
            StartQuest(availableQuests[0]);
        }
        if (completeButton != null)
        {
            completeButton.onClick.AddListener(CompleteCurrentQuest);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentQuest != null && currentQuest.isActive)
        {
            CheckQuestProgress();
            UpdateQuestUI();
        }
    }

   
    void UpdateQuestUI()
    {
        if (currentQuest == null) return;

        if (questTitleText != null)
        {
            questTitleText.text = currentQuest.questTitle;
        }

        if (questDescriptionText != null)
        {
            questDescriptionText.text = currentQuest.description;
        }

        if (questProgressText != null)
        {
            questProgressText.text = currentQuest.GetProgressText();
        }

    }

    
    public void StartQuest(QuestData quest)
    {
        if (quest == null) return;

        currentQuest = quest;                       //퀘스트를 받아와서 CurrentQuest에 셋팅한다. 
        currentQuest.Initialize();                  //지금 퀘스트를 초기화 한다.
        currentQuest.isActive = true;

        Debug.Log("퀘스트 시작 : " + questTitleText);
        UpdateQuestUI();
        if (questUI != null)
        {
            questUI.SetActive(true);
        }
    }

    
    void CheckDeliveryProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;               //유저의 위치를 찾는다.
        if (player == null) return;

        float distance = Vector3.Distance(player.position, currentQuest.deliveryPosition);      //유저와 도착지 거리를 계산한다.

        if (distance <= currentQuest.deliveryRedius)                                            //유저의 거리가 도착범위 안쪽인지 검사
        {
            if (currentQuest.currentProgress == 0)
            {
                currentQuest.currentProgress = 1;                                               //퀘스트 완료
            }
        }
        else
        {
            currentQuest.currentProgress = 0;                                                   //도착하지 못함
        }
    }

    
    public void AddCollectProgress(string itemTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Collect && currentQuest.targetTag == itemTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("아이템 수집 : " + itemTag);
        }
    }

   
    public void AddInteractProgress(string objectTag)
    {
        if (currentQuest == null || !currentQuest.isActive) return;

        if (currentQuest.questType == QuestType.Interact && currentQuest.targetTag == objectTag)
        {
            currentQuest.currentProgress++;
            Debug.Log("상호 작용 완료 : " + objectTag);
        }
    }

   
    public void CompleteCurrentQuest()
    {
        if (currentQuest == null || !currentQuest.isCompleted) return;

        Debug.Log("퀘스트 완료 !" + currentQuest.rewardMessage);

        
        if (completeButton != null)
        {
            completeButton.gameObject.SetActive(false);
        }

        
        currentQuestIndex++;
        if (currentQuestIndex < availableQuests.Length)
        {
            StartQuest(availableQuests[currentQuestIndex]);
        }
        else
        {
           
            currentQuest = null;
            if (questUI != null)
            {
                questUI.gameObject.SetActive(false);
            }
        }
    }

   
    void CheckQuestProgress()
    {
        if (currentQuest.questType == QuestType.Delivery)
        {
            CheckDeliveryProgress();
        }

        
        if (currentQuest.IsComplete() && !currentQuest.isCompleted)
        {
            currentQuest.isCompleted = true;

            
            if (completeButton != null)
            {
                completeButton.gameObject.SetActive(true);
            }
        }
    }

}