using UnityEngine;

public class PatrolState : AIState
{
    public PatrolState(AIController controller) : base(controller) { }
    
    private int _currentWaypointIndex = 0;

    public override void OnEnter()
    {
        Debug.Log("Entrando en estado de Patrulla.");
        // Asegúrate de que m_agent.speed se establece si tienes Nav Mesh Agent
        // m_agent.speed = m_controller.patrolSpeed; // Comentar o descomentar si m_agent está inicializado
        GoToNextWaypoint();
    }

    public override void UpdateState()
    {
        // 1. Condición de transición: ¿vemos al jugador?
        // (Asumiendo que m_playerTransform está inicializado en la clase base AIState)
        if (Vector3.Distance(m_controller.transform.position, m_playerTransform.position) < m_controller.detectionRadius)
        {
            m_controller.ChangeState(new ChaseState(m_controller));
            return;
        }

        // 2. Lógica del estado: ¿hemos llegado al waypoint?
        // (Asegúrate de que m_agent está inicializado)
        if (m_agent != null && !m_agent.pathPending && m_agent.remainingDistance < 0.5f)
        {
            GoToNextWaypoint();
        }
    }

    public override void OnExit() { /* No necesita lógica por ahora */ }

    private void GoToNextWaypoint()
    {
        if (m_controller.waypoints.Length == 0) return;
        
        // ¡CORRECCIÓN CLAVE AQUÍ! Accedemos al componente Transform antes de la posición.
        m_agent.destination = m_controller.waypoints[_currentWaypointIndex].transform.position; 
        
        _currentWaypointIndex = (_currentWaypointIndex + 1) % m_controller.waypoints.Length;
    }
}