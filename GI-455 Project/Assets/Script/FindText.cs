using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindText : MonoBehaviour
{
    TextData textData;
    [SerializeField] Text inputText;
    [SerializeField] Text showText;
    // Start is called before the first frame update
    void Start()
    {
        textData = GameObject.FindObjectOfType<TextData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FindData()
    {
        for(int i = 0; i <= textData.nameData.Length;i++)
        {          
            if(textData.nameData[i] == inputText.text)
            {
                showText.text = "[ " + "<color=green>" + inputText.text + " ]" + "</color>" + " is found.";
                return;
            }
            else if(textData.nameData[i] != inputText.text && i == textData.nameData.Length-1)
            {               
                    showText.text = "[ " + "<color=green>" + inputText.text + " ]" + "</color>" + " is not found.";
                    return;
            }
        }
    }
}
