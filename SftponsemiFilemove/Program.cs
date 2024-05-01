using System;
using System.IO;
using Renci.SshNet;

namespace SFTPconnect
{
	internal static class Program
	{
		static void Main()
		{
			string localDirectory = @"C:\xml\intransit\"; // Destination directory where files will be downloaded
			string sftpDirectory = "/files/FSC/intransit/"; // Remote SFTP directory path

			try
			{
				using (var client = new SftpClient("112.199.64.167", "onsemi_system", "ONSemi1*"))
				{
					client.Connect();

					var files = client.ListDirectory(sftpDirectory);

					foreach (var file in files)
					{
						if (file.IsDirectory) continue;

						string fileName = file.Name;
						string remoteFilePath = sftpDirectory + fileName;
						string localFilePath = Path.Combine(localDirectory, fileName);

						using (var stream = File.Create(localFilePath))
						{
							client.DownloadFile(remoteFilePath, stream);
						}

						client.DeleteFile(remoteFilePath);

						Console.WriteLine($"Downloaded and deleted file: {fileName}");
					}

					client.Disconnect();
				}

				Console.WriteLine("All files downloaded and deleted from SFTP server successfully!");
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}

			Environment.Exit(0);
		}
	}
}
