using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaFinal : MonoBehaviour
{
    [SerializeField]
    string escenaInicio = "Nivel1";

    public void VolverAJugar()
    {
        DatosPartida.NuevaPartida();
        SceneManager.LoadScene(escenaInicio);

    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}


