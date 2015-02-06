using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Androiddata : MonoBehaviour {

    public Transform touchPosX, touchPosY, touchCount, accelX, accelY, accelZ;
    Text touchPosXText, touchPosYText, touchCountText, accelXText, accelYText, accelZText;
    
	public static float gyrox,gyroy,gyroz;
	Vector2 touch;
	public float touchx, touchy;
	
	public float speed = 0.1F;
	public float rotspeed = 1.0F;
	Vector2 touchDeltaPosition, touchPosition;
	int touchcount;
	
	void Start () 
	{
		// Disable screen dimming
    	Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //Access to component text of the transforms assign them to UI text variables
        touchPosXText = touchPosX.GetComponent<Text>();
        touchPosYText = touchPosY.GetComponent<Text>();
        touchCountText = touchCount.GetComponent<Text>();

        accelXText = accelX.GetComponent<Text>();
        accelYText = accelY.GetComponent<Text>();
        accelZText = accelZ.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) 
		{
			touchPosition = Input.GetTouch(0).position;
        }

		touchcount = Input.touchCount;
		
		
	    gyrox=-Input.acceleration.x;//Input.gyro.userAcceleration.x; 
	    gyroy=-Input.acceleration.y;//Input.gyro.userAcceleration.y;
	    gyroz=-Input.acceleration.z;//Input.gyro.userAcceleration.z;
	    touchx=-touchPosition.x;
	    touchy=touchPosition.y;	
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
  			Application.Quit();
		}
		
		sendTracking();

        //Update device values
        touchPosXText.text = touchx.ToString("0");
        touchPosYText.text = touchy.ToString("0");
        touchCountText.text = touchcount.ToString();

        accelXText.text = gyrox.ToString("0.00");
        accelYText.text = gyroy.ToString("0.00");
        accelZText.text = gyroz.ToString("0.00");

	}
	
	void sendTracking()
	{
		if(UDPData.flag==true)
		{
		//Gyro data
		UDPData.sendString("[$]tracking,[$$]Android,[$$$]Gyro,position,"+gyrox.ToString()+","+gyroy.ToString()+","+gyroz.ToString()+","+"0"+";");
		//Touch
		UDPData.sendString("[$]tracking,[$$]Android,[$$$]Touch,position,"+touchx.ToString()+","+touchy.ToString()+","+"0"+","+"0"+";");
		//TouchCount
		UDPData.sendString("[$]tracking,[$$]Android,[$$$]TouchCount,count,"+touchcount.ToString()+","+"0"+","+"0"+","+"0"+";");
		}				
	}
		
}
