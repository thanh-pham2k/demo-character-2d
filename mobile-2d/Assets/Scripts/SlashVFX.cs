using System.Collections;
using UnityEngine;

public class SlashVFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] slashFrames;
    [SerializeField] private float frameDuration = 0.045f; // Fast lifetime ~180ms total

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Play()
    {
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        if (slashFrames != null && slashFrames.Length > 0)
        {
            for (int i = 0; i < slashFrames.Length; i++)
            {
                spriteRenderer.sprite = slashFrames[i];
                yield return new WaitForSeconds(frameDuration);
            }
        }
        Destroy(gameObject);
    }
}
