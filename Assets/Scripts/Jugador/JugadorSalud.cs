using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class JugadorSalud : MonoBehaviour
{
    [SerializeField]
    int vidasMaximas = 3;
    [SerializeField]
    int vidasTope = 5;

    [SerializeField]
    float alturaDano = 7f;
    [SerializeField]
    float alturaPorVidaExtra = 7f;
    [SerializeField]
    int danoMaximoCaida = 2;
    [SerializeField]
    float alturaMortal = 30f;

    [SerializeField]
    float tiempoInvulnerable = 1f;
    [SerializeField]
    float duracionMorir = 0.6f;
    [SerializeField]
    float retrasoReinicio = 1.3f;

    public int Vidas { get; private set; }

    public event Action<int> VidasCambiadas;

    JugadorController controlador;
    SpriteRenderer spriteRenderer;

    float alturaReferencia = 0f;
    bool estabaEnAire = false;
    float finInvulnerabilidad = 0f;
    bool parpadeando = false;

    void Awake()
    {
        Vidas = vidasMaximas;
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
    }

    void FixedUpdate()
    {
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
            VidasCambiadas?.Invoke(Vidas);
        }

        return puedeGanar;
    }

    public void RecibirDano(int cantidad)
    {
        bool puedeRecibir = !controlador.SinControl && Time.time >= finInvulnerabilidad;

        if (puedeRecibir)
        {
            Vidas = Mathf.Max(0, Vidas - cantidad);
            VidasCambiadas?.Invoke(Vidas);

            if (Vidas == 0)
            {
                Matar();
            }
            else
            {
                finInvulnerabilidad = Time.time + tiempoInvulnerable;
            }
        }
    }

    public void Matar()
    {
        if (!controlador.SinControl)
        {
            Vidas = 0;
            VidasCambiadas?.Invoke(Vidas);

            controlador.Morir();
            Invoke(nameof(Desvanecer), duracionMorir);
            Invoke(nameof(ReiniciarNivel), retrasoReinicio);
        }
    }

    void Desvanecer()
    {
        controlador.Desaparecer();
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ActualizarParpadeo()
    {
        if (Time.time < finInvulnerabilidad)
        {
            bool visible = (int)(Time.time * 10f) % 2 == 0;

            if (visible)
            {
                spriteRenderer.color = Color.white;
            }
            else
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.35f);
            }

            parpadeando = true;
        }
        else if (parpadeando)
        {
            spriteRenderer.color = Color.white;
            parpadeando = false;
        }
    }
}