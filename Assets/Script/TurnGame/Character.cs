using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Character : MonoBehaviour
{

    //ĳ���� ����
    public bool isPlayer;                   //�÷��̾� ����
    public int hp = 100;                    //ü��
    public int speed;                       //�ӵ�(�� ���� ����)

    //UI ��� ����
    TextMeshProUGUI nameText;               //�̸� ǥ��
    TextMeshProUGUI hpText;                 //HP ǥ��
    Vector3 startPos;                       //���� ��ġ (���� �ִϸ��̼� ��)


    // Start is called before the first frame update
    void Start()
    {
        SetupNameText();                    //UI �ؽ�Ʈ �ʱ�ȭ
        startPos = transform.position;      //���� ��ġ ����
    }


    //UI �ؽ�Ʈ �ʱ�ȭ
    void SetupNameText()
    {
        //�̸� �ؽ�Ʈ ����
        GameObject textObj = new GameObject("NameText");
        textObj.transform.SetParent(BattleSystem.Instance.uiCanvas.transform);
        nameText = textObj.AddComponent<TextMeshProUGUI>();
        nameText.text = isPlayer ? "P" : "E";
        nameText.fontSize = 36;
        nameText.alignment = TextAlignmentOptions.Center;

        //HP �ؽ��� ����
        GameObject hpObj = new GameObject("HPText");
        hpObj.transform.SetParent(BattleSystem.Instance.uiCanvas.transform);
        hpText = hpObj.AddComponent<TextMeshProUGUI>();
        hpText.fontSize = 30;
        hpText.alignment = TextAlignmentOptions.Center;


    }

    public void Attack(Character target)
    {
        if (!target.gameObject.activeSelf) return;  //���� Ÿ���̸� ����
        StartCoroutine(AttackRoutine(target));      //���� �ִϸ��̼� ����
    }

    void Update()
    {
        //ĳ���Ͱ� ��Ȱ��ȭ�Ǹ� UI ����
        if(!gameObject.activeSelf)
        {
            if (nameText != null) Destroy(nameText.gameObject);
            if (hpText != null) Destroy(hpText.gameObject);
            return;
        }

        //UI ��ġ ������Ʈ
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
        nameText.transform.position = screenPos;
        hpText.transform.position = screenPos + Vector3.down * 30;

        //���� �� ĳ���� ǥ�� (�ʷϻ�) BattleSystem ���� ���� ����
        nameText.color = (BattleSystem.Instance.GetCurrentChar() == this) ? Color.green : Color.white;
        hpText.text = hp.ToString();
    }



    //���� �ִϸ��̼� �ڷ�ƾ
    IEnumerator AttackRoutine(Character target)
    {
        //Ÿ�������� �̵�
        Vector3 attackPos = target.transform.position + (target.transform.position - transform.position).normalized * 1.5f;
        float moveTime = 0.3f;
        float elapsed = 0;
        
        //�̵� �ִϸ��̼�
        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveTime; 
            transform.position = Vector3.Lerp(startPos, attackPos, t);
            yield return null;
        }

        //������ ó��
        target.hp -= 20;
        if(target.hp <= 0) target.gameObject.SetActive(false);

        //����ġ�� ����
        while (elapsed < moveTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveTime;
            transform.position = Vector3.Lerp(attackPos, startPos, t);
            yield return null;
        }
        transform.position = startPos;
        
    }
}
