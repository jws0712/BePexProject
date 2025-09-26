//UnityEngine
using UnityEngine;

public class Note : MonoBehaviour
{
    private int noteHitLine;
    private double noteSpawnTime;
    private double noteHitTime;

    private bool isQuit;

    public double NoteHitTime => noteHitTime;
    public int NoteHitLine => noteHitLine;

    public void Initialize(int hitLine, double spawnTime)
    {
        noteHitLine = hitLine;
        noteSpawnTime = spawnTime;
        noteHitTime = noteSpawnTime + GameManager.Instance.NoteTravelTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;

        double t = (SoundManager.Instance.SongPosition - noteSpawnTime) / GameManager.Instance.NoteTravelTime;

        float targetY = Mathf.LerpUnclamped(
            GameManager.Instance.NoteSpawnTransformCenter.position.y, 
            GameManager.Instance.Center.position.y, 
            (float)t);

        transform.position = new Vector2(transform.position.x, targetY);
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
