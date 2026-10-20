using Unity.Cinemachine;
using UnityEngine;

// Hero combat feedback: sparks, sound, hit stop and camera shake
public class CombatFeedback : MonoBehaviour
{
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private Health health;
    [SerializeField] private CinemachineImpulseSource impulse;
    [SerializeField] private HitFlash hurtFlash;   // hero's red flash

    [Header("Hit: normal, heavy, armored")]
    [SerializeField] private GameObject hitVfx;
    [SerializeField] private GameObject heavyVfx;
    [SerializeField] private GameObject armorVfx;
    [SerializeField] private SoundData heavySound, armorSound;

    [Header("Damage to Hero")]
    [SerializeField] private SoundData hurtSound;

    void OnEnable()
    {
        combat.HitLanded += OnHitLanded;
        health.Damaged += OnHurt;
    }

    void OnDisable()
    {
        combat.HitLanded -= OnHitLanded;
        health.Damaged -= OnHurt;
    }

    private void OnHitLanded(IDamageable target, Vector3 point)
    {
        float scale = target is Health h ? h.DamageScale : 1f;
        if (scale < 1f)                          // armor: clang, no pause
        {
            Spawn(armorVfx, point);
            SoundPlayer.Instance.Play(armorSound, point);
            impulse.GenerateImpulse(0.15f);
        }
        else if (scale > 1f || combat.IsFinisher) // heavy hit
        {
            Spawn(heavyVfx, point);
            SoundPlayer.Instance.Play(heavySound, point);
            HitStop.Instance.Freeze(0.1f);
            impulse.GenerateImpulse(0.6f);
        }
        else
        {
            Spawn(hitVfx, point);                // target plays the sound
            HitStop.Instance.Freeze(0.05f);
            impulse.GenerateImpulse(0.25f);
        }
    }

    private void OnHurt(float amount, Vector3 point)
    {
        hurtFlash.Flash();
        SoundPlayer.Instance.Play(hurtSound, transform.position);
        HitStop.Instance.Freeze(0.06f);
        impulse.GenerateImpulse(0.8f);
    }

    private static void Spawn(GameObject prefab, Vector3 point)
    {
        if (prefab != null)
            Destroy(Instantiate(prefab, point, Quaternion.identity), 2f);
    }
}
