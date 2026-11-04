using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    [SerializeField]
    Transform objetivo;

    [SerializeField]
    float desfaseY = 1.5f;

    [SerializeField]
    float zonaMuertaArriba = 2f;
    [SerializeField]
    float zonaMuertaAbajo = 0.5f;

    [SerializeField]
    bool seguirX = true;
    [SerializeField]
    float zonaMuertaX = 7f;

    [SerializeField]
    float suavizado = 0.15f;
    [SerializeField]
    float suavizadoBajada = 0.05f;

    [SerializeField]
    bool usarLimites = true;
    [SerializeField]
    float limiteIzquierdo = -10f;
    [SerializeField]
    float limiteDerecho = 16f;
    [SerializeField]
    float limiteInferior = -4f;
    [SerializeField]
    float limiteSuperior = 48f;

    Camera camara;

    void Start()
    {
        camara = GetComponent<Camera>();

        if (objetivo == null)
        {
            Debug.LogError("Falta asignar el objetivo (Jugador) en " + name);
            enabled = false;
        }
        else
        {
            Vector2 inicio = new Vector2(transform.position.x, objetivo.position.y + desfaseY);

            if (seguirX)
            {
                inicio.x = objetivo.position.x;
            }

            inicio = LimitarAlNivel(inicio);
            transform.position = new Vector3(inicio.x, inicio.y, transform.position.z);
        }
    }

    void LateUpdate()
    {
        Vector2 destino = CalcularDestino();
        destino = LimitarAlNivel(destino);

        float tiempoY = suavizado;
        if (destino.y < transform.position.y)
        {
            tiempoY = suavizadoBajada;
        }

        float x = Mathf.Lerp(transform.position.x, destino.x, Time.deltaTime / Mathf.Max(suavizado, 0.01f));
        float y = Mathf.Lerp(transform.position.y, destino.y, Time.deltaTime / Mathf.Max(tiempoY, 0.01f));

        transform.position = new Vector3(x, y, transform.position.z);
    }

    Vector2 CalcularDestino()
    {
        Vector2 destino = transform.position;

        float yObjetivo = objetivo.position.y + desfaseY;

        if (yObjetivo > destino.y + zonaMuertaArriba)
        {
            destino.y = yObjetivo - zonaMuertaArriba;
        }
        else if (yObjetivo < destino.y - zonaMuertaAbajo)
        {
            destino.y = yObjetivo + zonaMuertaAbajo;
        }

        if (seguirX)
        {
            float diferencia = objetivo.position.x - destino.x;

            if (diferencia > zonaMuertaX)
            {
                destino.x = objetivo.position.x - zonaMuertaX;
            }
            else if (diferencia < -zonaMuertaX)
            {
                destino.x = objetivo.position.x + zonaMuertaX;
            }
        }

        return destino;
    }

    Vector2 LimitarAlNivel(Vector2 posicion)
    {
        if (usarLimites)
        {
            float mitadAlto = camara.orthographicSize;
            float mitadAncho = mitadAlto * camara.aspect;

            posicion.x = Limitar(posicion.x, limiteIzquierdo + mitadAncho, limiteDerecho - mitadAncho);
            posicion.y = Limitar(posicion.y, limiteInferior + mitadAlto, limiteSuperior - mitadAlto);
        }

        return posicion;
    }

    float Limitar(float valor, float minimo, float maximo)
    {
        float resultado;

        if (minimo > maximo)
        {
            resultado = (minimo + maximo) / 2f;
        }
        else
        {
            resultado = Mathf.Clamp(valor, minimo, maximo);
        }

        return resultado;
    }

    void OnDrawGizmosSelected()
    {
        if (usarLimites)
        {
            Vector3 centro = new Vector3(
                (limiteIzquierdo + limiteDerecho) / 2f,
                (limiteInferior + limiteSuperior) / 2f,
                0f
            );
            Vector3 tamano = new Vector3(
                limiteDerecho - limiteIzquierdo,
                limiteSuperior - limiteInferior,
                0f
            );

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(centro, tamano);
        }
    }
}