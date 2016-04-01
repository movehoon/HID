public class RosTopicPub {
	public string op { get; set; }
	public string topic { get; set; }
	public RosRaiseEvents msg { get; set; }
}

public class RosRaiseEvents {
	public string recognized_word { get; set; }
	public string[] events { get; set; }
}