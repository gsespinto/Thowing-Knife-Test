using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 2.5f;
    CharacterController charController;
    [SerializeField] GameObject playerModel;
    float gravity = 0.0f;

    private ThrowKnife throwKnifeScript;
    void Start()
    {
        charController = this.GetComponent<CharacterController>();
        throwKnifeScript = this.GetComponent<ThrowKnife>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Gravity();
    }

    void Movement()
    {
        Vector3 moveVec = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical")) * speed;
        charController.Move(moveVec);

        RotateToMovement(moveVec);
    }

    void Gravity()
    {
        if (charController.isGrounded)
        {
            gravity = 0.0f;
        }
        else
        {
            gravity -= 2 * Time.deltaTime;
        }
    }

    void RotateToMovement(Vector3 moveVec)
    {
        if (!throwKnifeScript.bLookingAtThrow)
        {
            moveVec.y = 0.0f;
            if (moveVec != Vector3.zero)
            {
                playerModel.transform.LookAt(this.transform.position + moveVec);
            }
        }
    }
}
