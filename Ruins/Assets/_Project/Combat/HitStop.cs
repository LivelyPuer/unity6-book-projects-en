using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }
    private Coroutine running;
    private float resumeScale = 1f;

    void Awake() => Instance = this;

    public void Freeze(float duration, float scale = 0.02f)
    {
        if (Time.timeScale == 0f) return;          // game is paused
        if (running != null) StopCoroutine(running);
        else resumeScale = Time.timeScale;
        running = StartCoroutine(FreezeRoutine(duration, scale));
    }

    private IEnumerator FreezeRoutine(float duration, float scale)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        // Don't undo a pause that was set during the freeze
        if (Time.timeScale == scale) Time.timeScale = resumeScale;
        running = null;
    }
}
