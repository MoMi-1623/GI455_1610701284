using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextData : MonoBehaviour
{
    public string[] nameData;
    [SerializeField] GameObject listText;
    // Start is called before the first frame update
    void Start()
    {
        foreach(string data in nameData)
        {
            var listObj = Instantiate(listText,transform);
            listObj.GetComponent<Text>().text = data;
            listObj.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
