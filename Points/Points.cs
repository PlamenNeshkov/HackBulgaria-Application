using System;
using System.Text.RegularExpressions;

namespace Points
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Regex xyPattern = new Regex(@"Starting\s+point:\s*\((\d+),\s*(\d+)\)");

			string startingPoint = Console.ReadLine();
			Match m = xyPattern.Match(startingPoint);
			if (!m.Success) {
				Console.WriteLine("Invalid input!");
				return;
			}

			int x = int.Parse(m.Groups[1].Value);
			int y = int.Parse(m.Groups[2].Value);

			string directions = Console.ReadLine();
			bool reversed = false;
			foreach (var dir in directions) 
			{
				if (dir == '~') 
				{
					reversed = !reversed;
				} 
				else if (reversed) 
				{
					switch (dir) 
					{
						case '^':
							y++;
							break;
						case '>':
							x--;
							break;
						case 'v':
							y--;
							break;
						case '<':
							x++;
							break;
					}
				} 
				else 
				{
					switch (dir) 
					{
						case '^':
							y--;
							break;
						case '>':
							x++;
							break;
						case 'v':
							y++;
							break;
						case '<':
							x--;
							break;
					}
				}
			}

			Console.WriteLine("({0}, {1})", x, y);
		}
	}
}
