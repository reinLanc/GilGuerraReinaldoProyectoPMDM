using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorEscenas : MonoBehaviour
{
    [SerializeField]
    string escenaInicio = "Nivel1";

    public void EmpezarPartida()
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
