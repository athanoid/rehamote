using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonChange : MonoBehaviour {

    public Transform sendButton, receiveButton;
    Button buttonSend, buttonReceive;
    public Sprite onText, offText;
    public static bool onButton = false;
    int button1Status, button2Status;
    public static bool sendOn, receiveOn;

    void Awake()
    {
        buttonSend = sendButton.gameObject.GetComponent<Button>();
        buttonReceive = receiveButton.gameObject.GetComponent<Button>();
        button1Status = 0;
    }

	void Update() 
    {
        if (button1Status == 0)
        {
            buttonSend.image.sprite = offText;
            sendOn = false;
        }
        else
        {
            buttonSend.image.sprite = onText;
            sendOn = true;
        }

        if (button2Status == 0)
        {
            buttonReceive.image.sprite = offText;
            receiveOn = false;
        }
        else
        {
            buttonReceive.image.sprite = onText;
            receiveOn = true;
        }
	}

    public void ChangeSendButtonImage(int change)
    {
        if (button1Status == 0)
        {
            button1Status = change;
        }
        else
        {
            button1Status = 0;
        }
    }

    public void ChangeReceiveButtonImage(int change)
    {
        if (button2Status == 0)
        {
            button2Status = change;
        }
        else
        {
            button2Status = 0;
        }
    }

    public void CloseApplication(int close)
    {
        if(close == 1)
        {
            Application.Quit();
        }
    }
}
