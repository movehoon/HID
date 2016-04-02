using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class RosBridgeManager : MonoBehaviour {

	public Program Program;
	public InputField PeopleCount;
	public Dropdown Gender;
	public Dropdown ClothColor;
	public Toggle Eyeglass;
	public Dropdown HairStyle;

	public void FaceDetection (bool detected) {
		RosFaceDetection data = new RosFaceDetection ();
		data.face_detected = detected;
		data.face_disappeared = !detected;
		data.face_id = new int[]{0};
		data.count = 1;

		if (detected)
			WriteData ("face_detection", @"{""face_detected"": true}", JsonMapper.ToJson(data));
		else
			WriteData ("face_detection", @"{""face_disappeared"": true}", JsonMapper.ToJson(data));
	}

	public void PersonEngagement (bool engaged) {
		RosEngagementDetection data = new RosEngagementDetection ();
		data.person_engaged = engaged;
		data.person_disengaged = !engaged;
		data.face_id = new int[]{0};
		data.count = 1;

		if (engaged)
			WriteData ("engagement_detection", @"{""person_engaged"": true}", JsonMapper.ToJson(data));
		else
			WriteData ("engagement_detection", @"{""person_disengaged"": true}", JsonMapper.ToJson(data));
	}

	public void PersonEnteredZone (int zone) {
		RosProxemicsDetection data = new RosProxemicsDetection ();
		data.person_entered_zone = true;
		data.face_id = new int[] {0};
		data.zone_id = new int[] { zone };
		data.count = 1;

		WriteData ("proxemics_detection", @"{""person_entered_zone"": true}", JsonMapper.ToJson(data));
	}

	public void PresenceDetection (bool approached) {
		RosPresenceDetection data = new RosPresenceDetection ();
		data.person_approached = approached;
		data.person_left = !approached;
		data.face_id = new List<int> ();
		data.face_id.Add (1);
		data.count = 1;

		if (approached)
			WriteData ("presence_detection", @"{""person_approached"": true}", JsonMapper.ToJson(data));
		else
			WriteData ("presence_detection", @"{""person_left"": true}", JsonMapper.ToJson(data));
	}

	public void SoundLocalization (bool left) {
		RosSoundLocalization data = new RosSoundLocalization ();
		data.loud_sound_detected = true;
		data.direction_lr = left? "left" : "right";
		data.direction_fb = "front";
//		data.transform = new float[]{0f, 0f, 0f};
		data.frame_id = "1";

		WriteData ("sound_localization", @"{""loud_sound_detected"": true}", JsonMapper.ToJson(data));
	}
	public void PointingGestureRecognized (int gestureID) {
		RosGestureRecognition data = new RosGestureRecognition ();
//		data.recognized = true;
//		data.name = new string[] {"unknown"};
//		data.confidence = new float[] {1f};
//		data.height = new int[] {100};
		data.type = new List<int> ();
		data.type.Add (gestureID);
//		data.description = new string[] {"poiinting gesture"};
//		data.transform = new float[] (0f, 0f, 0f);
		data.frame_id = "1";

		WriteData ("gesture_recognized", @"{""gesture_recognized"": true}", JsonMapper.ToJson(data));
	}
	public void ProspectRecognized (bool prospect) {
		RosProspectRecognized data = new RosProspectRecognized ();
		data.recognized = true;
		data.prospect = prospect ? "positive" : "negative";

		WriteData ("prospect_recognized", @"{""prospect_recognized"": true}", JsonMapper.ToJson(data));
	}
	public void PeopleDetected () {
		RosPersonRecognition data = new RosPersonRecognition ();
		data.recognized = true;
		data.count = PeopleCount.text;

		WriteData ("person_recognized", @"{""people_detected"": true}", JsonMapper.ToJson(data));
	}
	public void PersonIdentification () {
		RosPersonIdentification data = new RosPersonIdentification ();
		data.person_identified = true;
		data.gender = new string[] {Gender.value.ToString ()};
		data.cloth_color = new string[] {ClothColor.captionText.text};
		data.eyeglasses = new bool[] {Eyeglass.isOn};
		data.hair_style = new string[] {HairStyle.captionText.text};

		WriteData ("person_identification", @"{""person_identified"": true}", JsonMapper.ToJson(data));
	}
	public void IntensionAttension (bool attension) {
		RosProspectRecognized data = new RosProspectRecognized ();
//		data.recognized = true;
//		data.prospect = attension ? "positive" : "negative";
		if (attension)
			WriteData ("prospect_recognized", @"{""intension_attension"": true}", JsonMapper.ToJson(data));
		else
			WriteData ("prospect_recognized", @"{""intension_attension"": false}", JsonMapper.ToJson(data));
			}

	void WriteData (string event_name, string @event, string data) {
		RosEvent evt = new RosEvent ();
		evt.event_name = event_name;
		evt.@event = @event;
		evt.data = data;
		evt.by = "hid";

		RosCallService writeToMemory = new RosCallService ();
		writeToMemory.op = "call_service";
		writeToMemory.service = "/social_memory/write_data";
		writeToMemory.args = evt;

		string jsonString = JsonMapper.ToJson(writeToMemory);
		Program.Send (jsonString);
	}
}

public class RosFaceDetection {
	public bool face_detected { get; set; }
	public bool face_disappeared { get; set; }
	public int[] face_id { get; set; }
	public int count { get; set; }
}

public class RosEngagementDetection {
	public bool person_engaged { get; set; }
	public bool person_disengaged { get; set; }
	public int[] face_id { get; set; }
	public int count { get; set; }
}
public class RosProxemicsDetection {
	public bool person_entered_zone { get; set; }
	public int[] face_id { get; set; }
	public int[] zone_id { get; set; }
	public int count { get; set; }
}
public class RosPresenceDetection {
	public bool person_approached { get; set; }
	public bool person_left { get; set; }
	public List<int> face_id { get; set; }
	public int count { get; set; }
}
public class RosSoundLocalization {
	public bool loud_sound_detected { get; set; }
	public string direction_lr { get; set; }
	public string direction_fb { get; set; }
	public float[] transform { get; set; }
	public string frame_id { get; set; }
}
public class RosGestureRecognition {
	public bool recognized { get; set; }
	public string[] name { get; set; }
	public float[] confidence { get; set; }
	public int[] height { get; set; }
	public List<int> type { get; set; }
	public string[] description { get; set; }
	public float[] transform { get; set; }
	public string frame_id { get; set; }
}
public class RosProspectRecognized {
	public bool recognized { get; set; }
	public string prospect { get; set; }
}
public class RosPersonRecognition {
	public bool recognized { get; set; }
	public string count { get; set; }
	public string[] name { get; set; }
	public float[] confidence { get; set; }
	public int[] height { get; set; }
	public float[] transform { get; set; }
	public string frame_id { get; set; }
}
public class RosPersonIdentification {
	public bool person_identified { get; set; }
	public int[] session_face_id { get; set; }
	public int[] person_id { get; set; }
	public string[] name { get; set; }
	public string[] face_pos { get; set; }
	public float[] confidence { get; set; }
	public string[] gender { get; set; }
	public string[] cloth_color { get; set; }
	public bool[] eyeglasses { get; set; }
	public int[]  height { get; set; }
	public string[] hair_style { get; set; }
}