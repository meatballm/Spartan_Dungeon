using UnityEngine;
using System.Collections;

public class Buff : MonoBehaviour
{
    private PlayerController player;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        if (player == null)
            Debug.LogError("Buff 컴포넌트가 붙은 오브젝트에 PlayerController가 없습니다!");
    }

    // 이동 속도 버프
    public void ApplySpeedBuff(float speedMultiplier, float duration)
    {
        StartCoroutine(SpeedBuffCoroutine(speedMultiplier, duration));
    }

    private IEnumerator SpeedBuffCoroutine(float speedMultiplier, float duration)
    {
        float originalSpeed = player.moveSpeed;
        player.moveSpeed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        player.moveSpeed = originalSpeed;
    }

    // 데미지 버프
    public void ApplyJumpBuff(float bonusJump, float duration)
    {
        StartCoroutine(JumpBuffCoroutine(bonusJump, duration));
    }

    private IEnumerator JumpBuffCoroutine(float bonusJump, float duration)
    {
        float originalJump = player.jumpPower;
        player.jumpPower *= bonusJump;
        yield return new WaitForSeconds(duration);
        player.jumpPower = originalJump;
    }
}
