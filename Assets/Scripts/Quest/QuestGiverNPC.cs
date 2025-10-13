using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class QuestGiverNPC : InteractableObject
{
    [Header("Npc Quest Setting")]
    public QuestData questToGive;
    public string npcName = "Npc";
    public string questStartMessage = "새로운 퀘스트가 있습니다";
    public string noQuestMessage = "퀘스트가 없습니다";

    private QuestManager questManager;

    protected override void Start()
    {

        base.Start();
        questManager = FindAnyObjectByType<QuestManager>();

        if (questManager != null)
        {
            Debug.LogError("QuestManager 가 없습니다");
        }
        interactionText = "E" + npcName + "와 대화하기";
    }  
    public override void Interact()
    {
        base.Interact();
        questManager.StartQuest(questToGive);

    }
    private void Update()
    {
        if (questToGive != null && questManager != null && questManager.currentQuest == null)
        {
            interactionText = "E" + npcName + "와 대화하기";
        }
        else if (questManager != null && questManager.currentQuest != null)
        {
            interactionText = "E" + npcName;
        }
    
    
    
    }  





}

