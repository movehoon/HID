public class RosSoundLocalization {
	public bool loud_sound_detected { get; set; }
}

public class RosSoundLocalizationData {
	public string direction_lr { get; set; }
	public string direction_fb { get; set; }
	public float[] transform { get; set; }
	public string frame_id { get; set; }
}
