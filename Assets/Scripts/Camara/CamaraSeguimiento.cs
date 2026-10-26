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
    float suavizado = 0.15f;
    [SerializeField]
    float suavizadoBajada = 0.05f;
    [SerializeField]
    bool seguirX = false;

    float velocidadX;
    float velocidadY;

    void Start()
    {
        if (objetivo == null)
        {
            Debug.LogError("Falta asignar el objetivo (Jugador) en " + name);
            enabled = false;
        }
        else
        {
            transform.position = new Vector3(
                seguirX ? objetivo.position.x : transform.position.x,
                objetivo.position.y + desfaseY,
                transform.position.z
            );
        }
    }

    void LateUpdate()
    {
        float yObjetivo = objetivo.position.y + desfaseY;
        float yCamara = transform.position.y;
        float yDestino = yCamara;
        bool bajando = false;

        if (yObjetivo > yCamara + zonaMuertaArriba)
        {
            yDestino = yObjetivo - zonaMuertaArriba;
        }
        else if (yObjetivo < yCamara - zonaMuertaAbajo)
        {
            yDestino = yObjetivo + zonaMuertaAbajo;
            bajando = true;
        }

        float tiempo = bajando ? suavizadoBajada : suavizado;
        float y = Mathf.SmoothDamp(yCamara, yDestino, ref velocidadY, tiempo);

        float x = transform.position.x;
        if (seguirX)
        {
            x = Mathf.SmoothDamp(x, objetivo.position.x, ref velocidadX, suavizado);
        }

        transform.position = new Vector3(x, y, transform.position.z);
    }
}