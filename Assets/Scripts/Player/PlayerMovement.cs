using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    public float MovementMaxSpeed;
    public float MovementSpeedIncrease;
    public float MovementSpeedDecrease;

    private Rigidbody2D Rb;
    private Vector2 InputMovement = new(0, 0);
    private bool IsGrounded;

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        UpdateGroundedState();
        float targetSpeed = InputMovement.x * MovementMaxSpeed;
        UpdateFriction(targetSpeed);
        UpdateMovement(targetSpeed);
    }

    private void UpdateGroundedState()
    {
        IsGrounded = Rb.IsTouching(new ContactFilter2D() { maxNormalAngle = 136, minNormalAngle = 44, useNormalAngle = true });
    }

    private void UpdateFriction(float targetSpeed)
    {
        if (targetSpeed != 0) Rb.sharedMaterial = new PhysicsMaterial2D() { friction = 0 };
        else Rb.sharedMaterial = new PhysicsMaterial2D() { friction = 2.5f };
    }

    private void UpdateMovement(float targetSpeed)
    {
        float speedChangeVelocity = MovementSpeedDecrease;
        if (targetSpeed < 0 && Rb.velocity.x < 0) speedChangeVelocity = MovementSpeedIncrease;
        if (targetSpeed > 0 && Rb.velocity.x > 0) speedChangeVelocity = MovementSpeedIncrease;
        float newSpeedX = Mathf.MoveTowards(Rb.velocity.x, targetSpeed, speedChangeVelocity * Time.deltaTime * 300);

        Rb.velocity = new Vector2(newSpeedX, Rb.velocity.y);
    }

    //Coyote time
    //Space hold
    //Invoked by unity action system
    private void OnJump()
    {
        if (IsGrounded)
        {
            if (Rb.velocity.y > 0) Rb.velocity = new Vector2(Rb.velocity.x, 0);
            Rb.AddForce(new Vector2(0, 800));
        }
    }

    //Invoked by unity action system
    private void OnMove(InputValue value)
    {
        InputMovement = value.Get<Vector2>();
    }
}
