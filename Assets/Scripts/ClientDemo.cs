using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ClientDemo
{
	private TcpClient _client;
	
	private StreamReader _sReader;
	private StreamWriter _sWriter;
	
	private bool _isConnected;
	
	public ClientDemo(string ipAddress, int portNum)
	{
		_client = new TcpClient();
		_client.Connect(ipAddress, portNum);
		
		HandleCommunication();
	}
	
	public void HandleCommunication()
	{
		_sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
		_sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
		
		_isConnected = true;
		string sData = null;
		while (_isConnected)
		{
			Debug.Log ("&gt; ");
			sData = "hello world";
			
			// write data and make sure to flush, or the buffer will continue to 
			// grow, and your data might not be sent when you want it, and will
			// only be sent once the buffer is filled.
			_sWriter.WriteLine(sData);
			_sWriter.Flush();
			
			// if you want to receive anything
			// String sDataIncomming = _sReader.ReadLine();
		}
	}
}