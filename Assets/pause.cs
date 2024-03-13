using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    public bool pauseButton; 
    public RectTransform Panel;
    // Start is called before the first frame update
    void Start()
    {
        pauseButton = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(pauseButton == true){
            Time.timeScale = 0F;
            Panel.localScale = new Vector3(1,1);
        }
        else{
            Time.timeScale = 1F;
            Panel.localScale = new Vector3(0,0);
        }
    }

    public void cngPause(){
        if(pauseButton == true){
            pauseButton = false;
        }
        else{
            pauseButton = true;
        }
    }

    public void cngSuccess(){
        SceneManager.LoadScene("SuccessScene");
    }

    public void cngMain(){
        SceneManager.LoadScene("FailScene");
    }
}
