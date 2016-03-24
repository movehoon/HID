public class RosSpeechRecognized {
	public bool speech_recognized { get; set; }
}

public class RosSpeechData {
	public string recognized_word { get; set; }
	public double confidence { get; set; }
}
