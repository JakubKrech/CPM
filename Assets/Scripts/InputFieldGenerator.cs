using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldGenerator : MonoBehaviour
{
    public GameObject parent;
    public Text ammountOfInputsText;
    public InputField inputFieldPrefab;
    public List<InputField> inputFieldList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void refreshInputFields()
    {
        foreach (InputField ifield in inputFieldList)
        {
            Destroy(ifield.gameObject);
        }
        inputFieldList.Clear();


        int ammountOfInputs = int.Parse(ammountOfInputsText.text);

        for(int i = 1; i <= ammountOfInputs; i++)
        {
            InputField newInputField = (InputField)Instantiate(inputFieldPrefab, new Vector3(transform.position.x, transform.position.y + (i-1) * -30, transform.position.z), Quaternion.identity, parent.transform);
            newInputField.name = "InputField" + i;
            newInputField.inputIDText.text = i.ToString();
            inputFieldList.Add(newInputField);
        }
    }
}
