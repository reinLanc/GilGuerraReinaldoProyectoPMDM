using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour
{
    [SerializeField]
    string siguienteEscena = "Nivel2";

    [SerializeField]
    string estadoActivada = "Activada";
    [SerializeField]
    float retraso = 1.2f;

    Animator animator;
    bool alcanzada = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (!animator.HasState(0, Animator.StringToHash(estadoActivada)))
        {
            Debug.LogError("El Animator de '" + name + "' no tiene ningun estado llamado '" + estadoActivada + "'");
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        JugadorController jugador = otro.GetComponent<JugadorController>();
        JugadorSalud salud = otro.GetComponent<JugadorSalud>();

        if (!alcanzada && jugador != null && !jugador.SinControl)
        {
            alcanzada = true;

            if (salud != null)
            {
                salud.GuardarVidasParaSiguienteNivel();
            }

            animator.Play(estadoActivada);
            jugador.Desaparecer();
            Invoke(nameof(CargarSiguiente), retraso);
        }
    }

    void CargarSiguiente()
    {
        SceneManager.LoadScene(siguienteEscena);
    }
}