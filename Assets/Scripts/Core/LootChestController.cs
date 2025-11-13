using UnityEngine;

public class LootChestController : MonoBehaviour, IInteractable
{
    private bool _isOpened = false;

    public void Interact()
    {
        if (_isOpened)
        {
            Debug.Log("Este cofre ya ha sido abierto.");
            return;
        }

        _isOpened = true;
        Debug.Log("¡Has abierto el cofre y encontrado un tesoro!");

        // Aquí instanciarías un item, añadirías oro al inventario, etc.
    }
}