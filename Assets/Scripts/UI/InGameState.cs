using UnityEngine;

public class InGameState : UIState
{
    public InGameState(UIManager uiManager) : base(uiManager) { }

    public override void Enter()
    {
        Debug.Log("Entrando al estado de juego");
        m_uiManager.inGameHudPanel.SetActive(true);
        m_uiManager.mainMenuPanel.SetActive(false); // Asegura que el menú se oculta
        m_uiManager.pauseMenuPanel.SetActive(false); // Por si venías de pausa

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public override void Exit()
    {
        Debug.Log("Saliendo del estado de juego");
        m_uiManager.inGameHudPanel.SetActive(false);
    }
}
