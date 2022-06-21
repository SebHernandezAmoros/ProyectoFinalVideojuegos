using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextManager : MonoBehaviour
{
    public Text Historia;
    public GameObject Inicio;
    public GameObject Fin;
    private Vector3 PosicionAux;

    public float Suavizado;
    // Start is called before the first frame update
    void Start()
    {
        Historia.transform.position = Inicio.transform.position;
        PosicionAux = new Vector3(Fin.transform.position.x, Fin.transform.position.y-30, Fin.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        Historia.transform.position= Vector3.Lerp(Historia.transform.position, Fin.transform.position, Suavizado * Time.deltaTime);
        if (Historia.transform.position.y >= PosicionAux.y)
        {
            SceneManager.LoadScene(sceneName: "Capitule 1 Game");
        }
    }
}
