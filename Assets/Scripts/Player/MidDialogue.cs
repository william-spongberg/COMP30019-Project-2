using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MidDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public bool startFinished;
    bool textBegun;
    public int enemiesKilled = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //gameObject.SetActive(false);
        textBegun = false;
        startFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startFinished == true){
            gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.RightShift)){
            enemiesKilled += 1;
            //startFinished = false;
            //gameObject.SetActive(true);
        }

        if(enemiesKilled < 1){ 
            gameObject.SetActive(true);
            if(startFinished == false){
                gameObject.SetActive(true);
                textComponent.text = string.Empty;
                StartDialogue();
                textBegun = true;
                startFinished = true;
            }
        }
        if(textBegun == true){
            if(Input.GetMouseButtonDown(0)){
                if (textComponent.text == lines[index]){
                    NextLine();
                }
                else{
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
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
            gameObject.SetActive(false);
        }

    }
}