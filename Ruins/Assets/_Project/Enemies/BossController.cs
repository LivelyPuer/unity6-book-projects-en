using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossAttack
{
    public string trigger;          // Animator trigger
    public float minRange, maxRange;
    public float windup = 0.6f;     // clip start to end of strike, s
    public float recovery = 1f;     // vulnerability window after it, s
    public int phase = 1;           // available from this phase on
}

public class BossController : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform player;
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private List<BossAttack> attacks;
    [SerializeField] private float phase2Threshold = 0.5f;
    [SerializeField] private float phase2SpeedUp = 1.25f;

    [Header("Vulnerability")]
    [SerializeField] private float vulnerableDamage = 1.5f;  // in the window
    [SerializeField] private float armoredDamage = 0.4f;     // outside it

    public int Phase { get; private set; } = 1;
    public bool IsVulnerable { get; private set; }
    public event System.Action<int> PhaseChanged;
    public event System.Action<bool> VulnerabilityChanged;

    private float busyTimer, vulnerableUntil;
    private BossAttack last;

    void Start()
    {
        if (!agent.isOnNavMesh) agent.Warp(transform.position);
        health.DamageScale = armoredDamage;
    }

    void OnEnable() => health.Changed += OnHealthChanged;

    void OnDisable()
    {
        health.Changed -= OnHealthChanged;
        SetVulnerable(false);
    }

    void Update()
    {
        if (!health.IsAlive) return;
        if (IsVulnerable && Time.time > vulnerableUntil) SetVulnerable(false);
        busyTimer -= Time.deltaTime;
        if (busyTimer > 0f) return;

        float distance = Vector3.Distance(transform.position, player.position);
        BossAttack attack = PickAttack(distance);
        if (attack == null)
        {
            agent.SetDestination(player.position);   // close in
            return;
        }

        agent.ResetPath();
        FacePlayer();
        animator.SetTrigger(attack.trigger);
        float speed = Phase == 2 ? phase2SpeedUp : 1f;
        busyTimer = (attack.windup + attack.recovery + 0.5f) / speed;
        last = attack;
    }

    // Animation event at the end of the active frames: the Guardian
    // overextends and opens up, so the vulnerability window starts
    public void AttackEnd()
    {
        if (last == null || !health.IsAlive) return;
        float speed = Phase == 2 ? phase2SpeedUp : 1f;
        vulnerableUntil = Time.time + last.recovery / speed;
        SetVulnerable(true);
    }

    private void SetVulnerable(bool value)
    {
        if (IsVulnerable == value) return;
        IsVulnerable = value;
        health.DamageScale = value ? vulnerableDamage : armoredDamage;
        VulnerabilityChanged?.Invoke(value);
    }

    private BossAttack PickAttack(float distance)
    {
        var options = new List<BossAttack>();
        foreach (var a in attacks)
            if (a.phase <= Phase && distance >= a.minRange
                && distance <= a.maxRange && a != last)  // no repeats in a row
                options.Add(a);
        return options.Count > 0 ? options[Random.Range(0, options.Count)]
                                 : null;
    }

    private void OnHealthChanged(float current, float max)
    {
        if (Phase == 1 && current / max <= phase2Threshold)
        {
            Phase = 2;
            SetVulnerable(false);
            animator.SetTrigger("Roar");
            animator.speed = phase2SpeedUp;   // faster and faster
            busyTimer = 2f;                    // pause for the phase change
            health.Invulnerable = true;
            Invoke(nameof(EndPhaseTransition), 2f);
            PhaseChanged?.Invoke(Phase);
        }
    }

    private void EndPhaseTransition() => health.Invulnerable = false;

    // Fight reset (hero died): back to phase one and normal speed
    public void ResetFight()
    {
        CancelInvoke();
        SetVulnerable(false);
        Phase = 1;
        animator.speed = 1f;
        busyTimer = 0f;
        last = null;
        health.Invulnerable = false;
        health.ResetFull();
    }

    private void FacePlayer()
    {
        Vector3 to = player.position - transform.position;
        to.y = 0f;
        if (to.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(to);
    }
}
