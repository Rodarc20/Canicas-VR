using UnityEngine;
using UnityEngine.UI;//aqui hare modificaciones al ui, por ejemplo la barra de fuerza o marcador de llegada

public class PlayerThrow : MonoBehaviour {
    public int m_PlayerNumber = 1;
    public Slider m_Fuerza;
    public float m_MinForce = 10f;//estos valores los deberia recogerdel game manager
    public float m_MaxForce = 80f;//estos valores se establecen en el prefab, quiza no deberiaser asi, 
    public float m_MaxChargeTime = 2f;//un segudno en cargar toda la barra de fuerza
    public GameObject m_CanicaPlayerPrefab;
    public GameObject m_CanicaPlayer;//instancia de una canica de jugador
    private CanicaPlayer m_ScriptCP;

    private string m_ThrowButton;
    private float m_CurrentThrowForce;
    private float m_ChargeSpeed;
    [HideInInspector] public bool m_Throwed;

    void Start(){
        m_ThrowButton = "Fire1";
        m_ChargeSpeed = (m_MaxForce - m_MinForce) / m_MaxChargeTime;
        m_Throwed = false;
        //Setup()//por ahora lo llamo dentro de spawnPlayer en el gamemanager
    }
    private void OnEnable(){
        m_CurrentThrowForce = m_MinForce;
    }

    public void Setup(){
        m_Throwed = false;
        m_CurrentThrowForce = m_MinForce;
        m_CanicaPlayer = Instantiate(m_CanicaPlayerPrefab, transform.position, transform.rotation) as GameObject;
        m_Throwed = false;
        if(m_CanicaPlayer){
            m_ScriptCP = m_CanicaPlayer.GetComponent<CanicaPlayer>();
            m_ScriptCP.m_PlayerNumber = m_PlayerNumber;
            m_ScriptCP.m_Player = transform; //m_CanicaPlayer.GetComponent<CanicaPlayer>().m_Player = transform;
            m_ScriptCP.m_PlayerThrow = GetComponent<PlayerThrow>();
        }
        //no debo instanciar canicas cuando quiera, debo instanciar algunas especificas, o ejor dicho tener todas instanciadas en un array de canicas, pero estas canicas siempre deben conservar ss propiedases, es decir de quien era y el color por jemeplo
        //las canicas de los jugadores no se destruyen, solo pasan a tener un nuevo propietrio, tener cuidado con esto, y como haer para que conservern o cambien los propietarios
        //el problema es ageragr y eliminar de un array, para eso tengo que tener una funcion, copiar pegar y eliminar
    }
    private void Update(){
        //si me paso del maximo de la barra no debo lanzar la canica, por que puede que el jugador aun quiera modificar la direccion, por ello podra aun moverse, solo se disparara cuando el jugador suelte la tecla de deisparo
        //analizar estas logicas,
        if(m_CurrentThrowForce >= m_MaxForce && !m_Throwed){//si la fuerza esa mayor que el maximo, y aun no he disparado, entonces solo establesco el current en el max
            m_CurrentThrowForce = m_MaxForce;//se dispara solo cuando el jugador suslete la tecla
            m_Fuerza.value = m_CurrentThrowForce;//hay problemas con este if,buscar solucion
        }
        else if(Input.GetButtonDown(m_ThrowButton)){//cuando presioo por primera vez el boton,, su bi com pu
            //m_Throwed = false;//si apreto espacin denuevo despues de lanzar esta varialbe cambiara a falso, evitarlo
            //m_CurrentThrowForce = m_MinForce;
            m_Fuerza.value = m_CurrentThrowForce;
        }
        else if(Input.GetButton(m_ThrowButton) && !m_Throwed){//cuando mantendo presionado el boton pero aun no he disparado
            m_CurrentThrowForce += m_ChargeSpeed * Time.deltaTime;
            m_Fuerza.value = m_CurrentThrowForce;
            //aqui tambien van modificaciones la slider de la fuerza de lanzamiento
        }
        if(Input.GetButtonUp(m_ThrowButton) && !m_Throwed){//cuadno suelto el boton y aun no he disparado, eliminado el elseif
            //m_Throwed = true;
            Fire();
        }
        //hay un erro que impide que dispare, por ejemplo si mntego presionado espacio antes de que me habiliten el disparo
    }
    private void Fire(){
        m_ScriptCP.Fire(transform.forward * m_CurrentThrowForce);//ninguna de las dos funciona, el proble es que en cada update lo regresa a la posicion del jugador, cuando no este disparando
        m_Throwed = true;//esto funionando pero quiza esto deberia ir antes, dejarolo asi por ahora
    }//una ve z que se ha disparado, debo deshabilitar los controles, la pelota sigue por su cuenta
}
//este script debe tener la capacidad de almacenar todas las canicas que el jugador posee, o selecionarla del playermanager, si el playermanagger sea el que las posea,
//este script tambien debe ser capaz de tener un pequeño menu para que el ususario pueda escoger la canica a usar , o tambien en el player manager, por el hecho de que quiza tenga que estar en una funcion como pdate, es preferible que este en un script que hereda de monobehavior


// un objeto instaccinde sea hijo de otro, solo debo haccer objeto.transform.parent = gameobject.transfor, el segundo sera el padre del de la izquierda