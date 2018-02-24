using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cs_Muuki : MonoBehaviour {

    public float walkSpeed = 2, runSpeed = 6;
    public float gravity;
    public float jumpH = 1;
    
    public float pushPower = 2.0F;

    Animator animator;
    CharacterController charCon;

    public float smoothTurnTime = 0.2f;
    float smoothTurnVel;
    public float smoothSpeedTime = 0.2f;
    float smoothSpeedVel;
    float currSpeed;
    float velY;


	void Start () {
        animator = GetComponent<Animator>();
        charCon = GetComponent<CharacterController>();
	}

    void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }

        if (inputDir != Vector2.zero) {
            float targetRot = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref smoothTurnVel, smoothTurnTime);
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currSpeed = Mathf.SmoothDamp(currSpeed, targetSpeed, ref smoothSpeedVel, smoothSpeedTime);

        velY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currSpeed + Vector3.up * velY;
        charCon.Move(velocity * Time.deltaTime);
        if (charCon.isGrounded) velY = 0;
        //transform.Translate(transform.forward * currSpeed * Time.deltaTime, Space.World);

        float animSpdPercent = ((running) ? 0.5f : 0.25f) * inputDir.magnitude;
        animator.SetFloat("speedPercent", animSpdPercent, smoothSpeedTime, Time.deltaTime);

	}

    void Jump() {
        if (charCon.isGrounded) {
            float jumpVel = Mathf.Sqrt(-2 * gravity * jumpH);
            velY = jumpVel;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
        //body.AddForceAtPosition(pushDir * pushPower, hit.point);
        //body.AddForce(pushDir * pushPower);
    }
}
