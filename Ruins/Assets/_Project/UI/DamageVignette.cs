using UnityEngine;
using UnityEngine.UI;

// Red screen edges on damage: you feel the hit even if
// you are not looking at the health bar
public class DamageVignette : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image image;
    [SerializeField] private float peak = 0.8f;
    [SerializeField] private float fadeSpeed = 2.2f;   // fraction per second

    void OnEnable() => health.Damaged += OnDamaged;
    void OnDisable() => health.Damaged -= OnDamaged;
    void Start() => SetAlpha(0f);

    private void OnDamaged(float amount, Vector3 point) => SetAlpha(peak);

    void Update()
    {
        float a = image.color.a;
        if (a > 0f)
            SetAlpha(Mathf.MoveTowards(a, 0f,
                                       fadeSpeed * Time.unscaledDeltaTime));
    }

    private void SetAlpha(float a)
    {
        Color c = image.color;
        c.a = a;
        image.color = c;
    }
}
