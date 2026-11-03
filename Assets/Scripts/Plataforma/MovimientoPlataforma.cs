using UnityEngine;


public class MovimientoPlataforma : MonoBehaviour
{
    /*Parametros de la plataforma*/
    [SerializeField]
    float velocidad = 2f;

    [SerializeField]
    float recorrido = 4f;
    [SerializeField]
    bool vertical = false;
    [SerializeField]
    float espera = 0f;

    Rigidbody2D rb2D;
    Vector2 eje;
    float posInicial;
    float posFinal;
    int direccion = 1;
    float finEspera = 0f;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        if (vertical)
        {
            eje = Vector2.up;
        }
        else
        {
            eje = Vector2.right;
        }

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
        else
        {
            rb2D.linearVelocity = eje * direccion * velocidad;
        }
    }

    float PosicionEnEje()
    {
        return Vector2.Dot(rb2D.position, eje);
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