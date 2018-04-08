<Query Kind="Program">
  <Connection>
    <ID>ed6a75e9-d88d-41e5-bfce-1ff022af9c0b</ID>
    <Persist>true</Persist>
    <Server>PEM5203</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAzFeMuCRzxUWqnnAykDXR8QAAAAACAAAAAAADZgAAwAAAABAAAAB+wnwuJ7dqP5ShCEmAgALkAAAAAASAAACgAAAAEAAAAA/1Z/3WQSUN866huypxLkgIAAAA2vjb+cE6CK0UAAAA6agwlj0XKZw5NNR+jIgKL3M3G5E=</Password>
    <Database>TW_ISSUED</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
//	Util.OnDemand("test1", () => new Foo(){ Name = "A", ID = 0 }).Dump();
//	
//	Util.OnDemand("test2", () => 
//	{
//		Thread.Sleep(2000);
//		return new Foo(){ Name = "A", ID = 0 };
//	}).Dump();
//	
//	new Hyperlinq(@"D:\test.txt", @"Text file named test in D:\").Dump();
//	new Hyperlinq(@"www.yahoo.com.tw", @"Yahoo homepage").Dump();
//	new Hyperlinq(() => (new Foo(){ Name = "A", ID = 0 }).Dump(), "MyFooObject").Dump();
//	new Hyperlinq(QueryLanguage.SQL, "SELECT TOP 1 PN FROM PatentPATN", "Create and run the query").Dump();
//	
//	var pb = new Util.ProgressBar("download movie").Dump();
//	pb.Fraction = 0.5;
//	Thread.Sleep(1000);
//	pb.Percent = 80;
//	Thread.Sleep(1000);
//	pb.Visible = false;
//	
//	Util.HorizontalRun(true, new string[] { "A", "B", "C" }).Dump();
//	Util.HorizontalRun("First", new List<Foo>(){ new Foo(){ Name = "A", ID = 0 }, new Foo(){ Name = "B", ID = 1 }, new Foo(){ Name = "C", ID = 2 } }).Dump("List of Foo");
//	Util.VerticalRun(new string[] { "A", "B", "C" }).Dump();
	
//	var strArr = new string[] { "A", "B", "C" };
//	Util.Highlight(strArr).Dump();
//	Util.HighlightIf(strArr, x => x.Contains("B")).Dump();
//	strArr.Select(x => x != "B" ? Util.Highlight(x) : x).Dump();
//	
//	strArr.Select(x => x != "B" ? Util.Metatext(x) : x).Dump();
//	
//	var html = "<div style='color:red'>test</div>";
//	Util.RawHtml(XElement.Parse(html)).Dump();
//	Util.RawHtml(html).Dump();

//	Util.DisplayWebPage(@"https://html5test.com/", "test browser score in html5");
//	Util.Image(@"https://i-msdn.sec.s-msft.com/dynimg/IC591786.gif").Dump();
	
//	Util.GetPassword("lttest").Dump();
//	Util.SetPassword("lttest", "24634264");
//	Util.GetPassword("lttest").Dump();

//Util.ClearResults();
//Util.ReadLine().Dump();

//	Util.Cmd("dir");
//	Util.Cmd("mkdir TEST1");
//	Util.Cmd("dir");

//	Util.GetMyQueries().Dump(1);

//	var token = Util.GetQueryLifeExtensionToken();
//	token.Dispose();
//	using (var token = Util.GetQueryLifeExtensionToken())
//	{
//		
//	}
	
//	Util.Run(@"C:\Users\Howard.Liu\Documents\LINQPad Queries\test.linq", QueryResultFormat.Html).Dump();
}
public class Foo
{
	public string Name { get; set; }
	public int ID { get; set; }
}