using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class SendData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("EatingInterest")
        .Child("Ndistance")
        .SetValueAsync("0.101");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}