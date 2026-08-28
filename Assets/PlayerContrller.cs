using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float thrust = 10f; // Aumentado pois AddForce exige forças maiores que alterações de posição

    private Vector2 moveDirection;
    private bool isThrusting;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Lê os inputs do jogador no Update para garantir precisão de leitura
        HandleInput();
    }

    void FixedUpdate()
    {
        // Aplica a física no FixedUpdate para manter a simulação estável
        HandleMovement();
    }

    private void HandleInput()
    {
        isThrusting = Mouse.current.leftButton.isPressed;
        animator.SetBool("isMoving", isThrusting);

        if (isThrusting)
        {
            // Converte a posição do mouse na tela para coordenadas do mundo 2D
            Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            // Calcula a direção normalizada do personagem em direção ao mouse
            moveDirection = (mouseWorldPosition - transform.position).normalized;
        }
    }

    private void HandleMovement()
    {
        if (isThrusting)
        {
            // Aplica força contínua levando em consideração o tempo de física (Time.fixedDeltaTime)
            rb.AddForce(moveDirection * thrust * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }
}