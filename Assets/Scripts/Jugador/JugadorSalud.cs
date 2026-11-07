using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorSalud : MonoBehaviour
{
    [SerializeField]
    int vidasMaximas = 3;
    [SerializeField]
    int vidasTope = 10;
    [SerializeField]
    float alturaDano = 10.5f;
    [SerializeField]
    float alturaMortal = 30f;
    [SerializeField]
    float alturaPorVidaExtra = 7f;
    [SerializeField]
    int danoMaximoCaida = 2;

    [SerializeField]
    float tiempoInvulnerable = 1f;
    [SerializeField]
    float duracionMorir = 0.6f;
    [SerializeField]
    float retrasoReinicio = 1.3f;

    [SerializeField]
    string escenaGameOver = "GameOver";

    public int Vidas { get; private set; }

    JugadorController controlador;
    SpriteRenderer spriteRenderer;

    float alturaReferencia = 0f;
    bool estabaEnAire = false;
    float finInvulnerabilidad = 0f;

    void Awake()
    {
        if (DatosPartida.vidasInicioNivel < 0)
        {
            DatosPartida.vidasInicioNivel = vidasMaximas;
        }

        Vidas = Mathf.Clamp(DatosPartida.vidasInicioNivel, 1, vidasTope);
    }

    void Start()
    {
        controlador = GetComponent<JugadorController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        alturaReferencia = transform.position.y;
    }

    void Update()
    {
        ActualizarParpadeo();

        if (!controlador.SinControl)
        {
            ControlarCaida();
        }
    }

    void ControlarCaida()
    {
        float y = transform.position.y;

        if (controlador.EstaEnSuelo())
        {
            if (estabaEnAire)
            {
                EvaluarCaida(alturaReferencia - y);
                estabaEnAire = false;
            }

            alturaReferencia = y;
        }
        else
        {
            estabaEnAire = true;
            alturaReferencia = Mathf.Max(alturaReferencia, y);
        }
    }

    void EvaluarCaida(float altura)
    {
        if (altura >= alturaMortal)
        {
            Matar();
        }
        else if (altura >= alturaDano)
        {
            int dano = 1 + (int)((altura - alturaDano) / alturaPorVidaExtra);
            dano = Mathf.Min(dano, danoMaximoCaida);

            RecibirDano(dano);
        }
    }

    public bool GanarVida(int cantidad)
    {
        bool puedeGanar = !controlador.SinControl && Vidas < vidasTope;

        if (puedeGanar)
        {
            Vidas = Mathf.Min(vidasTope, Vidas + cantidad);
        }

        return puedeGanar;
    }

    public bool RecibirDano(int cantidad)
    {
        bool puedeRecibir = !controlador.SinControl && Time.time >= finInvulnerabilidad;

        if (puedeRecibir)
        {
            Vidas = Mathf.Max(0, Vidas - cantidad);

            if (Vidas == 0)
            {
                Matar();
            }
            else
            {
                finInvulnerabilidad = Time.time + tiempoInvulnerable;
                GestorAudio.Dano();
            }
        }
        return puedeRecibir;
    }

    public void GuardarVidasParaSiguienteNivel()
    {
        DatosPartida.vidasInicioNivel = Vidas;
    }

    public void Matar()
    {
        if (!controlador.SinControl)
        {
            Vidas = 0;

            controlador.Morir();
            GestorAudio.Morir();
            Invoke(nameof(Desvanecer), duracionMorir);
            Invoke(nameof(IrAGameOver), retrasoReinicio);
        }
    }

    void Desvanecer()
    {
        controlador.Desaparecer();
    }

    void IrAGameOver()
    {
        SceneManager.LoadScene(escenaGameOver);
    }

    void ActualizarParpadeo()
    {
        bool invulnerable = Time.time < finInvulnerabilidad && !controlador.SinControl;

        if (invulnerable)
        {
            spriteRenderer.enabled = (int)(Time.time * 10f) % 2 == 0;
        }
        else
        {
            spriteRenderer.enabled = true;
        }
    }
}