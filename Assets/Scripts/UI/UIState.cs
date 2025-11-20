/// <summary>
/// Clase base abstracta para todos los estados de la UI
/// Define el contrato que cada estado debe seguir: Enter() y Exit()
/// </summary>
public abstract class UIState
{
    protected UIManager m_uiManager;

    public UIState(UIManager uiManager)
    {
        m_uiManager = uiManager;
    }

    public abstract void Enter();
    public abstract void Exit();
}
