using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;
using static UnityEngine.GraphicsBuffer;

public class DraggableBlockScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image image;
    [SerializeField] private GameObject blockContent;
    public TextMeshProUGUI textCount;

    [HideInInspector] public BlockScript block;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    // Новое поле для клона: ссылка на оригинальный блок
    [HideInInspector] public DraggableBlockScript originalBlock;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void InitializeBlock(BlockScript newBlock)
    {
        block = newBlock;

        blockContent = Instantiate(newBlock.prefab, transform);

        foreach (var img in blockContent.GetComponentsInChildren<Image>())
            img.raycastTarget = false;

        foreach (var txt in blockContent.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            txt.raycastTarget = false;

        blockContent.GetComponent<Image>().enabled = false;

        RefreshCount();
    }

    public void RefreshCount()
    {
        textCount.text = count.ToString();
        textCount.gameObject.SetActive(count > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (count > 1)
        {
            // Уменьшаем количество в исходном блоке
            count--;
            RefreshCount();

            // Создаем клон
            GameObject clone = Instantiate(gameObject, transform.parent);
            DraggableBlockScript cloneScript = clone.GetComponent<DraggableBlockScript>();

            // Настраиваем клон
            cloneScript.count = 1;
            cloneScript.RefreshCount();
            cloneScript.originalBlock = this; // ссылка на оригинал

            // Чтобы клон не остался в слоте
            cloneScript.parentAfterDrag = null;
            cloneScript.image.raycastTarget = false;
            cloneScript.transform.SetParent(transform.root);
            cloneScript.transform.position = Input.mousePosition;

            // Говорим системе, что теперь перетаскивается клон
            eventData.pointerDrag = clone;
        }
        else
        {
            // Старая логика
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        if (parentAfterDrag != null)
        {
            transform.SetParent(parentAfterDrag);
        }
        else
        {
            // Если это клон и он никуда не положен – вернуть предмет
            if (originalBlock != null)
            {
                originalBlock.count++;
                originalBlock.RefreshCount();
            }

            Destroy(gameObject);
        }
    }
}
