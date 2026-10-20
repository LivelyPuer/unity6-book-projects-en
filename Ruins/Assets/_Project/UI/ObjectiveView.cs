using TMPro;
using UnityEngine;

public class ObjectiveView : MonoBehaviour
{
    [SerializeField] private Objective objective;
    [SerializeField] private TMP_Text label;

    void OnEnable()
    {
        objective.Changed += Redraw;
        Redraw(objective);
    }

    void OnDisable() => objective.Changed -= Redraw;

    private void Redraw(Objective o) => label.text = o.Completed
        ? "The sanctuary gates are open"
        : $"{o.title}: {o.Progress} / {o.required}";
}
