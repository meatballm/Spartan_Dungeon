using UnityEngine;
using System;
using System.Collections;

public class Buff : MonoBehaviour
{
    private PlayerController player;

    [Header("UI")]
    [SerializeField] private GameObject buffIconPrefab;  // BuffIcon 프리팹
    [SerializeField] private Transform buffContainer;    // Canvas 내 부모(예: BuffContainer)

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        if (player == null)
            Debug.LogError("Buff 스크립트가 붙은 오브젝트에 PlayerController가 없습니다!");
        if (buffIconPrefab == null || buffContainer == null)
            Debug.LogError("buffIconPrefab 또는 buffContainer 할당을 잊으셨습니다!");
    }

    public void ApplySpeedBuff(ItemData item, float speedMultiplier, float duration)
    {
        StartCoroutine(BuffRoutine(
            item.icon,
            () => player.moveSpeed *= speedMultiplier,
            () => player.moveSpeed /= speedMultiplier,
            duration));
    }

    // 점프력 버프
    public void ApplyJumpBuff(ItemData item, float jumpMultiplier, float duration)
    {
        StartCoroutine(BuffRoutine(
            item.icon,
            () => player.jumpPower *= jumpMultiplier,
            () => player.jumpPower /= jumpMultiplier,
            duration));
    }

    private IEnumerator BuffRoutine(Sprite icon, Action onStart, Action onEnd, float duration)
    {
        var go = Instantiate(buffIconPrefab, buffContainer);
        var buffIcon = go.GetComponent<BuffIcon>();
        buffIcon.Init(icon, duration);

        onStart();
        yield return new WaitForSeconds(duration);
        onEnd();
    }
}
