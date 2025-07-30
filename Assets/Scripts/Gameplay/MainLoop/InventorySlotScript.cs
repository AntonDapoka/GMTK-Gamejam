using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            DraggableBlockScript block = eventData.pointerDrag.GetComponent<DraggableBlockScript>();
            block.parentAfterDrag = transform;
        }
    }
}
