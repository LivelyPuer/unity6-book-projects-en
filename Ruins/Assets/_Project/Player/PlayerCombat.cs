using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private static readonly int AttackId = Animator.StringToHash("Attack");
    private static readonly int ComboId = Animator.StringToHash("ComboIndex");

    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private Animator animator;

    [Header("Hitbox")]
    [SerializeField] private Transform hitPoint;     // at the sword blade
    [SerializeField] private float hitRadius = 0.9f;
    [SerializeField] private LayerMask hittable;

    [Header("Combo")]
    [SerializeField] private float[] comboDamage = { 10f, 12f, 22f };
    [SerializeField] private float bufferTime = 0.3f;

    [Header("Feedback")]
    [SerializeField] private float lunge = 6f;         // forward lunge, m/s
    [SerializeField] private float aimRadius = 3.5f;   // aim assist
    [SerializeField] private TrailRenderer trail;      // blade trail
    [SerializeField] private SoundData swingSound;

    private readonly Collider[] hits = new Collider[16];
    private readonly HashSet<IDamageable> hitThisSwing = new();
    private int comboIndex;
    private bool attacking, comboWindow, hitboxActive;
    private float bufferTimer;

    public event System.Action<IDamageable, Vector3> HitLanded;
    public bool IsFinisher => comboIndex == comboDamage.Length - 1;

    void OnEnable() => input.AttackPressed += BufferAttack;
    void OnDisable() => input.AttackPressed -= BufferAttack;

    private void BufferAttack() => bufferTimer = bufferTime;

    void Update()
    {
        if (bufferTimer > 0f)
        {
            bufferTimer -= Time.deltaTime;
            if (!attacking && motor.CanMove && motor.IsGrounded
                && !motor.IsDodging)
                StartAttack(0);
            else if (attacking && comboWindow
                     && comboIndex < comboDamage.Length - 1)
                StartAttack(comboIndex + 1);
        }
        if (hitboxActive) ApplyHits();
    }

    private void StartAttack(int index)
    {
        bufferTimer = 0f;
        comboIndex = index;
        attacking = true;
        comboWindow = false;
        motor.CanMove = false;
        AimAtNearest();
        motor.Nudge(transform.forward * lunge);
        animator.SetInteger(ComboId, index);
        animator.SetTrigger(AttackId);
    }

    // Turn to the nearest enemy close by so the swing doesn't miss
    private void AimAtNearest()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position,
            aimRadius, hits, hittable, QueryTriggerInteraction.Collide);
        float best = float.MaxValue;
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            Vector3 to = hits[i].transform.position - transform.position;
            to.y = 0f;
            if (to.sqrMagnitude < best) { best = to.sqrMagnitude; dir = to; }
        }
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    private void ApplyHits()
    {
        int count = Physics.OverlapSphereNonAlloc(hitPoint.position,
            hitRadius, hits, hittable, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            if (!hits[i].TryGetComponent(out IDamageable target)) continue;
            if (!hitThisSwing.Add(target)) continue;   // once per swing
            Vector3 point = hits[i].ClosestPoint(hitPoint.position);
            target.TakeDamage(comboDamage[comboIndex], point);
            HitLanded?.Invoke(target, point);
        }
    }

    // --- animation events ---
    public void AttackStart()
    {
        hitThisSwing.Clear();
        hitboxActive = true;
        trail.emitting = true;
        SoundPlayer.Instance.Play(swingSound, hitPoint.position);
    }

    public void AttackEnd()
    {
        hitboxActive = false;
        trail.emitting = false;
    }

    public void ComboWindow() => comboWindow = true;

    public void AttackFinished(int index)
    {
        if (index != comboIndex) return;   // event from previous clip
        ResetCombo();
        motor.CanMove = true;
    }

    // Interrupt the attack from outside (death, stun): the clip won't
    // finish, and the AttackFinished event won't fire
    public void ResetCombo()
    {
        attacking = comboWindow = hitboxActive = false;
        comboIndex = 0;
        bufferTimer = 0f;
        trail.emitting = false;
    }

    void OnDrawGizmosSelected()
    {
        if (hitPoint == null) return;
        Gizmos.color = hitboxActive ? Color.red : new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(hitPoint.position, hitRadius);
    }
}
