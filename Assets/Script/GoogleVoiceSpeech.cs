﻿//	Copyright (c) 2016 steele of lowkeysoft.com
//        http://lowkeysoft.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
// Acquired from https://github.com/steelejay/LowkeySpeech
//
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;

//using System.Web;


[RequireComponent (typeof (AudioSource))]

public class GoogleVoiceSpeech : MonoBehaviour {

		struct ClipData
		{
				public int samples;
		}
        string voicecommand = "";
		const int HEADER_SIZE = 44;
		private int minFreq;
		private int maxFreq;
		private bool micConnected = false;
		private AudioSource goAudioSource;
		public string apiKey;
        public GameObject player;
        //public float targetTime = 60.0f;
        float elapseTime;
        bool timechecking = false;
      
        Vector3 forwardmove = new Vector3(0, 0, 0);
        int forwardDirection = 0; //0-stay, 1 - z axis, 2 -z axis, 3- x axis, 4- -axis

        string ResponesFromCoroutine;

    void Start () {

      
				if(Microphone.devices.Length <= 0)
				{
						
						Debug.LogWarning("Microphone not connected!");
				}
				else 
				{
						
						micConnected = true;

						Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
						
						if(minFreq == 0 && maxFreq == 0)
						{							
								maxFreq = 44100;
						}
                        
						goAudioSource = this.GetComponent<AudioSource>();
				}
	}

    void Update()
    {

        if (voicecommand != "")
        {
            Debug.Log("voicecommand = " + voicecommand);
            if (voicecommand == "right")
                player.transform.position += new Vector3(0.35f, 0f, 0f);
            if (voicecommand == "left")
                player.transform.position += new Vector3(-0.35f, 0f, 0f);
            if (voicecommand == "long right")
                player.transform.position += new Vector3(0.7f, 0f, 0f);
            if (voicecommand == "long left")
                player.transform.position += new Vector3(-0.7f, 0f, 0f);

            if (voicecommand == "forward") {
                float rn = UnityEngine.Random.Range(0, 1);
                if(rn >= 0.5)
                    player.transform.position += new Vector3(0f, 0.3f, 0f);
                else
                    player.transform.position += new Vector3(0f, 0.3f, 0f);

            }

            if (voicecommand == "back")
                player.transform.position += new Vector3(0f, -0.3f, 0f);



            if (voicecommand == "right down")
                player.transform.position += new Vector3(0.35f, -0.3f, 0f);

            if (voicecommand == "left down")
                player.transform.position += new Vector3(-0.35f, -0.3f, 0f);

            if (voicecommand == "right up")
                player.transform.position += new Vector3(0.35f, 0.3f, 0f);

            if (voicecommand == "left up")
                player.transform.position += new Vector3(-0.35f, 0.3f, 0f);









            voicecommand = "";

        }

     }

        void OnGUI() 
		{
				//If there is a microphone
				if(micConnected)
				{
						//If the audio from any microphone isn't being recorded
						if(!Microphone.IsRecording(null))
						{
								//Case the 'Record' button gets pressed
								if(GUI.Button(new Rect(0, 20, 100, 50), "Say"))
								{
										//Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource
										goAudioSource.clip = Microphone.Start( null, true, 7, maxFreq); //Currently set for a 7 second clip
								}
						}
						else //Recording is in progress
						{
							    //Case the 'Stop and Play' button gets pressed
								if(GUI.Button(new Rect(0, 20, 100, 50), "Stop"))
								{
                                        elapseTime = 0f;
                                        timechecking = true;

                                        float filenameRand = UnityEngine.Random.Range (0.0f, 10.0f);

										string filename = "testing" + filenameRand;

										Microphone.End(null); //Stop the audio recording

										if (!filename.ToLower().EndsWith(".wav")) {
												filename += ".wav";
										}

                    
                                       var filePath = Path.Combine("testing/", filename);
							           filePath = Path.Combine(Application.persistentDataPath, filePath);
										
									
										Directory.CreateDirectory(Path.GetDirectoryName(filePath));
										SavWav.Save (filePath, goAudioSource.clip); //Save a temporary Wav File

										string apiURL = "https://speech.googleapis.com/v1/speech:recognize?&key=" + apiKey;
										string Response;

                         System.DateTime now = System.DateTime.Now;
                        //Response = HttpUploadFile (apiURL, filePath, "file", "audio/wav; rate=44100");         
                        StartCoroutine(
                        HttpUploadFileCoroutine(apiURL, filePath, "file", "audio/wav; rate=44100"));
                        
                        System.DateTime now2 = System.DateTime.Now;
                        //Debug.Log("second=" + (now2.Second - now.Second) + "," + "milisencod=" +
                        //(now2.Millisecond - now.Millisecond));

                        /*
                        var jsonresponse = SimpleJSON.JSON.Parse(Response);

					    if (jsonresponse != null) {		
							//string resultString = jsonresponse ["result"] [0].ToString ();
							//var jsonResults = SimpleJSON.JSON.Parse (resultString);

							//string transcripts = jsonResults ["alternative"] [0] ["transcript"].ToString ();

							//Debug.Log ("transcript string: " + transcripts );
							//temp TextBox.text = transcripts;

					    }
                        */
                } //if(GUI.Button(new Rect(0, 20, 100, 50), "Stop"))

                   GUI.Label(new Rect(0, 200, 100, 50), "Recording in progress...");
                } //else //Recording is in progress
                }
				else // No microphone
			  	{						
						GUI.contentColor = Color.red;
						GUI.Label(new Rect(Screen.width/2-100, Screen.height/2-25, 100, 50), "Microphone not connected!");
				}
		}

    IEnumerator HttpUploadFileCoroutineX(string url, string file, string paramName, string contentType)
    {
        //HttpUploadFile(url, file, paramName, contentType);

        yield return 0;
    }

    IEnumerator HttpUploadFileCoroutine(string url, string file, string paramName, string contentType) {

        //Debug.Log("url=" + url + "\n");

        System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;

        Byte[] bytes = File.ReadAllBytes(file);
        //yield return 0;
        String file64 = Convert.ToBase64String(bytes,
                                         Base64FormattingOptions.None);
        yield return 0;
        //try
        { 
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //yield return 0;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                yield return 0;
                //en-US
                //ko-KR
                string json = "{ \"config\": { \"languageCode\" : \"ko-KR\", \"encoding\" : \"LINEAR16\", \"sampleRateHertz\" : 48000}, \"audio\" : { \"content\" : \"" + file64 + "\"}}";

                streamWriter.Write(json);
                yield return 0;
                streamWriter.Flush();
                streamWriter.Close();
            }
            yield return 0;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            //Debug.Log("httpResponse after web =" + httpResponse + "\n");
            yield return 0;
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //yield return 0;
                string result = streamReader.ReadToEnd();
                //Debug.Log("Response:" + result + "[end]");

                //yield return 0;
                if (result.Contains("왼쪽")) //left"))
                {
                    voicecommand = "left";
                    //Debug.Log("left recognized");
                }

                if (result.Contains("오른쪽")) //right"))
                    voicecommand = "right";

                if (result.Contains("길게") && result.Contains("오른쪽")) // long right"))
                    voicecommand = "long right";

                if (result.Contains("길게") && result.Contains("왼쪽")) //long left"))
                    voicecommand = "long left";

                if (result.Contains("앞으로")) //left"))
                {
                    voicecommand = "forward";
                    //Debug.Log("left recognized");
                }
                if (result.Contains("뒤로")) //left"))
                {
                    voicecommand = "back";
                    //Debug.Log("left recognized");
                }


                if (result.Contains("오른쪽") && result.Contains("위로")) //long left"))
                    voicecommand = "right up";



                if (result.Contains("오른쪽") && result.Contains("아래로")) //long left"))
                    voicecommand = "right down";



                if (result.Contains("왼쪽") && result.Contains("위로")) //long left"))
                    voicecommand = "left up";




                if (result.Contains("왼쪽") && result.Contains("아래로")) //long left"))
                    voicecommand = "left down";




            }

        
        } 
        //catch (WebException ex)
        {
            //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            //Debug.Log("error=" + resp);
 
        }
        //return "empty";
        yield return 0;
		
	}

}
		
