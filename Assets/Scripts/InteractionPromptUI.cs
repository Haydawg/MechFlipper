using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    public bool isDisplayed;
    // Start is called before the first frame update
    void Start()
    {
        uiPanel.SetActive(false);
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var rotation = mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, worldUp: rotation * Vector3.up);
    }


    public void SetUp(string promptText)
    {
        this.promptText.text = promptText;    
        uiPanel.SetActive(true);
        isDisplayed= true;
    }

    public void Close()
    {
        isDisplayed= false;
        uiPanel.SetActive(false);
    }
}
