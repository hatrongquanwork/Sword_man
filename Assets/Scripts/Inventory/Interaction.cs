using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask interactableLayers;
    public float interactionRange = 2f;
    public float interactionRangeNPC = 2f;
    private WindowHandle windowHandle;

    private void Start()
    {
        windowHandle = GameObject.Find("CameraHolder").GetComponent<WindowHandle>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractNPC();
            Interact();
        }
    }

    public void Interact()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayers))
        {
            PickUp pickup = hit.transform.GetComponent<PickUp>();

            if(pickup != null)
            {
                windowHandle.inventory.AddItem(pickup);
            }
        }
    }

    public void InteractNPC()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionRangeNPC);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                npcInteractable.NPCInteract();
            }
        }
    }

    public NPCInteractable GetInteractableObject()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactionRangeNPC);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NPCInteractable npcInteractable))
            {
                return npcInteractable;
            }
        }
        return null;
    }

    public StuffInteractable GetInteractableObject2()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out StuffInteractable stuff))
            {
                return stuff;
            }
        }
        return null;
    }
}
