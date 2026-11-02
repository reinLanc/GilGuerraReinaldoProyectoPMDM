using UnityEngine;

public class PinaVida : MonoBehaviour
{
    [SerializeField]
    int vidas = 1;
    [SerializeField]
    string estadoRecogida = "Recogida";
    [SerializeField]
    float duracionRecogida = 0.5f;

    Animator animator;
    Collider2D colision;
    bool recogida = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        colision = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        JugadorSalud salud = otro.GetComponent<JugadorSalud>();

        if (!recogida && salud != null && salud.GanarVida(vidas))
        {
            recogida = true;
            colision.enabled = false;
            animator.Play(estadoRecogida);
            Destroy(gameObject, duracionRecogida);
        }
    }
}