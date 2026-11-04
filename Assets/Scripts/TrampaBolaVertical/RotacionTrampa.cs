using UnityEngine;

public class RotacionTrampa : MonoBehaviour
{
    [SerializeField]
    float velocidadGiro = 90f;

    void Update()
    {
        transform.Rotate(0f, 0f, velocidadGiro * Time.deltaTime);
    }
}