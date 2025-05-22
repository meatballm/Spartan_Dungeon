using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuffIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    public void Init(Sprite sprite, float duration)
    {
        iconImage.sprite = sprite;
        iconImage.fillAmount = 1f;
        StartCoroutine(FillRoutine(duration));
    }

    private IEnumerator FillRoutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            iconImage.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
