using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DependencyResolving
{
	public class PackageNotFoundException : System.Exception
	{
		public PackageNotFoundException(string message) : base(message)
		{
		}
	}

	public class PackageManager
	{
		public string ProjectPath { get; private set; }

		Dictionary<string, string[]> allPackages;
		string[] dependencies;
		string[] installedModules;

		public PackageManager(string projectPath)
		{
			this.ProjectPath = projectPath;

			string allPackagesPath = Path.Combine(projectPath, "all_packages.json");
			string allPackagesJson;
			if (File.Exists(allPackagesPath))
			{
				allPackagesJson = File.ReadAllText(allPackagesPath);
				this.allPackages = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(allPackagesJson);
			}
			else
			{
				throw new FileNotFoundException("all_packages.json missing!");
			}

			string dependenciesPath = Path.Combine(projectPath, "dependencies.json");
			JObject dependenciesJObject;
			if (File.Exists(dependenciesPath))
			{
				dependenciesJObject = JObject.Parse(File.ReadAllText(dependenciesPath));
				this.dependencies = dependenciesJObject["dependencies"].Select(x => (string)x).ToArray();
			}
			else
			{
				throw new FileNotFoundException("dependencies.json missing!");
			}

			string installedModulesPath = Path.Combine(projectPath, "installed_modules");
			this.installedModules = Directory.GetDirectories(installedModulesPath);
		}

		private bool IsInstalled(string package)
		{
			string packagePath = Path.Combine(this.ProjectPath, "installed_modules", package);
			return installedModules.Contains(packagePath);
		}

		private void MakePackageDirectory(string package)
		{
			string packagePath = Path.Combine(this.ProjectPath, "installed_modules", package);
			Directory.CreateDirectory(packagePath);
		}

		public void InstallDependency(string dependency)
		{
			Console.WriteLine("Installing {0}.", dependency);

			if (this.allPackages.ContainsKey(dependency))
			{
				if (IsInstalled(dependency))
				{
					Console.WriteLine("{0} is already installed.", dependency);
				}
				else
				{
					MakePackageDirectory(dependency);

					var subdependencies = new List<string>();
					foreach(var subdependency in this.allPackages[dependency])
					{
						subdependencies.Add(subdependency);
					}

					if (subdependencies.Count > 0)
					{
						Console.WriteLine ("In order to install {0}, we need {1}.", 
							               dependency, String.Join(" and ", subdependencies));

						foreach(var subdependency in subdependencies)
						{
							InstallDependency(subdependency);
						}
					}
				}
			}
			else
			{
				throw new PackageNotFoundException("Dependency missing from packages list!");
			}
		}

		public void InstallDependencies()
		{
			foreach (var dependency in this.dependencies)
			{
				InstallDependency(dependency);
			}
			Console.WriteLine("All done.");
		}
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Write("Enter project path: ");
			string projectPath = Console.ReadLine();

			var myPackageManager = new PackageManager(projectPath);
			myPackageManager.InstallDependencies();
		}			
	}
}
