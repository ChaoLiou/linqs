<Query Kind="Program">
  <Connection>
    <ID>bf173835-64b5-46c8-8ead-be213b9c3fad</ID>
    <Persist>true</Persist>
    <Server>per7204</Server>
    <SqlSecurity>true</SqlSecurity>
    <NoPluralization>true</NoPluralization>
    <NoCapitalization>true</NoCapitalization>
    <ExcludeRoutines>true</ExcludeRoutines>
    <UserName>sa</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAy9CcIRwFMkeDZ/T5kehZ8gAAAAACAAAAAAADZgAAwAAAABAAAABDljVXlJSnNC8U6QodTMjOAAAAAASAAACgAAAAEAAAAJ4QQv9MugO9yu26rylPJyEIAAAAdr8Uwkr5YFsUAAAA/L0TAhgNfCXlkpMLa6f2jJ6JaGA=</Password>
    <Database>_PatentEP</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Namespace>System.ComponentModel</Namespace>
</Query>

void Main()
{
	var list = new List<test_HL>();
	list.Add(new test_HL(){sn = "A"});
	list.Add(new test_HL(){sn = "B"});
	list.Add(new test_HL(){sn = "CCC"});
	list.Add(new test_HL(){sn = "D"});
	list.Add(new test_HL(){sn = "E"});
	bulkInsert<test_HL>("Data Source=PER7204;Initial Catalog=_PatentEP;Persist Security Info=True;User ID=sa;pwd=happy99;", "test_HL", list);
}
public class test_HL
{
	public string sn { get; set; }
}

private static void bulkInsert<T>(string connection, string tableName, IList<T> list)
{
	using (var bulkCopy = new SqlBulkCopy(connection))
	{
		bulkCopy.BatchSize = list.Count;
		bulkCopy.DestinationTableName = tableName;

		var table = new DataTable();
		var props = TypeDescriptor.GetProperties(typeof(T))
								   //Dirty hack to make sure we only have system data types 
								   //i.e. filter out the relationships/collections
								   .Cast<PropertyDescriptor>()
								   .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
								   .ToArray();

		foreach (var propertyInfo in props)
		{
			bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
			table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
		}

		var values = new object[props.Length];
		foreach (var item in list)
		{
			for (var i = 0; i < values.Length; i++)
			{
				values[i] = props[i].GetValue(item);
			}

			table.Rows.Add(values);
		}

		bulkCopy.WriteToServer(table);
	}
}