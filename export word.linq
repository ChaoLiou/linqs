<Query Kind="Program">
  <Connection>
    <ID>6cb2cfb4-898b-41ea-8440-8d053d4b5117</ID>
    <Persist>true</Persist>
    <Server>per7103</Server>
    <SqlSecurity>true</SqlSecurity>
    <Database>LT_EPS_HiwinDEV</Database>
    <UserName>epsasp</UserName>
    <Password>AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAFWmeR2tRMUyuY9cPf3OWuAAAAAACAAAAAAADZgAAwAAAABAAAABoxM8R3rIdmh/IcbAtz020AAAAAASAAACgAAAAEAAAABc5YBs7dJPDm5yflGxkejUQAAAAbjQ6Nk7ob4tF/YS30SQ4CBQAAAAbQ1FFSnuOyENNl7VPcq8suzseSA==</Password>
  </Connection>
  <Reference>&lt;RuntimeDirectory&gt;\WPF\WindowsBase.dll</Reference>
  <NuGetReference>openxml.sdk.25</NuGetReference>
  <Namespace>DocumentFormat.OpenXml</Namespace>
  <Namespace>DocumentFormat.OpenXml.Packaging</Namespace>
  <Namespace>DocumentFormat.OpenXml.Wordprocessing</Namespace>
</Query>

void Main()
{
	var basic = this.BASICs.Single(x => x.SN == 3685);
	string document = @"C:\Users\howard.liu\Desktop\DT001.docx";
	string fileName = @"C:\Users\howard.liu\Desktop\qwe.png";
	InsertAPicture(document, fileName, basic);
}

public void InsertAPicture(string document, string fileName, BASIC basic)
{
		byte[] byteArray = File.ReadAllBytes(document);
		using (MemoryStream stream = new MemoryStream())
		{
			stream.Write(byteArray, 0, (int)byteArray.Length);
			
			using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(stream, true))
			{
				ModifyWord(wordDoc, basic);				
			}
			
			File.WriteAllBytes(@"D:\DT001.doc", stream.ToArray()); 
		}
}

public enum InventorDataType
{
	CN_Name,
	EN_Name,
	ID
}

public enum CustomizeFieldType
{
	///<summary>本案專案編號</summary>
	CurrentProjectCode = 11,
	///<summary>創作目的</summary>
	CreationPurpose = 21,
	///<summary>公司效益</summary>
	CompanyBenefit = 22,
	///<summary>檢索前案之條件</summary>
	ConditionsOfPriorArtSearching = 23,
	///<summary>檢索到的相關前案號碼</summary>
	NumbersOfPriorArtSearching = 24,
	///<summary>習知技術-現有技術的作法</summary>
	PriorArtMethod = 25,
	///<summary>習知技術-現有技術的缺點</summary>
	PriorArtWeakPoint = 26,
	///<summary>習知技術圖式</summary>
	PriorArtPicture,
	///<summary>專利之技術特徵-本創作特徵</summary>
	CreationFeature = 27,
	///<summary>專利之技術特徵-代表圖</summary>
	CreationFeaturePictures,
	///<summary>創作原理</summary>
	CreationPrinciple = 28,
	///<summary>元件符號說明</summary>
	ComponentSymbolDescription = 29,
	///<summary>備註</summary>
	Memo = 30
}
private string getInventorData(InventorDataType inventorDataType, BASIC basic)
{
	var resultList = new List<string>();
	foreach (var i_sn in basic.JOIN_InvsInBasicData.OrderBy(x => x.Sequence).Select(x => x.I_SN))
	{
		var inventor = this.INVENTORs.Single(x => x.SN == i_sn);
		switch (inventorDataType)
		{
			case InventorDataType.CN_Name:
				resultList.Add(inventor.CN_Name);
				break;
			case InventorDataType.EN_Name:
				resultList.Add(inventor.EN_Name);
				break;
			case InventorDataType.ID:
				resultList.Add(inventor.ID);
				break;
		}
	}
	return string.Join("; ", resultList);
}

private string getCustomizeFieldData(CustomizeFieldType customizeFieldType, BASIC basic)
{
	var cfv = this.CustomizeFieldValues.Where(x => x.B_SN == basic.SN);
	switch (customizeFieldType)
	{
		case CustomizeFieldType.CurrentProjectCode:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.CurrentProjectCode).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.CreationPurpose:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.CreationPurpose).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.CompanyBenefit:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.CompanyBenefit).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.ConditionsOfPriorArtSearching:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.ConditionsOfPriorArtSearching).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.NumbersOfPriorArtSearching:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.NumbersOfPriorArtSearching).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.PriorArtMethod:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.PriorArtMethod).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.PriorArtWeakPoint:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.PriorArtWeakPoint).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.CreationFeature:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.CreationFeature).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.CreationPrinciple:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.CreationPrinciple).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.ComponentSymbolDescription:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.ComponentSymbolDescription).Select(x => x.FieldValue).SingleOrDefault();
		case CustomizeFieldType.Memo:
			return cfv.Where(x => x.FieldSN == (int)CustomizeFieldType.Memo).Select(x => x.FieldValue).SingleOrDefault();
		default:
			return string.Empty;
	}
}

public void ModifyWord(WordprocessingDocument wordDoc, BASIC basic)
{
	var table = wordDoc.MainDocumentPart.Document.Body.Elements<Table>().ToArray();
	
	//提案編號：1, 0 智權收案日：1, 1 上銀編號：1, 2
	InsertData(1, 0, basic.Proposal_SN, table[0]);
	InsertData(1, 1, basic.Propose_Case.ToString(), table[0]);
	InsertData(1, 2, basic.HiwinNumber, table[0]);
	//創作名稱	中文				 2, 2
	//			英文(請務必填寫)	3, 2
	InsertData(2, 2, basic.CN_Name, table[0]);
	InsertData(3, 2, basic.EN_Name, table[0]);
	//創作人	 中文				  			4, 2
	//		    英文(請務必填寫)			   5, 2
	//員工編號	請依創作人順序填寫(請務必填寫)：6, 1
	InsertData(4, 2, getInventorData(InventorDataType.CN_Name, basic), table[0]);
	InsertData(5, 2, getInventorData(InventorDataType.EN_Name, basic), table[0]);
	InsertData(6, 1, getInventorData(InventorDataType.ID, basic), table[0]);
	
	//補做分類 ClassDefXml BasicToClassDefXml
	
	//本案專案編號：5, 0
	//創作目的：
	//	7, 0
	//公司效益：（例如每年或總共可為公司增加多少營業額或利潤，評估之理由為何；本創作對於競爭對手的產品排他性為何；以上兩點請務必填寫）
	//	9, 0
	//檢索前案之條件：10, 0
	//檢索到的相關前案號碼：11, 0
	InsertData(5, 0, getCustomizeFieldData(CustomizeFieldType.CurrentProjectCode, basic), table[1]);
	InsertData(7, 0, getCustomizeFieldData(CustomizeFieldType.CreationPurpose, basic), table[1]);
	InsertData(9, 0, getCustomizeFieldData(CustomizeFieldType.CompanyBenefit, basic), table[1]);
	InsertData(10, 0, getCustomizeFieldData(CustomizeFieldType.ConditionsOfPriorArtSearching, basic), table[1]);
	InsertData(11, 0, getCustomizeFieldData(CustomizeFieldType.NumbersOfPriorArtSearching, basic), table[1]);
	//習知技術：（請描述現今的做法；請列出和本案最像的專利前案號碼，並強調目前之方法有哪些缺點或不方便？而本案即是為了改良其缺點，可用草圖輔助說明。）
	//現有技術的作法	現有技術的缺點
	//	2, 0			2, 1
	//習知技術圖式：
	//	4, 0
	//專利之技術特徵：（包含了這些特徵就會和所有的先前技術不同；此部分請寫出與前案不同之處即可最好附上圖示比對說明，以利於審查會向委員說明）
	//本創作特徵	代表圖(可看出本案主要特徵之圖式)
	//	7, 0		7, 1	
	//創作原理：（請詳細敘述本創作研發過程中所應用或使用之理論根據，例如:定理、公式、實測數據、圖表…等，本創作是使用什麼自然法則？）
	//	9, 0
	//元件符號說明：（請說明本創作實際應用之功能及作業方式，如何據以實施，請附簡圖，簡圖上請標示元件號 碼，並說明各元件間之連結關係；請附上專用名詞之英文及說明其意義。）
	//	11, 0
	//備註：
	//	13, 0
	InsertData(2, 0, getCustomizeFieldData(CustomizeFieldType.PriorArtMethod, basic), table[2]);
	InsertData(2, 1, getCustomizeFieldData(CustomizeFieldType.PriorArtWeakPoint, basic), table[2]);
	InsertImage(4, 0, CustomizeFieldType.PriorArtPicture, basic, wordDoc, table[2]);
	InsertData(7, 0, getCustomizeFieldData(CustomizeFieldType.CreationFeature, basic), table[2]);
	InsertImage(7, 1, CustomizeFieldType.CreationFeaturePictures, basic, wordDoc, table[2]);
	InsertData(9, 0, getCustomizeFieldData(CustomizeFieldType.CreationPrinciple, basic), table[2]);
	InsertData(11, 0, getCustomizeFieldData(CustomizeFieldType.ComponentSymbolDescription, basic), table[2]);
	InsertData(13, 0, getCustomizeFieldData(CustomizeFieldType.Memo, basic), table[2]);
}

public void InsertData(int row,int cell, string source, Table table)
{
	Paragraph p = table.Elements<TableRow>().ElementAt(row).Elements<TableCell>().ElementAt(cell).Elements<Paragraph>().First();   
   	Run r = new Run();
	RunProperties runProp = new RunProperties();
   	runProp.Append(new RunFonts() { EastAsia = "DFKai-SB" });
   	runProp.Append(new FontSize() { Val = "24" });
	r.Append(new Text(source));
	r.PrependChild<RunProperties>(runProp);
   	p.Append(r);
}

public void InsertImage(int row, int cell, CustomizeFieldType customizeFieldType, BASIC basic, WordprocessingDocument wordDoc, Table table)
{
	var path = @"D:\Repo\EPS\Code\Prod\Hiwin\WebApp.EPS\upload\";
	var size = 0;
	List<string> relativePathList = null;
	switch (customizeFieldType)
	{
		case CustomizeFieldType.PriorArtPicture:
			relativePathList = this.UP_PICS.Where(x => x.B_SN == basic.SN).OrderBy(x => x.SN).Select(x => x.B_SN + "\\" + x.F_NAME).ToList();
			size = 4762500;
			break;
		case CustomizeFieldType.CreationFeaturePictures:
			relativePathList = this.BasicFigures.Where(x => x.B_SN == basic.SN).OrderBy(x => x.SN).Select(x => "Figure\\" + x.B_SN + "\\" + x.File_IDName).ToList();
			size = 3000000;
			break;
	}
	foreach (var relativePath in relativePathList)
	{
		Paragraph p = table.Elements<TableRow>().ElementAt(row).Elements<TableCell>().ElementAt(cell).Elements<Paragraph>().First();   
		Run r = new Run(GetDrawingObject(path + relativePath, wordDoc, size));
		p.Append(r);
	}
}

public Drawing GetDrawingObject(string fileName, WordprocessingDocument wordDoc, int size)
{
	MainDocumentPart mainPart = wordDoc.MainDocumentPart;
	ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
	using (FileStream stream = new FileStream(fileName, FileMode.Open))
	{
		imagePart.FeedData(stream);
	}
	
	string relationshipId = mainPart.GetIdOfPart(imagePart);
	var element = 
	new Drawing(
		new DocumentFormat.OpenXml.Drawing.Wordprocessing.Inline(
			new DocumentFormat.OpenXml.Drawing.Wordprocessing.Extent() { Cx = size, Cy = size },//圖片長寬
				new DocumentFormat.OpenXml.Drawing.Wordprocessing.EffectExtent() { LeftEdge = 0L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L }, 
					new DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties() { Id = (UInt32Value)1U, Name = "Picture 1" },
						new DocumentFormat.OpenXml.Drawing.Wordprocessing.NonVisualGraphicFrameDrawingProperties(
							new DocumentFormat.OpenXml.Drawing.GraphicFrameLocks() { NoChangeAspect = true }),
								new DocumentFormat.OpenXml.Drawing.Graphic(
									new DocumentFormat.OpenXml.Drawing.GraphicData(
										new DocumentFormat.OpenXml.Drawing.Pictures.Picture(
											new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureProperties(
												new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualDrawingProperties() { Id = (UInt32Value)0U, Name = "New Bitmap Image.jpg" },
													new DocumentFormat.OpenXml.Drawing.Pictures.NonVisualPictureDrawingProperties()),
														new DocumentFormat.OpenXml.Drawing.Pictures.BlipFill(
															new DocumentFormat.OpenXml.Drawing.Blip(
																new DocumentFormat.OpenXml.Drawing.BlipExtensionList(
																	new DocumentFormat.OpenXml.Drawing.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" })) { Embed = relationshipId, CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Print },
																		new DocumentFormat.OpenXml.Drawing.Stretch(
																			new DocumentFormat.OpenXml.Drawing.FillRectangle())),
																				new DocumentFormat.OpenXml.Drawing.Pictures.ShapeProperties(
																					new DocumentFormat.OpenXml.Drawing.Transform2D(
																						new DocumentFormat.OpenXml.Drawing.Offset() { X = 0L, Y = 0L },
																							new DocumentFormat.OpenXml.Drawing.Extents() { Cx = 990000L, Cy = 792000L }),
																								new DocumentFormat.OpenXml.Drawing.PresetGeometry(
																									new DocumentFormat.OpenXml.Drawing.AdjustValueList()) { Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle }))) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })) { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U, EditId = "50D07946" });
	return element;
}
