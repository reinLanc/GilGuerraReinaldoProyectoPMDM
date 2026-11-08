using UnityEngine;
using UnityEngine.InputSystem;

public class JugadorController : MonoBehaviour
{
    InputAction movimientoAction;
    InputAction saltarAction;
    Rigidbody2D rb2D;
    BoxCollider2D boxCollider2d;
    Animator animator;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    LayerMask suelo;

    [SerializeField]
    float velocidad = 5f;
    [SerializeField]
    float velocidadSalto = 12f;
    [SerializeField]
    int maxSaltos = 2;

    /*Deteccion de suelo*/
    [SerializeField]
    float distanciaSuelo = 0.15f;

    float umbralSubida = 0.1f;
    float finRetroceso = 0f;
    float entradaX = 0f;
    int numSaltos = 0;
    bool enSuelo = false;
    bool muerto = false;
    bool desapareciendo = false;
    Vector2 velocidadSuelo = Vector2.zero;
    EstadoJugador estadoActual = EstadoJugador.Parado;

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
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        animator.Play(estadoActual.ToString());
    }

    void Update()
    {
        ComprobarSuelo();

        if (!SinControl && Time.time >= finRetroceso)
        {
            Moverse();
            Saltar();
        }

        ActualizarAnimacion();
    }

    void ComprobarSuelo()
    {
        enSuelo = EstaTocandoSuelo();
        velocidadSuelo = Vector2.zero;

        if (enSuelo)
        {
            Rigidbody2D plataforma = LanzarCaja().rigidbody;

            if (plataforma != null)
            {
                velocidadSuelo = plataforma.linearVelocity;
            }
        }
    }

    bool EstaTocandoSuelo()
    {
        return LanzarCaja();
    }

    RaycastHit2D LanzarCaja()
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

    float VelocidadVertical()
    {
        return rb2D.linearVelocityY - velocidadSuelo.y;
    }

    void Moverse()
    {
        entradaX = movimientoAction.ReadValue<Vector2>().x;
        rb2D.linearVelocityX = entradaX * velocidad + velocidadSuelo.x;
    }

    void Saltar()
    {
        bool subiendo = VelocidadVertical() > umbralSubida;

        if (EstaTocandoSuelo() && !subiendo)
        {
            numSaltos = 0;
        }
        else if (!EstaTocandoSuelo() && numSaltos == 0)
        {
            numSaltos = 1;  
        }

        if (saltarAction.WasPressedThisFrame())
        {
            if (EstaTocandoSuelo() && !subiendo)
            {
                Impulsar();
            }
            else if (!EstaTocandoSuelo() && !subiendo && numSaltos < maxSaltos)
            {
                Impulsar();
            }
        }
    }

    void Impulsar()
    {
        rb2D.linearVelocityY = velocidadSalto;
        numSaltos++;
        GestorAudio.Salto();
    }

    void ActualizarAnimacion()
    {
        EstadoJugador nuevoEstado = CalcularEstado();

        if (nuevoEstado != estadoActual)
        {
            estadoActual = nuevoEstado;
            animator.Play(estadoActual.ToString());
        }

        if (entradaX != 0f)
        {
            spriteRenderer.flipX = entradaX < 0f;
        }
    }

    EstadoJugador CalcularEstado()
    {
        EstadoJugador estado = EstadoJugador.Parado;

        // El orden importa pq gana la primera condicion que se cumple
        if (desapareciendo)
        {
            estado = EstadoJugador.Desaparecer;
        }
        else if (muerto)
        {
            estado = EstadoJugador.Morir;
        }
        else if (VelocidadVertical() > umbralSubida)
        {
            estado = EstadoJugador.Saltar;
            if (numSaltos >= 2) estado = EstadoJugador.DobleSalto;
        }
        else if (!enSuelo)
        {
            estado = EstadoJugador.Caer;
        }
        else if (entradaX != 0f)
        {
            estado = EstadoJugador.Correr;
        }

        return estado;
    }

    void Detener()
    {
        entradaX = 0f;
        rb2D.linearVelocity = Vector2.zero;
    }

    public void Morir()
    {
        muerto = true;
        Detener();
    }

    public void Desaparecer()
    {
        desapareciendo = true;
        Detener();
        rb2D.simulated = false;
    }

    public void Retroceder(Vector2 impulso, float duracion)
    {
        if (!SinControl)
        {
            entradaX = 0f;
            finRetroceso = Time.time + duracion;
            rb2D.linearVelocity = impulso;
        }
    }
}