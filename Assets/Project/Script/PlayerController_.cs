using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_ : MonoBehaviour
{
    public CharacterController SelectPlayer;
    public float turnSpeed;
    public float moveSpeed;
    public float JumpPow;

    private float Gravity;
    private float xRotate, yRotate, xRotateMove, yRotateMove;
    private Vector3 MoveDir;


    // Start is called before the first frame update
    void Start()
    {
        turnSpeed = 500.0f;
        moveSpeed = 5.0f;
        Gravity = 10.0f;
        MoveDir = Vector3.zero;
        JumpPow = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectPlayer == null)return;
        if (SelectPlayer.isGrounded)
        {
            MoveDir =  new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            MoveDir = SelectPlayer.transform.TransformDirection(MoveDir);
            MoveDir *= moveSpeed;


            if(Input.GetButton("Jump")) MoveDir.y = JumpPow;
        }
        else
        {
            MoveDir.y -= Gravity * Time.deltaTime;
        }
        SelectPlayer.Move(MoveDir * Time.deltaTime);

        if (Input.GetMouseButton(0))
        {
            yRotateMove = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
            yRotate = transform.eulerAngles.y + yRotateMove;

            xRotateMove = -Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;
            xRotate = xRotate + xRotateMove;

            xRotate = Mathf.Clamp(xRotate, -90, 90);

            transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        }
    }
}
