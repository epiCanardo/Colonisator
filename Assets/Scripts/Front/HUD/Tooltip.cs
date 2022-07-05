using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI headerText;
    [SerializeField]
    private TextMeshProUGUI contentText;
    [SerializeField]
    private LayoutElement layoutElement;
    [SerializeField]
    private int maxCharacters;
    [SerializeField]
    private RectTransform rect;

    public void SetText(string content, string header = "")
    {
        int headerLength = headerText.text.Length;
        int contentLength = contentText.text.Length;

        layoutElement.enabled = (headerLength > maxCharacters || contentLength > maxCharacters);

        if (string.IsNullOrEmpty(header))
            headerText.gameObject.SetActive(false);
        else
        {
            headerText.gameObject.SetActive(true);
            headerText.text = header;
        }

        contentText.text = content;
    }


    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;
        rect.pivot = new Vector2(pivotX, pivotY);
        transform.position = mousePosition;
    }
}
