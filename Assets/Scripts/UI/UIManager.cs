using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;

/// <summary>
/// Gestiona los estados de la UI y las transiciones entre ellos
/// Utiliza el Patrón de Diseño State, arquitectura limpia y escalable
/// Implementa un Singleton para un acceso global sencillo
/// </summary>
public class UIManager : MonoBehaviour
{
    // Singleton Pattern
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject inGameHudPanel;

    // Estados de la UI
    private UIState _currentState;
    public GameObject victoryPanel;

    [Header("Loading Screen")]
    public GameObject loadingScreenPanel;
    public Slider loadingBar;

    [Header("HUD")]
    public TextMeshProUGUI infoText;

    public MainMenuState MainMenuState { get; private set; }
    public InGameState InGameState { get; private set; }
    public PauseMenuState PauseMenuState { get; private set; }

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Para que persista entre escenas

        // Inicialización de los estados
        MainMenuState = new MainMenuState(this);
        InGameState = new InGameState(this);
        PauseMenuState = new PauseMenuState(this);
    }

    private void Start()
    {
        // El estado inicial al arrancar el juego
        ChangeState(MainMenuState);
    }
    
    private void Update()
    {
        // Lógica para pausar el juego
        Keyboard keyboard = Keyboard.current;
        if (keyboard.escapeKey.wasPressedThisFrame)
        {
            if (_currentState == InGameState)
            {
                ChangeState(PauseMenuState);
            }
            else if (_currentState == PauseMenuState)
            {
                ChangeState(InGameState);
            }
        }
    }

    public void ChangeState(UIState newState)
    {
        // Salir del estado actual si existe
        _currentState?.Exit();
        
        // Entrar en el nuevo estado
        _currentState = newState;
        _currentState.Enter();
    }

    // Métodos para los botones de la UI
    public async void OnPlayButtonClicked()
    {
        // mostrar la pantalla de carga
        loadingScreenPanel.SetActive(true);
        mainMenuPanel.SetActive(false); // ocultar el menú principal

        // inicia la carga de la escena de forma asíncrona
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync("Level_001");

        // evitar que la escena se active automáticamente al llegar al 90%
        sceneLoadOperation.allowSceneActivation = false;

        // mientras la escena se carga...
        while (!sceneLoadOperation.isDone)
        {
            // el progreso de LoadSceneAsync se detiene en 0.9 hasta que se permite la activación
            // mapeamos al 90% para la barra de progreso
            float progress = Mathf.Clamp01(sceneLoadOperation.progress / 0.9f);
            loadingBar.value = progress;

            // si la carga ha alcanzado el 90%...
            if (sceneLoadOperation.progress >= 0.9f)
            {
                // podemos mostrar un aviso y esperar una tecla del usuario (opcional)
                // aqui solo activamos la escena cargada
                sceneLoadOperation.allowSceneActivation = true;
            }

            // es el equivalente de 'yield return null' en una corrutina
            await Task.Yield();
        }

        // una vez que la escena está cargada y activa, cambiamos el estado
        ChangeState(InGameState);
        loadingScreenPanel.SetActive(false);
    }

    public void OnResumeButtonClicked()
    {
        ChangeState(InGameState);
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    public void ShowVictoryPanel()
    {
        inGameHudPanel.SetActive(false);
        victoryPanel.SetActive(true);
    }

    private void OnEnable()
    {
        GameEvents.OnTargetFocused += HandleTargetFocused;
        GameEvents.OnTargetLost += HandleTargetLost;
    }

    private void OnDisable()
    {
        GameEvents.OnTargetFocused -= HandleTargetFocused;
        GameEvents.OnTargetLost -= HandleTargetLost;
    }

    private void HandleTargetLost(GameObject @object)
    {
        // COMPROBACIÓN DE SEGURIDAD: Solo intentar interactuar si infoText está asignado
        if (infoText != null) 
        {
            infoText.gameObject.SetActive(false);
        }
        else
        {
            // Opcional: registrar un error para recordar la asignación
            Debug.LogError("UIManager: infoText no está asignado en el Inspector. Asigna tu TextMeshProUGUI.");
        }
    }

    private void HandleTargetFocused(GameObject @object)
    {
        // COMPROBACIÓN DE SEGURIDAD: Solo intentar interactuar si infoText está asignado
        if (infoText != null)
        {
            infoText.gameObject.SetActive(true);
        }
    }
}