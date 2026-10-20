using UnityEngine;

public interface IInteractable
{
    string GetPrompt(GameObject actor);     // "Open", "Pick up"
    bool CanInteract(GameObject actor);
    void Interact(GameObject actor);
}
