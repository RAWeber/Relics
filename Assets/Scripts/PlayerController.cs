using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public float mouseSensitivityX = 1;
    public float mouseSensitivityY = 1;
    public float runSpeed = 6;
    public float jumpForce = 220;
    public LayerMask groundedMask;

    Rigidbody playerRigidbody;
    Transform cameraTransform;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    float verticalLookRotation;
    bool grounded;

    void Awake () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerRigidbody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        CalculateMove();
        CalculateLook();
        CalculateJump();
    }

    void CalculateMove()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(inputX, 0, inputY).normalized;
        Vector3 targetMoveAmount = moveDir * runSpeed;

        if (!grounded)
        {
            targetMoveAmount /= 2;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            targetMoveAmount /= 3;
        }
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
    }

    void CalculateLook()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);
        cameraTransform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void CalculateJump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            playerRigidbody.AddForce(transform.up * jumpForce);
        }

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        grounded = Physics.Raycast(ray, out hit, 1 + .1f, groundedMask);
    }

    void FixedUpdate()
    {
        Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + localMove);
    }
}
