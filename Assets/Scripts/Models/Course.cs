using UnityEngine;
using Firebase.Firestore;

public class Course : MonoBehaviour
{
    [FirestoreProperty]
    public string Description { get; set; }
    [FirestoreProperty]
    public string Image { get; set; }
    [FirestoreProperty]
    public string Name { get; set; }
    [FirestoreProperty]
    public bool Locked { get; set; }
}
