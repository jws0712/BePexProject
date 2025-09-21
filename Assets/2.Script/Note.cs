using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float noteSpeed;
    [SerializeField] private AudioClip noteHitSFX;

    private SpriteRenderer spriteRenderer;

    public SpriteRenderer Renderer => spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += Vector3.down * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        if (spriteRenderer.sprite == null) return;

        SoundManager.Instance.PlaySFX(noteHitSFX);
        spriteRenderer.sprite = null;
    }
}
