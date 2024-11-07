using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyGame.QuestSystem;
using UnityEngine.UI;
public class SimpleQuestProgressUI : MonoBehaviour
{
    [Header("Quest List")]
    [SerializeField] private Transform questListParent;                 //����Ʈ ����� ǥ�õ� �θ� Transform
    [SerializeField] private GameObject questPrefabs;                   //����Ʈ UI ������

    [Header("Pregress Test")]
    [SerializeField] private Button KillEnemyButton;                    //�� óġ �׽�Ʈ ��ư
    [SerializeField] private Button CollectItemButton;                  //������ ���� �׽�Ʈ ��ư

    private QuestManager questManager;


    // Start is called before the first frame update
    void Start()
    {
        questManager = QuestManager.Instance;

        //��ư �̺�Ʈ ����
        KillEnemyButton.onClick.AddListener(OnKillEnemy);
        CollectItemButton.onClick.AddListener(OnCollectItem);

        //�̺�Ʈ ���
        questManager.OnQuestStarted += UpdateQuestUI;
        questManager.OnQuestCompleted += UpdateQuestUI;

        //�ʱ� ����Ʈ ���� ǥ��
        RefreshQuestList();
    }

    private void CreateQuestUI(Quest quest) //���� ����Ʈ UI ����
    {
        GameObject questObj = Instantiate(questPrefabs, questListParent);

        TextMeshProUGUI titleText = questObj.transform.Find("TitleText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI progressText = questObj.transform.Find("ProgressText").GetComponent<TextMeshProUGUI>();

        titleText.text = quest.Title;
        progressText.text = $"Progress: {quest.GetProgress():P0}";
    }

    private void UpdateQuestUI(Quest quest) //����Ʈ ���� ����� UI ������Ʈ
    {
        RefreshQuestList();
    }
    //����Ʈ ��� ���ΰ�ħ
    private void RefreshQuestList()
    {
        foreach (Transform child in questListParent) //���� UI����
        {
            Destroy(child.gameObject);
        }

        foreach (var quest in questManager.GetActiveQuest())    //Ȱ�� ����Ʈ ǥ��
        {
            CreateQuestUI(quest);
        }
    }

    //�� óġ ��ư �̺�Ʈ
    private void OnKillEnemy()
    {
        questManager.OnEnemykilled("Rat");
        RefreshQuestList();
    }

    //������ ���� ��ư �̺�Ʈ
    private void OnCollectItem()
    {
        questManager.OnItemCollected("Herb");
        RefreshQuestList();

    }
    
}
