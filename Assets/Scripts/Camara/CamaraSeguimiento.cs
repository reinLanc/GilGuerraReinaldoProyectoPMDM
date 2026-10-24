using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    [SerializeField]
    Transform objetivo;

    [SerializeField]
    float desfaseY = 2f;
    [SerializeField]
    float zonaMuertaY = 1.5f;
    [SerializeField]
    float suavizado = 0.15f;
    [SerializeField]
    bool seguirX = false;

    Vector3 velocidadCamara;

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

        if (yObjetivo > yCamara + zonaMuertaY)
        {
            yDestino = yObjetivo - zonaMuertaY;
        }
        else if (yObjetivo < yCamara - zonaMuertaY)
        {
            yDestino = yObjetivo + zonaMuertaY;
        }

        float xDestino = seguirX ? objetivo.position.x : transform.position.x;

        Vector3 destino = new Vector3(xDestino, yDestino, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, destino, ref velocidadCamara, suavizado);
    }
}