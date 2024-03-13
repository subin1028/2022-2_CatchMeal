using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARCoreFaceRegionTest : MonoBehaviour
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
    private GameObject mNoseObject;
    public GameObject mNosePrefab;
    private GameObject mJawObject;
    public GameObject mJawPrefab;
    private XRFaceMesh mFaceMesh;

    

    // Start is called before the first frame update
    void Start()
    {
        //ARFaceManager init.
        mArFaceManager = GetComponent<ARFaceManager>();
        //ARSessionOrigin init.
        mSessionOrigin = GetComponent<ARSessionOrigin>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().fontSize = 30;
        GameObject.Find("poseButton").GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;
        





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
            customPos.position.y = mFaceRegions[0].pose.position.y;
            //customPos.position.y = (mFaceRegions[1].pose.position.y + mFaceRegions[2].pose.position.y) / 2;

            mCenterObject.transform.rotation = customPos.rotation;
            mCenterObject.transform.position = customPos.position;






            //face.vertices
            coreSub.GetFaceMesh(face.trackableId, Allocator.Persistent, ref mFaceMesh);
            if(!mNoseObject){
                mNoseObject = Instantiate(mNosePrefab, mSessionOrigin.trackablesParent);
            }

            if(!mJawObject){
                mJawObject = Instantiate(mJawObject, mSessionOrigin.trackablesParent);
            }

            if (!mUpperLibObject)
            {
                mUpperLibObject = Instantiate(mUpperLibPrefab, mSessionOrigin.trackablesParent);
            }

            if (!mLowerLibObject)
            {
                mLowerLibObject = Instantiate(mLowerLibPrefab, mSessionOrigin.trackablesParent);
            }

            //makeLogXYZ(mFaceMesh.vertices[0].x.ToString(), mFaceMesh.vertices[0].y.ToString(), mFaceMesh.vertices[0].z.ToString());
            
            Pose nosePose = new Pose();
            nosePose.rotation = mFaceRegions[0].pose.rotation;
            nosePose.position.x = mFaceMesh.vertices[25].x;
            nosePose.position.y = mFaceMesh.vertices[25].y;
            nosePose.position.z = mFaceMesh.vertices[25].z;
            mNoseObject.transform.rotation = nosePose.rotation;
            mNoseObject.transform.position = nosePose.position;
            
            Pose JawPose = new Pose();
            JawPose.rotation = mFaceRegions[0].pose.rotation;
            JawPose.position.x = mFaceMesh.vertices[200].x;
            JawPose.position.y = mFaceMesh.vertices[200].y;
            JawPose.position.z = mFaceMesh.vertices[200].z;
            mJawObject.transform.rotation = JawPose.rotation;
            mJawObject.transform.position = JawPose.position;




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
            float new_distance = Vector3.Distance(nosePose.position, JawPose.position);
            //makeLog("Lib distance: " + distance);
            if(new_distance >= 0.010)
            {
                makeLog("턱 벌림");
                //makeLog("입 OPEN(" + distance + ")");
            }
            else if(distance >= 0.050){
                makeLog("입 벌림");
            }
            else
            {
                makeLog("턱 닫힘");
                //makeLog("입 CLOSE(" + distance + ")");
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
}
