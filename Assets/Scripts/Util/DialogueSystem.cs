using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{

    public Dialogue[] dialogues;
    public int[] triggers;
    public int index;
    public Counter tracker;
    bool inProgress;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i < dialogues.Length; i++){
            dialogues[i].gameObject.SetActive(false);
        } 
        index = 0;
        inProgress = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(index < dialogues.Length && dialogues[index].IsFinished()){
            dialogues[index].gameObject.SetActive(false);
            index += 1;
            inProgress = false;
        }
        if(index < dialogues.Length && tracker.getSlainEnemies() == triggers[index] && !inProgress){
            dialogues[index].gameObject.SetActive(true);
            inProgress = true;
        }

    }
    public bool getProgress(){
        return inProgress;
    }
}
