using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    public CurrencySystem CurrencySystem { get; private set; }
    public InputManager InputManager { get; private set; }
    public CarController CarController { get; private set; }
    public UIManager UIManager { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        InitializeSystems();
    }

    private void InitializeSystems()
    {
        CurrencySystem = GetComponent<CurrencySystem>();
        InputManager = GetComponent<InputManager>();
        CarController = FindObjectOfType<CarController>();
        UIManager = FindObjectOfType<UIManager>();

        if (CurrencySystem == null)
            CurrencySystem = gameObject.AddComponent<CurrencySystem>();
        if (InputManager == null)
            InputManager = gameObject.AddComponent<InputManager>();
        if (CarController == null)
            Debug.LogError("CarController not found in scene!");
        if (UIManager == null)
            Debug.LogError("UIManager not found in scene!");

        WireUpSystems();
    }

    private void WireUpSystems()
    {
        if (InputManager != null && CarController != null)
        {
            InputManager.OnClick.AddListener(CarController.PlayClickAnimation);
        }

        if (InputManager != null && CurrencySystem != null)
        {
            InputManager.OnClick.AddListener(() => CurrencySystem.AddPoints(1));
        }

        if (CurrencySystem != null && UIManager != null)
        {
            CurrencySystem.OnPointsChanged.AddListener(UIManager.UpdatePointsDisplay);
        }
    }
}
