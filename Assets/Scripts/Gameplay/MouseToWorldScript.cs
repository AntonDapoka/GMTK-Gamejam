using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseToWorldScript : MonoBehaviour
{
    public Camera finalCamera;        // ������, ������� ���������� Quad ������ (������ B)
    public Camera worldCamera;        // ������, ������� �������� ��� (������ A)
    public Renderer renderQuad;       // MeshRenderer Quad��, �� ������� �������� RenderTexture
    public Vector2 renderTextureSize = new Vector2(320, 200); // ������ RenderTexture

    public Transform aimTarget;       // ������, ���� �������

    void Update()
    {
        Ray ray = finalCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == renderQuad.gameObject)
            {
                // 1. ������ �� Quad��
                Vector2 uv = hit.textureCoord; // ���������� ��������� � �������� (0..1)

                // 2. ��������� � ������� RenderTexture
                float pixelX = uv.x * renderTextureSize.x;
                float pixelY = uv.y * renderTextureSize.y;

                // 3. ����������� ������� ��� ViewportPoint
                Vector3 viewportPoint = new Vector3(pixelX / renderTextureSize.x, pixelY / renderTextureSize.y, 0f);

                // 4. ������ ��� �� worldCamera � ��� �����
                Ray worldRay = worldCamera.ViewportPointToRay(viewportPoint);

                // 5. ������: ���������, ���� ��������� ���� � XZ-��������� (y=0)
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                if (groundPlane.Raycast(worldRay, out float distance))
                {
                    Vector3 worldPos = worldRay.GetPoint(distance);
                    aimTarget.position = worldPos;
                }
            }
        }
    }
}