using UnityEngine;
using Firebase.Firestore;
using System.Collections.Generic;

public class Lesson : MonoBehaviour
{
    [FirestoreProperty]
    public string Description { get; set; }
    [FirestoreProperty]
    public int Index { get; set; }
    [FirestoreProperty]
    public string Name { get; set; }
    [FirestoreProperty]
    public List<string> Objects { get; set; }
    [FirestoreProperty]
    public int Stars { get; set; }
    [FirestoreProperty]
    public string TypeOfLesson { get; set; }
    [FirestoreProperty]
    public bool Locked { get; set; }
}
