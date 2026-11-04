using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField]
    string NivelInicio = "Nivel1";

    public void Jugar()
    {
        DatosPartida.NuevaPartida();
        SceneManager.LoadScene(NivelInicio);
    }
}
