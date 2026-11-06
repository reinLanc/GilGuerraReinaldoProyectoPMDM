using UnityEngine;

public class GestorAudio : MonoBehaviour
{
    static GestorAudio instancia;
    AudioSource fuenteMusica;

    [SerializeField]
    AudioSource fuenteEfectos;

    [SerializeField]
    AudioClip sonidoSalto;
    [SerializeField]
    AudioClip sonidoRecoger;
    [SerializeField]
    AudioClip sonidoDano;
    [SerializeField]
    AudioClip sonidoMorir;
    [SerializeField]
    AudioClip sonidoMeta;

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

      void Tocar(AudioClip sonido)
    {
        if (sonido != null)
        {
            fuenteEfectos.PlayOneShot(sonido);
        }
    }

     public static void Salto()
    {
        if (instancia != null)
        {
            instancia.Tocar(instancia.sonidoSalto);
        }
    }

    public static void Recoger()
    {
        if (instancia != null)
        {
            instancia.Tocar(instancia.sonidoRecoger);
        }
    }

    public static void Dano()
    {
        if (instancia != null)
        {
            instancia.Tocar(instancia.sonidoDano);
        }
    }

    public static void Morir()
    {
        if (instancia != null)
        {
            instancia.Tocar(instancia.sonidoMorir);
        }
    }

    public static void Meta()
    {
        if (instancia != null)
        {
            instancia.Tocar(instancia.sonidoMeta);
        }
    }

    public static void Reproducir(AudioClip sonido)
    {
        if (instancia != null)
        {
            instancia.Tocar(sonido);
        }
    }
}
