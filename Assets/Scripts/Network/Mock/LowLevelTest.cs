using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Text;

public class LowLevelTest : MonoBehaviour {

	public string Interface = "127.0.0.1";
	public int Port = 6969;
	public string TriggerString = "\ff\ff\ff\ffTSource Engine Query\00";

	int recHostId;
	int connectionId;
	int channelId;
	int dataSize;
	byte[] buffer = new byte[1024];
	byte error;

	// Use this for initialization
	void Start () {
	
		UnityEngine.Networking.NetworkTransport.Init ();

		ConnectionConfig connConf = new ConnectionConfig ();

		HostTopology hostTopology = new HostTopology (connConf, 1024);

		int hostId = NetworkTransport.AddHost (hostTopology, Port, Interface);
		print ("Now listening on " + Interface + ":" + Port);
		

	}

	// Update is called once per frame
	void Update () {
		NetworkEventType recData = NetworkTransport.Receive (out recHostId, out connectionId, out channelId, buffer, buffer.Length, out dataSize, out error);

		if (error == 0) {

			switch (recData) {
			case NetworkEventType.Nothing:
				break;
			case NetworkEventType.ConnectEvent:

				break;
			case NetworkEventType.DataEvent:
				print ("Data received");
				if (buffer.Equals (Encoding.ASCII.GetBytes (TriggerString))) {
					print ("Query request received");
				}
				break;
			case NetworkEventType.DisconnectEvent:

				break;
			}
		}
	}
}
