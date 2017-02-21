using UnityEngine;

public class PlayerAim : MonoBehaviour {//el jugador en toria es un solo punto, es decir la camara no se movera cuadno el jugador lance su canica, por lo tanto debe ser separado
    public int m_PlayerNumber = 1;
    public float m_TraslateSpeed = 50f;
    public float m_RotateSpeed = 90f;
    public Transform m_CenterGameZone;//que dberia estar en la posicion 0, 0.5, 0
    public Transform m_SpawnPoint;

    private string m_TraslateAxisName;
    private string m_RotateAxisName;
    private float m_TraslateInputValue;
    private float m_RotateInputValue;

    private void Awake(){
        m_TraslateAxisName = "Traslate";//los dos jugadores usan las mismas teclas para interactuar, por lo tanto el nombre de los ejes deberia ser el mismo//esto tendre que modifiacar
        m_RotateAxisName = "Rotate";
    }

    private void OnEnable(){//quiza deba ser publica, por el intercambio de turnos de los jugadores, tendre que habilitar y deshabilitar la bolita del jugador, y constantemente resetear su posicio
        m_TraslateInputValue = 0f;
        m_RotateInputValue = 0f;
    }

    private void Update(){//aqui detecto si hay teclas presionadas
        m_TraslateInputValue = Input.GetAxis(m_TraslateAxisName);
        m_RotateInputValue = Input.GetAxis(m_RotateAxisName);// si pusiera esto solo dentro de fixed update la rotacion no tiene efecto fisico, creo, por lo tanto cuando presion para ortar nunca entrara en fixed update
    }

    private void FixedUpdate(){//podria ir dentro de update
        Traslate();
        Rotate();
    }

    private void Traslate(){
        transform.RotateAround(m_CenterGameZone.position, Vector3.up /*new Vector3 (0f, 1f, 0f)*/, m_TraslateSpeed * m_TraslateInputValue * Time.deltaTime);
    }

    private void Rotate(){
        transform.Rotate(Vector3.up * m_RotateInputValue * m_RotateSpeed * Time.deltaTime, Space.World);//debo limitar eso, para ello puedo establecer cierto limistas al inicio, y hsegurarme que este vector resultante, no se aslga de esos valores

    }
    
    public void Reset(){//poa ahora su unica llamada es en un comentario, pero seria util si usara a varios jugadores
        transform.position = m_SpawnPoint.position;
        transform.rotation = m_SpawnPoint.rotation;
    }
    //si modificara la angulo de disparo, este seria el script encargado de eso
}