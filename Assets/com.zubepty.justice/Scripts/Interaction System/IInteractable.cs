public interface IInteractable
{
    void Interact();
    void CancelInteraction();

    bool IsInteractionOngoing { get; }
    string PromptMessage { get; }
}