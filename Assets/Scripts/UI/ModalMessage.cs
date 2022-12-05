using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ricimi;
using System;

public class ModalMessage : MonoBehaviour
{
    //Modal
    [HideInInspector]
    private Image borderColour;
    [HideInInspector]
    private TextMeshProUGUI title;
    [HideInInspector]
    private TextMeshProUGUI message;
    [HideInInspector]
    //Button
    private Image buttonColour;
    [HideInInspector]
    private TextMeshProUGUI textButton;
    //Colour reference for Modal and Button
    public Sprite[] spriteBorderColour;
    public Sprite[] spriteButtonColour;
    //getters
    [HideInInspector]
    public string textTitle { get; set; }
    [HideInInspector]
    public string textMessage { get; set; }
    [HideInInspector]
    public string textAction { get; set; }
    [HideInInspector]
    public int spriteIndexBorderColour { get; set; }
    [HideInInspector]
    public int spriteIndexButtonColour { get; set; }
    private bool setValues;
    public ModalMessage()
    {
    }
    //     public ModalMessage(Canvas canvas, GameObject prefab, string textTitle, string textMessage, string textAction, int spriteIndexBorderColour, int spriteIndexButtonColour)
    //    {
    //         borderColour.sprite = spriteBorderColour[spriteIndexBorderColour];
    //         Debug.Log("borderColour " + borderColour.name);
    //         title.text = textTitle;
    //         message.text = textMessage;
    //         buttonColour.sprite = spriteButtonColour[spriteIndexButtonColour];
    //         textButton.text = textAction;
    //    }

    private void Awake()
    {
        setValues = false;
        /*
        Modal - Message
            Popup - Basic
                Background
                    Image
        */
        borderColour = transform.GetChild(0).GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        /*
        Modal - Message
            Title
        */
        title = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        /*
        Modal - Message
            Text - Blue
        */
        message = transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        /*
        Modal - Message
            Buttons
                Button - Blue (Teal)
                    Text
        */
        textButton = transform.GetChild(3).GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        /*
        Modal - Message
            Buttons
                Button - Blue (Teal)
                    Button
        */
        buttonColour = transform.GetChild(3).GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        for (int i = 0; i < 1; i++)
        {
            title.text = textTitle;
            message.text = textMessage;
            borderColour.sprite = spriteBorderColour[spriteIndexBorderColour];
            borderColour.sprite = spriteBorderColour[spriteIndexBorderColour];

            buttonColour.sprite = spriteButtonColour[spriteIndexButtonColour];
            textButton.text = textAction;
        }
    }

    public void SetData()
    {

    }
}
