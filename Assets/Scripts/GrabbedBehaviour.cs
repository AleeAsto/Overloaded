using UnityEngine;

public abstract class GrabbableBehaviour : MonoBehaviour
{
    public virtual void OnPickedUp(PlayerGrab player) {}
    public virtual void OnDropped(PlayerGrab player) {}
    public virtual void OnCarriedUpdate(PlayerGrab player, Vector2 moveInput) {}

    public virtual bool WantsToBlockDrop() => false;

    // (Opcional) Mensaje/razón para UI si quieres feedback
    public virtual string GetDropBlockHint() => null;
}
