//UnityEngine
using UnityEngine;

public class Note : MonoBehaviour
{
    private int noteHitLine;

    private bool isQuit;

    private double noteSpawnTime;
    private double noteHitTime;

    public int NoteHitLine => noteHitLine;
    public double NoteHitTime => noteHitTime;

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;

        double t = (GameManager.Instance.SongPosition - noteSpawnTime) / GameManager.Instance.NoteTravelTime;

        float targetY = Mathf.LerpUnclamped(GameManager.Instance.NoteSpawnTransformCenter.position.y, GameManager.Instance.JudgeLineCenter.position.y, (float)t);

        transform.position = new Vector2(transform.position.x, targetY);
    }

    public void Initialize(LineType hitLine, double spawnTime)
    {
        noteHitLine = (int)hitLine;
        noteSpawnTime = spawnTime;
        noteHitTime = noteSpawnTime + GameManager.Instance.NoteTravelTime;
    }

    private void OnApplicationQuit()
    {
        isQuit = true;
    }

    private void OnBecameInvisible()
    {
        if (isQuit) return;
        JudgeManager.Instance.JudgeNoteOutCamera(this);
    }
}
