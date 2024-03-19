using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public bool opened;

    [Header("Settings")]
    public int inventorySize = 24;
    public int hotbarSize = 6;


    [Header("Refs")]
    public GameObject dropModel;
    public Transform dropPos;
    public GameObject slotTemplate;
    public Transform contentHolder;
    public Transform hotbarContentHolder;


    private Slot[] inventorySlots;
    private Slot[] hotbarSlots;

    private void Start()
    {
        GenerateHotBarSlots();
        GenerateSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            hotbarSlots[0].Use();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            hotbarSlots[1].Use();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            hotbarSlots[2].Use();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            hotbarSlots[3].Use();
        if (Input.GetKeyDown(KeyCode.Alpha5))
            hotbarSlots[4].Use();
        if (Input.GetKeyDown(KeyCode.Alpha6))
            hotbarSlots[5].Use();


        if (Input.GetKeyDown(KeyCode.Tab))
            opened = !opened;

        if (opened)
        {
            transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(-10000, 0, 0);
        }
    }

    private void GenerateSlots()
    {
        List<Slot> inventorySlots_ = new List<Slot>();


        // Generate slots
        for (int i = 0; i < inventorySize; i++)
        {
            Slot slot = Instantiate(slotTemplate.gameObject, contentHolder).GetComponent<Slot>();

            inventorySlots_.Add(slot);
        }

        inventorySlots = inventorySlots_.ToArray();

    }

    private void GenerateHotBarSlots()
    {
        List<Slot> inventorySlots_ = new List<Slot>();
        List<Slot> hotbatList = new List<Slot>();


        // Generate slots
        for (int i = 0; i < hotbarSize; i++)
        {
            Slot slot = Instantiate(slotTemplate.gameObject, hotbarContentHolder).GetComponent<Slot>();

            inventorySlots_.Add(slot);
            hotbatList.Add(slot);
        }

        inventorySlots = inventorySlots_.ToArray();
        hotbarSlots = hotbatList.ToArray();

    }

    public void AddItem(PickUp pickUp)
    {
        if (pickUp.data.isStackable)
        {
            Slot stackableSlot = null;

            // TRY FINDING STACKABLE SLOT
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (!inventorySlots[i].IsEmpty)
                {
                    if (inventorySlots[i].data == pickUp.data && inventorySlots[i].stackSize < pickUp.data.maxStack)
                    {
                        stackableSlot = inventorySlots[i];
                        break;
                    }

                }
            }

            if (stackableSlot != null)
            {

                // IF IT CANNOT FIT THE PICKED UP AMOUNT
                if (stackableSlot.stackSize + pickUp.stackSize > pickUp.data.maxStack)
                {
                    int amountLeft = (stackableSlot.stackSize + pickUp.stackSize) - pickUp.data.maxStack;



                    // ADD IT TO THE STACKABLE SLOT
                    stackableSlot.AddItemToSlot(pickUp.data, pickUp.data.maxStack);

                    // TRY FIND A NEW EMPTY STACK
                    for (int i = 0; i < inventorySlots.Length; i++)
                    {
                        if (inventorySlots[i].IsEmpty)
                        {
                            inventorySlots[i].AddItemToSlot(pickUp.data, amountLeft);
                            inventorySlots[i].UpdateSlot();

                            break;
                        }
                    }



                    Destroy(pickUp.gameObject);
                }
                // IF IT CAN FIT THE PICKED UP AMOUNT
                else
                {
                    stackableSlot.AddStackAmount(pickUp.stackSize);

                    Destroy(pickUp.gameObject);
                }

                stackableSlot.UpdateSlot();
            }
            else
            {
                Slot emptySlot = null;


                // FIND EMPTY SLOT
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (inventorySlots[i].IsEmpty)
                    {
                        emptySlot = inventorySlots[i];
                        break;
                    }
                }

                // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
                if (emptySlot != null)
                {
                    emptySlot.AddItemToSlot(pickUp.data, pickUp.stackSize);
                    emptySlot.UpdateSlot();

                    Destroy(pickUp.gameObject);
                }
                else
                {
                    pickUp.transform.position = dropPos.position;
                }
            }

        }
        else
        {
            Slot emptySlot = null;


            // FIND EMPTY SLOT
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    emptySlot = inventorySlots[i];
                    break;
                }
            }

            // IF WE HAVE AN EMPTY SLOT THAN ADD THE ITEM
            if (emptySlot != null)
            {
                emptySlot.AddItemToSlot(pickUp.data, pickUp.stackSize);
                emptySlot.UpdateSlot();

                Destroy(pickUp.gameObject);
            }
            else
            {
                pickUp.transform.position = dropPos.position;
            }

        }
    }

    public void DropItem(Slot slot)
    {
        PickUp pickup = Instantiate(dropModel, dropPos).AddComponent<PickUp>();
        pickup.transform.position = dropPos.position;
        pickup.transform.SetParent(null);

        pickup.data = slot.data;
        pickup.stackSize = slot.stackSize;

        slot.Clean();
    }

    public void DragDrop(Slot from, Slot to) 
    { 
        // Swaping
        if(from.data != to.data)
        {
            ItemSO data = to.data;
            int stackSize = to.stackSize;

            to.data = from.data;
            to.stackSize = from.stackSize;

            from.data = data;
            from.stackSize = stackSize;
        }
        // Stacking
        else
        {
            if (from.data.isStackable)
            {
                if(from.stackSize + to.stackSize > from.data.maxStack)
                {
                    int amountLeft = (from.stackSize + to.stackSize) - from.data.maxStack;

                    from.stackSize = amountLeft;

                    to.stackSize = to.data.maxStack;
                }
            }
            else
            {
                ItemSO data = to.data;
                int stackSize = to.stackSize;

                to.data = from.data;
                to.stackSize = from.stackSize;

                from.data = data;
                from.stackSize = stackSize;
            }
        }

        from.UpdateSlot();
        to.UpdateSlot();
    }
}
