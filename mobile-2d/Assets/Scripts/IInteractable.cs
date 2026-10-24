using UnityEngine;
using System;

public interface IInteractable
{
    string InteractionPrompt { get; }
    void Interact();
    Transform GetTransform();
}
