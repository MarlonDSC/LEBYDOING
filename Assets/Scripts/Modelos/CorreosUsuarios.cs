using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
public class CorreosUsuarios 
{
    [JsonProperty("secuencia")]
    public int Secuencia { get; set; }

    [JsonProperty("correoElectronico")]
    public string CorreoElectronico { get; set; }

    [JsonProperty("tipoSolicitante")]
    public string TipoSolicitante { get; set; }
}
