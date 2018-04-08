<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	process("biblio");
	//process("text");
}

private static void process(string rootDirectoryName)
{
	var ftpClient = new FTP(@"ftp://wwwftp.wipo.int.", "learningtec", "cg.pd377hch");
	var directoryTypeFTPItemInfoList = ftpClient.Listing(rootDirectoryName).OrderByDescending(x => x.Name).Dump();
	var latestDirectoryTypeFTPItemInfo = directoryTypeFTPItemInfoList.FirstOrDefault();
	var fileTypeFTPItemInfoList = ftpClient.Listing(latestDirectoryTypeFTPItemInfo.FullPath).Where(x => x.Name == "index.lst").ToList().Dump();
	var sourceRootPath = @"\\per7204\WIPOData\WIPOFTP\";
	var _retryMaxTimes = 3;
	fileTypeFTPItemInfoList.ForEach(x =>
	{
		var retryTimes = 0;
		var destionationPath = Path.Combine(sourceRootPath, DateTime.Now.Year.ToString(), x.FullPath).Replace("/", "\\");
		while (!ftpClient.Download(x.FullPath, destionationPath))
		{
			if (++retryTimes > _retryMaxTimes)
			{
				break;
			}
		}
	});
}

public class FTP
{
	private string _host = null;
	private string _user = null;
	private string _pass = null;
	private int _bufferSize = 2048;

	public FTP(string hostIP, string userName, string password) { _host = hostIP; _user = userName; _pass = password; }

	public bool Download(string remoteFilePath, string localFilePath)
	{
		var success = false;
		try
		{
			checkNCreateDirectory(localFilePath);
			var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + remoteFilePath);
			ftpRequest.Credentials = new NetworkCredential(_user, _pass);
			ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
			using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
			using (var ftpStream = ftpResponse.GetResponseStream())
			using (var localFileStream = new FileStream(localFilePath, FileMode.Create))
			{
				var byteBuffer = new byte[_bufferSize];
				var bytesRead = ftpStream.Read(byteBuffer, 0, _bufferSize);
				while (bytesRead > 0)
				{
					localFileStream.Write(byteBuffer, 0, bytesRead);
					bytesRead = ftpStream.Read(byteBuffer, 0, _bufferSize);
				}
				success = true;
			}
		}
		catch (Exception e)
		{
			
		}
		return success;
	}

	public List<FTPItemInfo> Listing(string directory)
	{
		var list = new List<FTPItemInfo>();
		try
		{
			var ftpRequest = (FtpWebRequest)FtpWebRequest.Create(_host + "/" + directory);
			ftpRequest.Credentials = new NetworkCredential(_user, _pass);
			ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
			using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
			using (var ftpStream = ftpResponse.GetResponseStream())
			using (var ftpReader = new StreamReader(ftpStream))
			{
				while (ftpReader.Peek() != -1)
				{
					list.Add(new FTPItemInfo(directory, ftpReader.ReadLine()));
				}
			}
		}
		catch (Exception e) 
		{ 
		}
		return list;
	}

	private static void checkNCreateDirectory(string path)
	{
		var isUNCPath = path.StartsWith(@"\\");
		var currentPath = isUNCPath ? Path.GetPathRoot(path).Substring(0, Path.GetPathRoot(path).LastIndexOf("\\")) : Path.GetPathRoot(path);
		foreach (var directoryName in path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Where(x => !x.Contains(".")))
		{
			currentPath = Path.Combine(currentPath, directoryName).Dump();
			if (!Directory.Exists(currentPath))
			{
				Directory.CreateDirectory(currentPath);
			}
		}
	}
}


public class FTPItemInfo
{
	public string Mode { get; set; }
	public int Links { get; set; }
	public string Owner { get; set; }
	public string Group { get; set; }
	public string Size { get; set; }
	public string CreatedDate { get; set; }
	public string Name { get; set; }
	public string FullPath { get; set; }

	public FTPItemInfo(string directoryPath, string responseText)
	{
		var pattern = @"^(?<dir>[\-ld])(?<permission>([\-r][\-w][\-xs]){3})\s+(?<filecode>\d+)\s+(?<owner>\w+)\s+(?<group>\w+)\s+(?<size>\d+)\s+(?<timestamp>((?<month>\w{3})\s+(?<day>\d{2})\s+(?<hour>\d{1,2}):(?<minute>\d{2}))|((?<month>\w{3})\s+(?<day>\d{2})\s+(?<year>\d{4})))\s+(?<name>.+)$";
		var m = Regex.Match(responseText, pattern);
		if (m.Success)
		{
			Mode = m.Groups["permission"].Value;
			Links = int.Parse(m.Groups["filecode"].Value);
			Owner = m.Groups["owner"].Value;
			Group = m.Groups["group"].Value;
			Size = bytesToSize(long.Parse(m.Groups["size"].Value));
			CreatedDate = m.Groups["timestamp"].Value;
			Name = m.Groups["name"].Value;
			FullPath = $"{directoryPath}/{m.Groups["name"].Value}";
		}
	}

	private static string bytesToSize(double bytesLength)
	{
		var sizes = new string[] { "B", "KB", "MB", "GB" };
		var order = 0;
		while (bytesLength >= 1024 && order + 1 < sizes.Length)
		{
			order++;
			bytesLength = bytesLength / 1024;
		}
		return string.Format("{0:0.##} {1}", bytesLength, sizes[order]);
	}

	private static string bytesToSize(long bytesLength)
	{
		var sizes = new string[] { "B", "KB", "MB", "GB" };
		var order = 0;
		while (bytesLength >= 1024 && order + 1 < sizes.Length)
		{
			order++;
			bytesLength = bytesLength / 1024;
		}
		return string.Format("{0:0.##} {1}", bytesLength, sizes[order]);
	}
}