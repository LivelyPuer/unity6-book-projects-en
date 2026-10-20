using UnityEngine;
using UnityEngine.AI;

// Feeds the NavMesh agent's speed to the Animator (locomotion tree)
public class AgentAnimator : MonoBehaviour
{
    private static readonly int SpeedId = Animator.StringToHash("Speed");

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;

    void Update()
    {
        float speed = agent.enabled ? agent.velocity.magnitude : 0f;
        animator.SetFloat(SpeedId, speed, 0.1f, Time.deltaTime);
    }
}
