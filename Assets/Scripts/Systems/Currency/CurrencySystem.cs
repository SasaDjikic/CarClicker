using UnityEngine;
using UnityEngine.Events;

public class CurrencySystem : MonoBehaviour
{
    private int _totalPoints = 0;
    private float _multiplier = 1f;

    public UnityEvent<int> OnPointsChanged = new UnityEvent<int>();

    public int GetPoints() => _totalPoints;
    public float GetMultiplier() => _multiplier;

    public void AddPoints(int baseAmount)
    {
        int earnedPoints = Mathf.RoundToInt(baseAmount * _multiplier);
        _totalPoints += earnedPoints;
        OnPointsChanged.Invoke(_totalPoints);
    }

    public void SetMultiplier(float multiplier)
    {
        _multiplier = multiplier;
    }

    public void ResetPoints()
    {
        _totalPoints = 0;
        OnPointsChanged.Invoke(_totalPoints);
    }
}
