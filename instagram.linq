<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	var access_token = "2918794647.1677ed0.c3a86b3694834cad93daf68b779eadd0";//http://instagram.pixelunion.net/
	var userid = "1480055471";//http://www.otzberg.net/iguserid/
	var next_url = string.Empty;
	using (var wc = new WebClient())
	{
		var s = wc.DownloadString($@"https://api.instagram.com/v1/users/{userid}/media/recent?access_token={access_token}");
		var instagramJson = JsonConvert.DeserializeObject<InstagramJson>(s);
		foreach (var data in instagramJson.data)
		{
			Util.Image(data.images.standard_resolution.url).Dump();
		}
    }
}

public class Pagination
{
	public string next_url { get; set; }
	public string next_max_id { get; set; }
}

public class Meta
{
	public int code { get; set; }
}

public class Comments
{
	public int count { get; set; }
	public List<object> data { get; set; }
}

public class Datum2
{
	public string username { get; set; }
	public string profile_picture { get; set; }
	public string id { get; set; }
    public string full_name { get; set; }
}

public class Likes
{
    public int count { get; set; }
    public List<Datum2> data { get; set; }
}

public class LowResolution
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Thumbnail
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class StandardResolution
{
    public string url { get; set; }
    public int width { get; set; }
    public int height { get; set; }
}

public class Images
{
    public LowResolution low_resolution { get; set; }
    public Thumbnail thumbnail { get; set; }
    public StandardResolution standard_resolution { get; set; }
}

public class From
{
    public string username { get; set; }
    public string profile_picture { get; set; }
    public string id { get; set; }
    public string full_name { get; set; }
}

public class Caption
{
    public string created_time { get; set; }
    public string text { get; set; }
    public From from { get; set; }
    public string id { get; set; }
}

public class User
{
    public string username { get; set; }
    public string profile_picture { get; set; }
    public string id { get; set; }
    public string full_name { get; set; }
}

public class Datum
{
    public object attribution { get; set; }
    public List<string> tags { get; set; }
    public string type { get; set; }
    public object location { get; set; }
    public Comments comments { get; set; }
    public string filter { get; set; }
    public string created_time { get; set; }
    public string link { get; set; }
    public Likes likes { get; set; }
    public Images images { get; set; }
    public List<object> users_in_photo { get; set; }
	public Caption caption { get; set; }
	public bool user_has_liked { get; set; }
	public string id { get; set; }
	public User user { get; set; }
}

public class InstagramJson
{
	public Pagination pagination { get; set; }
	public Meta meta { get; set; }
	public List<Datum> data { get; set; }
}