using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowHandle : MonoBehaviour
{
    private CameraController cam;
    private bool windowOpened;
    [HideInInspector] public InventoryManager inventory;


    private void Start()
    {
        cam = GetComponent<CameraController>();
        inventory = GameObject.Find("All Menus").GetComponent<InventoryManager>();
    }

    private void Update()
    {
        if (windowOpened)
        {
            cam.lockCursor = false;
            cam.canMove = false;
        }
        else
        {
            cam.lockCursor = true;
            cam.canMove = true;
        }

        if (inventory.opened)
            windowOpened = true;
        else
            windowOpened = false;
    }
}
