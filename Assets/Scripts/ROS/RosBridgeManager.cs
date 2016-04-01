using UnityEngine;
using System.Collections;
using LitJson;

public class RosBridgeManager : MonoBehaviour {

	public Program Program;

	public void FaceDetection (bool detected) {
		RosFaceDetected @event = new RosFaceDetected ();
		@event.face_detected = detected;

		RosFaceDetection data = new RosFaceDetection ();
		data.face_detected = detected;
		data.face_disappeared = !detected;
		data.face_id = new int[]{0};
		data.count = 1;

		WriteData ("face_detection", JsonMapper.ToJson(@event), JsonMapper.ToJson(data));
	}

	public void PersonEngagement (bool engaged) {
		RosEngagementDetection @event = new RosEngagementDetection ();
		@event.person_engaged = engaged;

		RosEngagementDetectionData data = new RosEngagementDetectionData ();
		data.person_engaged = engaged;
		data.person_disengaged = !engaged;
		data.face_id = new int[]{0};
		data.count = 1;

		WriteData ("engagement_detection", JsonMapper.ToJson(@event), JsonMapper.ToJson(data));
	}

	public void PersonEnteredZone (int zone) {
		RosProxemicsDetection @event = new RosProxemicsDetection ();
		@event.person_entered_zone = true;

		RosProxemicsDetectionData data = new RosProxemicsDetectionData ();
		data.person_entered_zone = true;
		data.face_id = new int[] {0};
		data.zone_id = new int[] { zone };
		data.count = 1;

		WriteData ("proxemics_detection", JsonMapper.ToJson(@event), JsonMapper.ToJson(data));
	}

	public void PresenceDetection (bool approached) {
		RosPresenceDetection @event = new RosPresenceDetection ();
		@event.person_approached = approached;

		RosPresenceDetectionData data = new RosPresenceDetectionData ();
		data.person_approached = approached;
		data.person_left = !approached;
		data.face_id = new int[]{0};
		data.count = 1;

		WriteData ("presence_detection", JsonMapper.ToJson(@event), JsonMapper.ToJson(data));
	}

	public void SoundLocalization (bool left) {
		RosSoundLocalization @event = new RosSoundLocalization ();
		@event.loud_sound_detected = true;

		RosSoundLocalizationData data = new RosSoundLocalizationData ();
		data.direction_lr = left? "left" : "right";
		data.direction_fb = "front";
		data.transform = new float[]{0f, 0f, 0f};
		data.frame_id = "1";

		WriteData ("sound_localization", JsonMapper.ToJson(@event), JsonMapper.ToJson(data));
	}

	public void ProspectRecognized (bool prospect) {
		RosProspectRecognized @event = new RosProspectRecognized ();
		@event.recognized = true;

		RosProspectRecognizedData data = new RosProspectRecognizedData ();
		data.prospect = prospect ? "positive" : "negative";

		WriteData ("prospect_recognized", JsonMapper.ToJson(@event), JsonMapper.ToJson(data));
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
