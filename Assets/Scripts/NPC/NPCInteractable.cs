using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private GameObject dialogue;
    public AudioClip soundnpc; 
    public void NPCInteract()
    {
        dialogue.SetActive(true);
        AudioSource.PlayClipAtPoint(soundnpc, transform.position);
    }
}
