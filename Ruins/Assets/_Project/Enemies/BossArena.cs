using UnityEngine;
using UnityEngine.AI;

// Boss arena: when the hero enters, wakes the Guardian, shows its bar
public class BossArena : MonoBehaviour
{
    [SerializeField] private BossController boss;
    [SerializeField] private CanvasGroup bossBar;
    [SerializeField] private Health bossHealth;
    [SerializeField] private Health playerHealth;
    [SerializeField] private string bossName = "Guardian of the Ruins";
    private bool started;
    private Vector3 home;
    private Quaternion homeRotation;

    void Start()
    {
        boss.enabled = false;
        bossBar.alpha = 0f;
        home = boss.transform.position;
        homeRotation = boss.transform.rotation;
        bossHealth.Died += OnBossDied;
        playerHealth.Died += OnPlayerDied;
    }

    void OnTriggerEnter(Collider other)
    {
        if (started || !other.CompareTag("Player")) return;
        started = true;
        boss.enabled = true;
        bossBar.alpha = 1f;
        CombatMusic.Engaged.Add(this);
        Toast.Show(bossName);
    }

    // Hero died: the Guardian goes back to its post and heals up
    private void OnPlayerDied()
    {
        if (!started || !bossHealth.IsAlive) return;
        started = false;
        boss.enabled = false;
        bossBar.alpha = 0f;
        CombatMusic.Engaged.Remove(this);
        boss.GetComponent<NavMeshAgent>().Warp(home);
        boss.transform.rotation = homeRotation;
        boss.ResetFight();
    }

    private void OnBossDied()
    {
        bossBar.alpha = 0f;
        CombatMusic.Engaged.Remove(this);
        Toast.Show("The Guardian has fallen. The Ruins are free!");
    }
}
