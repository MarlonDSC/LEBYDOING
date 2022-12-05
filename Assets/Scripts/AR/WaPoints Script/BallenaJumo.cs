using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallenaJumo : MonoBehaviour
{

    private float twoPi = Mathf.PI * 2f;
    [SerializeField] private float amplitude = 2.0f;
    [SerializeField] private float periodInSec = 120;
    private float frequency = 2.0f;
    [SerializeField] private float phase = 0.5f;
	public float altura;
	public float adelantarBallena;
	public float contadorBallena;
	
	[Header("Tiempo para ejecutar transicion")]
	[SerializeField] private float tiempoTranscurrido;
	[SerializeField] private float tiempoReset;
	
    void Start()
    {
        frequency = 1 / periodInSec;
    }
    void Update()
    {
        float x = amplitude * Mathf.Cos(twoPi * Time.time * frequency + phase);
        float z = amplitude * Mathf.Sin(twoPi * Time.time * frequency + phase);
	    transform.localPosition = new Vector3(x, altura , z);
	    
	    tiempoTranscurrido -= Time.deltaTime;
	    if (tiempoTranscurrido < 0)
	    {
		    StartCoroutine(AdelantarBallena());
		    tiempoTranscurrido = tiempoReset;
		    
	    }
    }
    
	IEnumerator AdelantarBallena()
	{
		yield return new WaitForSeconds(contadorBallena);
		phase += adelantarBallena;
	    
	}


}
