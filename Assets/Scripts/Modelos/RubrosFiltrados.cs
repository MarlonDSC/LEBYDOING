using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class RubrosFiltrados 
{
    [JsonProperty("dataList")]
    public List<ListaRubros> DataList { get; set; }

    [JsonProperty("singleData")]
    public object SingleData { get; set; }

    [JsonProperty("succeded")]
    public bool Succeded { get; set; }

    [JsonProperty("entityId")]
    public int EntityId { get; set; }

    [JsonProperty("message")]
    public object Message { get; set; }

    [JsonProperty("errors")]
    public List<object> Errors { get; set; }
}
