<Query Kind="Program">
  <NuGetReference>Dapper</NuGetReference>
  <Namespace>Dapper</Namespace>
</Query>

void Main()
{
	var query = "";
	var filter = new string[]
	{
		"Scenario",
	};
	var severNameArr = new string[]
	{
		"PEM5201",
		"PEM5202",
		"PEM5203",
		"PEM5204",
		"PEM5205",
		"PEM5206",
		//"PEM5207",//VM
		//"PEM5208",//VM
		"PEM5209",
		"PEM5210",
		"PEM5211",
		"PEM5212",
		"PEM5213",
		"PEM5214",
		//"PEM52015",
		//"PEM52016",//VM

		@"PER7101\SQL2012",
		@"PER7101\SQL2014",
		//"PER7102",
		"PER7103",
		"PER7104",
		"PER7105",

		//"PER7201",
		//"PER7202",//VM
		"PER7203",
		"PER7204",
		"PER7205",
	};
	var selectDBNames = "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model','msdb')";
	var connectionString = "Data Source={0};Initial Catalog=master;Persist Security Info=True;User ID={1};pwd={2};";
	
	var dbsInEachServer = new List<ServerInfo>();
	foreach (var sn in severNameArr)
	{
		try
		{
			using (var cn = new SqlConnection(string.Format(connectionString, sn, "sa", "happy99")))
			{
				dbsInEachServer.Add(new ServerInfo()
				{
					ServerName = sn,
					DbList = cn.Query<string>(selectDBNames)
				});
			}
		}
		catch (Exception e1)
		{
			if (e1.HResult != -2146232060) continue;
			try
				{
					using (var cn = new SqlConnection(string.Format(connectionString, sn, "webpatx", "sdclalopbawidnhpelomf4200")))
					{
						dbsInEachServer.Add(new ServerInfo()
						{
							ServerName = sn,
							DbList = cn.Query<string>(selectDBNames)
						});
					}
				}
				catch (Exception e2)
				{
					if (e2.HResult != -2146232060) continue;
					try
					{
						using (var cn = new SqlConnection(string.Format(connectionString, sn, "webpat_user", "webpat_user")))
						{
							dbsInEachServer.Add(new ServerInfo()
							{
								ServerName = sn,
								DbList = cn.Query<string>(selectDBNames)
							});
						}
					}
					catch (Exception e3)
					{
						if (e3.HResult != -2146232060) continue;
						dbsInEachServer.Add(new ServerInfo()
						{
							ServerName = sn,
							DbList = null
						});
					}
				}
		}
	}
	foreach (var serverInfo in dbsInEachServer)
	{
		serverInfo.DbList = serverInfo.DbList.Where(x => x.Contains(query));
	}
	Util.HorizontalRun(true, dbsInEachServer).Dump();
}

public class ServerInfo
{
	public string ServerName { get; set; }
	public IEnumerable<string> DbList { get; set; }
}