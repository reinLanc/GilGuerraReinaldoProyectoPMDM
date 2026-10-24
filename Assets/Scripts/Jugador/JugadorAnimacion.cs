using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class JugadorAnimacion : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EstadoJugador? estadoActual = null;

    private readonly Dictionary<EstadoJugador, int> hashes = new Dictionary<EstadoJugador, int>();

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach (EstadoJugador estado in Enum.GetValues(typeof(EstadoJugador)))
        {
            hashes[estado] = Animator.StringToHash(estado.ToString());
        }
    }

    public void CambiarEstado(EstadoJugador nuevoEstado)
    {
        if (estadoActual != nuevoEstado)
        {
            estadoActual = nuevoEstado;
            animator.Play(hashes[nuevoEstado]);
        }
    }

    public void MirarAIzquierda(bool izquierda)
    {
        // flipX 
        spriteRenderer.flipX = izquierda;
    }
}