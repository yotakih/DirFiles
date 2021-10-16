using System;
using System.Collections.Generic;
using System.IO;

namespace DirFiles
{
	class Program
	{
		string RootDir = "";
		string OutputFilePath = "";
		StreamWriter SwOutput = null;
		static void Main(string[] args)
		{
			Console.WriteLine("DirFiles  Start!! " + DateTime.Now);
			new Program().Proc(args);
			Console.WriteLine($"DirFiles  End!!  " + DateTime.Now);
		}
		void Proc(string[] args)
		{
			try
			{
				if (!this.AnalizeArgs(args))
					return;
				if (this.OutputFilePath.Trim() != "")
					this.SwOutput = new StreamWriter(this.OutputFilePath, false);
				else
					this.SwOutput = new StreamWriter(Stream.Null);
				if (!Directory.Exists(this.RootDir)) return;
				DirFileFunc(this.RootDir, this.SwOutput);
			}
			catch (Exception e)
			{
				Console.WriteLine("error:" + e.Message);
			}
			finally
			{
				if (!(this.SwOutput is null))
					this.SwOutput.Close();
			}
		}

		void DirFileFunc(string dirpath, StreamWriter sw)
		{
			string[] files = Directory.GetFiles(dirpath);
			Array.Sort(files);
			for (int i=0; i<files.Length; i++)
			{
				string fullpath = files[i];
				this.SwOutput.WriteLine(
					String.Format(
						"{1}{0}{2}{0}{3}",
						 "\t",
						 fullpath.Substring(this.RootDir.Length),
						 Path.GetFileName(fullpath),
						 File.GetLastWriteTime(fullpath)
					));
			}
			string[] dirs = Directory.GetDirectories(dirpath);
			Array.Sort(dirs);
			for (int i=0; i<dirs.Length; i++)
			{
				DirFileFunc(dirs[i], this.SwOutput);
			}
		}

		bool AnalizeArgs(string[] args)
		{
			bool ret = true;
			switch (args.Length)
			{
				case 2:
					this.RootDir = args[0];
					this.OutputFilePath = args[1];
					ret = true;
					break;
				default:
					Usage();
					ret = false;
					break;
			}
			return ret;
		}
		void Usage()
		{
			Console.WriteLine(
				$"Usage:\n" +
				$" arg1: select Root Dir\n" +
				$" arg2: output file path\n"
			);
		}
	}
}
