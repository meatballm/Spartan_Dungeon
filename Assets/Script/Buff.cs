using UnityEngine;
using System;
using System.Collections;

public class Buff : MonoBehaviour
{
    private PlayerController player;

    [Header("UI")]
    [SerializeField] private GameObject buffIconPrefab;  // BuffIcon ������
    [SerializeField] private Transform buffContainer;    // Canvas �� �θ�(��: BuffContainer)

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        if (player == null)
            Debug.LogError("Buff ��ũ��Ʈ�� ���� ������Ʈ�� PlayerController�� �����ϴ�!");
        if (buffIconPrefab == null || buffContainer == null)
            Debug.LogError("buffIconPrefab �Ǵ� buffContainer �Ҵ��� �����̽��ϴ�!");
    }

    public void ApplySpeedBuff(ItemData item, float speedMultiplier, float duration)
    {
        StartCoroutine(BuffRoutine(
            item.icon,
            () => player.moveSpeed *= speedMultiplier,
            () => player.moveSpeed /= speedMultiplier,
            duration));
    }

    // ������ ����
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
