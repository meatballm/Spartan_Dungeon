using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlatform : MonoBehaviour
{
    public float JumpPower=100;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObj = collision.gameObject;
        Rigidbody rigidbody = otherObj.GetComponent<Rigidbody>();
        rigidbody.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        CharacterManager.Instance.Player.controller.DoubleJumpCharge();
    }
}
