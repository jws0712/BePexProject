using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Note : MonoBehaviour
{
    private float noteSpeed;

    private double noteHitTime;

    private AudioClip sfx;
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer Renderer => spriteRenderer;

    public double NoteHitTime => noteHitTime;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        noteSpeed = GameManager.Instance.NoteSpeed;
        noteHitTime = SoundManager.Instance.SongPosition + GameManager.Instance.NoteHitTime;
        sfx = GameManager.Instance.NoteHitSfx;
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
        SoundManager.Instance.PlaySFX(sfx);
    }

    private void OnBecameInvisible()
    {
        JudgementManager.Instance.JudgeNoteOutCamera(this);
    }
}
