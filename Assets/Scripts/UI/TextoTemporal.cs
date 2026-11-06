using UnityEngine;

[DisallowMultipleComponent]
public class TextoTemporal : MonoBehaviour
{
    [SerializeField]
    float duracionVisible = 4f;
    [SerializeField]
    float duracionFundido = 1f;

    CanvasGroup grupo;
    float inicioFundido = 0f;
    float finFundido = 0f;

    void Start()
    {
        grupo = GetComponent<CanvasGroup>();
        grupo.alpha = 1f;
        inicioFundido = Time.time + duracionVisible;
        finFundido = inicioFundido + duracionFundido;
    }

    void Update()
    {
        if (Time.time >= finFundido)
        {
            gameObject.SetActive(false);
        }
        else if (Time.time >= inicioFundido)
        {
            float progreso = Mathf.InverseLerp(inicioFundido, finFundido, Time.time);
            grupo.alpha = 1f - progreso;
        }
    }
}