using TMPro;
using UnityEngine;

public class HUDVidas : MonoBehaviour
{
    TMP_Text texto;
    JugadorSalud salud;
    int vidasMostradas = -1;

    void Start()
    {
        texto = GetComponent<TMP_Text>();

        salud = FindFirstObjectByType<JugadorSalud>();

        if (salud == null)
        {
            Debug.LogError("No hay ningun JugadorSalud en la escena");
        }
    }

    void Update()
    {
        if (salud != null && salud.Vidas != vidasMostradas)
        {
            vidasMostradas = salud.Vidas;
            texto.text = "Vidas: " + vidasMostradas;
        }
    }
}