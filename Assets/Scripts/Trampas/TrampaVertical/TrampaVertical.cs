using UnityEngine;

public class TrampaVertical : MonoBehaviour
{
    [SerializeField]
    float velocidad = 2f;
    [SerializeField]
    float velocidadBajada = 0f;
    [SerializeField]
    float alturaSubida = 3f;
    [SerializeField]
    float alturaBajada = 3f;
    [SerializeField]
    float esperaArriba = 0f;
    [SerializeField]
    float esperaAbajo = 0f;

    float yMaxima;
    float yMinima;
    int direccion = 1;
    float finEspera = 0f;

    void Start()
    {
        yMaxima = transform.position.y + alturaSubida;
        yMinima = transform.position.y - alturaBajada;
    }

    void Update()
    {
        if (Time.time >= finEspera)
        {
            Mover();
            ComprobarLimites();
        }

    }

    void Mover()
    {
        float v = velocidad;
        if (direccion == -1 && velocidadBajada > 0f)
        {
            v = velocidadBajada;
        }
        float y = transform.position.y + direccion * v * Time.deltaTime;
        y = Mathf.Clamp(y, yMinima, yMaxima);
        //transform.position += new Vector3(0f, direccion * velocidad * Time.deltaTime, 0f);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    void ComprobarLimites()
    {
        if (direccion == 1 && transform.position.y >= yMaxima)
        {
            direccion = -1;
            finEspera = Time.time + esperaArriba;
        }
        else if (direccion == -1 && transform.position.y <= yMinima)
        {
            direccion = 1;
            finEspera = Time.time + esperaAbajo;
        }
    }

    public bool Cayendo
    {
        get { return direccion == -1; }
    }

    public bool EnReposoAbajo
    {
        get { return direccion == -1 && transform.position.y <= yMinima + 0.01f; }
    }
}