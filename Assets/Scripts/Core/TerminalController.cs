using UnityEngine;

public class TerminalController : MonoBehaviour, IInteractable
{
    public Light terminalLight;
    
    private bool _isActive = false;

    public void Interact()
    {

        _isActive = !_isActive;

        if (_isActive)
        {
            terminalLight.color = Color.green;
            Debug.Log("Estado del sistema: [Activo]");
        }
        else
        {
            terminalLight.color = Color.red;
            Debug.Log("Estado del sistema: [Inactivo]");
        }
    }

    private void Awake()
    {
        if (terminalLight != null)
        {
            terminalLight.color = Color.red;
            _isActive = false;
        }
    }
}