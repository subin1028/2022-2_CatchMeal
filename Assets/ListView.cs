using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity;
using UnityEngine.SceneManagement;

public static class sharedbool{
    public static bool load;
}
public class Part : IEquatable<Part>
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Result { get; set; }
        public string Cha { get; set; }

        public override string ToString()
        {
            return "ID: " + Time + "   Name: " + Cha;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Part objAsPart = obj as Part;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }

    public bool Equals(Part other)
    {
        throw new NotImplementedException();
    }
    // Should also override == and != operators.
}
    
public class ListView : MonoBehaviour
{
    List<Part> parts = new List<Part>();

    // Start is called before the first frame update
      private float fDestroyTime = 2f;
      private float fTickTime;
      public GameObject buttonTemplate;
        public GameObject g;
        public bool delay;
        public string date;
        public int temp;
        public int count;


    void Start()
    {
        buttonTemplate = transform.GetChild (1).gameObject;
        ReadRecord();
        delay = false;
        sharedbool.load = true;
        
    }

    // Update is called once per frame
    void Update()
    {

       if (sharedbool.load == true)
       {
        ReadRecord();
        int n = parts.Count;
        Debug.Log(n);
        for(int i = 0; i < n; i++){
            g = Instantiate(buttonTemplate, transform);
            g.transform.GetChild (0).GetComponent<TMP_Text>().text = parts[i].Date;
            g.transform.GetChild (1).GetComponent<TMP_Text>().text = parts[i].Time;
            g.transform.GetChild (2).GetComponent<TMP_Text>().text = parts[i].Result;
            g.transform.GetChild (3).GetComponent<TMP_Text>().text = parts[i].Cha;
        }  
        delay = true;
        sharedbool.load = false;
       }
        
    }

    public void ReadRecord() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	    reference.Child("personal Record").GetValueAsync().ContinueWith( task => {
            		if(task.IsCompleted){
                		DataSnapshot snapshot = task.Result;
                        Debug.Log(snapshot.ChildrenCount);
                        count = (int)snapshot.ChildrenCount;

                        foreach(DataSnapshot data in snapshot.Children){
                            date = data.Key.ToString();
                            Debug.Log(date);
                            IDictionary persRecord = (IDictionary)data.Value;
                            parts.Add(new Part() {Date = date, Time = persRecord["time"].ToString(), Result = persRecord["result"].ToString(), Cha = persRecord["cha"].ToString()});
                        }
           		}
        	});	
    }

}
