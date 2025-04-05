using System.Collections;
using UnityEngine;
using TMPro;

public class TextShakeEffect : MonoBehaviour
{
    private TMP_Text textMesh;
    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;

    public float shakeMagnitude = 5f; // Сила тряски
    public float shakeSpeed = 10f; // Скорость тряски

    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;
        originalVertices = new Vector3[textInfo.meshInfo.Length][];

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = new Vector3[textInfo.meshInfo[i].vertices.Length];
        }

        while (true)
        {
            textMesh.ForceMeshUpdate();

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible) continue;

                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                // Создаём небольшое смещение для каждой вершины
                Vector3 shakeOffset = new Vector3(
                    Mathf.Sin(Time.time * shakeSpeed + i) * shakeMagnitude,
                    Mathf.Cos(Time.time * shakeSpeed + i) * shakeMagnitude,
                    0);

                for (int j = 0; j < 4; j++)
                {
                    vertices[vertexIndex + j] += shakeOffset * Time.deltaTime;
                }
            }

            // Обновляем меш
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            }

            yield return null;
        }
    }
}
