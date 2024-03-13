using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using static Firebase.Extensions.TaskExtension;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public class success : MonoBehaviour
{
    public class Record{
        public string date;
        public string time;
        public string result;
        public string cha;
        public Record(string time, string result, string cha){
            this.time = time;
            this.result = result;
            this.cha = cha;
        }
    }
    public string chacha_n;
    public string chacha_r;
    public TMP_Text txt;
    private int numnum;


    DatabaseReference reference;
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        takeEnable();
        txt.text = sharedCha.cha_n[sharedCha.r_num] + " 획득!";
        numnum = sharedCha.r_num + 1;
        chacha_r = "cha (" + numnum + ")";
        GameObject.Find(chacha_r).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(1);

    }

    // Update is called once per frame
    void Update()
    {
    }


    public void writeNewRecord(string i, string r_time, string r_result, string r_cha){
        Record record = new Record(r_time, r_result, r_cha);
        string json = JsonUtility.ToJson(record);
        reference.Child(i).Child(GetCurrentDate()).SetRawJsonValueAsync(json);
    }

    public static string GetCurrentDate(){
        return DateTime.Now.ToString(("yy-MM-dd HH:mm:ss tt"));
    }

    void takeEnable(){
        for(int i=1; i<= 3; i++){
            chacha_n = "cha (" + i + ")";
            GameObject.Find(chacha_n).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        }
    }

    /*private void readRecord(){
        reference.Child("EatingInterest").GetValueAsync().ContinueWith( task => {
            		if(task.IsCompleted){
                		DataSnapshot snapshot = task.Result;
                        Debug.Log(snapshot.ChildrenCount);

                        foreach(DataSnapshot data in snapshot.Children){
                            IDictionary personRecord = (IDictionary)data.Value;
                            Debug.Log("num: " + personRecord["num"] + ", date: " + personRecord["date"])
                        }
           		}
        	});	
    }*/
}
