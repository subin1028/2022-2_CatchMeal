using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using static Firebase.Extensions.TaskExtension;
using UnityEngine.SceneManagement;
using System;

public static class sharedCha{
    public static string[] cha_n = new string[] {"하츄핑", "차차핑", "바로핑"};
    public static int r_num;

}

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARCoreFaceRegionManager_R : MonoBehaviour
{ 
    public class Record{
        public string time;
        public string result;
        public string cha;
        public string num_c;
        public Record(string time, string result, string cha, string num_c){
            this.time = time;
            this.result = result;
            this.cha = cha;
            this.num_c = num_c;
        }
    }

    DatabaseReference reference;
    System.Random random = new System.Random(); 
    //public static string[] cha_n = new string[] {"하츄핑", "차차핑", "바로핑"};
    //AR face manager info
    private ARFaceManager mArFaceManager;
    //코 , 좌/우 머리의 좌표값을 AR상에 매핑하기 위한 sessionorigin을 통해 좌표값 get
    private ARSessionOrigin mSessionOrigin;
    public Button posebutton;

    //코를 기준으로 생성할 프리팹 
    public GameObject mCenterPrefab;    
    //생선한 코 오브젝트 
    private GameObject mCenterObject;

    //GetRegionPoses시 output
    private NativeArray<ARCoreFaceRegionData> mFaceRegions;

    public GameObject mUpperLibPrefab;
    public GameObject mLowerLibPrefab;
    private GameObject mUpperLibObject;
    private GameObject mLowerLibObject;
    private XRFaceMesh mFaceMesh;
    public GameObject pre1;
    public GameObject pre2;
    public GameObject pre3;
    public GameObject pre4;
    private bool openJaw;
    private bool check;
    private int count;
    private int spoon;
    public int compact_num;
    public string compact_name;
    public string deco_name;

    private int nAlphaCnt;
    private float[] fArrAlphaRange;
    private bool first_check;

    public float total_timer; // 전체 시간 측정
    public float eat_timer; // 인식 경과 시점
    public float happy_time; // happy 적용 시간 체크
    public bool gamePlay;
    int min;
    int sec;
    int hour;
    string db_time;
    string g_result;
    string take_time;

    // Start is called before the first frame update
    void Start()
    {
        sharedCha.r_num = random.Next(0,3);
        spoon = 0;
        compact_num = 1;
        mArFaceManager = GetComponent<ARFaceManager>();
        //ARSessionOrigin init.
        mSessionOrigin = GetComponent<ARSessionOrigin>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        apply_crawn(SharedCrawn.sharedcrawn);
        GameObject.Find("poseButton").GetComponentInChildren<Text>().fontSize = 30;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;

        fArrAlphaRange = new float[] {0, 0.05f, 0.1f, 0.15f, 0.2f,0.25f, 0.3f,0.35f, 0.4f,0.45f, 0.5f,0.55f, 0.6f,0.65f, 0.7f,0.75f, 0.8f,0.85f, 0.9f, 0.95f, 1f};
        nAlphaCnt = 0;
        //GameObject.Find("compact (1)").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(fArrAlphaRange[nAlphaCnt]);
        openJaw = false;
        count = 0;
        check = false;
        first_check = false;
        gamePlay = false;
        total_timer = 0;
        min = 0;
        hour = 0;
        sec = 0;
        db_time = "";
        takeEnable();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        /*FirebaseDatabase.DefaultInstance
                    .GetReference("EatingInterest")
                    .Child("totalTime")
                    .SetValueAsync(db_time);*/


        /*DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	    reference.Child("EatingInterest").GetValueAsync().ContinueWith( task => {
            		if(task.IsCompleted){
                		DataSnapshot snapshot = task.Result;
                        standard = snapshot.Child("Ndistance").Value.ToString();
           		}
        	});	*/
    }

    // Update is called once per frame
    void Update()
    {
        //Android target임으로 arcore api만 사용하기 위함
        ARCoreFaceSubsystem coreSub = (ARCoreFaceSubsystem)mArFaceManager.subsystem;

        foreach (var face in mArFaceManager.trackables)        
        {

            //Camera로부터 인식한 face의 id를 가지고 mFaceRegions에 할당
            coreSub.GetRegionPoses(face.trackableId, Allocator.Persistent, ref mFaceRegions);

            if (!mCenterObject)
            {
                mCenterObject = Instantiate(mCenterPrefab, mSessionOrigin.trackablesParent);
            }


            Pose customPos = new Pose();
            customPos.rotation = mFaceRegions[0].pose.rotation;

            customPos.position.x = mFaceRegions[0].pose.position.x;
            customPos.position.z = mFaceRegions[0].pose.position.z;
            //customPos.position.y = mFaceRegions[0].pose.position.y;

             customPos.position.y = (mFaceRegions[0].pose.position.y + mFaceRegions[1].pose.position.y + 
                mFaceRegions[2].pose.position.y + mFaceRegions[3].pose.position.y 
                + mFaceRegions[4].pose.position.y + mFaceRegions[5].pose.position.y) / 2;
            
            mCenterObject.transform.rotation = customPos.rotation;
            mCenterObject.transform.position = customPos.position;




        //GameObject.Find("poseButton").GetComponentInChildren<Text>().text = str;



            //face.vertices
            coreSub.GetFaceMesh(face.trackableId, Allocator.Persistent, ref mFaceMesh);
            if (!mUpperLibObject)
            {
                mUpperLibObject = Instantiate(mUpperLibPrefab, mSessionOrigin.trackablesParent);
            }

            if (!mLowerLibObject)
            {
                mLowerLibObject = Instantiate(mLowerLibPrefab, mSessionOrigin.trackablesParent);
            }

            //makeLogXYZ(mFaceMesh.vertices[0].x.ToString(), mFaceMesh.vertices[0].y.ToString(), mFaceMesh.vertices[0].z.ToString());
            
            Pose upperPos = new Pose();
            upperPos.rotation = mFaceRegions[0].pose.rotation;
            upperPos.position.x = mFaceMesh.vertices[25].x; // 0
            upperPos.position.y = mFaceMesh.vertices[25].y;
            upperPos.position.z = mFaceMesh.vertices[25].z;
            mUpperLibObject.transform.rotation = upperPos.rotation;
            mUpperLibObject.transform.position = upperPos.position;

            Pose lowerPos = new Pose();
            lowerPos.rotation = mFaceRegions[0].pose.rotation;
            lowerPos.position.x = mFaceMesh.vertices[200].x; // 14
            lowerPos.position.y = mFaceMesh.vertices[200].y;
            lowerPos.position.z = mFaceMesh.vertices[200].z;
            mLowerLibObject.transform.rotation = lowerPos.rotation;
            mLowerLibObject.transform.position = lowerPos.position;

            float distance = Vector3.Distance(upperPos.position, lowerPos.position);
            total_timer += Time.deltaTime;
            if(openJaw == false){
                    if(count == 20){
                        reaction_happy();
                        makeLog_f("한 조각 완성☆");
                    }
                    eat_timer += Time.deltaTime;
                    if(eat_timer >= 10f){
                        reaction_dong();
                        makeLog_f("꼭꼭 잘 씹어먹어주세요 :(");
                    }
                   
            }

            else if(openJaw == true){
                if(count == 20){
                        reaction_happy();
                        makeLog_f("한 조각 완성☆");
                }
                eat_timer = 0;
                reaction_basic();
                makeLog_f("잘하고 있어요!!");
            }
 
            //makeLog("길이: " + distance);
            
            if(distance >= (ShareValue.sharedvalue + 0.0015) && openJaw == false && (Mathf.Abs(face.transform.rotation.y) < 0.02f))
            {
                openJaw = true;
                if(count == 20){
                        count = 0;
                        spoon += 1;
                        nAlphaCnt = 0;
                        compact_num += 1;
                }
                

                if(compact_num == 13 && first_check == false){
                    for(int i=1; i<13;i++){
                        compact_name = "compact (" + i + ")";
                        GameObject.Find(compact_name).SetActive(false);
                    }
                    GameObject.Find("compact").transform.Find("circle").gameObject.SetActive(true);
                    compact_num = 1;
                    first_check = true;
                }

                if(first_check == true && compact_num == 7){
                    GameObject.Find("compact").transform.Find("back_deco").gameObject.SetActive(true);
                }

                if(first_check == true && compact_num == 11){
                    GameObject.Find("compact").transform.Find("h_back").gameObject.SetActive(true);
                }

                if(spoon == 26){
                    makeLog_f("다 먹었어요!");
                    return_time(total_timer);
                    db_time = hour + "h " + min + "m " + sec + "s";
                    writeNewRecord(db_time, "성공", sharedCha.cha_n[sharedCha.r_num], (sharedCha.r_num+1).ToString());
                    UploadCha(sharedCha.r_num);
                }
                //reaction_basic();
            }
            else 
            {
                if(distance < (ShareValue.sharedvalue + 0.001) && openJaw == true && (Mathf.Abs(face.transform.rotation.y) < 0.02f)){
                    //compact_c1.color.a = 0.1f;
                    
                    //compact_c1.a += 0.05f;
                    count += 1;
                    nAlphaCnt++;
                    

                    openJaw = false;
                    if(spoon <= 11){
                        compact_name = "compact (" + compact_num + ")";
                        GameObject.Find(compact_name).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(fArrAlphaRange[nAlphaCnt]);
                    }
                    else if(spoon <= 25){
                        deco_name = "deco (" + compact_num + ")";
                        GameObject.Find(deco_name).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(fArrAlphaRange[nAlphaCnt]);

                    }
                    
                    
                }
                //makeLog(str);
                 makeLog("씹은 횟수: " + count + " 숟가락 횟수: " + spoon + "알파값: " + nAlphaCnt + "고개: " + Mathf.Abs(face.transform.rotation.y));
                
                
            }
            
            
            
            

            //makeLogMerge(mFaceMesh.vertices[0].x.ToString(), mFaceMesh.vertices[0].y.ToString(), mFaceMesh.vertices[0].z.ToString()
            //           , mFaceMesh.vertices[14].x.ToString(), mFaceMesh.vertices[14].y.ToString(), mFaceMesh.vertices[14].z.ToString());


        }


    }

    

    void makeLog(string str)
    {
        //GameObject.Find("poseButton").GetComponentInChildren<Text>().text = str;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().text = "\t" + str;
    }

    void makeLog_f(string str)
    {
        //GameObject.Find("poseButton").GetComponentInChildren<Text>().text = str;
        GameObject.Find("compliment").GetComponentInChildren<Text>().text = "\t" + str;
    }

    void makeLogXYZ(string x, string y, string z)
    {
        GameObject.Find("poseButton").GetComponentInChildren<Text>().text
            = "\tX: " + x + "\r\n"
            + "\tY: " + y + "\r\n"
            + "\tZ: " + z;
    }

    void makeLogMerge(string x, string y, string z, string xx, string yy, string zz)
    {
        GameObject.Find("poseButton").GetComponentInChildren<Text>().text
            = "\tUX: " + x + " |#| LX" + xx + "\r\n"
            + "\tUY: " + y + " |#| LY" + yy + "\r\n"
            + "\tUZ: " + z + " |#| LZ" + zz + "";
    }

    void takeEnable(){
        for(int i=1; i<=12; i++){
            compact_name = "compact (" + i + ")";
            GameObject.Find(compact_name).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        }
        for(int i=1; i<=14; i++){
            deco_name = "deco (" + i + ")";
            GameObject.Find(deco_name).GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        }
        GameObject.Find("circle").SetActive(false);
        GameObject.Find("h_back").SetActive(false);
        GameObject.Find("back_deco").SetActive(false);
        GameObject.Find("dongdong").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        GameObject.Find("b_happy").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
    }

    void return_time(float tictok){
        hour = (int)tictok / 3600;
        min = (int)(tictok % 3600) / 60;
        sec = (int)(tictok - 3600*hour - 60*min);
    }

    void reaction_basic(){
        GameObject.Find("dongdong").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        GameObject.Find("b_happy").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        GameObject.Find("basic").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(1);
    }

    void reaction_dong(){
        GameObject.Find("dongdong").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(1);
        GameObject.Find("b_happy").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        GameObject.Find("basic").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
    }

    void reaction_happy(){
        happy_time = 0;
        happy_time += Time.deltaTime;
        if(happy_time <= 3f){
            GameObject.Find("dongdong").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
            GameObject.Find("b_happy").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(1);
            GameObject.Find("basic").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
        }
        else if(happy_time > 3f){
            GameObject.Find("dongdong").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
            GameObject.Find("b_happy").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(0);
            GameObject.Find("basic").GetComponentInChildren<Image>().canvasRenderer.SetAlpha(1);
        }
    }

    public void apply_crawn(int a){
        if(a==1)    mCenterPrefab = pre1;
        if(a==2)    mCenterPrefab = pre2;
        if(a==3)    mCenterPrefab = pre3;
        if(a==4)    mCenterPrefab = pre4;
    }

    public void moveSet(){
        SceneManager.LoadScene("SetScene");
    }

    public void UploadRecord(){
        return_time(total_timer);
        db_time = hour + "h " + min + "m " + sec + "s";
        writeNewRecord(db_time, "성공", sharedCha.cha_n[sharedCha.r_num],  (sharedCha.r_num+1).ToString());
        UploadCha(sharedCha.r_num);
    }

    public void writeNewRecord(string r_time, string r_result, string r_cha, string n_cha){
        Record record = new Record(r_time, r_result, r_cha, n_cha);
        string json = JsonUtility.ToJson(record);
        reference.Child("personal Record").Child(GetCurrentDate()).SetRawJsonValueAsync(json);
    }

    public static string GetCurrentDate(){
        return DateTime.Now.ToString(("yy-MM-dd HH:mm:ss tt"));
    }

    public void UploadFail(){
        db_time = hour + "h " + min + "m " + sec + "s";
        writeNewRecord(db_time, "실패", "-", "-");
    }

    public void UploadCha(int num){
        reference.Child("Collection").Child((num+1).ToString()).SetValueAsync("1");
    }

    // void changeset(){
    //     set = true;
    // }
}