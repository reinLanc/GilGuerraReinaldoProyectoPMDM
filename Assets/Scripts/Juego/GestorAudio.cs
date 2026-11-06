using UnityEngine;

public class GestorAudio : MonoBehaviour
{
    static GestorAudio instancia;
    AudioSource fuenteMusica;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            fuenteMusica = GetComponent<AudioSource>();
            fuenteMusica.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
