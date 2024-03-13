using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using static Firebase.Extensions.TaskExtension;
using UnityEngine.SceneManagement;
using System;

public class Charact : IEquatable<Charact>{
    public string key {get; set;}
    public string val {get; set;}

    public bool Equals(Charact other){
        throw new NotImplementedException();
    }

}
public class collection : MonoBehaviour
{
    bool check = false;
    List<Charact> charact = new List<Charact>();
    private float fDestroyTime = 1f;
    private float fTickTime;
    void Start()
    {
        ReadRecord();
    }

    void Update()
    {
        fTickTime += Time.deltaTime;
        if ( fTickTime >= fDestroyTime && check == false)
        {
            int n = charact.Count;
            Debug.Log(n);
            for(int i = 0; i < n; i++){
                Debug.Log(charact[i].val);
                if(charact[i].val == 1.ToString()){
                    string cha_name = "black_tiniping (" + (i + 1) + ")";
                    Debug.Log(cha_name);
                    GameObject.Find(cha_name).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0); 
                    GameObject.Find(cha_name).SetActive(false);
                }      
            }  
            check = true;
        }

    }

    public void ReadRecord() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	    reference.Child("Collection").GetValueAsync().ContinueWith( task => {
            		if(task.IsCompleted){
                		DataSnapshot snapshot = task.Result;
                        foreach(DataSnapshot data in snapshot.Children){
                            string a = data.Key.ToString();
                            string b = data.Value.ToString();
                            charact.Add(new Charact() {key = a, val = b});
                        }
                        
           		    }   
        });	

    }
}

/*

                                */