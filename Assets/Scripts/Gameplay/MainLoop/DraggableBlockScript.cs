using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// ����, ������� ����� �������������, ��������� � ����������� ����� ������ ������,
/// � ����� ��������� �� �����.
/// </summary>
public class DraggableBlockScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI textCount;

    [Header("������")]
    public BlockScript block;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public DraggableBlockScript originalBlock;

    // ������ ������ ������ ����� ����� (���� ��� ����)
    public List<Image> slotImages = new List<Image>();

    private Canvas rootCanvas;

    private void Awake()
    {
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void InitializeBlock(BlockScript newBlock)
    {
        block = newBlock;

        image.raycastTarget = true;

        foreach (var img in gameObject.GetComponentsInChildren<Image>())
        {
            if (img.GetComponent<SlotMarker>() != null || img.GetComponent<ShouldBeActiveMarker>() != null)
                img.raycastTarget = true;
            else
                img.raycastTarget = false;
        }

        foreach (var txt in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (txt.GetComponent<SlotMarker>() != null || txt.GetComponent<ShouldBeActiveMarker>() != null)
                txt.raycastTarget = true;
            else
                txt.raycastTarget = false;
        }

        RefreshCount();
    }

    public void RefreshCount()
    {
        textCount.text = count.ToString();
        textCount.gameObject.SetActive(count > 1);
    }

    // === DRAG ===

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (block == null || !block.isInteractable)
            return;

        foreach (var img in gameObject.GetComponentsInChildren<Image>())
        {
            img.raycastTarget = false;
        }

        // ���� ���� ��������� � ����� � �������� ���
        if (transform.parent != null && transform.parent.TryGetComponent<Image>(out var slotParent)
            && slotImages.Contains(slotParent) == false)
        {
            parentAfterDrag = null; // �� �������� ���
        }

        if (count > 1)
        {
            count--;
            RefreshCount();

            GameObject clone = Instantiate(block.prefab, transform.parent);
            DraggableBlockScript cloneScript = clone.GetComponent<DraggableBlockScript>();

            cloneScript.count = 1;
            cloneScript.RefreshCount();
            cloneScript.originalBlock = this;

            cloneScript.parentAfterDrag = null;
            cloneScript.image.raycastTarget = false;
            cloneScript.transform.SetParent(rootCanvas.transform);
            cloneScript.transform.position = Input.mousePosition;

            eventData.pointerDrag = clone;
        }
        else
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(rootCanvas.transform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (block == null || !block.isInteractable)
            return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (block == null || !block.isInteractable)
            return;

        image.raycastTarget = true;

        foreach (var img in gameObject.GetComponentsInChildren<Image>())
        {
            if (img.GetComponent<SlotMarker>() != null || img.GetComponent<ShouldBeActiveMarker>() != null) 
                img.raycastTarget = true;
            else
                img.raycastTarget = false;
        }

        foreach (var txt in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (txt.GetComponent<SlotMarker>() != null || txt.GetComponent<ShouldBeActiveMarker>() != null) 
                txt.raycastTarget = true;
            else
                txt.raycastTarget = false;
        }
            

        // ���������, ��������� �� �� ��� ������
        var target = eventData.pointerEnter;
        if (target != null)
        {
            Image slotImage = target.GetComponent<Image>();

            if (slotImage != null && slotImage.GetComponent<SlotMarker>() != null) // ��� "Slot" ��� ������
            {
                // ��������� ���� ���� � ����
                transform.SetParent(slotImage.transform);
                transform.localPosition = Vector3.zero;
                return;
            }
        }

        // ���� �� ������ � ���� � ���������� �� ����� ��� ����������
        if (parentAfterDrag != null)
        {
            transform.SetParent(parentAfterDrag);
        }
        else
        {
            if (originalBlock != null)
            {
                originalBlock.count++;
                originalBlock.RefreshCount();
            }

            Destroy(gameObject);
        }
    }

    // === CLICK ===
    public void OnPointerClick(PointerEventData eventData)
    {
        // ���� ���� �������� � ���� � �� ����� ��������� ���
        if (transform.parent != null && transform.parent.GetComponent<SlotMarker>() != null)
        {
            transform.SetParent(rootCanvas.transform);
            transform.position = Input.mousePosition;
            parentAfterDrag = null;
        }
    }
}
