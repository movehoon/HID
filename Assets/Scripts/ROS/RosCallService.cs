public class RosCallService {
	public string op { get; set; }
	public string service { get; set; }
	public RosEvent args { get; set; }
}

public class RosEvent {
	public string event_name { get; set; }
	public string @event { get; set; }
	public string data { get; set; }
	public string by { get; set; }
}
