using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIFunc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textMeshPro;
    private float originalFontSize;
    public float hoverFontSizeIncrease = 2f; // The amount to increase the font size on hover

    void Start()
    {
        // Ensure the TextMeshPro component is assigned
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }
        // Store the original font size
        originalFontSize = textMeshPro.fontSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Increase font size on hover
        textMeshPro.fontSize = originalFontSize + hoverFontSizeIncrease;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset font size when not hovering
        textMeshPro.fontSize = originalFontSize;
    }

    public void QuitGame() { 
        Application.Quit();
        Debug.Log("Quitting Game!");
    }
}
