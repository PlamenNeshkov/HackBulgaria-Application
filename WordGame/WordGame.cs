using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordGame
{
	public class WordGame
	{
		public char[,] Matrix { get; private set; }
		int rows;
		int cols;

		public string Word { get; private set; }
		public int Occurences { get; private set; }

		public WordGame(char[,] matrix, string word) 
		{
			this.Matrix = matrix;
			this.rows = matrix.GetLength(0);
			this.cols = matrix.GetLength(1);

			this.Word = word;
			this.Occurences = 0;
		}

		private void TryFind(int row, int col, string dir, int wordPos = 0) 
		{
			if (this.Matrix[row, col] == this.Word[wordPos])
			{
				if (wordPos == this.Word.Length - 1)
				{
					this.Occurences++;
					return;
				}

				switch (dir)
				{
					case "N":
						if (row - 1 >= 0)
						{
							TryFind(row - 1, col, "N", wordPos + 1);
						}
						break;

					case "E":
						if (col + 1 < this.cols)
						{
							TryFind(row, col + 1, "E", wordPos + 1);
						}
						break;

					case "S":
						if (row + 1 < this.rows)
						{
							TryFind(row + 1, col, "S", wordPos + 1);
						}
						break;

					case "W":
						if (col - 1 >= 0)
						{
							TryFind(row, col - 1, "W", wordPos + 1);
						}
						break;

					case "NE":
						if (row - 1 >= 0 && col + 1 < this.cols)
						{
							TryFind(row - 1, col + 1, "NE", wordPos + 1);
						}
						break;

					case "NW":
						if (row - 1 >= 0 && col - 1 >= 0)
						{
							TryFind(row - 1, col - 1, "NW", wordPos + 1);
						}
						break;

					case "SE":
						if (row + 1 < this.rows && col + 1 < this.cols)
						{
							TryFind(row + 1, col + 1, "SE", wordPos + 1);
						}
						break;

					case "SW":
						if (row + 1 < this.rows && col - 1 >= 0)
						{
							TryFind(row + 1, col - 1, "SW", wordPos + 1);
						}
						break;
				}
			}

			return;
		}

		public void TraverseTable()
		{
			for (int row = 0; row < this.rows; row++)
			{
				for (int col = 0; col < this.cols; col++)
				{
					this.TryFind(row, col, "N");
					this.TryFind(row, col, "E");
					this.TryFind(row, col, "S");
					this.TryFind(row, col, "W");
					this.TryFind(row, col, "NE");
					this.TryFind(row, col, "NW");
					this.TryFind(row, col, "SE");
					this.TryFind(row, col, "SW");
				}
			}
		}
	}

	class MainClass
	{
		public static void Main(string[] args)
		{
			/*char[,] matrix = {
				{ 'i', 'v', 'a', 'n' },
				{ 'e', 'v', 'n', 'h' },
				{ 'i', 'n', 'a', 'v' },
				{ 'm', 'v', 'v', 'n' },
				{ 'q', 'r', 'i', 't' }
			};*/

			Console.WriteLine("Input the dimensions of the table...");
			Console.Write("Rows: ");
			int rows = int.Parse(Console.ReadLine());
			Console.Write("Cols: ");
			int cols = int.Parse(Console.ReadLine());

			Console.WriteLine("Input each row of the table with the elements separated by whitespace:");
			char[,] matrix = ReadMatrix (rows, cols);

			var ivanGame = new WordGame(matrix, "ivan");

			ivanGame.TraverseTable();

			Console.WriteLine(ivanGame.Occurences);
		}

		public static char[,] ReadMatrix(int rows, int cols)
		{
			char[,] matrix = new char[rows, cols];

			for (int row = 0; row < rows; row++) 
			{
				string line = Console.ReadLine();
				if (string.IsNullOrWhiteSpace(line)) 
				{
					throw new FormatException();
				}
				string[] lineElements = Regex.Split(line, @"\s+")
					                   .Where (s => s != string.Empty).ToArray();

				if (lineElements.Length != cols) 
				{
					throw new FormatException();
				}
				for (int col = 0; col < cols; col++) 
				{
					matrix [row, col] = lineElements[col][0];
				}
			}

			return matrix;
		}
	}
}
