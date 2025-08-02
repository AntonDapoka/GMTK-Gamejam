using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotScript : MonoBehaviour, IDropHandler
{
    [SerializeField] private InventoryScript inventory; // ������ �� ���������, ����� ����� maxStackedBlocks

    public void OnDrop(PointerEventData eventData)
    {
        DraggableBlockScript droppedBlock = eventData.pointerDrag.GetComponent<DraggableBlockScript>();
        if (droppedBlock == null) return;

        // ���� ���� ������ � ������ ������
        if (transform.childCount == 0)
        {
            droppedBlock.parentAfterDrag = transform;
            return;
        }

        // ���� ���� �������� � ���������, ����� �� ��������
        DraggableBlockScript existingBlock = GetComponentInChildren<DraggableBlockScript>();
        if (existingBlock != null &&
            existingBlock.block == droppedBlock.block &&                // ���� � ��� �� ���
            existingBlock.block.isStackable &&                         // ����� �������
            existingBlock.count < inventory.MaxStackedBlocks)          // �� ����������
        {
            // ������� ��� ����� ��������
            int freeSpace = inventory.MaxStackedBlocks - existingBlock.count;

            // ������� ���������
            int amountToMove = Mathf.Min(droppedBlock.count, freeSpace);

            // ����������� ���������� � �����
            existingBlock.count += amountToMove;
            existingBlock.RefreshCount();

            // ��������� ���������� � ������������ �����
            droppedBlock.count -= amountToMove;

            if (droppedBlock.count <= 0)
            {
                // ���� ���� ������� � ������� ���
                Destroy(droppedBlock.gameObject);
            }
            else
            {
                // ���� ���-�� ��������, ���� ������������ �� ������� �����
                droppedBlock.RefreshCount();
                //droppedBlock.parentAfterDrag = droppedBlock.parentAfterDrag;
            }
        }
        else
        {
            // ���� �������� ������ � ���������� ������� �� �����
            //droppedBlock.parentAfterDrag = droppedBlock.parentAfterDrag;
        }
    }
}