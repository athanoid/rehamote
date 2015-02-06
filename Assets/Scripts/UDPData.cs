using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using UnityEngine.UI;

public class UDPData : MonoBehaviour {

    public Transform AdressSendDataInputField, PortSendDataInputField;
    InputField InputField1, InputField2;
    bool first = true;


	private static int localPort;
	
    // prefs 
    public static string IP ;  // define in init
    public static int port ;  // define in init

    private string ipField = "192.168.10.42";
	private string portField = "1202";
	
	public static bool flag=false;
		
	// "connection" 
    public static IPEndPoint remoteEndPoint;
    public static UdpClient client;

	
	
	// Use this for initialization
	void Start ()
	{
    //    init();	

        //sets the initial adress and port values
        InputField1 = AdressSendDataInputField.GetComponent<InputField>();
        InputField1.text = ipField;

        InputField2 = PortSendDataInputField.GetComponent<InputField>();
        InputField2.text = portField;
	}
	
	// Update is called once per frame
	void Update () 
	{	
        //updating the ipfield if user changes the ip adress
        ipField = AdressSendDataInputField.GetComponent<InputField>().text;
        InputField1.text = ipField;

        portField = PortSendDataInputField.GetComponent<InputField>().text;
        InputField2.text = portField;

        //	send UDP
        if (ButtonChange.sendOn)
        {
            if (first)
            {
                //Debug.Log("started");
                IP = ipField;
                port = int.Parse(portField);
                init();
                flag = true;
                Debug.Log("Start UDP");
                Handheld.Vibrate();
                first = false;
            }
        }
        else
        {
            if (flag)
            {
                if (!first)
                {
                    //Debug.Log("stoped");
                    flag = false;
                    first = true;
                    client.Close();
                    Debug.Log("Stop UDP");
                    Handheld.Vibrate();
                }
            }
        }
	}
	
	public static void init()
    { 

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

        client = new UdpClient();  

        // status
        print("Sending to "+IP+" : "+port);
    }
	
	
	 // sendData
    public static void sendString(string message)
    {
        try 
        {
               if (message != "") 
                {
                    // UTF8 encoding to binary format.
                     byte[] data = Encoding.UTF8.GetBytes(message);

                    // Send the message to the remote client.
                   client.Send(data, data.Length, remoteEndPoint);		
                }
        }

        catch (Exception err)
        {
          Debug.Log(err.ToString());
        }
    }//end of sendString
	
	
	 void OnApplicationQuit () 
		{
		if (client != null) 
          client.Close();
		}	
	
	
}
