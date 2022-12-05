using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    [FirestoreProperty]
    public int Gifts { get; set; }
    [FirestoreProperty]
    public int Losses { get; set; }
    [FirestoreProperty]
    public int Rank { get; set; }
    [FirestoreProperty]
    public int Wins { get; set; }
}
