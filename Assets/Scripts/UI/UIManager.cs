using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsDisplay;

    private void Start()
    {
        if (pointsDisplay == null)
        {
            pointsDisplay = FindObjectOfType<TextMeshProUGUI>();
        }
    }

    public void UpdatePointsDisplay(int currentPoints)
    {
        if (pointsDisplay != null)
        {
            pointsDisplay.text = $"Points: {currentPoints}";
        }
    }
}
