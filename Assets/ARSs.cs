using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


public class ARSs : MonoBehaviour
{
    //AR face manager info
    private ARFaceManager mArFaceManager;
    //�� , ��/�� �Ӹ��� ��ǥ���� AR�� �����ϱ� ���� sessionorigin�� ���� ��ǥ�� get
    private ARSessionOrigin mSessionOrigin;


    //�ڸ� �������� ������ ������ 
    public GameObject mCenterPrefab;
    //������ �� ������Ʈ 
    private GameObject mCenterObject;

    //GetRegionPoses�� output
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


    void Start()
    {
        //ARFaceManager init.
        mArFaceManager = GetComponent<ARFaceManager>();
        //ARSessionOrigin init.
        mSessionOrigin = GetComponent<ARSessionOrigin>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // GameObject.Find("poseButton").GetComponentInChildren<Text>().fontSize = 30;
        // GameObject.Find("poseButton").GetComponentInChildren<Text>().alignment = TextAnchor.MiddleLeft;

    }

    // Update is called once per frame
    void Update()
    {

        //Android target������ arcore api�� ����ϱ� ����
        ARCoreFaceSubsystem coreSub = (ARCoreFaceSubsystem)mArFaceManager.subsystem;

        foreach (var face in mArFaceManager.trackables)
        {

            //Camera�κ��� �ν��� face�� id�� ������ mFaceRegions�� �Ҵ�
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
            customPos.position.y = (mFaceRegions[0].pose.position.y + mFaceRegions[1].pose.position.y + mFaceRegions[2].pose.position.y + mFaceRegions[3].pose.position.y + mFaceRegions[4].pose.position.y) / 2;


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
            //makeLog("Lib distance: " + distance);
            //if (distance >= 0.020)
            //{
                //makeLog("�� OPEN");
                //makeLog("�� OPEN(" + distance + ")");
            //}
            //else
            //{
                //makeLog("�� CLOSE");
                //makeLog("�� CLOSE(" + distance + ")");
            //}

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


