using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UcrManager {

	public struct UcrData{
		public byte type;
		public byte id;
		public int data;
	};

	List<UcrData> _data = new List<UcrData> ();

	public int Count () {
		return _data.Count;
	}

	public UcrData Pop () {
		UcrData ucrData = new UcrData ();
		if (_data.Count > 0) {
			ucrData = _data [0];
			_data.RemoveAt(0);
		}
		return ucrData;
	}

	private static List<byte> _recvBuffer = new List<byte>();
	private static bool _isReceivedMessage;
	public void Decode (byte[] bytes, int length)
	{
		for (int i = 0 ; i < length ; i++) {
			_recvBuffer.Add (bytes[i]);
		}

		while (_recvBuffer.Count >= 7) {
			if (_FindHeader (0xaa)) {
				if (_recvBuffer.Count >= 2) {
					if (_recvBuffer [1] + 2 <= _recvBuffer.Count) {
						_CheckUcrMessage ();
					}
				}
			}
		}
	}
	private bool _FindHeader(byte header)
	{
		bool foundHeader = false;
		while (_recvBuffer.Count > 1)
		{
			if (_recvBuffer[0] == header)
			{
				foundHeader = true;
				break;
			}
			_recvBuffer.RemoveAt(0);
		}
		return foundHeader;
	}
	private void _CheckUcrMessage()
	{
		if (_recvBuffer[0] == 0xaa)
		{
			byte checksum = 0;
			for (int i = 0; i < _recvBuffer[1]; i++)
			{
				checksum += _recvBuffer[i + 2];
			}
			if (checksum == 0)
			{
				UcrData ucrData;
				ucrData.type = _recvBuffer[2];
				ucrData.id = _recvBuffer[3];
				if (_recvBuffer[1] == 4)
					ucrData.data = _recvBuffer[4] << 8;
				else
					ucrData.data = _recvBuffer[5] << 8 | _recvBuffer[4];
				_data.Add(ucrData);
			}
		}
		_recvBuffer.RemoveRange(0, _recvBuffer[1] + 2);
	}
}
