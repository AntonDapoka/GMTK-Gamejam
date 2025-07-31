using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trail;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingDuration = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private KeyCode dashingKeyCode = KeyCode.LeftShift;
    [SerializeField] private AnimationCurve dashingCurve;


    private Vector3 moveDirection;

    private bool canDash = true;
    private bool isDashing;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (!trail)
        {
            trail = GetComponent<TrailRenderer>();
        }
    }

    private void Update()
    {
        HandleInput();

        if (!isDashing)
        {
            Move();
        }
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
       
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (Input.GetKeyDown(dashingKeyCode) && canDash && moveDirection != Vector3.zero)
        {
            StartCoroutine(Dash());
        }
    }

    private void Move()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector3(moveDirection.x*moveSpeed,0, moveDirection.z * moveSpeed); 
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //rb.velocity = new Vector3(moveDirection.x* dashingPower,0f, moveDirection.z * dashingPower);
        trail.emitting = true;

        float elapsed = 0f;
        Vector3 dashDirection = moveDirection;

        while (elapsed < dashingDuration)
        {
            float t = elapsed / dashingDuration;
            float power = dashingCurve.Evaluate(t) * dashingPower;


            rb.velocity = dashDirection * power;
            elapsed += Time.deltaTime;
            yield return null;
        }



        rb.velocity = Vector3.zero;
        trail.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
