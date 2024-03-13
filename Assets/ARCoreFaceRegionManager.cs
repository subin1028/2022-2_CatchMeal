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

public static class SharedCrawn
{
    public static int sharedcrawn;
}

public class ARCoreFaceRegionManager : MonoBehaviour
{
    //AR face manager info
    private ARFaceManager mArFaceManager;
    //코 , 좌/우 머리의 좌표값을 AR상에 매핑하기 위한 sessionorigin을 통해 좌표값 get
    private ARSessionOrigin mSessionOrigin;


    //코를 기준으로 생성할 프리팹 
    public GameObject mCenterPrefab;    
    //생성한 코 오브젝트 
    private GameObject mCenterObject;

    //GetRegionPoses시 output
    private NativeArray<ARCoreFaceRegionData> mFaceRegions;

    public GameObject mUpperLibPrefab;
    public GameObject mLowerLibPrefab;
    private GameObject mUpperLibObject;
    private GameObject mLowerLibObject;
    private XRFaceMesh mFaceMesh;

    public Button savebutton;

    public GameObject pre1;
    public GameObject pre2;
    public GameObject pre3;
    public GameObject pre4;
    private int temp_crawn;

    public void change_1()
    {
        mCenterPrefab = pre1;
        GameObject.Destroy(mCenterObject.transform.gameObject, 0);
        temp_crawn = 1;

    }

    public void change_2()
    {
        mCenterPrefab = pre2;
        GameObject.Destroy(mCenterObject.transform.gameObject, 0);
        temp_crawn = 2;
    }

    public void change_3()
    {
        mCenterPrefab = pre3;
        GameObject.Destroy(mCenterObject.transform.gameObject, 0);
        temp_crawn = 3;
    }

    public void change_4()
    {
        mCenterPrefab = pre4;
        GameObject.Destroy(mCenterObject.transform.gameObject, 0);
        temp_crawn = 4;
    }



    void Start()
    {
        //ARFaceManager init.
        mArFaceManager = GetComponent<ARFaceManager>();
        //ARSessionOrigin init.
        mSessionOrigin = GetComponent<ARSessionOrigin>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().fontSize = 30;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        temp_crawn = 1;
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
                upperPos.position.x = mFaceMesh.vertices[0].x;
                upperPos.position.y = mFaceMesh.vertices[0].y;
                upperPos.position.z = mFaceMesh.vertices[0].z;
                mUpperLibObject.transform.rotation = upperPos.rotation;
                mUpperLibObject.transform.position = upperPos.position;

                Pose lowerPos = new Pose();
                lowerPos.rotation = mFaceRegions[0].pose.rotation;
                lowerPos.position.x = mFaceMesh.vertices[14].x;
                lowerPos.position.y = mFaceMesh.vertices[14].y;
                lowerPos.position.z = mFaceMesh.vertices[14].z;
                mLowerLibObject.transform.rotation = lowerPos.rotation;
                mLowerLibObject.transform.position = lowerPos.position;

                float distance = Vector3.Distance(upperPos.position, lowerPos.position);

                
            }


    }

    void makeLog(string str)
    {
        //GameObject.Find("poseButton").GetComponentInChildren<Text>().text = str;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().text = "\t" + str;
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

    public void savecrawn(){
        FirebaseDatabase.DefaultInstance
                .GetReference("EatingInterest")
                .Child("saveCrawn")
                .SetValueAsync(temp_crawn);
                SharedCrawn.sharedcrawn = temp_crawn;
    }

    public void cngScene(){
        SceneManager.LoadScene("newGameMain");
    }
}