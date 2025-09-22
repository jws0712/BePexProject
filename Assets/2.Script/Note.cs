using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Note : MonoBehaviour
{
    private float noteHitTime;
    private float noteSpeed;
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer Renderer => spriteRenderer;

    public float NoteHitTime => noteHitTime;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        noteSpeed = GameManager.Instance.NoteSpeed;
        noteHitTime = SoundManager.Instance.SongPosition + GameManager.Instance.NoteHitTime;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameStateType.Pause) return;
        transform.position += Vector3.down * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        if (spriteRenderer.sprite == null) return;

        spriteRenderer.sprite = null;
    }
}
