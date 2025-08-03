using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableBlockScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TextMeshProUGUI textCount;

    public BlockScript block;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;
    [HideInInspector] public DraggableBlockScript originalBlock;

    public void InitializeBlock(BlockScript newBlock)
    {
        block = newBlock;

        foreach (var img in gameObject.GetComponentsInChildren<Image>())
            img.raycastTarget = false;

        foreach (var txt in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            txt.raycastTarget = false;

        image.raycastTarget = true;

        RefreshCount();
    }

    public void RefreshCount()
    {
        textCount.text = count.ToString();
        textCount.gameObject.SetActive(count > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Если блок не интерактивен — выходим
        if (block == null || !block.isInteractable)
            return;

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
            cloneScript.transform.SetParent(transform.root);
            cloneScript.transform.position = Input.mousePosition;

            eventData.pointerDrag = clone;
        }
        else
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
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
}