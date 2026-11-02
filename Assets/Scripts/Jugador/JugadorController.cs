using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class JugadorController : MonoBehaviour
{
    InputAction movimientoAction;
    InputAction saltarAction;
    Rigidbody2D rb2D;
    BoxCollider2D boxCollider2d;
    JugadorAnimacion animacion;

    [SerializeField]
    LayerMask suelo;

    [SerializeField]
    float velocidad = 5f;
    [SerializeField]
    float velocidadSalto = 12f;
    [SerializeField]
    int maxSaltos = 2;


    [SerializeField]
    float distanciaSuelo = 0.15f;

    [SerializeField]
    float tiempoGracia = 0.1f;

    float umbralVertical = 0.1f;
    float entradaX = 0f;
    bool saltoPedido = false;
    bool enSuelo = false;
    float ultimoContactoSuelo = -1f;
    Vector2 velocidadSuelo = Vector2.zero;
    int saltosUsados = 0;
    bool muerto = false;
    bool desapareciendo = false;

    public bool SinControl
    {
        get { return muerto || desapareciendo; }
    }

    public bool EstaEnSuelo()
    {
        return enSuelo;
    }

    void Start()
    {
        movimientoAction = InputSystem.actions.FindAction("Moverse");
        saltarAction = InputSystem.actions.FindAction("Saltar");
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        animacion = GetComponent<JugadorAnimacion>();
    }

    void Update()
    {
        if (!SinControl)
        {
            LeerInput();
        }

        ActualizarAnimacion();
    }

    void FixedUpdate()
    {
        ComprobarSuelo();

        if (!SinControl)
        {
            Moverse();
            Saltar();
        }
    }

    void LeerInput()
    {
        entradaX = movimientoAction.ReadValue<Vector2>().x;

        if (saltarAction.WasPressedThisFrame())
        {
            saltoPedido = true;
        }
    }

    void ComprobarSuelo()
    {
        RaycastHit2D contacto = BuscarSuelo();
        bool tocando = contacto.collider != null;

        if (tocando)
        {
            ultimoContactoSuelo = Time.time;
        }

        enSuelo = Time.time - ultimoContactoSuelo <= tiempoGracia;

        if (tocando && contacto.rigidbody != null)
        {
            velocidadSuelo = contacto.rigidbody.linearVelocity;
        }
        else
        {
            velocidadSuelo = Vector2.zero;
        }
    }

    float VelocidadVerticalPropia()
    {
        return rb2D.linearVelocityY - velocidadSuelo.y;
    }

    void Moverse()
    {
        rb2D.linearVelocityX = entradaX * velocidad + velocidadSuelo.x;
    }

    void Saltar()
    {
        bool estaSubiendo = VelocidadVerticalPropia() > umbralVertical;

        if (enSuelo && !estaSubiendo)
        {
            saltosUsados = 0;
        }
        else if (!enSuelo && saltosUsados == 0)
        {
            saltosUsados = 1;
        }

        if (saltoPedido)
        {
            if (saltosUsados < maxSaltos)
            {
                rb2D.linearVelocityY = velocidadSalto;
                saltosUsados++;
            }

            saltoPedido = false;
        }
    }

    void ActualizarAnimacion()
    {
        animacion.CambiarEstado(CalcularEstado());

        if (entradaX != 0f)
        {
            animacion.MirarAIzquierda(entradaX < 0f);
        }
    }

    EstadoJugador CalcularEstado()
    {
        EstadoJugador estado;

        // El orden importa pq gana la primera condicion que se cumple
        if (desapareciendo)
        {
            estado = EstadoJugador.Desaparecer;
        }
        else if (muerto)
        {
            estado = EstadoJugador.Morir;
        }
        else if (VelocidadVerticalPropia() > umbralVertical)
        {
            if (saltosUsados >= 2)
            {
                estado = EstadoJugador.DobleSalto;
            }
            else
            {
                estado = EstadoJugador.Saltar;
            }
        }
        else if (!enSuelo)
        {
            estado = EstadoJugador.Caer;
        }
        else if (entradaX != 0f)
        {
            estado = EstadoJugador.Correr;
        }
        else
        {
            estado = EstadoJugador.Parado;
        }

        return estado;
    }

    RaycastHit2D BuscarSuelo()
    {
        Vector2 tamano = new Vector2(boxCollider2d.bounds.size.x - 0.1f, boxCollider2d.bounds.size.y);

        return Physics2D.BoxCast(
            boxCollider2d.bounds.center,
            tamano,
            0f,
            Vector2.down,
            distanciaSuelo,
            suelo
        );
    }

    public void Morir()
    {
        muerto = true;
        entradaX = 0f;
        saltoPedido = false;
        rb2D.linearVelocity = Vector2.zero;
    }

    public void Desaparecer()
    {
        desapareciendo = true;
        entradaX = 0f;
        saltoPedido = false;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.simulated = false;
    }
}