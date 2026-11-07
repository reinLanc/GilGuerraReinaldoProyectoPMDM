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

    float finRetroceso = 0f;

    float umbralSubida = 0.1f;
    float entradaX = 0f;
    bool enSuelo = false;
    Vector2 velocidadSuelo = Vector2.zero;
    int numSaltos = 0;
    bool muerto = false;
    bool desapareciendo = false;
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
        Collider2D piso = BuscarSuelo();
        enSuelo = piso != null;

        if (enSuelo && piso.attachedRigidbody != null)
        {
            velocidadSuelo = piso.attachedRigidbody.linearVelocity;
        }
        else
        {
            velocidadSuelo = Vector2.zero;
        }
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

        if (enSuelo && !subiendo)
        {
            numSaltos = 0;
        }
        else if (!enSuelo && numSaltos == 0)
        {
            numSaltos = 1;
        }

        if (saltarAction.WasPressedThisFrame())
        {
            if (numSaltos == 0 || (numSaltos < maxSaltos && !subiendo))
            {
                rb2D.linearVelocityY = velocidadSalto;
                numSaltos++;
                GestorAudio.Salto();
            }
        }
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
        else if (VelocidadVertical() > umbralSubida)
        {
            if (numSaltos >= 2)
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

    Collider2D BuscarSuelo()
    {
        Vector2 tamano = new Vector2(boxCollider2d.bounds.size.x - 0.1f, boxCollider2d.bounds.size.y);
        Vector2 centro = (Vector2)boxCollider2d.bounds.center + Vector2.down * distanciaSuelo;

        return Physics2D.OverlapBox(centro, tamano, 0f, suelo);
    }

    public void Morir()
    {
        muerto = true;
        entradaX = 0f;
        rb2D.linearVelocity = Vector2.zero;
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

    public void Desaparecer()
    {
        desapareciendo = true;
        entradaX = 0f;
        rb2D.linearVelocity = Vector2.zero;
        rb2D.simulated = false;
    }
}