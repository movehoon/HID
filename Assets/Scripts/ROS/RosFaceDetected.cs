public class RosFaceDetected {
	public bool face_detected { get; set; }
}

public class RosFaceDetection {
	public bool face_detected { get; set; }
	public bool face_disappeared { get; set; }
	public int[] face_id { get; set; }
	public int count { get; set; }
}
