using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AssemblerSlotScript : MonoBehaviour, IDropHandler
{
    public int index;
    [SerializeField] private BlockAssembleScript assembler;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            DraggableBlockScript block = eventData.pointerDrag.GetComponent<DraggableBlockScript>();
            block.parentAfterDrag = transform;

            if (block.block.typeBlock == BlockType.Start)
            {
                assembler.ChangeBlockStartIndex(index);
            }
            else if (block.block.typeBlock == BlockType.End)
            {
                assembler.ChangeBlockEndIndex(index);
            }
        }
    }
}
