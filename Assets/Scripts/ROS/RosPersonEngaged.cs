public class RosPersonEngaged {
	public bool person_engaged { get; set; }
}

public class RosPersonEngagedData {
	public bool face_detected { get; set; }
	public bool face_disappeared { get; set; }
	public int[] face_id { get; set; }
	public int count { get; set; }
}
