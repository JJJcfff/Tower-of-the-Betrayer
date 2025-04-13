// Authors: Jeff Cui, Elaine Zhao
// Handles the visual hover effect for potion buttons in the UI.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color normalColor = new Color(1f, 1f, 1f, 1f);
    public Color hoverColor = new Color(0.9f, 0.9f, 1f, 1f);
    
    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
        // Set initial color
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
    }
} 