using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using Cosmos;
using Cosmos.System.Graphics;
using System.Drawing;
using System.IO;

namespace VisualOS
{
	public class Kernel : Sys.Kernel
	{
		string current_directory = @"0:\";
		protected override void BeforeRun()
		{
			Console.WriteLine("VisualOS booted successfully. Now loading...");
			var fs = new Sys.FileSystem.CosmosVFS();
			Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
		}

		protected override void Run()
		{
			Console.Write(current_directory + "> ");
			var input = Console.ReadLine();
			if (input == "shutdown")
			{
				Sys.Power.Shutdown();
			}
			if (input == "reboot")
			{
				Sys.Power.Reboot();
			}
			if (input == "help")
			{
				Console.WriteLine("shutdown: Shutdown, reboot: Reboot, help: Help Menu, listfil: List all files, listdir: List all directories, read:Print file contents");
			}
			if (input == "listfil")
			{
				string[] files = GetFiles(current_directory);
				for (int i = 0; i < files.Length; i++)
				{
					Console.WriteLine(files[i]);
				}
			}
			if (input == "listdir")
			{
				string[] files = GetDirectories(current_directory);
				for (int i = 0; i < files.Length; i++)
				{
					Console.WriteLine(files[i]);
				}
			}
			if (input == "read")
			{
				Console.Write("File: ");
				var file_path = Console.ReadLine();
				Console.WriteLine(ReadText(current_directory + "/" + file_path));
			}
			if (input == "create")
			{
				Console.Write("Filename: ");
				var filename = Console.ReadLine();
				Console.Write("Contents: ");
				var contents = Console.ReadLine();
				CreateFile(current_directory + "/" + filename, contents);
			}
			if (input == "delete")
			{
				Console.Write("Filename: ");
				var filename = Console.ReadLine();
				DeleteFile(current_directory + "/" + filename);
			}
			if (input == "cd")
			{
				Console.Write("Name: ");
				var directory = Console.ReadLine();
				string[] files = GetDirectories(current_directory);
				current_directory = current_directory + directory;
			}
			if (input == "cd..")
			{
				Console.Write("Name: ");
				var directory = Console.ReadLine();
				string[] files = GetDirectories(current_directory);
				current_directory = directory;
			}
			if (input == "drive")
			{
				DriveInfo[] allDrives = DriveInfo.GetDrives();

				foreach (DriveInfo d in allDrives)
				{
					Console.WriteLine("Drive:" + d.Name);
					Console.WriteLine("  Drive type: " + d.DriveType);
					if (d.IsReady == true)
					{
						Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
						Console.WriteLine("  File system: {0}", d.DriveFormat);
						Console.WriteLine(
							"  Available space to current user:{0, 15} bytes",
							d.AvailableFreeSpace);

						Console.WriteLine(
							"  Total available space:          {0, 15} bytes",
							d.TotalFreeSpace);

						Console.WriteLine(
							"  Total size of drive:            {0, 15} bytes ",
							d.TotalSize);
					}
				}
			}
			if(input == "cdd")
			{
				Console.Write("Drive: ");
				var directory = Console.ReadLine();
				string[] files = GetDirectories(current_directory);
				current_directory = @"" + directory + ":/";
			}
			if(input == "cp")
			{
				Console.Write("File-path: ");
				var path = Console.ReadLine();
				CreateFile(current_directory + "/copy_contents.txt", ReadText(path));
			}

		}
		public string[] ReadLines(string FileAdr) // Returns the lines of text in string[].
		{
			string[] FileRead;
			FileRead = File.ReadAllLines(FileAdr);
			return FileRead;
		}

		public string ReadText(string FileAddr) // Returns the file in a single string.
		{
			string f_contents = "";
			f_contents = File.ReadAllText(FileAddr);
			return f_contents;
		}

		public byte[] ReadByte(string FileAdr) // Returns the read file in bytes.
		{
			byte[] FileRead;
			FileRead = File.ReadAllBytes(FileAdr);
			return FileRead;
		}
		public string[] GetFiles(string Adr) // Adr means path.
		{
			string[] Files = new string[Directory.GetFiles(Adr).Length];
			if (Files.Length > 0)
				Files = Directory.GetFiles(Adr);
			else
				Files[0] = "No files found.";

			return Files;
		}

		public string[] GetDirectories(string Adr)
		{
			string[] Directories = new string[Directory.GetDirectories(Adr).Length];
			if (Directories.Length > 0)
				Directories = Directory.GetDirectories(Adr);
			else
				Directories[0] = "No directories found.";

			return Directories;
		}
		public void CreateFile(string name, string contents)
		{
			using (FileStream fs = File.Create(name))
			{
				// writing data in string
				string dataasstring = contents; //your data
				byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
				fs.Write(info, 0, info.Length);

				// writing data in bytes already
				byte[] data = new byte[] { 0x0 };
				fs.Write(data, 0, data.Length);
			}
		}
		public void DeleteFile(string name)
		{
			File.Delete(name);
		}
	}
}
