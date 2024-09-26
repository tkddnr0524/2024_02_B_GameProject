using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//UnityMonoBehavour �� ���� ���׸� �̱��� ����

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    //�� �̱����� ������ �ν��Ͻ�
    private static T instance;

    //������ �������� �����ϱ� ���� ��� ��ü
    private static readonly object _lock = new object();

    //���ø����̼��� ���� ������ Ȯ���ϴ� �÷���
    private static bool isQuitting = false;

    //�̱��� �ν��Ͻ��� ���� ���� ������
    public static T Instance
    {
        get
        {
            //���ø����̼� ���� �� ��Ʈ ��ü ���� ������ ���� üũ
            if (isQuitting)
            {
                Debug.Log($"[�̱���] '{typeof(T)}' �ν��Ͻ��� ���ø����̼� ���� �߿� ���� �ǰ� �ֽ��ϴ�. ��Ʈ ��ü ������ ���� null ��ȯ");
                return null;
            }

            //������ �������� ���� ���
            lock (_lock)
            {
                //�ν��Ͻ��� ������ ã�ų� ����
                if (instance == null)
                {
                    //������ ���� �ν��Ͻ� ã��
                    instance = (T)FindObjectOfType(typeof(T));

                    //�ν��Ͻ��� ������ ���� ����
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject($"{typeof(T)}(Singleton)");
                        instance = singletonObject.AddComponent<T>();

                        //�� �ε� �� �ν��Ͻ� ����
                        DontDestroyOnLoad(singletonObject);

                        Debug.Log($"[�̱���]{typeof(T)}�� �ν��Ͻ��� DonDestoryOnLoad�� �����Ǿ����ϴ�. ");
                    }  
                }
                return instance;
            }
        }

       
    }


    protected virtual void Awake()
    {
        //�ν��Ͻ��� ������ �̰��� �ν��Ͻ��� ����
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        //�ν��Ͻ��� �̹� �ְ� �̰��� �ƴϸ� �ı�
        else if (instance != this)
        {
            Debug.LogWarning($"[�̱���] {typeof(T)}�� �ٸ� �ν��Ͻ��� �̹� �����մϴ�. �� �ߺ��� �ı��մϴ�. ");
            Destroy(gameObject);
        }
    }


    //OnApplicationQuit �޼��� ���� �÷��׸� �����ϴµ� ���
    protected virtual void OnApplicationQuit()
    {
        isQuitting = true;
    }

    //OnDestroy �޼��� ����ġ ���� �ı��� üũ �ϴ� �� ���
    protected virtual void OnDestroy()
    {
        //��ü�� �ı��ǰ� ������ ���ø����̼��� ���� ���� �ƴ϶�� �����̱� ������ �α׸� ����
        if (!isQuitting)
        {
            Debug.LogWarning($"[�̱���] {typeof(T)}�� �ν��Ͻ��� ���ø����̼� ���ᰡ �ƴ� �������� �ı� , ������ ��");
        }
        
        isQuitting = true;    
    }
    

    //���� �����ӿ� �׼��� �����ϴ� �ڷ�ƾ
    private System.Collections.IEnumerator ExcuteOnNextFrame(Action action)
    {
        //���� �����ӱ��� ���
        yield return null;

        //�׼� ����
        action();
    }
}