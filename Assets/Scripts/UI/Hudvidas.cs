using TMPro;
using UnityEngine;

public class HUDVidas : MonoBehaviour
{
    TMP_Text texto;
    JugadorSalud salud;

    void Start()
    {
        texto = GetComponent<TMP_Text>();
        salud = FindFirstObjectByType<JugadorSalud>();

        if (salud == null)
        {
            Debug.LogError("No hay ningun JugadorSalud en la escena");
            enabled = false;
        }
        else
        {
            salud.VidasCambiadas += Actualizar;
            Actualizar(salud.Vidas);
        }
    }

    void OnDestroy()
    {
        if (salud != null)
        {
            salud.VidasCambiadas -= Actualizar;
        }
    }

    void Actualizar(int vidas)
    {
        texto.text = "Vidas: " + vidas;
    }
}