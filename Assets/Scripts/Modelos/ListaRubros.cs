using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class ListaRubros 
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("categoria")]
    public string Categoria { get; set; }
}
