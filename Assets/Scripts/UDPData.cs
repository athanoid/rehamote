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
	
	//public GUIStyle titlestyle, textstyle, fieldstyle, buttonstyle1, buttonstyle2, boxStyle;
	
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
	//	send UDP
	//	[$]<data type> , [$$]<device> , [$$]<item> , <transformation> , <param_1> , <param_2> , .... , <param_N>	

        //updating the ipfield if user changes the ip adress
        ipField = AdressSendDataInputField.GetComponent<InputField>().text;
        InputField1.text = ipField;

        portField = PortSendDataInputField.GetComponent<InputField>().text;
        InputField2.text = portField;

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
	
	/*
	void OnGUI () 
	{

        GUI.depth = 1;
//		GUI.Label(new Rect(Screen.width/2-20, 280, 500, 20), "Gyro");
//		GUI.Label(new Rect(Screen.width/2-100, 320, 500, 20), "x:"+Androiddata.gyrox);
//		GUI.Label(new Rect(Screen.width/2-100, 350, 500, 20), "y:"+Androiddata.gyroy);
//		GUI.Label(new Rect(Screen.width/2-100, 380, 500, 20), "z:"+Androiddata.gyroz);
				
		//Network Group
//		GUI.BeginGroup (new Rect (Screen.width / 2 + 300, Screen.height/2 - 320, 200, 120));
		GUI.BeginGroup (new Rect (10 , 10, Screen.width-20, Screen.height/2));
	    //GUI.color = Color.gray;
		GUI.Box (new Rect (10,20,Screen.width - 40, Screen.height * 0.5f - 40), " ", boxStyle);
        GUI.Label(new Rect(Screen.width * 0.5f - 60, 40, 100, 20), "SEND DATA", titlestyle);
		//GUI.color = Color.white;
		
		
		GUI.Label(new Rect(50, 120, 100, 20), "Address:", textstyle);
		GUI.Label(new Rect(50, 160, 100, 20), "Port:", textstyle);
		ipField = GUI.TextField (new Rect (Screen.width/2-30, 120, 150, 25), ipField, fieldstyle);
		portField = GUI.TextField (new Rect (Screen.width/2-30, 160, 150, 25), portField, fieldstyle);
		
GUI.enabled = !flag;				
		//Start sending UDP
		if (GUI.Button (new Rect (Screen.width/2-130, 195, 90, 80), "Connect", buttonstyle1)) 
		{
			IP = ipField;
			port = int.Parse(portField);
			
			init();
			
			flag=true;
			Debug.Log("Start UDP");
			Handheld.Vibrate();
		}	
GUI.enabled = true;	
		
GUI.enabled = flag;		
		//Stop sending UDP
		if (GUI.Button (new Rect (Screen.width/2+ 15, 195, 90, 80), "Disconnect", buttonstyle2)) 
		{		
			flag=false;
			client.Close();
			Debug.Log("Stop UDP");
			Handheld.Vibrate();
		}
 GUI.enabled = true;
		
		GUI.EndGroup (); // end network group
		 			
	}
    */
	
	
	public static void init()
    {       			
//        Debug.Log("UDPSend.init()");     

        // define
//        IP="127.0.0.1";
//        port=1202;   
		
        // ----------------------------
        // Send
        // ----------------------------

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
			
			//byte[] data = Encoding.ASCII.GetBytes(message); asci

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
