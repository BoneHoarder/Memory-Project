using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    [SerializeField] float delay= 5f;

    private string sceneName;

    float time= 5f;

    void Update(){

        if(time< delay){

            time+= Time.deltaTime;

            if(time>=delay){
                SceneManager.LoadScene(sceneName);
            }
        }
    }

   public void Change(string sceneName){
        sceneName= sceneName.Trim();
        SceneManager.LoadScene(sceneName);
        time= 0f;
   }
}
