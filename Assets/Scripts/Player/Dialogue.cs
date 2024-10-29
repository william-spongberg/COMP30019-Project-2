using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    bool startFinished;
    bool started = false;
    public PauseManagerScript pauser;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
        startFinished = false;
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(started == false){
            textComponent.text = string.Empty;
            StartDialogue();
            startFinished = false;
            started = true;
        }
        if(Input.GetKeyDown(KeyCode.Return) && !pauser.currentlyPaused()){
            if (textComponent.text == lines[index]){
                NextLine();
            }
            else{
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine(){
        lines[index] = lines[index].Replace("@", System.Environment.NewLine);
        foreach (char c in lines[index].ToCharArray()){
            textComponent.text+= c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine(){
        if (index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());

        }
        else{
            startFinished = true;
            //gameObject.SetActive(false);
        }

    }

    //public void hide(){
      //  gameObject.
    //}

    public bool IsFinished(){
        return(startFinished);
    }
}
