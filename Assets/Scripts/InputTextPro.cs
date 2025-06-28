using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputTextPro : MonoBehaviour
{
    public TextMeshProUGUI textMeshProComponent;
    public TMP_InputField inputField;
    void Start()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.placeholder.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
