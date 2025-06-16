using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScenas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EscenaInicio()
    {
        SceneManager.LoadScene("Inicio");
    }

    // Update is called once per frame
    public void EscenaPrueba()
    {
        SceneManager.LoadScene("Pruebas");
    }

    public void EscenaFinal()
    {
        SceneManager.LoadScene("FinDelJuego");
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }
}
