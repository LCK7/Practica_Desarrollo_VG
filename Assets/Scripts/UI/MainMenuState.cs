using UnityEngine;

public class MainMenuState : UIState
{
    public MainMenuState(UIManager uiManager) : base(uiManager) { }

    public override void Enter()
    {
        Debug.Log("Entrando al estado de Menú Principal");
        m_uiManager.mainMenuPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void Exit()
    {
        Debug.Log("Saliendo del estado de Menú Principal");
        m_uiManager.mainMenuPanel.SetActive(false);
    }
}
