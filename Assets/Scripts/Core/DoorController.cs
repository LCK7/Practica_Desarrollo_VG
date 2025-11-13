using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    private bool _isOpen = false;

    public void Interact()
    {
        _isOpen = !_isOpen;
        Debug.Log(_isOpen ? "La puerta se ha ABIERTO." : "La puerta se ha CERRADO.");

        // Aquí activarías una animación o rotarías el objeto.
    }
}