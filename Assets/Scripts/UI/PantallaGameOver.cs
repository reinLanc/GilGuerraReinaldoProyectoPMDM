using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaGameOver : MonoBehaviour
{
    [SerializeField]
    string menuInicio = "Nivel1";
    public void Reiniciar()
    {
        DatosPartida.NuevaPartida();
        SceneManager.LoadScene(menuInicio);
    }
}
