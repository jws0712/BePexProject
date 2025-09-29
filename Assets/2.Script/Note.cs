//UnityEngine
using UnityEngine;

public class Note : MonoBehaviour
{
    private int noteHitLine;

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

    public void Init(LineType hitLine, double spawnTime)
    {
        noteHitLine = (int)hitLine;
        noteSpawnTime = spawnTime;
        noteHitTime = noteSpawnTime + GameManager.Instance.NoteTravelTime;

        Debug.Log(noteSpawnTime);
    }
}
