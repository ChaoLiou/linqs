<Query Kind="Program">
  <Reference>&lt;ProgramFilesX86&gt;\Rebex\FTP SSL for .NET 4.0 Trial\bin\Rebex.Net.Ftp.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Rebex\FTP SSL for .NET 4.0 Trial\bin\Rebex.Net.ProxySocket.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Rebex\FTP SSL for .NET 4.0 Trial\bin\Rebex.Net.SecureSocket.dll</Reference>
  <Reference>&lt;ProgramFilesX86&gt;\Rebex\FTP SSL for .NET 4.0 Trial\bin\Rebex.Security.dll</Reference>
  <Namespace>Rebex.Net</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>Rebex.Security.Certificates</Namespace>
</Query>

void Main()
{
	var ftpUrl = "ftp://ftp.kipris.or.kr";
	var userID = "Ltech";
	var password = "Ltech2015";
	//ftp(ftpUrl, userID, password);
	newFtp(ftpUrl, userID, password);
}

private void ftp(string ftpUrl, string userID, string password)
{
	var req = (System.Net.FtpWebRequest)System.Net.FtpWebRequest.Create(ftpUrl);
	req.Credentials = new NetworkCredential(userID, password);
	req.UseBinary = true;
	req.UsePassive = true;
	req.KeepAlive = true;
	req.Timeout = 30 * 60 * 1000;
	req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
	var resp = (System.Net.FtpWebResponse)req.GetResponse();
//	using (var r = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
//	{
//		...
//	}
}

private void newFtp(string ftpUrl, string ftpId, string ftpPwd)
{
	var ftp = new Ftp();
	//ftp.ValidatingCertificate += ftp_ValidatingCertificate;
	TlsParameters par = new TlsParameters();
	par.CertificateVerifier = new FingerprintVerifier();
	var ftpConnectMessage = ftp.Connect(ftpUrl, 10021, par, FtpSecurity.Implicit);
	var ftpLoginMessage = ftp.Login(ftpId, ftpPwd);
	ftp.GetList("KR_PAT/").Dump();
	ftp.GetFile("20160205_NOTICE.txt", "D:/20160205_NOTICE.txt");
}

public class FingerprintVerifier : ICertificateVerifier
{
	// This method gets called during the SSL handshake
	// process when the certificate chain is received from the server.
	public TlsCertificateAcceptance Verify(TlsSocket socket, string commonName, CertificateChain certChain) => TlsCertificateAcceptance.Accept;
}
//private void ftp_ValidatingCertificate(object sender, SslCertificateValidationEventArgs e) => e.Accept();