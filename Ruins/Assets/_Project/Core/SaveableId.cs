using UnityEngine;

[DisallowMultipleComponent]
public class SaveableId : MonoBehaviour
{
    [SerializeField] private string id;
    public string Id => id;

    // Called when the component is added in the Editor
    void Reset() => id = System.Guid.NewGuid().ToString("N");

    [ContextMenu("Generate New ID")]
    private void Regenerate() => id = System.Guid.NewGuid().ToString("N");
}
