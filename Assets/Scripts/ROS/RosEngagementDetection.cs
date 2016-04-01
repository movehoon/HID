public class RosEngagementDetection {
	public bool person_engaged { get; set; }
}

public class RosEngagementDetectionData {
	public bool person_engaged { get; set; }
	public bool person_disengaged { get; set; }
	public int[] face_id { get; set; }
	public int count { get; set; }
}
