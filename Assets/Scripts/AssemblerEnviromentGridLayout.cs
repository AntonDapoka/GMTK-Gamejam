using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class AssemblerEnviromentGridLayout : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 cellSize = new Vector2(100, 100);
    public Vector2 spacing = new Vector2(10, 10);
    public int constraintCount = 3; // ���-�� ����� � ������

    [Header("Padding (������� �� ����)")]
    public RectOffset padding;

    [Header("Exceptions")]
    public List<RectTransform> exceptions = new List<RectTransform>();

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateLayout();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying)
            UpdateLayout();
    }
#endif

    private bool initialized = false;

    private void LateUpdate()
    {
        if (!initialized)
        {
            // ������ LateUpdate ����� ������
            UpdateLayout();
            initialized = true;
        }
        else
        {
            // ���� ����� ������������� ������ ���� � ����� �������� ���
            UpdateLayout();
        }
    }

    public void UpdateLayout()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        List<RectTransform> validChildren = new List<RectTransform>();
        foreach (RectTransform child in rectTransform)
        {
            if (child == null) continue;
            if (exceptions.Contains(child)) continue;
            validChildren.Add(child);
        }

        if (validChildren.Count == 0) return;

        int columns = Mathf.Max(1, constraintCount);
        int rows = Mathf.CeilToInt((float)validChildren.Count / columns);

        // ����� ������ �����
        float totalWidth = columns * cellSize.x + (columns - 1) * spacing.x;
        float totalHeight = rows * cellSize.y + (rows - 1) * spacing.y;

        // �����: ��������� ������ �������� ��� ����� + ��������
        rectTransform.sizeDelta = new Vector2(
            padding.left + padding.right + totalWidth,
            padding.top + padding.bottom + totalHeight
        );

        // ��������� ����� (����� ����)
        float startX = padding.left;
        float startY = -padding.top;

        for (int i = 0; i < validChildren.Count; i++)
        {
            int row = i / columns;
            int column = i % columns;

            float x = startX + column * (cellSize.x + spacing.x);
            float y = startY - row * (cellSize.y + spacing.y);

            RectTransform child = validChildren[i];
            child.anchorMin = new Vector2(0, 1);
            child.anchorMax = new Vector2(0, 1);
            child.pivot = new Vector2(0, 1);

            child.anchoredPosition = new Vector2(x, y);
            child.sizeDelta = cellSize;
        }
    }

}