using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Note : MonoBehaviour
{
    private float noteSpeed;
    private AudioClip sfx;
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer Renderer => spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        noteSpeed = GameManager.Instance.NoteSpeed;
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

        SoundManager.Instance.PlaySFX(sfx);
        spriteRenderer.sprite = null;
    }
}
