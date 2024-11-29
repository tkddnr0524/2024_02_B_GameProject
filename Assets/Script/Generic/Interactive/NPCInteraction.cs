using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour , IInteractable
{
    public string GetInteractPrompt() => "대화하기";
    public float GetInteractionDistance() => 2f;

    public bool CanInteract(GameObject player) => true;
    public void OnInteract(GameObject player)
    {
        FloatingTextManager.Instance.ShowFloatingText("안녕하세요 ! ", transform.position);
    }
}
