using UnityEngine;

public class Peligro : MonoBehaviour
{
    [SerializeField]
    bool muerteInstantanea = true;
    [SerializeField]
    int dano = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        JugadorSalud salud = collision.gameObject.GetComponent<JugadorSalud>();

        if (salud != null)
        {
            if (muerteInstantanea)
            {
                salud.Matar();
            }
            else
            {
                salud.RecibirDano(dano);
            }
        }
    }
}