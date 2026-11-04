using UnityEngine;
public class JugadorAnimacion : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    EstadoJugador estadoActual = EstadoJugador.Parado;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator.Play(estadoActual.ToString());
    }

    public void CambiarEstado(EstadoJugador nuevoEstado)
    {
        if (nuevoEstado != estadoActual)
        {
            estadoActual = nuevoEstado;
            animator.Play(nuevoEstado.ToString());
        }
    }

    public void MirarAIzquierda(bool izquierda)
    {
        spriteRenderer.flipX = izquierda;
    }
}