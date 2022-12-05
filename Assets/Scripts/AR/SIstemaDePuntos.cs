using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SIstemaDePuntos : MonoBehaviour
{
    GameObject star1, star2, star3;
    GameObject popUpWin;

     // set it to true when gameplay has started, to false when level finished or game paused
     private bool timerRunning = true;
     public void SetTimerRunning (bool value) { timerRunning = value; }
 
     private float remainingTime = 60.0f;
 
     private void Update () {
         if (timerRunning) {
             remainingTime -= Time.deltaTime;
         }

        Debug.LogWarning(remainingTime);

        // popUpWin = GameObject.FindGameObjectWithTag("PopUpWin");
        star1 = GameObject.Find("Star1").transform.GetChild(1).gameObject;
        star2 = GameObject.Find("Star2").transform.GetChild(1).gameObject;
        star3 = GameObject.Find("Star3").transform.GetChild(1).gameObject;
        EncenderEstrellas();
        
     }
     // call from anywhere you want to know how many stars remain. Mathf.Clamp() used to not go below 0 or above 3
     // +1 added to form the following pattern: 40-60 seconds = 3 stars, 20-40 seconds = 2 stars, 0-20 seconds 1 star, 0 stars otherwise
     public int GetPoints () {
         return Mathf.Clamp((int)(remainingTime / 20) + 1, 0, 3);
     }

     private void EncenderEstrellas()
     {
        //  if(popUpWin.activeSelf){
        //      timerRunning = false;
        //  }
        //  else{timerRunning = true;}
         
         if(GetPoints() == 3){
             star3.SetActive(true);
             star2.SetActive(true);
             star1.SetActive(true);
         }

         else if(GetPoints() == 2){
             star3.SetActive(false);
             star2.SetActive(true);
             star1.SetActive(true);
         }

        else if(GetPoints() == 1){
            star3.SetActive(false);
            star2.SetActive(false);
            star1.SetActive(true);
         }

         else if(GetPoints() < 1){
            star3.SetActive(false);
            star2.SetActive(false);
            star1.SetActive(false);
         }
     }


}
