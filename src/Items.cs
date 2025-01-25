using System.Collections.Generic;
using System.Drawing;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс предоставляет обработчики для сущностей собираемых предметов
	/// </summary>
	public static class ItemsSupport
		{
		/// <summary>
		/// Метод добавляет собираемые объекты на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="AllowSecondFloor">Флаг, разрешающий размещение на внутренних площадках</param>
		/// <param name="Probabilities">Набор вероятностей для видов предметов</param>
		/// <param name="ForceFloorPlacement">Флаг указывает на необходимость расположения
		/// прямо на полу (для исключения «застревания» во враге)</param>
		public static void WriteMapItem (Point RelativePosition,
			bool AllowSecondFloor, bool ForceFloorPlacement, byte[] Probabilities)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);

			// Запись
			MapSupport.Write ("{\n");

			// Обходной вариант с собираемым объектом
			if (!hiddenObjectWritten && (MapSupport.MapNumber % 10 == 0))
				{
				hiddenObjectWritten = true;
				MapSupport.AddEntity (MapClasses.HiddenObject);

				int limit = (MapSupport.MapsLimit / 10) * 10;
				if (MapSupport.MapNumber == limit)
					{
					MapSupport.Write ("\"spawnflags\" \"4\"\n");
					MapSupport.Write ("\"MinimumToTrigger\" \"" + (limit / 10).ToString () + "\"\n");
					}
				else
					{
					MapSupport.Write ("\"MinimumToTrigger\" \"1\"\n");
					}

				goto finishItem;
				}

			List<byte> itemsProbabilityLine = new List<byte> ();
			itemsProbabilityLine.Add (255);

			for (int i = 0; i < Probabilities.Length; i++)
				{
				// Эти виды оружия доступны только в ящиках
				if (ItemsOnlyFromCrates.Contains (i))
					continue;

				if ((i == i_357) && (MapSupport.MapNumber < 2))
					continue;
				if ((i == i_sat) && (MapSupport.MapNumber < 3))
					continue;
				if ((i == i_crs) && (MapSupport.MapNumber < 4))
					continue;
				if ((i == i_gau) && (MapSupport.MapNumber < 5))
					continue;

				for (int j = 0; j < Probabilities[i]; j++)
					itemsProbabilityLine.Add ((byte)i);
				}

			// Запись объекта
			int crItem = RDGenerics.RND.Next (itemsProbabilityLine.Count);
			if ((itemsProbabilityLine.Count < 1) || (crItem == 0))
				crItem = 255;
			else
				crItem = itemsProbabilityLine[crItem];

			// Выбор предмета
			switch (crItem)
				{
				// Документы
				default:
				case 255:
					MapSupport.AddEntity (MapClasses.Document);
					break;

				case i_hek:
				case i_bat:
				case i_gre:
				case i_hnd:
				case i_sat:
				case i_357:
				case i_crs:
				case i_gau:
					MapSupport.AddEntity (items[crItem]);
					break;
				}

		finishItem:
			int z = ForceFloorPlacement ? 0 : 40;
			if (AllowSecondFloor)
				z += (RDGenerics.RND.Next (2) * MapSupport.DefaultWallHeight);

			MapSupport.Write ("\"angles\" \"0 " + RDGenerics.RND.Next (360) + " 0\"\n");
			MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
				z.ToString () + "\"\n");   // На некоторой высоте над полом
			MapSupport.Write ("}\n");
			}

		// Флаг наличия записи об объекте-достижении
		private static bool hiddenObjectWritten = false;

		/// <summary>
		/// Возвращает число доступных видов врагов
		/// </summary>
		public static uint AvailableItemsTypes
			{
			get
				{
				return (uint)items.Length;
				}
			}

		// Набор имён классов предметов
		private static MapClasses[] items = new MapClasses[] {
			// Прямая генерация
			MapClasses.HealthKit,
			MapClasses.Armor,
			MapClasses.HandGrenade,
			MapClasses.HandGun,
			MapClasses.Satchel,
			MapClasses.Python,
			MapClasses.Crossbow,
			MapClasses.Gauss,
			MapClasses.Crowbar,
			MapClasses.HornetGun,

			// Ящики
			MapClasses.MachineGun,
			MapClasses.Shotgun,
			MapClasses.RPG,
			MapClasses.TripMineWeapon,
			MapClasses.Snark,
			MapClasses.Egon,
			MapClasses.Axe,
			};
		private const int i_hek = 0;
		private const int i_bat = 1;
		private const int i_gre = 2;
		private const int i_hnd = 3;
		private const int i_sat = 4;
		private const int i_357 = 5;
		private const int i_crs = 6;
		private const int i_gau = 7;
		private const int i_bar = 8;
		private const int i_hor = 9;
		private const int i_9ar = 10;
		private const int i_sho = 11;
		private const int i_rpg = 12;
		private const int i_min = 13;
		private const int i_sna = 14;
		private const int i_ego = 15;
		private const int i_axe = 16;

		/// <summary>
		/// Метод возвращает код объекта для ящика на основе указанных разрешений
		/// </summary>
		/// <param name="Probabilities">Набор вероятностей для видов предметов</param>
		public static uint GetRandomItemForCrate (byte[] Probabilities)
			{
			// Определение доступных вариантов
			List<byte> itemsProbabilityLine = new List<byte> ();
			for (int i = 0; i < Probabilities.Length; i++)
				for (int j = 0; j < Probabilities[i]; j++)
					itemsProbabilityLine.Add ((byte)i);

			// Выбор варианта
			int crItem = RDGenerics.RND.Next (itemsProbabilityLine.Count);
			if (itemsProbabilityLine.Count < 1)
				crItem = i_hek;
			else
				crItem = itemsProbabilityLine[crItem];

			// Возврат
			if (itemIndices[crItem].Length < 2)
				return itemIndices[crItem][0];

			return itemIndices[crItem][RDGenerics.RND.Next (itemIndices[crItem].Length)];
			}

		private static uint[][] itemIndices = new uint[][] {
			new uint[] { 2 },
			new uint[] { 1 },
			new uint[] { 17 },
			new uint[] { 3, 4 },
			new uint[] { 19 },

			new uint[] { 12, 13 },
			new uint[] { 10, 11 },
			new uint[] { 23, 16 },
			new uint[] { 22 },
			new uint[] { 21 },

			new uint[] { 5, 6 },
			new uint[] { 8, 9 },
			new uint[] { 14, 15 },
			new uint[] { 18 },
			new uint[] { 20 },

			new uint[] { 24 },
			new uint[] { 26 },
			};

		/// <summary>
		/// Возвращает список предметов, доступных только в ящиках
		/// </summary>
		public static List<int> ItemsOnlyFromCrates
			{
			get
				{
				return new List<int> (itemsOnlyFromCrates);
				}
			}
		private static int[] itemsOnlyFromCrates = new int[] {
			i_sho,
			i_rpg,
			i_axe,
			i_ego,
			i_sna,
			i_min,
			i_9ar,
			i_hor,
			i_bar,
			};
		}
	}
