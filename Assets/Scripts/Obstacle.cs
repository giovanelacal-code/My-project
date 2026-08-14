using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        // Redimensionamento aleatório
        float ramdomSize = Random.Range(0.5f, 1f);
        transform.localScale = new Vector3(ramdomSize, ramdomSize, 1);

        // Acessa o Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Aplica o impulso inicial
        Impulsionar();
    }

    // Chamado automaticamente no momento em que o meteoro PÁRA de colidir com algo
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Se após a colisão a velocidade for quase zero (parou ou desacelerou muito), aplica a força novamente
        if (rb.linearVelocity.magnitude < 0.5f)
        {
            Impulsionar();
        }
    }

    // Função privada para reaproveitar a força de translação e rotação
    private void Impulsionar()
    {
        // 1. Translação com AddForce
        Vector2 direcao = Random.insideUnitCircle.normalized;
        float forcaImpulso = Random.Range(10f, 25f);
        rb.AddForce(direcao * forcaImpulso, ForceMode2D.Impulse);

        // 2. Rotação com AddTorque
        float forcaGiro = Random.Range(-25f, 25f);
        rb.AddTorque(forcaGiro, ForceMode2D.Impulse);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Destrói ao sair da tela
    }
}