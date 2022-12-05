using UnityEngine;
using Firebase.Firestore;

public class Module : MonoBehaviour
{
    [FirestoreProperty]
    public string Description { get; set; }
    [FirestoreProperty]
    public string Image { get; set; }
    [FirestoreProperty]
    public string Name { get; set; }
}
