using UnityEngine;

public class AIController : MonoBehaviour

{
    [Header("AI Settings")]
    public GameObject[] waypoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRadius = 10f;
    public float loseSightRadius = 15f;
    private AIState _currentState;

    private void Awake()
    {
        ChangeState(new PatrolState(this));
    }

    private void Update()
    {
        _currentState?.UpdateState();
    }

    public void ChangeState(AIState newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState.OnEnter();
    }
}
