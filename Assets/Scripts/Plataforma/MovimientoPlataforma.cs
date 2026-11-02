using UnityEngine;

public class MovimientoPlataforma : MonoBehaviour
{
    [SerializeField]
    float velocidad = 2f;
    [SerializeField]
    float recorrido = 4f;
    [SerializeField]
    bool vertical = false;
    [SerializeField]
    float espera = 0f;
    [SerializeField]
    SpriteRenderer riel;

    Rigidbody2D rb2D;
    float posInicial;
    float posFinal;
    int direccion = 1;
    float finEspera = 0f;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        posInicial = PosicionEnEje();
        posFinal = posInicial + recorrido;

        ColocarRiel();
    }

    void FixedUpdate()
    {
        ActualizarDireccion();

        if (Time.time < finEspera)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        else
        {
            rb2D.linearVelocity = EjeMovimiento() * direccion * velocidad;
        }
    }

    float PosicionEnEje()
    {
        if (vertical)
        {
            return rb2D.position.y;
        }
        else
        {
            return rb2D.position.x;
        }
    }

    Vector2 EjeMovimiento()
    {
        if (vertical)
        {
            return Vector2.up;
        }
        else
        {
            return Vector2.right;
        }
    }

    void ActualizarDireccion()
    {
        float pos = PosicionEnEje();

        if (direccion == 1 && pos >= posFinal)
        {
            direccion = -1;
            finEspera = Time.time + espera;
        }
        else if (direccion == -1 && pos <= posInicial)
        {
            direccion = 1;
            finEspera = Time.time + espera;
        }
    }

    void ColocarRiel()
    {
        if (riel != null && riel.sprite != null)
        {
            Vector2 eslabon = riel.sprite.bounds.size;

            if (vertical)
            {
                riel.size = new Vector2(eslabon.x, recorrido);
                riel.transform.position = new Vector3(
                    transform.position.x,
                    posInicial + recorrido / 2f,
                    riel.transform.position.z
                );
            }
            else
            {
                riel.size = new Vector2(recorrido, eslabon.y);
                riel.transform.position = new Vector3(
                    posInicial + recorrido / 2f,
                    transform.position.y,
                    riel.transform.position.z
                );
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 inicio = transform.position;

        if (Application.isPlaying)
        {
            if (vertical)
            {
                inicio = new Vector3(transform.position.x, posInicial, 0f);
            }
            else
            {
                inicio = new Vector3(posInicial, transform.position.y, 0f);
            }
        }

        Vector3 eje = vertical ? Vector3.up : Vector3.right;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(inicio, inicio + eje * recorrido);
    }
}