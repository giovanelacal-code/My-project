using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float accelerationForce = 8f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 720f; // Aumentado para resposta mais rápida ao mouse

    [Tooltip("Distância mínima para a nave parar de acelerar ao chegar no destino")]
    public float stoppingDistance = 0.5f;

    [Header("Sistema de Tiro")]
    public GameObject laserPrefab;
    public Transform firePoint;

    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private Vector3 currentMousePosition; // Posição atual do mouse em tempo real
    private bool isMoving = false;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // Comportamento no espaço (desliza por ~2s)
        rb.linearDamping = 1f;
        rb.gravityScale = 0f;

        targetPosition = transform.position;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            // 1. Atualiza a posição atual do mouse no mundo 2D (SEMPRE)
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            currentMousePosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
            currentMousePosition.z = 0f;

            // 2. Define o ponto de destino ao clicar com o botão esquerdo
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                targetPosition = currentMousePosition;
                isMoving = true;
            }
        }

        // 3. Disparo de Laser
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // --- 1. ROTAÇÃO INDEPENDENTE (SEMPRE APONTA PARA O MOUSE) ---
        RotateTowardsMouse();

        // --- 2. PROPULSÃO EM DIREÇÃO AO PONTO CLICADO ---
        if (!isMoving) return;

        Vector2 directionToTarget = (targetPosition - transform.position);
        float distance = directionToTarget.magnitude;

        // Se chegou perto do ponto clicado, para de aplicar força e deixa deslizar
        if (distance <= stoppingDistance)
        {
            isMoving = false;
            return;
        }

        // Aplica força na direção do PONTO CLICADO (não da frente da nave)
        rb.AddForce(directionToTarget.normalized * accelerationForce, ForceMode2D.Force);

        // --- 3. LIMITE DE VELOCIDADE ---
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void RotateTowardsMouse()
    {
        Vector2 directionToMouse = (currentMousePosition - transform.position);

        // Evita rotação tremida se o mouse estiver exatamente sobre a nave
        if (directionToMouse.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg - 90f;
            float nextAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(nextAngle);
        }
    }

    void Shoot()
    {
        if (laserPrefab != null && firePoint != null)
        {
            Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        }
    }
}