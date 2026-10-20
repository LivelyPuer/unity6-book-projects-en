using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private float delay = 2.5f;

    public Transform RespawnPoint { get; set; }

    void OnEnable() => health.Died += OnDied;
    void OnDisable() => health.Died -= OnDied;

    private void OnDied()
    {
        combat.ResetCombo();   // attack interrupted, reset its state
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        motor.CanMove = false;
        yield return new WaitForSeconds(delay);
        // fade the screen here (SceneLoader from Chapter 26 or a CanvasGroup)
        motor.Teleport(RespawnPoint.position, RespawnPoint.rotation);
        animator.SetBool("Dead", false);
        animator.Play("Locomotion", 0, 0f);
        health.ResetFull();
        motor.CanMove = true;
    }
}
