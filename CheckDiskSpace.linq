<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Management.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.Install.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.JScript.dll</Reference>
  <Namespace>System.Management</Namespace>
</Query>

void Main()
{

	var memberList = new List<Member>()
	{
		new Member(){ Name = "PatentJPA", Server = "PEM5211", Path = @"C:\\MSSQL\\PatentJPA\\PatentJPA.mdf", UserName = @"ltc.local\howard", Password = Util.GetPassword("ltclocal") },
		//		new Member(){ Name = "JPAImageInfo", Server = "PEM5211", Path = @"C:\\MSSQL\\PatentJPA\\JPAImageInfo.mdf", UserName = @"ltc.local\howard", Password = Util.GetPassword("ltclocal") },
		//		new Member(){ Name = "CNImageInfo", Server = "PEM5211", Path = @"C:\\MSSQL\\PatentJPA\\CNImageInfo.mdf", UserName = @"ltc.local\howard", Password = Util.GetPassword("ltclocal") },
		//		new Member(){ Name = "JPBImageInfo", Server = "PEM5206", Path = @"C:\\MSSQL\\JPB\\JPBImageInfo.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentJPB", Server = "PEM5206", Path = @"C:\\MSSQL\\JPB\\PatentJPB.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentPAJ", Server = "PEM5204", Path = @"C:\\MSSQL\\PatentPAJ\\PatentPAJ.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentEPA", Server = "PEM5204", Path = @"C:\\MSSQL\\PatentEPA\\PatentEPA.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentEPB", Server = "PEM5210", Path = @"C:\\MSSQL\\PatentEPB\\PatentEPB.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentWO",  Server = "PEM5205", Path = @"C:\\MSSQL\\PatentWO\\PatentWO.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		//		new Member(){ Name = "WOImageInfo",  Server = "PEM5205", Path = @"C:\\MSSQL\\PatentWO\\WOImageInfo.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "US_Published", Server = "PER7105", Path = @"H:\\MSSQL\\DATA\\US_Published\\US_Published.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "US_ISSUED", Server = "PER7203", Path = @"D:\\MSSQL\\US_ISSUED\\US_ISSUED.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "TW_PUBLISHED", Server = "PEM5203", Path = @"C:\\MSSQL\\DataX86\\TW_PUBLISHED.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "TW_ISSUED", Server = "PEM5203", Path = @"C:\\MSSQL\\DataX86\\TW_ISSUED.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentCNNew",  Server = "PEM5214", Path = @"D:\\MSSQL\\PatentCNNew\\PatentCNNew_01.ndf", UserName = @"ltc.local\howard", Password = Util.GetPassword("ltclocal") },
		new Member(){ Name = "PatentDOCDB",  Server = "PER7205", Path = @"D:\\MSSQL\\PatentDOCDB\\PatentDOCDB.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
		new Member(){ Name = "PatentKipris",  Server = "PEM5213", Path = @"C:\\MSSQL\\NewData\\PatentKipris.mdf", UserName = @"ltc\howard.liu", Password = Util.GetPassword("ltc") },
	};

	foreach (var member in memberList)
	{
		var connectionOptions = new ConnectionOptions();
		connectionOptions.Username = member.UserName;
		connectionOptions.Password = member.Password;
		var managementScope = new ManagementScope($@"\\{member.Server}\root\cimv2", connectionOptions);

		var objectQuery1 = new ObjectQuery($"SELECT * FROM CIM_DataFile WHERE Name = '{member.Path}'");
		var managementObjectSearcher1 = new ManagementObjectSearcher(managementScope, objectQuery1);
		var managementObjectCollection1 = managementObjectSearcher1.Get();

		var objectQuery2 = new ObjectQuery("SELECT * from Win32_LogicalDisk WHERE DriveType=3");
		var managementObjectSearcher2 = new ManagementObjectSearcher(managementScope, objectQuery2);
		var managementObjectCollection2 = managementObjectSearcher2.Get();

		var mdfSize = string.Empty;
		foreach (ManagementObject managementObject in managementObjectCollection1)
		{
			mdfSize = managementObject["FileSize"].ToString();
		}

		var resultList = new List<Result>();
		foreach (ManagementObject managementObject in managementObjectCollection2)
		{
			var pb1 = new Util.ProgressBar();
			pb1.Caption = formatSize(managementObject["FreeSpace"].ToString()) + " free of " + formatSize(managementObject["Size"].ToString())
			+ "(" + formatSize(managementObject["FreeSpace"].ToString()) + " / " + formatSize(managementObject["Size"].ToString()) + ")";
			pb1.Fraction = 1 - (double)(decimal.Parse(managementObject["FreeSpace"].ToString()) / decimal.Parse(managementObject["Size"].ToString()));

			Util.ProgressBar pb2 = null;
			if (member.Path[0] == managementObject["Name"].ToString()[0])
			{
				pb2 = new Util.ProgressBar();
				pb2.Caption = formatSize(mdfSize) + " / " + formatSize(managementObject["Size"].ToString());
				pb2.Fraction = 1 - (double)(decimal.Parse(mdfSize) / decimal.Parse(managementObject["Size"].ToString()));
			}

			resultList.Add(new Result()
			{
				Message = member.Name + "|" + member.Server + "|" + managementObject["Name"].ToString(),
				PB1 = pb1,
				PB2 = pb2
			});
		}

		Util.HorizontalRun(false, resultList).Dump();
	}
}

public class Member
{
	public string Name { get; set; }
	public string Server { get; set; }
	public string Path { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
}

public class Result
{
	public string Message { get; set; }
	public Util.ProgressBar PB1 { get; set; }
	public Util.ProgressBar PB2 { get; set; }
}

private string formatSize(string size)
{
	decimal size_int = 0;
	var count = 0;
	if (decimal.TryParse(size, out size_int))
	{
		while (size_int >= 1024)
		{
			size_int /= 1024;
			count++;
		}
	}
	var fsize = size_int.ToString();
	if (fsize.IndexOf(".") != -1 && fsize.Substring(fsize.IndexOf(".") + 1).Length > 2)
	{
		fsize = fsize.Substring(0, fsize.IndexOf(".") + 3);
	}
	switch (count)
	{
		case 0:
			fsize += " Bytes";
			break;
		case 1:
			fsize += " KB";
			break;
		case 2:
			fsize += " MB";
			break;
		case 3:
			fsize += " GB";
			break;
		case 4:
			fsize += " TB";
			break;
	}
	return fsize;
}