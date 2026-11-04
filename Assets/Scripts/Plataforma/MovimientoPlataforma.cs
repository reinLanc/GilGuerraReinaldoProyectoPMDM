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
    }

    void FixedUpdate()
    {
        ActualizarDireccion();

        if (Time.time < finEspera)
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        else if (vertical)
        {
            rb2D.linearVelocity = new Vector2(0f, direccion * velocidad);
        }
        else
        {
            rb2D.linearVelocity = new Vector2(direccion * velocidad, 0f);
        }
    }

    float PosicionEnEje()
    {
        float posicion;

        if (vertical)
        {
            posicion = rb2D.position.y;
        }
        else
        {
            posicion = rb2D.position.x;
        }

        return posicion;
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
}