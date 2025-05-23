using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPlatform : MonoBehaviour
{
    public float JumpPower=100;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObj = collision.gameObject;
        Rigidbody rigidbody = otherObj.GetComponent<Rigidbody>();
        rigidbody.AddForce(new Vector3(0,1,1) * JumpPower, ForceMode.Impulse);
        CharacterManager.Instance.Player.controller.DoubleJumpCharge();
    }
}
