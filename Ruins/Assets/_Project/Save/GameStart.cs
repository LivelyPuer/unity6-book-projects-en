using UnityEngine;
using UnityEngine.SceneManagement;

// Level start: reset the objective, load the save, set the respawn point
public class GameStart : MonoBehaviour
{
    [SerializeField] private Objective objective;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private PlayerRespawn respawn;
    [SerializeField] private Transform startPoint;
    [SerializeField] private int slot = 0;

    void Start()
    {
        objective.ResetRuntime();
        respawn.RespawnPoint = startPoint;
        if (saveSystem.TryLoad(slot))
            Toast.Show("Game loaded");
    }

    public void NewGame()
    {
        saveSystem.Delete(slot);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
