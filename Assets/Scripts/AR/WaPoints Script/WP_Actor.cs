using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WP_Actor : MonoBehaviour
{

    private float twoPi = Mathf.PI * 2f;
    [SerializeField] private float amplitude = 2.0f;
    [SerializeField] private float periodInSec = 120;
    private float frequency = 2.0f;
    [SerializeField] private float phase = 0.5f;
    public Transform cajaASeguir;
	public GameObject efectoAgua;
	public float altura;
	public float adelantarBallena;
	public float contadorBallena;

    [Header("Tiempo para ejecutar transicion")]
    [SerializeField] private float tiempoTranscurrido;
    [SerializeField] private float tiempoReset;

	public Animator anim;
    
	

    void Start()
    {
        frequency = 1 / periodInSec;
    }
    void Update()
    {
	    float x = amplitude * Mathf.Cos(twoPi * Time.time * frequency + phase);
	    float z = amplitude * Mathf.Sin(twoPi * Time.time * frequency + phase);
	    transform.localPosition = new Vector3(x, altura, z);
	    

        transform.LookAt(new Vector3(cajaASeguir.position.x, cajaASeguir.position.y, cajaASeguir.position.z));

        tiempoTranscurrido -= Time.deltaTime;
        if (tiempoTranscurrido < 0)
        {
            anim.SetTrigger("ActivarSalpicada");
	        StartCoroutine(IniciarEfectoDeAgua());
	        StartCoroutine(AdelantarBallena());
            tiempoTranscurrido = tiempoReset;
	        Debug.LogWarning("Detecta animacion");
        }
		
	    
		
    }

    IEnumerator IniciarEfectoDeAgua()
    {

	    yield return new WaitForSeconds(2.10f);
	    efectoAgua.SetActive(true);
        if (efectoAgua.activeSelf)
        {
	        yield return new WaitForSeconds(0.5f);
            efectoAgua.SetActive(false);
        }
	    
    }
    
	IEnumerator AdelantarBallena()
	{
		yield return new WaitForSeconds(contadorBallena);
		phase += adelantarBallena;
		
	}
    

}