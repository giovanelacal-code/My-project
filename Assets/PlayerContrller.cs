using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float thrust = 1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Chama a verificação do movimento a cada frame
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            animator.SetBool("isMoving", true);
        }
        else if (animator.GetBool("isMoving"))
        {
            animator.SetBool("isMoving", false);
        }
    }
}