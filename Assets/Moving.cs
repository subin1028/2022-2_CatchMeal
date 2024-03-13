using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Moving : MonoBehaviour
{
    public Button changeBtn;
    // Start is called before the first frame update
    void changeScene()
    {
        SceneManager.LoadScene("newDistance");
    }
    
    void Start()
    {
        changeBtn.onClick.AddListener(changeScene);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
