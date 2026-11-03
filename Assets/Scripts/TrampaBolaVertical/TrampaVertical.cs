using UnityEngine;

public class TrampaVertical : MonoBehaviour
{
    [SerializeField]
    float velocidad = 2f;
    [SerializeField]
    float alturaSubida = 3f;
    [SerializeField]
    float alturaBajada = 3f;

    float yMaxima;
    float yMinima;
    int direccion = 1;

    void Start()
    {
        yMaxima = transform.position.y + alturaSubida;
        yMinima = transform.position.y - alturaBajada;
    }

    void Update()
    {
        Mover();
        ComprobarLimites();
    }

    void Mover()
    {
        transform.position += new Vector3(0f, direccion * velocidad * Time.deltaTime, 0f);
    }

    void ComprobarLimites()
    {
        if (transform.position.y >= yMaxima)
        {
            direccion = -1;
        }
        else if (transform.position.y <= yMinima)
        {
            direccion = 1;
        }
    }
}