using System;
using System.Drawing;
using System.IO;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс обеспечивает вспомогательные методы для создания карт Xash3D
	/// </summary>
	public static class MapSupport
		{
		/// <summary>
		/// Стандартная высота стен на картах
		/// </summary>
		public static string WallHeightString = WallHeightInt.ToString ();

		/// <summary>
		/// Стандартная высота стен на картах
		/// </summary>
		public const int WallHeightInt = 128;

		/// <summary>
		/// Стандартная длина стен на картах
		/// </summary>
		public const int WallLength = 128;

		/// <summary>
		/// Возвращает максимально допустимое количество карт
		/// </summary>
		public const int MapsLimit = 999;

		/// <summary>
		/// Возвращает формат номеров карт
		/// </summary>
		public const string MapsNumbersFormat = "D3";

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		public static void WriteMapEndPoint_Finish (StreamWriter SW, Point RelativePosition)
			{
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"game_end\"\n");
			SW.Write ("\"targetname\" \"DevEnd02\"\n");
			SW.Write ("\"origin\" \"" + (x - 8).ToString () + " " + (y - 8).ToString () + " " +
				(WallHeightInt - 8).ToString () + "\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"player_loadsaved\"\n");
			SW.Write ("\"targetname\" \"DevEnd01\"\n");
			SW.Write ("\"holdtime\" \"6\"\n");
			SW.Write ("\"message\" \"DEV_END\"\n");
			SW.Write ("\"duration\" \"1\"\n");
			SW.Write ("\"messagetime\" \"2\"\n");
			SW.Write ("\"loadtime\" \"6\"\n");
			SW.Write ("\"rendercolor\" \"0 0 0\"\n");
			SW.Write ("\"renderamt\" \"255\"\n");
			SW.Write ("\"origin\" \"" + (x - 8).ToString () + " " + (y + 8).ToString () + " " +
				(WallHeightInt - 8).ToString () + "\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"multi_manager\"\n");
			SW.Write ("\"targetname\" \"DevM\"\n");
			SW.Write ("\"DevEnd01\" \"0\"\n");
			SW.Write ("\"DevEnd02\" \"5.5\"\n");
			SW.Write ("\"origin\" \"" + (x + 8).ToString () + " " + (y - 8).ToString () + " " +
				(WallHeightInt - 8).ToString () + "\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_once\"\n");
			SW.Write ("\"target\" \"DevM\"\n");
			SW.Write ("{\n");

			SW.Write ("( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("( " + (x + 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x - 8).ToString () + " " + (y - 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x - 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x + 8).ToString () + " " + (y - 8).ToString () + " 16 ) " +
				"( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x + 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " 16 ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("}\n}\n");

			WriteMapPortal (SW, RelativePosition, true);
			}

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		/// <param name="MapNumber">Номер текущей карты</param>
		public static void WriteMapEndPoint (StreamWriter SW, Point RelativePosition, uint MapNumber)
			{
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			// Защита
			if (MapNumber >= MapsLimit)
				{
				WriteMapEndPoint_Finish (SW, RelativePosition);
				return;
				}

			// Добавление
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"info_landmark\"\n");
			SW.Write ("\"targetname\" \"" + Program.MainAlias + (MapNumber + 1).ToString (MapsNumbersFormat) + "m\"\n");
			SW.Write ("\"origin\" \"" + (x - 8).ToString () + " " + (y - 8).ToString () + " 40\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_changelevel\"\n");
			SW.Write ("\"map\" \"" + Program.MainAlias + (MapNumber + 1).ToString (MapsNumbersFormat) + "\"\n");
			SW.Write ("\"landmark\" \"" + Program.MainAlias + (MapNumber + 1).ToString (MapsNumbersFormat) + "m\"\n");
			SW.Write ("{\n");

			SW.Write ("( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("( " + (x + 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x - 8).ToString () + " " + (y - 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x - 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x + 8).ToString () + " " + (y - 8).ToString () + " 16 ) " +
				"( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 16).ToString () + " ) " +
				"AAATRIGGER [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + (x + 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " 16 ) " +
				"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " 16 ) " +
				"AAATRIGGER [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("}\n}\n");

			WriteMapPortal (SW, RelativePosition, true);
			}

		// Метод создаёт портал на карте
		private static void WriteMapPortal (StreamWriter SW, Point RelativePosition, bool Exit)
			{
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"env_sprite\"\n");
			SW.Write ("\"spawnflags\" \"1\"\n");
			SW.Write ("\"angles\" \"0 0 0\"\n");
			SW.Write ("\"rendermode\" \"5\"\n");
			SW.Write ("\"renderamt\" \"" + (Exit ? "255" : "100") + "\"\n");
			SW.Write ("\"rendercolor\" \"0 0 0\"\n");
			SW.Write ("\"framerate\" \"10.0\"\n");
			SW.Write ("\"model\" \"sprites/" + (Exit ? "exit" : "enter") + "1.spr\"\n");
			SW.Write ("\"scale\" \"1\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " " + (WallHeightInt / 2).ToString () + "\"\n");
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает точку входа на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки входа</param>
		/// <param name="MapNumber">Номер текущей карты</param>
		public static void WriteMapEntryPoint (StreamWriter SW, Point RelativePosition, uint MapNumber)
			{
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;
			string xs = x.ToString ();
			string ys = y.ToString ();

			// Первая карта
			if (MapNumber == 1)
				{
				SW.Write ("{\n");
				SW.Write ("\"classname\" \"info_player_start\"\n");
				SW.Write ("\"angles\" \"0 0 0\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"item_suit\"\n");
				SW.Write ("\"spawnflags\" \"1\"\n");
				SW.Write ("\"angles\" \"0 0 0\"\n");
				SW.Write ("\"target\" \"Preset\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 32\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"weapon_9mmAR\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 36\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"weapon_shotgun\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"weapon_handgrenade\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 44\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"weapon_handgrenade\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 48\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"ammo_9mmbox\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 52\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"ammo_buckshot\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 56\"\n");
				SW.Write ("}\n{\n");

				SW.Write ("\"classname\" \"game_player_set_health\"\n");
				SW.Write ("\"targetname\" \"Preset\"\n");
				SW.Write ("\"dmg\" \"200\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 64\"\n");
				SW.Write ("}\n");
				}

			// Все последующие
			else
				{
				SW.Write ("{\n");
				SW.Write ("\"classname\" \"info_landmark\"\n");
				SW.Write ("\"targetname\" \"" + Program.MainAlias + MapNumber.ToString (MapsNumbersFormat) + "m\"\n");
				SW.Write ("\"origin\" \"" + (x - 8).ToString () + " " + (y - 8).ToString () + " 40\"\n");

				SW.Write ("}\n{\n");
				SW.Write ("\"classname\" \"trigger_changelevel\"\n");
				SW.Write ("\"map\" \"" + Program.MainAlias + (MapNumber - 1).ToString (MapsNumbersFormat) + "\"\n");
				SW.Write ("\"landmark\" \"" + Program.MainAlias + MapNumber.ToString (MapsNumbersFormat) + "m\"\n");
				SW.Write ("{\n");

				SW.Write ("( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + WallHeightString + " ) " +
					"AAATRIGGER [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + WallHeightString + " ) " +
					"AAATRIGGER [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + WallHeightString + " ) " +
					"( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + WallHeightString + " ) " +
					"AAATRIGGER [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + WallHeightString + " ) " +
					"AAATRIGGER [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"( " + (x + 8).ToString () + " " + (y - 8).ToString () + " " + WallHeightString + " ) " +
					"( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + WallHeightString + " ) " +
					"AAATRIGGER [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x + 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"( " + (x - 8).ToString () + " " + (y + 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"( " + (x - 8).ToString () + " " + (y - 8).ToString () + " " + (WallHeightInt - 1).ToString () + " ) " +
					"AAATRIGGER [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("}\n}\n");
				}

			WriteMapPortal (SW, RelativePosition, false);
			WriteMapSound (SW, RelativePosition, "Teleport1", AmbientTypes.None);
			}

		/*/// <summary>
		/// Метод записывает точку входа на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки входа</param>
		public static void WriteMapEntryPoint_Old (StreamWriter SW, Point RelativePosition)
			{
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"info_player_start\"\n");
			SW.Write ("\"angles\" \"0 0 0\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 40\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"item_suit\"\n");
			SW.Write ("\"spawnflags\" \"1\"\n");
			SW.Write ("\"angles\" \"0 0 0\"\n");
			SW.Write ("\"target\" \"Preset\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 32\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"weapon_9mmAR\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 36\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"weapon_shotgun\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 40\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"weapon_handgrenade\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 44\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"weapon_handgrenade\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 48\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"ammo_9mmbox\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 52\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"ammo_buckshot\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 56\"\n");
			SW.Write ("}\n{\n");

			SW.Write ("\"classname\" \"game_player_set_health\"\n");
			SW.Write ("\"targetname\" \"Preset\"\n");
			SW.Write ("\"dmg\" \"200\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 64\"\n");
			SW.Write ("}\n");

			WriteMapPortal (SW, RelativePosition, false);
			}*/

		/// <summary>
		/// Метод добавляет собираемые объекты на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="MapNumber">Номер текущей карты</param>
		public static void WriteMapItem (StreamWriter SW, Point RelativePosition, Random Rnd, uint MapNumber)
			{
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			SW.Write ("{\n");

			// Диапазон противников задаётся ограничением на верхнюю границу диапазона ГПСЧ
			int prngRange;
			switch (MapNumber)
				{
				case 0:
				case 1:
					prngRange = 15;
					break;

				case 2:
					prngRange = 16;
					break;

				case 3:
					prngRange = 17;
					break;

				case 4:
					prngRange = 18;
					break;

				case 5:
					prngRange = 19;
					break;

				case 6:
					prngRange = 20;
					break;

				case 7:
					prngRange = 21;
					break;

				case 8:
					prngRange = 22;
					break;

				case 9:
					prngRange = 23;
					break;

				default:
					prngRange = 24;
					break;
				}

			switch (Rnd.Next (prngRange))
				{
				// Аптечки
				default:
					SW.Write ("\"classname\" \"item_healthkit\"\n");
					break;

				// Броня
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					SW.Write ("\"classname\" \"item_battery\"\n");
					break;

				// Гранаты
				case 6:
				case 17:
					SW.Write ("\"classname\" \"weapon_handgrenade\"\n");
					break;

				// Пистолет
				case 15:
					SW.Write ("\"classname\" \"weapon_9mmhandgun\"\n");
					break;

				// Гранаты с радиоуправлением
				case 16:
					SW.Write ("\"classname\" \"weapon_satchel\"\n");
					break;

				// .357
				case 5:
				case 18:
					SW.Write ("\"classname\" \"weapon_357\"\n");
					break;

				// Арбалет
				case 19:
				case 20:
					SW.Write ("\"classname\" \"weapon_crossbow\"\n");
					break;

				// Гаусс
				case 21:
				case 22:
					SW.Write ("\"classname\" \"weapon_gauss\"\n");
					break;

				// Монтировка или радиограната
				case 23:
					if (Rnd.Next (5) > 3)
						SW.Write ("\"classname\" \"weapon_crowbar\"\n");
					else
						SW.Write ("\"classname\" \"weapon_satchel\"\n");
					break;
				}

			SW.Write ("\"angles\" \"0 " + Rnd.Next (360) + " 0\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 0\"\n");
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод добавляет врагов на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="MapNumber">Номер карты, позволяющий выполнять наполнение с прогрессом</param>
		public static void WriteMapEnemy (StreamWriter SW, Point RelativePosition, Random Rnd, uint MapNumber)
			{
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			SW.Write ("{\n");

			// Диапазон противников задаётся ограничением на верхнюю границу диапазона ГПСЧ
			int prngRange;
			switch (MapNumber)
				{
				case 0:
				case 1:
				case 2:
					prngRange = 10;
					break;

				case 3:
					prngRange = 11;
					break;

				case 4:
					prngRange = 12;
					break;

				case 5:
					prngRange = 13;
					break;

				case 6:
					prngRange = 14;
					break;

				case 7:
					prngRange = 15;
					break;

				case 8:
					prngRange = 16;
					break;

				case 9:
					prngRange = 17;
					break;

				case 10:
					prngRange = 18;
					break;

				default:
					prngRange = 19;
					break;
				}

			// Добавление
			switch (Rnd.Next (prngRange))
				{
				// Солдаты
				default:
					SW.Write ("\"classname\" \"monster_human_grunt\"\n");
					SW.Write ("\"weapons\" \"" + gruntWeapons[Rnd.Next (gruntWeapons.Length)] + "\"\n");
					break;

				// Зомби
				case 0:
				case 17:
					SW.Write ("\"classname\" \"monster_zombie\"\n");
					SW.Write ("\"skin\" \"" + Rnd.Next (2).ToString () + "\"\n");
					break;

				// Крабы
				case 1:
				case 18:
					SW.Write ("\"classname\" \"monster_headcrab\"\n");
					break;

				// Алиены
				case 10:
					SW.Write ("\"classname\" \"monster_alien_slave\"\n");
					break;

				// Куры
				case 11:
					SW.Write ("\"classname\" \"monster_bullchicken\"\n");
					break;

				// Ассассины
				case 12:
				case 13:
					SW.Write ("\"classname\" \"monster_human_assassin\"\n");
					break;

				// Турели
				case 14:
					SW.Write ("\"classname\" \"monster_miniturret\"\n");
					SW.Write ("\"spawnflags\" \"32\"\n");
					SW.Write ("\"orientation\" \"0\"\n");
					break;

				// Турели
				case 15:
					SW.Write ("\"classname\" \"monster_turret\"\n");
					SW.Write ("\"spawnflags\" \"32\"\n");
					SW.Write ("\"orientation\" \"0\"\n");
					break;

				// Солдаты алиенов
				case 16:
					SW.Write ("\"classname\" \"monster_alien_grunt\"\n");
					break;
				}

			SW.Write ("\"angles\" \"0 " + Rnd.Next (360) + " 0\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 0\"\n");
			SW.Write ("}\n");
			}
		private static string[] gruntWeapons = new string[] { "1", "3", "5", "8", "10" };

		/// <summary>
		/// Метод записывает дверь на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		public static void WriteMapGate (StreamWriter SW, Point RelativePosition)
			{
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_door\"\n");
			SW.Write ("\"angles\" \"90 0 0\"\n");
			SW.Write ("\"speed\" \"100\"\n");
			SW.Write ("\"movesnd\" \"3\"\n");
			SW.Write ("\"stopsnd\" \"1\"\n");
			SW.Write ("\"wait\" \"-1\"\n");
			SW.Write ("\"lip\" \"4\"\n");

			WriteMapWall (SW, RelativePosition, "MetalGate06", false);
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает стену на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура стены</param>
		/// <param name="ToSky">Флаг, указывающий на необходимость довести стену до неба</param>
		public static void WriteMapWall (StreamWriter SW, Point RelativePosition, string Texture, bool ToSky)
			{
			bool vertical = (RelativePosition.X % 2 == 0);
			int x1, y1, x2, y2;
			int h = WallHeightInt;
			if (ToSky)
				h += 16;

			SW.Write ("{\n");

			if (vertical)
				{
				x1 = RelativePosition.X * WallLength / 2;
				y1 = (RelativePosition.Y - 1) * WallLength / 2;
				y2 = (RelativePosition.Y + 1) * WallLength / 2;

				SW.Write ("( " + (x1 + 8).ToString () + " " + (y2 - 8).ToString () + " " + h.ToString () + " ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " " + h.ToString () + " ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 - 8).ToString () + " " + (y2 - 8).ToString () + " 0 ) " +
					"( " + (x1 - 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " 0 ) " +
					Texture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 - 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x1 - 8).ToString () + " " + (y2 - 8).ToString () + " 0 ) " +
					"( " + (x1 - 8).ToString () + " " + (y2 - 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 + 8).ToString () + " " + (y2 - 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + x1.ToString () + " " + y1.ToString () + " 0 ) " +
					"( " + (x1 - 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x1 - 8).ToString () + " " + (y1 + 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + x1.ToString () + " " + y2.ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y2 - 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y2 - 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 - 8).ToString () + " " + (y2 - 8).ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y2.ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y2.ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				}
			else
				{
				y1 = RelativePosition.Y * WallLength / 2;
				x1 = (RelativePosition.X - 1) * WallLength / 2;
				x2 = (RelativePosition.X + 1) * WallLength / 2;

				SW.Write ("( " + (x2 - 8).ToString () + " " + (y1 - 8).ToString () + " " + h.ToString () + " ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 - 8).ToString () + " " + h.ToString () + " ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + (x2 - 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " 0 ) " +
					Texture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x2 - 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x2 - 8).ToString () + " " + (y1 + 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x2 - 8).ToString () + " " + (y1 - 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 - 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 - 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + x1.ToString () + " " + y1.ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + (x1 + 8).ToString () + " " + (y1 + 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x1 + 8).ToString () + " " + (y1 - 8).ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " 0 ) " +
					"( " + x1.ToString () + " " + y1.ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + x2.ToString () + " " + y1.ToString () + " 0 ) " +
					"( " + (x2 - 8).ToString () + " " + (y1 - 8).ToString () + " 0 ) " +
					"( " + (x2 - 8).ToString () + " " + (y1 - 8).ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x2 - 8).ToString () + " " + (y1 + 8).ToString () + " 0 ) " +
					"( " + x2.ToString () + " " + y1.ToString () + " 0 ) " +
					"( " + x2.ToString () + " " + y1.ToString () + " " + h.ToString () + " ) " +
					Texture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				}

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает декаль на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Decal">Текстура декали</param>
		/// <param name="Top">Флаг отрисовки декали на потолке вместо пола</param>
		public static void WriteMapDecal (StreamWriter SW, Point RelativePosition, string Decal, bool Top)
			{
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"infodecal\"\n");
			SW.Write ("\"texture\" \"" + Decal + "\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " " + (Top ? WallHeightString : "0") + "\"\n");
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает звуковое сопровождение и эффект помещения на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Ambient">Тип эмбиента</param>
		/// <param name="Sound">Звук</param>
		public static void WriteMapSound (StreamWriter SW, Point RelativePosition, string Sound, AmbientTypes Ambient)
			{
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			int h;
			switch (Ambient)
				{
				case AmbientTypes.Echo:
					h = 2;
					break;

				case AmbientTypes.Sky:
					h = 4;
					break;

				default:
					h = 7;
					break;
				}

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"ambient_generic\"\n");
			SW.Write ("\"spawnflags\" \"2\"\n");
			SW.Write ("\"message\" \"ambience/" + Sound + ".wav\"\n");
			SW.Write ("\"health\" \"" + h.ToString () + "\"\n");
			SW.Write ("\"pitch\" \"100\"\n");
			SW.Write ("\"pitchstart\" \"100\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 88\"\n");

			if (Ambient != AmbientTypes.None)
				{
				SW.Write ("}\n{\n");
				SW.Write ("\"classname\" \"env_sound\"\n");
				SW.Write ("\"radius\" \"192\"\n");
				SW.Write ("\"roomtype\" \"" + ((Ambient == AmbientTypes.Sky) ? "0" : "18") + "\"\n");
				SW.Write ("\"origin\" \"" + x + " " + y + " 64\"\n");
				}

			SW.Write ("}\n");
			}

		/// <summary>
		/// Варианты эмбиента звука
		/// </summary>
		public enum AmbientTypes
			{
			/// <summary>
			/// На открытом пространстве
			/// </summary>
			Sky,

			/// <summary>
			/// В помещении
			/// </summary>
			Echo,

			/// <summary>
			/// Отключён
			/// </summary>
			None
			}

		/// <summary>
		/// Метод записывает декали пути от входа к выходу на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="StartOrFinish">Флаг указывает на начальную или конечную точку пути</param>
		public static void WriteMapPathTrace (StreamWriter SW, Point RelativePosition, bool StartOrFinish)
			{
			string decal = "{foot_r";
			if (StartOrFinish)
				decal = "{target";
			else if (leftStep)
				decal = "{foot_l";

			WriteMapDecal (SW, RelativePosition, decal, false);

			leftStep = !leftStep;
			}
		private static bool leftStep = false;

		/// <summary>
		/// Метод записывает заголовок карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="MapNumber">Текущий номер карты</param>
		public static void WriteMapHeader (StreamWriter SW, uint MapNumber)
			{
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"worldspawn\"\n");
			SW.Write ("\"message\" \"ESHQ: Randomaze #" + MapNumber.ToString (MapsNumbersFormat) + " by FDL\"\n");
			SW.Write ("\"MaxRange\" \"3000\"\n");
			SW.Write ("\"mapversion\" \"220\"\n");
			SW.Write ("\"skyname\" \"eshq_desmor_\"\n");
			SW.Write ("\"light\" \"1\"\n");
			SW.Write ("\"sounds\" \"1\"\n");
			SW.Write ("\"WaveHeight\" \"0.1\"\n");
			SW.Write ("\"newunit\" \"1\"\n");
			SW.Write ("\"wad\" \"" + RDGenerics.AppStartupPath + "..\\valve\\eshq.wad\"\n");

			if (MapNumber == 1)
				{
				SW.Write ("\"chaptertitle\" \"" + ProgramDescription.AssemblyTitle + "\"\n");
				SW.Write ("\"startdark\" \"1\"\n");
				SW.Write ("\"gametitle\" \"1\"\n");
				}
			}

		/// <summary>
		/// Метод записывает закрывающий элемент геометрии карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		public static void WriteMapTerminator (StreamWriter SW)
			{
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает пол и потолок на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="Section">Секция карты</param>
		/// <param name="FloorTexture">Текстура пола</param>
		/// <param name="RoofTexture">Текстура потолка</param>
		/// <param name="RealMapHeight">Реальная (в точках) ширина карты</param>
		/// <param name="RealMapWidth">Реальная (в точках) длина карты</param>
		public static void WriteMapRoofFloor (StreamWriter SW, byte Section, int RealMapWidth, int RealMapHeight,
			string RoofTexture, string FloorTexture)
			{
			bool negX = ((Section & NegativeX) != 0);
			bool negY = ((Section & NegativeY) != 0);

			string x1 = (negX ? (-RealMapWidth / 2) : 0).ToString ();
			string x2 = (negX ? 0 : (RealMapWidth / 2)).ToString ();
			string y1 = (negY ? (-RealMapHeight / 2) : 0).ToString ();
			string y2 = (negY ? 0 : (RealMapHeight / 2)).ToString ();

			SW.Write ("{\n");
			SW.Write ("( " + x2 + " " + y1 + " 0 ) ( " + x1 + " " + y1 + " 0 ) ( " + x1 + " " + y2 + " 0 ) " +
				FloorTexture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("( " + x2 + " " + y2 + " -16 ) ( " + x1 + " " + y2 + " -16 ) ( " + x1 + " " + y1 + " -16 ) " +
				FloorTexture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("( " + x1 + " " + y2 + " -16 ) ( " + x2 + " " + y2 + " -16 ) ( " + x2 + " " + y2 + " 0 ) " +
				FloorTexture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + x2 + " " + y1 + " -16 ) ( " + x1 + " " + y1 + " -16 ) ( " + x1 + " " + y1 + " 0 ) " +
				FloorTexture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + x1 + " " + y1 + " -16 ) ( " + x1 + " " + y2 + " -16 ) ( " + x1 + " " + y2 + " 0 ) " +
				FloorTexture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + x2 + " " + y2 + " -16 ) ( " + x2 + " " + y1 + " -16 ) ( " + x2 + " " + y1 + " 0 ) " +
				FloorTexture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("}\n{\n");

			int h1 = WallHeightInt + 32;
			int h2 = WallHeightInt;
			if (IsSkyTexture (RoofTexture))
				h2 += 16;

			SW.Write ("( " + x2 + " " + y1 + " " + h1.ToString () + " ) " +
				"( " + x1 + " " + y1 + " " + h1.ToString () + " ) " +
				"( " + x1 + " " + y2 + " " + h1.ToString () + " ) " +
				RoofTexture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("( " + x2 + " " + y2 + " " + h2.ToString () + " ) " +
				"( " + x1 + " " + y2 + " " + h2.ToString () + " ) " +
				"( " + x1 + " " + y1 + " " + h2.ToString () + " ) " +
				RoofTexture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
			SW.Write ("( " + x1 + " " + y2 + " " + h2.ToString () + " ) " +
				"( " + x2 + " " + y2 + " " + h2.ToString () + " ) " +
				"( " + x2 + " " + y2 + " " + h1.ToString () + " ) " +
				RoofTexture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + x2 + " " + y1 + " " + h2.ToString () + " ) " +
				"( " + x1 + " " + y1 + " " + h2.ToString () + " ) " +
				"( " + x1 + " " + y1 + " " + h1.ToString () + " ) " +
				RoofTexture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + x1 + " " + y1 + " " + h2.ToString () + " ) " +
				"( " + x1 + " " + y2 + " " + h2.ToString () + " ) " +
				"( " + x1 + " " + y2 + " " + h1.ToString () + " ) " +
				RoofTexture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("( " + x2 + " " + y2 + " " + h2.ToString () + " ) " +
				"( " + x2 + " " + y1 + " " + h2.ToString () + " ) " +
				"( " + x2 + " " + y1 + " " + h1.ToString () + " ) " +
				RoofTexture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод проверяет текстуру на соответствие псевдониму неба
		/// </summary>
		/// <param name="Texture">Текстура для проверки</param>
		/// <returns>Возвращает true, если текстура является псевдонимом неба</returns>
		public static bool IsSkyTexture (string Texture)
			{
			return (Texture == SkyTexture);
			}

		/// <summary>
		/// Возвращает имя текстуры неба
		/// </summary>
		public const string SkyTexture = "sky";

		/// <summary>
		/// Метод получает секцию по координатам точки
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <returns>Возвращает номер секции карты</returns>
		public static byte GetSection (Point RelativePosition)
			{
			byte section = 0;
			if (RelativePosition.X < 0)
				section += NegativeX;
			if (RelativePosition.Y < 0)
				section += NegativeY;

			return section;
			}
		private const int NegativeX = 0x01;
		private const int NegativeY = 0x02;

		/// <summary>
		/// Метод записывает освещение на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="RoofTexture">Текстура потолка</param>
		/// <param name="AddBulb">Флаг указывает, что добавляется лампочка, а не источник света</param>
		public static bool WriteMapLight (StreamWriter SW, Point RelativePosition, string RoofTexture, bool AddBulb)
			{
			// Защита
			if (MapSupport.IsSkyTexture (RoofTexture) && EnvironmentAdded)
				return false;

			// Создание цвета
			if (string.IsNullOrWhiteSpace (lightColor))
				{
				Random rnd = new Random ();
				lightColor = "\"_light\" \"" + (224 + rnd.Next (32)).ToString () + " " +
					(224 + rnd.Next (32)).ToString () + " " +
					(112 + rnd.Next (32)).ToString () + " 125\"\n";
				}

			// Добавление атмосферного освещения
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			if (!AddBulb && IsSkyTexture (RoofTexture))
				{
				SW.Write ("{\n");
				SW.Write ("\"classname\" \"light_environment\"\n");
				SW.Write (lightColor);
				SW.Write ("\"_fade\" \"1.0\"\n");
				SW.Write ("\"angles\" \"330 180 0\"\n");
				SW.Write ("\"origin\" \"" + x.ToString () + " " + y.ToString () + " " +
					(WallHeightInt - 8).ToString () + "\"\n");
				SW.Write ("}\n");

				EnvironmentAdded = true;
				return true;
				}

			// Добавление лампы
			if (!AddBulb)
				{
				SW.Write ("{\n");
				SW.Write ("\"classname\" \"light\"\n");
				SW.Write (lightColor);
				SW.Write ("\"_fade\" \"1.0\"\n");
				SW.Write ("\"origin\" \"" + x.ToString () + " " + y.ToString () + " " +
					(WallHeightInt - 12).ToString () + "\"\n");
				SW.Write ("}\n");
				}
			else if (!IsSkyTexture (RoofTexture))
				{
				/*SW.Write ("\"classname\" \"func_wall\"\n");
				SW.Write ("\"rendercolor\" \"0 0 0\"\n");
				SW.Write ("\"zhlt_lightflags\" \"2\"\n");*/
				SW.Write ("{\n");

				SW.Write ("( " + (x + 16).ToString () + " " + (y - 16).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 16).ToString () + " " + (y - 16).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 16).ToString () + " " + (y + 16).ToString () + " " + WallHeightString + " ) " +
					RoofTexture + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + (x + 16).ToString () + " " + (y + 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"( " + (x + 16).ToString () + " " + (y + 16).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 16).ToString () + " " + (y + 16).ToString () + " " + WallHeightString + " ) " +
					RoofTexture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x - 16).ToString () + " " + (y - 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"( " + (x - 16).ToString () + " " + (y - 16).ToString () + " " + WallHeightString + " ) " +
					"( " + (x + 16).ToString () + " " + (y - 16).ToString () + " " + WallHeightString + " ) " +
					RoofTexture + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x - 16).ToString () + " " + (y + 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"( " + (x - 16).ToString () + " " + (y + 16).ToString () + " " + WallHeightString + " ) " +
					"( " + (x - 16).ToString () + " " + (y - 16).ToString () + " " + WallHeightString + " ) " +
					RoofTexture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x + 16).ToString () + " " + (y - 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"( " + (x + 16).ToString () + " " + (y - 16).ToString () + " " + WallHeightString + " ) " +
					"( " + (x + 16).ToString () + " " + (y + 16).ToString () + " " + WallHeightString + " ) " +
					RoofTexture + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + (x + 16).ToString () + " " + (y + 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"( " + (x - 16).ToString () + " " + (y + 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"( " + (x - 16).ToString () + " " + (y - 16).ToString () + " " + (WallHeightInt - 4).ToString () + " ) " +
					"~LAMP07 [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("}\n");

				return false;
				}

			return true;
			}

		/// <summary>
		/// Возвращает true, если небесный свет уже был добавлен
		/// </summary>
		public static bool EnvironmentAdded = false;
		private static string lightColor = "";

		/*/// <summary>
		/// Возвращает общее число созданных динамических объектов
		/// </summary>
		public static uint TotalEntities
			{
			get
				{
				return totalEntities;
				}
			}
		private static uint totalEntities = 0;*/
		}
	}
