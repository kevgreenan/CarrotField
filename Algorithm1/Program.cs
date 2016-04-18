using System;
using System.Collections.Generic;

namespace Algorithm1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			int level = 1;

			Sector game = new Sector (level);

			game.Initialize ();

			game.Play ();

		}
	}

	public class Sector 
	{
		//
		//--: Variables and constructors
		//

		private int level;
		private int size;
		private string[,] mapsector;
		private int bound0;
		private int bound1;
		private string cursor = "* ";
		private string item = "! ";
		private int points;
		private int steps;
		private int numofevents;

		public Sector (int l) {
			level = l;
			size = (level + 4) * 2;
			mapsector = new string[size, size];
			bound0 = mapsector.GetUpperBound (0);
			bound1 = mapsector.GetUpperBound (1);
			points = 0;
		}

		public int Level { 
			get { return level; }
			set { 
				if (value >= 1 && value <= 96) {
					level = value;
				}
			}
		}

		public int Size{ 
			get { return size; }
			set { size = value; }
		}

		public char Move { get; set; }

		public int Xcurrent { get; set; }

		public int Ycurrent { get; set; }

		public string[,] MapSector {
			get { return mapsector; }
			set { mapsector = value; }
		}

		public int Bound0 { 
			get { return bound0; }
			set { bound0 = value; }
		}

		public int Bound1 { 
			get { return bound1; }
			set { bound1 = value; }
		}

		public int Points {
			get { return points; }
			set { points = value; }
		}

		public int Steps {
			get { return steps; }
			set { steps = value; }
		}

		public int NumOfEvents {
			get { return numofevents; }
			set { numofevents = value; }
		}

		//
		//--: Methods
		//

		public void SetSector() {
			for (int row = 0; row <= Bound0; row++) {
				for (int col = 0; col <= Bound1; col++) {
					MapSector [row, col] = "^ ";
				}
			}
			MapSector [0, 0] = cursor;
			//MapSector [5, 5] = item; // For testing purposes

			// Generate items randomly
			Random random = new Random ();
			int NumberOfItems = random.Next (Level, Size/2 * Size/2);
			NumOfEvents = NumberOfItems;
			for (int i = 0; i < NumberOfItems; i++) {
				MapSector [random.Next (1, Size - 1), random.Next (1, Size - 1)] = item;
			}

		}

		public void ShowWelcome() {
			Console.WriteLine ("Welcome to level {0}.", Level);
		}

		public void ShowSector() {
			if (Steps == 0) {
				Console.Clear ();
				Console.WriteLine ("You lose.");
				Console.WriteLine ("You scored {0} points.", Points);
			} else {
				Console.WriteLine ("Level: {0}", Level);
				Console.WriteLine ("Steps: {0}", Steps);
				Console.WriteLine ("Points: {0}", Points);
				//Console.WriteLine ("Current position: ({0},{1})", Xcurrent, Ycurrent); // For testing purposes
				for (int row = 0; row <= Bound0; row++) {
					for (int col = 0; col <= Bound1; col++) {
						Console.Write (MapSector [row, col]);
					}
					Console.WriteLine ();
				}
			}
		}

		public void Initialize() {
			Size = (Level + 4) * 2;
			MapSector = new string[Size, Size];
			Bound0 = MapSector.GetUpperBound (0);
			Bound1 = MapSector.GetUpperBound (1);
			ShowWelcome ();
			SetSector ();
			Xcurrent = 0;
			Ycurrent = 0;
			Steps = 100;
			MapSector [Xcurrent, Ycurrent] = cursor;
		}

		public void MoveCursor() {
			Console.Write ("Move(w/a/s/d): ");
			string TempMove = Console.ReadLine ();
			if (TempMove == "w" || TempMove == "a" || TempMove == "s" || TempMove == "d") {
				Move = Convert.ToChar (TempMove);
			} else if (TempMove == "W" || TempMove == "A" || TempMove == "S" || TempMove == "D") {
				Move = Convert.ToChar (TempMove);
			} else {
				Move = 'q'; // this will do nothing, see the switch below
			}
			Console.Clear ();

			MapSector [Xcurrent, Ycurrent] = "  ";

			switch (Move) {
			case 'd':
				if (Ycurrent < (Size - 1))
					Ycurrent += 1;
				break;
			case 'a':
				if (Ycurrent != 0)
					Ycurrent -= 1;
				break;
			case 'w':
				if (Xcurrent != 0)
					Xcurrent -= 1;
				break;
			case 's':
				if (Xcurrent < (Size - 1))
					Xcurrent += 1;
				break;
			default:
				break;
			}

			// ITEM LOGIC

			if (MapSector [Xcurrent, Ycurrent] == "! ") {
				Steps -= 1;
				DoEvent ();
			} else {
				Steps -= 1;
			}
			MapSector [Xcurrent, Ycurrent] = cursor;
		}

		public void Play() {
			bool LevelUp = false;
			int CurrentLevel = Level;
			while (LevelUp == false) {
				ShowSector ();
				MoveCursor ();
				if (Level > CurrentLevel) {
					LevelUp = true;
				} else {
					LevelUp = false;
				}
			}
			NewLevel ();
		}

		public void NewLevel() {
			Initialize ();
			Play ();
		}

		public void DoEvent() {
			// Points, Traps, Quests, Special
			string [] events = new string[] { "point", "point", "point", "point", "trap", "trap", "trap", "trap", "point", "Golden Carrot", "Golden Carrot" };
			bool FoundGoldenCarrot = false;
			string thisEvent = " ";
			Random random = new Random ();
			if (NumOfEvents < 2) {
				thisEvent = "Golden Carrot";
			} else {
				thisEvent = events [random.Next (0, 10)];
			}

			Console.Clear ();
			Console.WriteLine ("You landed on a {0}!", thisEvent);

			switch (thisEvent) {
			case "point":
				Points += 1;
				break;
			case "trap":
				Points -= random.Next (0, Points/2);
				break;
			case "Golden Carrot":
				Points += Steps;
				FoundGoldenCarrot = true;
				break;
			default:
				break;
			}
			NumOfEvents -= 1;
			if (FoundGoldenCarrot == true) { // Make this more complicated
				Level += 1;
			}
		}
	}
}
