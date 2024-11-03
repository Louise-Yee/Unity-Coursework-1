// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class TutManager : MonoBehaviour
// {
//     public GameObject[] popUps;
//     private int popUpIndex;
//     //public Player player;
//     // Start is called before the first frame update
//     void Start()
//     {
//         //player.jumpHeight = 0;
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         for(int i = 0; i < popUps.Length; i++){
//             if(i == popUpIndex){
//                 popUps[popUpIndex].SetActive(true);
//             } else{
//                 popUps[popUpIndex].SetActive(false);
//             }
//         }
//         if(popUpIndex == 0){
//             if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
//                 popUpIndex++;
//             }   
//         } else if(popUpIndex == 1){
//             if(Input.GetKeyDown(KeyCode.Space)){
//                 player.jumpHeight = 1.5f;
//                 popUpIndex++;
//             }
//         }else if(popUpIndex == 2){

//         }
        
//     }
// }
