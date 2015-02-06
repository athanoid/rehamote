/*
    -----------------------
    UDP-Receive
    -----------------------
     [url]http://msdn.microsoft.com/de-de/library/bb979228.aspx#ID0E3BAC[/url]
*/

using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using UnityEngine.UI;

public class UDPReceive : MonoBehaviour {

    public Transform PortReceiveDataInputField, adressLabel, buzzLabel;
    InputField portInputField;
    Text adressLabelText, buzzLabelText;


	// receiving Thread
    Thread receiveThread; 
    // udpclient object
    UdpClient client;
    //public Texture2D bgText;
 
    // public
    public int port; // define > init
	
//	private string ipField = "127.0.0.1";
	private string portField = "1204";
	
	public static bool flag=false;

    // infos
    public string lastReceivedUDPPacket="";
    public string allReceivedUDPPackets=""; // clean up this from time to time!
	
	private string text ="";

    bool first = true;
    bool buzz = false;

    string joint;
    
	/*
	//text style
	public GUIStyle titlestyle, textstyle, fieldstyle, buttonstyle1, buttonstyle2, boxStyle;
	
	void OnGUI () 
	{
        GUI.depth = 2;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bgText);

		//Network Group
		GUI.BeginGroup (new Rect (10, (Screen.height*0.5f)+10, Screen.width-20, Screen.height*0.5f));
	//	GUI.color = Color.yellow;
		GUI.Box (new Rect (10,0,Screen.width - 40, Screen.height*0.5f - 40), " ", boxStyle);
        GUI.Label(new Rect(Screen.width * 0.5f - 60, 20, 100, 20), "RECEIVE DATA", titlestyle);
		//GUI.color = Color.white;
		
		//display local ip address
		GUI.Label(new Rect(50, 100, 200, 20), "Address: "+ "    " + LocalIPAddress(), textstyle);
				
		GUI.Label(new Rect(50, 140, 100, 20), "Port:", textstyle);
		portField = GUI.TextField (new Rect (Screen.width/2-30, 140, 150, 25), portField, fieldstyle);
		
	GUI.enabled = !flag;				
		//Start sending UDP
		if (GUI.Button (new Rect (Screen.width/2-130, 175, 90, 80), "Start", buttonstyle1)) 
		{
			port = int.Parse(portField);		
			init();	
			flag=true;
			print("Start Server");
			Handheld.Vibrate();
		}	
	GUI.enabled = true;	
		
	GUI.enabled = flag;		
		//Stop sending UDP
		if (GUI.Button (new Rect (Screen.width/2+15, 175, 90, 80), "Stop", buttonstyle2)) 
		{		
			flag=false;
			receiveThread.Abort();
			client.Close();
			print("Stop Server");
			Handheld.Vibrate();
		}
 	GUI.enabled = true;
		

		
		//display raw data
		//GUI.color = Color.gray;
		GUI.Label(new Rect(20, 320, 100, 300), "Raw Data: "+lastReceivedUDPPacket, titlestyle);
		//GUI.color = Color.white;
		
		GUI.EndGroup (); // end network group
		
						
	}*/

	
	
    // start from unity3d
    public void Start()
    {
  //    init(); 
        portInputField = PortReceiveDataInputField.GetComponent<InputField>();
        portInputField.text = portField;
        adressLabelText = adressLabel.GetComponent<Text>(); 
        buzzLabelText = buzzLabel.GetComponent<Text>();
    }
  
	
        
    // init
    public void init()

    {			
        // define port
 //       port = 1202;

        // status
        UnityEngine.Debug.Log("Receiving from 127.0.0.1 : " + port);

        // ----------------------------
        // monitor
        // ----------------------------

        // Local endpoint define (where messages are received).

        // Create a new thread to receive incoming messages.

        receiveThread = new Thread(new ThreadStart(ReceiveData));

        receiveThread.IsBackground = true;
        
        receiveThread.Start();
    }
 

    // receive thread 
    private void ReceiveData() 
    {
        UnityEngine.Debug.Log("ReceiveData: ");
    //    port = int.Parse(portField);	
        client = new UdpClient(port);

        while (true) 
        {
            try 
            { 
                // receive Bytes from 127.0.0.1
					IPEndPoint anyIP = new IPEndPoint(IPAddress.Loopback, 0);

        	        byte[] data = client.Receive(ref anyIP);

                //  UTF8 encoding in the text format.
					text = Encoding.UTF8.GetString(data);
                    UnityEngine.Debug.Log("text: " + text);
           	    //text = Encoding.ASCII.GetString(data); asci encoding
               
                // latest UDPpacket
                lastReceivedUDPPacket = text; 
				
				//	[$]<data  type> , [$$]<device> , [$$$]<joint> , <transformation> , <param_1> , <param_2> , .... , <param_N>
								
				
				string[] separators = {"[$]","[$$]","[$$$]",",",";"," "};
			      
			    string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			 	         					
				string datatype = words[0];
				string device = words[1];
				joint = words[2];

                UnityEngine.Debug.Log("joint: " + joint);

                if (joint == "b")
                {
                    buzz = true;
                }

				string transformationtype = words[3];
//				float udpx = float.Parse(words[4]);
//				float udpy = float.Parse(words[5]);
//				float udpz = float.Parse(words[6]);
//				float udpw = float.Parse(words[7]);
				
//				Debug.Log("datatype: "+datatype+" device: "+device+" joint: "+joint+" transformationtype: "+transformationtype+" X: "+udpx+" Y: "+udpy+" Z: "+udpz+" W: "+udpw);
				
               allReceivedUDPPackets=allReceivedUDPPackets+text;
               
            }
            catch (Exception err) 
            {
                print(err.ToString());
            }
        }
    }
	
	
void Update () 
	{
        portInputField.text = PortReceiveDataInputField.GetComponent<InputField>().text;
        adressLabelText.text = LocalIPAddress();
        buzzLabelText.text = text;
        portField = PortReceiveDataInputField.GetComponent<InputField>().text;

        //print(text);

        if (ButtonChange.receiveOn)
        {
            if (first)
            {
                first = false;
                port = int.Parse(portField);
                
                init();
                flag = true;
                print("Start Server");
                Handheld.Vibrate();
            }
        }
        else
        {
            if (!first)
            {
                first = true;
                flag = false;
                receiveThread.Abort();
                client.Close();
                print("Stop Server");
                Handheld.Vibrate();
            }
        }

        if (buzz)
        {
            Handheld.Vibrate();
            buzz = false;
        }
	}	
	
	
	//get local ip address
	public string LocalIPAddress()
	 {
		if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
    	{
        return null;
    	}
		
	   IPHostEntry host;
	   string localIP = "";
	   host = Dns.GetHostEntry(Dns.GetHostName());
	   foreach (IPAddress ip in host.AddressList)
	   {
	     if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
	     {
	       localIP = ip.ToString();
	     }
	   }
	   return localIP;
	 }
	
    
    // getLatestUDPPacket
    // cleans up the rest
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets="";
        return lastReceivedUDPPacket;
    }

	
	
	void OnDisable() 
	{ 
	    if (receiveThread!= null)
		{
		    receiveThread.Abort();
		    client.Close();
		}
	 } 
	
		
	void OnApplicationQuit () 
	{
		if (receiveThread!= null)
		{
		    receiveThread.Abort(); 
		    client.Close();
		}
	}
	

}