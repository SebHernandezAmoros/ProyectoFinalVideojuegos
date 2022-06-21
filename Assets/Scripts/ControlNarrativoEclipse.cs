using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlNarrativoEclipse : MonoBehaviour
{
    public PlayerController Player = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.life)
        {
            Debug.Log("MUERTO...O TALVEZ NO");
            SceneManager.LoadScene(sceneName: "Capitule 1");
        }
        
    }
}
