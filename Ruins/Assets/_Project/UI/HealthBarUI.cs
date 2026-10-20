using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health health;    // component from Chapter 9
    [SerializeField] private Image fill;       // Image Type: Filled
    [SerializeField] private Image trail;      // light bar under the fill
    [SerializeField] private float trailDelay = 0.4f;
    [SerializeField] private float trailSpeed = 0.8f; // fraction per second

    private float target = 1f;
    private float hold;

    void OnEnable() => health.Changed += OnChanged;
    void OnDisable() => health.Changed -= OnChanged;

    private void OnChanged(float current, float max)
    {
        target = current / max;
        fill.fillAmount = target;
        if (trail.fillAmount > target) hold = trailDelay; // damage
        else trail.fillAmount = target;                   // healing
    }

    void Update()
    {
        if (trail.fillAmount <= target) return;
        hold -= Time.deltaTime;
        if (hold <= 0f)
            trail.fillAmount = Mathf.MoveTowards(
                trail.fillAmount, target, trailSpeed * Time.deltaTime);
    }
}
