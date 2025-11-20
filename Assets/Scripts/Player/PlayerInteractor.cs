using UnityEngine;
using UnityEngine.InputSystem;
using System; // Necesario para usar Action

public class PlayerInteractor : MonoBehaviour
{
    // Variables de configuración
    [SerializeField] private float _interactionDistance = 2f;

    // Variables internas del script
    private Camera _mainCamera;
    private PlayerInputActions _inputActions;
    
    // VARIABLE FALTANTE (para llevar el rastro del objeto enfocado)
    private GameObject _current; 

    private void Awake()
    {
        _mainCamera = Camera.main;
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        _inputActions.Player.Interact.performed -= OnInteract;
        _inputActions.Player.Disable();
    }

    private void Update()
    {
        DetectAndShowFeedback();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        PerformRaycastInteraction();
    }

    private void PerformRaycastInteraction()
    {
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    private void DetectAndShowFeedback()
    {
        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _interactionDistance))
        {
            if (hit.collider.GetComponent<IInteractable>() != null)
            {
                // Si es interactuable: Establecemos el foco
                SetFocus(hit.collider.gameObject);
            }
            else
            {
                // Si NO es interactuable: Limpiamos el foco
                ClearFocus();
            }
        }
        else
        {
            // Si no golpeamos nada: Limpiamos el foco
            ClearFocus();
        }
    }

    private void SetFocus(GameObject target)
    {
        if (_current == target) return;

        if (_current != null)
        {
            // COMENTADO: Si no tienes OutlineComponent, esto causa error CS0246.
            // _current.GetComponent<OutlineComponent>()?.Remove(); 
            GameEvents.TriggerTargetLost(_current);
        }

        _current = target;
        // COMENTADO: Si no tienes OutlineComponent, esto causa error CS0246.
        // _current.GetComponent<OutlineComponent>()?.Apply();
        GameEvents.TriggerTargetFocused(_current);
    }

    private void ClearFocus()
    {
        if (_current == null) return;

        // COMENTADO: Si no tienes OutlineComponent, esto causa error CS0246.
        // _current.GetComponent<OutlineComponent>()?.Remove();
        GameEvents.TriggerTargetLost(_current);

        _current = null;
    }
}