<Query Kind="Program">
  <Connection>
    <ID>d4ab6ddc-1cc0-4a58-8da2-31b2ec7c2d0d</ID>
    <Persist>true</Persist>
    <Server>per7203</Server>
    <SqlSecurity>true</SqlSecurity>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAzFeMuCRzxUWqnnAykDXR8QAAAAACAAAAAAADZgAAwAAAABAAAAAmHSJfWujX+hNGtqMt4USUAAAAAASAAACgAAAAEAAAAHVbVhw51n9kVImmo5Uu7fEIAAAA8Iem9R+5zmUUAAAAZQkTKT4hhJpAXai2C52H1zOIsak=</Password>
    <Database>US_ISSUED</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	var list = new List<Foo>()
	{
		new Foo()
		{
			Name = "A",
			Score = 5
		},
		new Foo()
		{
			Name = "B",
			Score = 5
		},
		new Foo()
		{
			Name = "C",
			Score = 3
		},
		new Foo()
		{
			Name = "D",
			Score = 10
		},	
	};
	
	Util.HorizontalRun(true,
	list.DenseRank(),
	//list.DenseRank(false),
	list.DenseRank<Foo>("Score"),
	//list.DenseRank<Foo>("Score", false),
	list.DenseRankSuper("Score")
	//list.DenseRankSuper("Score", false)
	).Dump();
}

public class Foo
{
	public string Name { get; set; }
	public int Score { get; set; }
	public int Rank { get; set; }
}

public static class DenseRanker
{
	public static IEnumerable<Foo> DenseRank(this IEnumerable<Foo> source, bool desc = true)
	{
		var i = 0;
		var last = 0;
		if (desc)
		{
			source = source.OrderByDescending(x => x.Score);
		}
		else
		{
			source = source.OrderBy(x => x.Score);
		}
		
		return source.Select(x => 
		{
			var isSameAsLastOne = x.Score != last;
			last = x.Score;
			return new Foo()
			{
				Name = x.Name,
				Score = x.Score,
				Rank = isSameAsLastOne ? ++i : i
			};
		});
	}
	
	public static IEnumerable DenseRank<T>(this IEnumerable<T> source, string propname, bool desc = true)
	{
		var i = 0;
		var last = 0;
		if (desc)
		{
			source = source.OrderByDescending(x => typeof(T).GetProperty(propname).GetValue(x));
		}
		else
		{
			source = source.OrderBy(x => typeof(T).GetProperty(propname).GetValue(x));
		}
		
		return source.Select(x => 
		{
			var isSameAsLastOne = (int)typeof(T).GetProperty(propname).GetValue(x) != last;
			last = (int)typeof(T).GetProperty(propname).GetValue(x);
			return new 
			{
				Item = x,
				Rank = isSameAsLastOne ? ++i : i
			};
		});
	}
	
	public static IEnumerable DenseRankSuper(this IEnumerable<object> source, string propname, bool desc = true)
	{
		return doDenseRank<object>(source.First().GetType(), source, propname, desc);
	}
	
	private static IEnumerable doDenseRank<T>(Type type, IEnumerable<T> source, string propname, bool desc = true)
	{
		var i = 0;
		var last = 0;
		if (desc)
		{
			source = source.OrderByDescending(x => type.GetProperty(propname).GetValue(x));
		}
		else
		{
			source = source.OrderBy(x => type.GetProperty(propname).GetValue(x));
		}
		
		return source.Select(x => 
		{
			var isSameAsLastOne = (int)type.GetProperty(propname).GetValue(x) != last;
			last = (int)type.GetProperty(propname).GetValue(x);
			return new 
			{
				Item = x,
				Rank = isSameAsLastOne ? ++i : i
			};
		});
	}
}