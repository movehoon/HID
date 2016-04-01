public class RosPresenceDetection {
	public bool person_approached { get; set; }
}

public class RosPresenceDetectionData {
	public bool person_approached { get; set; }
	public bool person_left { get; set; }
	public int[] face_id { get; set; }
	public int count { get; set; }
}
