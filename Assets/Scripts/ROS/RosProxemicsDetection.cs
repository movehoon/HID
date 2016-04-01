public class RosProxemicsDetection {
	public bool person_entered_zone { get; set; }
}

public class RosProxemicsDetectionData {
	public bool person_entered_zone { get; set; }
	public int[] face_id { get; set; }
	public int[] zone_id { get; set; }
	public int count { get; set; }
}
