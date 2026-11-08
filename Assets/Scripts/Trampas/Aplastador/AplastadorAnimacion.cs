using UnityEngine;

public class AplastadorAnimacion : MonoBehaviour
{
    [SerializeField]
    string estadoNormal = "Idle";
    [SerializeField]
    string estadoCayendo = "Enojado";
    [SerializeField]
    string estadoGolpe = "GolpeAbajo";
    Animator animator;
    TrampaVertical trampa;
    string estadoActual = "";
    void Start()
    {
        animator = GetComponent<Animator>();
        trampa = GetComponent<TrampaVertical>();
    }

    void Update()
    {
        string nuevoEstado = estadoNormal;

        if (trampa.EnReposoAbajo)
        {
            nuevoEstado = estadoGolpe;
        }
        else if (trampa.Cayendo)
        {
            nuevoEstado = estadoCayendo;
        }

        if (nuevoEstado != estadoActual)
        {
            estadoActual = nuevoEstado;
            animator.Play(estadoActual);
        }
    }
}
