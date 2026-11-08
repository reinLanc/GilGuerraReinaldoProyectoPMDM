using UnityEngine;
public class TrampaFuego : MonoBehaviour
{
    [SerializeField]
    float tiempoApagado = 2f;
    [SerializeField]
    float tiempoAviso = 0.4f;
    [SerializeField]
    float tiempoEncendido = 2f;
    [SerializeField]
    float retrasoInicial = 0f;
    [SerializeField]
    int dano = 1;

    [SerializeField]
    Collider2D basePisable;
    [SerializeField]
    float fuerzaRetrocesoX = 6f;
    [SerializeField]
    float fuerzaRetrocesoY = 7f;
    [SerializeField]
    float duracionRetroceso = 0.3f;

    [SerializeField]
    string estadoApagado = "Apagado";
    [SerializeField]
    string estadoAviso = "Encendiendo";
    [SerializeField]
    string estadoEncendido = "Encendido";

    enum Fase
    {
        Apagado,
        Aviso,
        Encendido
    }

    Animator animator;
    Collider2D zonaDano;
    Fase fase = Fase.Apagado;
    float proximoCambio = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        zonaDano = GetComponent<Collider2D>();

        zonaDano.enabled = false;
        animator.Play(estadoApagado);
        proximoCambio = Time.time + retrasoInicial + tiempoApagado;
    }

    void Update()
    {
        if (Time.time >= proximoCambio)
        {
            AvanzarFase();
        }
    }

    void AvanzarFase()
    {
        if (fase == Fase.Apagado)
        {
            fase = Fase.Aviso;
            animator.Play(estadoAviso);
            proximoCambio = Time.time + tiempoAviso;
        }
        else if (fase == Fase.Aviso)
        {
            fase = Fase.Encendido;
            animator.Play(estadoEncendido);
            ActivarBase(false);
            zonaDano.enabled = true;
            proximoCambio = Time.time + tiempoEncendido;
        }
        else
        {
            fase = Fase.Apagado;
            animator.Play(estadoApagado);
            zonaDano.enabled = false;
            ActivarBase(true);
            proximoCambio = Time.time + tiempoApagado;
        }
    }

    void ActivarBase(bool activo)
    {
        if(basePisable != null)
        {
            basePisable.enabled = activo;
        }
    }

    void OnTriggerStay2D(Collider2D otro)
    {
        JugadorSalud salud = otro.GetComponent<JugadorSalud>();
        JugadorController jugador = otro.GetComponent<JugadorController>();

        if (salud != null && salud.RecibirDano(dano) && jugador!= null)
        {
            Empujar(jugador);
        }
    }

    void Empujar(JugadorController jugador)
    {
        float lado = Mathf.Sign(jugador.transform.position.x - transform.position.x);
        Vector2 impulso = new Vector2(lado * fuerzaRetrocesoX,fuerzaRetrocesoY);

        jugador.Retroceder(impulso, duracionRetroceso);
    }
}