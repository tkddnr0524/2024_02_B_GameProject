using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
    

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance  {  get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private float checkRadius = 3f;
    [SerializeField] private LayerMask interactableLayers;

    private IInteractable currentInteractable;
    private GameObject player;

    private void Awake()
    {
        if (Instance == null) 
            Instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void UpdatePrompt()
    {
        if (currentInteractable != null)
        {
            promptText.text = $"[E] {currentInteractable.GetInteractPrompt()}";
            promptText.gameObject.SetActive(true);

        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckInteractables();
        if(Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.OnInteract(player);
        }
    }

    private void CheckInteractables()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, checkRadius, interactableLayers);   //주변 상호작용 가능한 객체
        IInteractable closest = null;
        float closetsDistance = float.MaxValue;

        foreach (var col in colliders)
        {
            if (col.TryGetComponent<IInteractable>(out var interactable))
            {
                float distance = Vector3.Distance(player.transform.position, col.transform.position);

                if (distance <= interactable.GetInteractionDistance() && distance < closetsDistance && interactable.CanInteract(player))
                {
                    closest = interactable;
                    closetsDistance = distance;
                }
            }
        }
        //가장 가까운 상호작용 대상 업데이트
        currentInteractable = closest;
        UpdatePrompt();
    }
}
