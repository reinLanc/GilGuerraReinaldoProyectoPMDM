using UnityEngine;

public class Peligro : MonoBehaviour
{
    [SerializeField]
    bool muerteInstantanea = true;
    [SerializeField]
    int dano = 1;

    /*Retroceso*/
    [SerializeField]
    bool retroceso = true;
    [SerializeField]
    float fuerzaRetrocesoX = 6f;
    [SerializeField]
    float fuerzaRetrocesoY = 7f;
    [SerializeField]
    float duracionRetroceso = 0.3f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        JugadorSalud salud = collision.gameObject.GetComponent<JugadorSalud>();
        JugadorController jugador = collision.gameObject.GetComponent<JugadorController>();

        if (salud != null)
        {
            if (muerteInstantanea)
            {
                salud.Matar();
            }
            else
            {
                salud.RecibirDano(dano);
                if (retroceso && jugador != null)
                {
                    Empujar(jugador);
                }
            }
        }
    }

    void Empujar(JugadorController jugador)
    {
        float lado = Mathf.Sign(jugador.transform.position.x - transform.position.x);
        Vector2 impulso = new Vector2(lado * fuerzaRetrocesoX, fuerzaRetrocesoY);

        jugador.Retroceder(impulso, duracionRetroceso);
    }
}