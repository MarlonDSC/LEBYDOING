using Firebase.Firestore;
using System;
using System.Collections.Generic;
using UnityEngine;
[FirestoreData]
public class User
{
    [FirestoreProperty]
    public string Username { get; set; }
    [FirestoreProperty]
    public string Name { get; set; }
    [FirestoreProperty]
    public string LastName { get; set; }

    [FirestoreProperty]
    public int Hearts { get; set; }

    [FirestoreProperty]
    public int Coins { get; set; }

    [FirestoreProperty]
    public string Sex { get; set; }

    [FirestoreProperty]
    public string ProfilePicture { get; set; }
    [FirestoreProperty]
    public string PhoneNumber { get; set; }
    [FirestoreProperty]
    public string UID { get; set; }
    [FirestoreProperty]
    public string Email { get; set; }
    [FirestoreProperty]
    public DateTime CreatedAt { get; set; }
    [FirestoreProperty]
    public string Grade { get; set; }
    [FirestoreProperty]
    public Dictionary<string, object> Statistics { get; set; }
    // [FirestoreProperty]
    // public List<Statistics> Statistics { get; set; }
    // public List<object> Statistics { get; set; }
    // public Statistics Statistics {get; set;}

    public string GetString(string KeyName)
    {
        return PlayerPrefs.GetString(KeyName);
    }
}