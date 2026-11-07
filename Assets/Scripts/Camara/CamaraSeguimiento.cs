using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    [SerializeField]
    Transform objetivo;

    [SerializeField]
    float desfaseY = 1.5f;
    [SerializeField]
    float velocidadSeguimiento = 5f;


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
            transform.position = CalcularDestino();
        }
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            CalcularDestino(),
            velocidadSeguimiento * Time.deltaTime
        );
    }

    Vector3 CalcularDestino()
    {
        float mitadAlto = camara.orthographicSize;
        float mitadAncho = mitadAlto * camara.aspect;

        float x = Mathf.Clamp(objetivo.position.x, limiteIzquierdo + mitadAncho, limiteDerecho - mitadAncho);
        float y = Mathf.Clamp(objetivo.position.y + desfaseY, limiteInferior + mitadAlto, limiteSuperior - mitadAlto);

        return new Vector3(x, y, transform.position.z);
    }
}