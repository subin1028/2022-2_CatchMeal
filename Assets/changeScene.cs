using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class changeScene : MonoBehaviour
{
    public Button backbutton;
    public Button gamebutton;
    public Button recordbutton;
    public Button ARbutton;
    public Button albumButton;
    public Button setButton;
    // Start is called before the first frame update
    void Start()
    {
        gamebutton.onClick.AddListener(cngReadyScene);
        //recordbutton.onClick.AddListener(cngARScene);
        //albumButton.onClick.AddListener(cngARScene);
        ARbutton.onClick.AddListener(cngARScene);
        //ARbutton.onClick.AddListener(cngARScene);
        setButton.onClick.AddListener(cngSetScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void cngGameScene(){
        SceneManager.LoadScene("GameScene");
    }
    public void cngMainScene(){
        SceneManager.LoadScene("newGameMain", LoadSceneMode.Single);
    }

    public void cngARScene(){
        SceneManager.LoadScene("ARSaveScene");
    }

    public void cngReadyScene(){
        SceneManager.LoadScene("FixSetScene");
    }

    public void cngSetScene(){
        SceneManager.LoadScene("SetScene");
    }

    public void cngRecordScene(){
        SceneManager.LoadScene("recordScene");
        sharedbool.load = true;
    }

    public void cngCollectScene(){
        SceneManager.LoadScene("CollectScene");
    }
}
