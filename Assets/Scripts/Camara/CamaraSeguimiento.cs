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
    float velocidadX;
    float velocidadY;

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
            float x = transform.position.x;
            if (seguirX)
            {
                x = objetivo.position.x;
            }

            transform.position = new Vector3(
                LimitarX(x),
                LimitarY(objetivo.position.y + desfaseY),
                transform.position.z
            );
        }
    }

    void LateUpdate()
    {
        float xCamara = transform.position.x;
        float yCamara = transform.position.y;

        float xDestino = LimitarX(CalcularDestinoX(xCamara));
        float yDestino = LimitarY(CalcularDestinoY(yCamara));

        float tiempoY = suavizado;
        if (yDestino < yCamara)
        {
            tiempoY = suavizadoBajada;
        }

        float x = Mathf.SmoothDamp(xCamara, xDestino, ref velocidadX, suavizado);
        float y = Mathf.SmoothDamp(yCamara, yDestino, ref velocidadY, tiempoY);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    float CalcularDestinoY(float yCamara)
    {
        float yObjetivo = objetivo.position.y + desfaseY;
        float yDestino = yCamara;

        if (yObjetivo > yCamara + zonaMuertaArriba)
        {
            yDestino = yObjetivo - zonaMuertaArriba;
        }
        else if (yObjetivo < yCamara - zonaMuertaAbajo)
        {
            yDestino = yObjetivo + zonaMuertaAbajo;
        }

        return yDestino;
    }

    float CalcularDestinoX(float xCamara)
    {
        float xDestino = xCamara;

        if (seguirX)
        {
            float diferencia = objetivo.position.x - xCamara;

            if (diferencia > zonaMuertaX)
            {
                xDestino = objetivo.position.x - zonaMuertaX;
            }
            else if (diferencia < -zonaMuertaX)
            {
                xDestino = objetivo.position.x + zonaMuertaX;
            }
        }

        return xDestino;
    }

    float LimitarX(float x)
    {
        float resultado = x;

        if (usarLimites)
        {
            float mitadAncho = camara.orthographicSize * camara.aspect;
            resultado = Limitar(x, limiteIzquierdo + mitadAncho, limiteDerecho - mitadAncho);
        }

        return resultado;
    }

    float LimitarY(float y)
    {
        float resultado = y;

        if (usarLimites)
        {
            float mitadAlto = camara.orthographicSize;
            resultado = Limitar(y, limiteInferior + mitadAlto, limiteSuperior - mitadAlto);
        }

        return resultado;
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