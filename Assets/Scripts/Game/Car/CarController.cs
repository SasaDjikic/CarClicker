using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour
{
    [SerializeField] public Animator animator;
    [SerializeField] public float baseSpeed = 5f;
    [SerializeField] public float clickAnimationDuration = 0.5f;

    private float _currentVisualSpeed = 0f;
    private float _targetSpeed = 0f;
    private int _totalUpgradeLevels = 0;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        UpdateVisualSpeed();
    }

    public void PlayClickAnimation()
    {
        _targetSpeed = baseSpeed + (_totalUpgradeLevels * 0.1f);

        if (animator != null)
        {
            animator.SetFloat("Speed", _targetSpeed);
        }

        StartCoroutine(AnimateSpeed());
    }

    private IEnumerator AnimateSpeed()
    {
        float elapsedTime = 0f;
        float startSpeed = _currentVisualSpeed;

        while (elapsedTime < clickAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            _currentVisualSpeed = Mathf.Lerp(startSpeed, _targetSpeed, elapsedTime / clickAnimationDuration);

            if (animator != null)
            {
                animator.SetFloat("Speed", _currentVisualSpeed);
            }

            yield return null;
        }

        _currentVisualSpeed = _targetSpeed;

        yield return new WaitForSeconds(0.2f);

        elapsedTime = 0f;
        while (elapsedTime < clickAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            _currentVisualSpeed = Mathf.Lerp(_targetSpeed, 0f, elapsedTime / clickAnimationDuration);

            if (animator != null)
            {
                animator.SetFloat("Speed", _currentVisualSpeed);
            }

            yield return null;
        }

        _currentVisualSpeed = 0f;
    }

    public void UpdateVisualSpeed()
    {
        _targetSpeed = baseSpeed + (_totalUpgradeLevels * 0.1f);
    }

    public void AddUpgradeLevel()
    {
        _totalUpgradeLevels++;
        UpdateVisualSpeed();
    }

    public float GetCurrentSpeed() => _currentVisualSpeed;
    public float GetTargetSpeed() => _targetSpeed;
}
