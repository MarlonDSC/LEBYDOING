using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ModeloPostItla 
{
	[JsonProperty("idSolicitante")]
	public int idSolicitante { get; set; }

	[JsonProperty("idVacante")]
	public int idVacante { get; set; }

	[JsonProperty("idEmpresa")]
	public int idEmpresa { get; set; }
}
