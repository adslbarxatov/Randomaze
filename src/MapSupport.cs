using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RD_AAOW
	{
	/// <summary>
	/// Результаты проверки позиции на соседство с целевыми объектами.
	/// Образуют флаговое поле
	/// </summary>
	public enum CPResults
		{
		/// <summary>
		/// Нет объектов
		/// </summary>
		None = 0x0,

		/// <summary>
		/// Справа
		/// </summary>
		Right = 0x1,

		/// <summary>
		/// Слева
		/// </summary>
		Left = 0x2,

		/// <summary>
		/// Снизу
		/// </summary>
		Down = 0x4,

		/// <summary>
		/// Сверху
		/// </summary>
		Up = 0x8
		}

	/// <summary>
	/// Класс обеспечивает вспомогательные методы для создания карт Xash3D
	/// </summary>
	public static class MapSupport
		{
		/// <summary>
		/// Стандартная высота стен на картах
		/// </summary>
		public static int WallHeight
			{
			get
				{
				return wallHeight;
				}
			}
		private static int wallHeight;

		/// <summary>
		/// Возвращает стандартную высоту стен лабиринта
		/// </summary>
		public const int DefaultWallHeight = 128;

		/// <summary>
		/// Возвращает флаг двухэтажного режима
		/// </summary>
		public static bool TwoFloors
			{
			get
				{
				return (wallHeight > DefaultWallHeight);
				}
			}

		/// <summary>
		/// Стандартная длина стен на картах
		/// </summary>
		public const int WallLength = 128;

		/// <summary>
		/// Возвращает максимально допустимое количество карт
		/// </summary>
		public const int MapsLimit = 999;

		/// <summary>
		/// Метод формирует каноничное имя карты по её номеру
		/// </summary>
		/// <param name="MapNumber">Номер карты</param>
		/// <returns>Строка с название карты</returns>
		public static string BuildMapName (uint MapNumber)
			{
			return RandomazeForm.MainAlias + MapNumber.ToString ("D3");
			}

		/// <summary>
		/// Метод записывает заголовок карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="MapNumber">Текущий номер карты</param>
		/// <param name="Lightness">Уровень затемнения неба (0.0 – 1.0)</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="TwoFloors">Инициализация двухэтажной карты</param>
		public static void WriteMapHeader (StreamWriter SW, uint MapNumber, Random Rnd, float Lightness,
			bool TwoFloors)
			{
			// Начало карты
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"worldspawn\"\n");
			SW.Write ("\"message\" \"ES: Randomaze map " + BuildMapName (MapNumber) + " by FDL\"\n");
			SW.Write ("\"mapversion\" \"220\"\n");

			// Инициализация неба
			skyIndex = Rnd.Next (skyTypes.Length / 2);

			float inc = Lightness;
			if (inc < 0.0f)
				inc = 0.0f;
			if (inc > 1.0f)
				inc = 1.0f;
			inc = (1.0f - inc) * skyTypes.Length / 2.0f;

			skyIndex += (int)inc;
			SW.Write ("\"skyname\" \"" + skyTypes[skyIndex] + "\"\n");

			// Параметры карты
			SW.Write ("\"MaxRange\" \"3000\"\n");
			SW.Write ("\"light\" \"1\"\n");
			SW.Write ("\"sounds\" \"1\"\n");
			SW.Write ("\"WaveHeight\" \"0.1\"\n");
			SW.Write ("\"newunit\" \"1\"\n");
			SW.Write ("\"wad\" \"" + RandomazeForm.MainWAD + "\"\n");

			// Параметры первой карты
			if (MapNumber == 1)
				{
				SW.Write ("\"chaptertitle\" \"" + ProgramDescription.AssemblyTitle + "\"\n");
				SW.Write ("\"startdark\" \"1\"\n");
				SW.Write ("\"gametitle\" \"1\"\n");
				}

			// Создание цвета ламп
			if (string.IsNullOrWhiteSpace (lightColor))
				{
				lightColor = "\"_light\" \"" + (224 + Rnd.Next (32)).ToString () + " " +
					(224 + Rnd.Next (32)).ToString () + " " +
					(112 + Rnd.Next (32)).ToString () + " " + (TwoFloors ? "200" : "150") + "\"\n";
				subLightColor = "\"_light\" \"" + (224 + Rnd.Next (32)).ToString () + " " +
					(224 + Rnd.Next (32)).ToString () + " " +
					(112 + Rnd.Next (32)).ToString () + " 100\"\n";
				}

			// Выбор высоты карты
			wallHeight = DefaultWallHeight;
			if (TwoFloors)
				wallHeight *= 2;
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
		/// Метод формирует абсолютные координаты объекта карты из относительных
		/// </summary>
		/// <param name="RelativePosition">Относительная точка</param>
		public static Point EvaluateAbsolutePosition (Point RelativePosition)
			{
			return new Point (RelativePosition.X * WallLength / 2, RelativePosition.Y * WallLength / 2);
			}

		// Метод записывает точку выхода с карты
		private static void WriteMapEndPoint_Finish (StreamWriter SW, Point RelativePosition)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string x1 = (p.X - 8).ToString ();
			string y1 = (p.Y - 8).ToString ();
			string x2 = (p.X + 8).ToString ();
			string y2 = (p.Y + 8).ToString ();
			string z1 = "16";
			string z2 = (wallHeight - 16).ToString ();
			string z3 = (wallHeight - 8).ToString ();

			// Запись
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"game_end\"\n");
			SW.Write ("\"targetname\" \"DevEnd02\"\n");
			SW.Write ("\"origin\" \"" + x1 + " " + y1 + " " + z3 + "\"\n");

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
			SW.Write ("\"origin\" \"" + x1 + " " + y2 + " " + z3 + "\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"multi_manager\"\n");
			SW.Write ("\"targetname\" \"DevM\"\n");
			SW.Write ("\"DevEnd01\" \"0\"\n");
			SW.Write ("\"DevEnd02\" \"5.5\"\n");
			SW.Write ("\"origin\" \"" + x2 + " " + y1 + " " + z3 + "\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_once\"\n");
			SW.Write ("\"target\" \"DevM\"\n");

			WriteBlock (SW, x1, y1, z1, x2, y2, z2,
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
					TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");

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
			// Защита
			if (MapNumber >= MapsLimit)
				{
				WriteMapEndPoint_Finish (SW, RelativePosition);
				return;
				}

			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string mapName = BuildMapName (MapNumber + 1);

			// Запись
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"info_landmark\"\n");
			SW.Write ("\"targetname\" \"" + mapName + "m\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 40\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_changelevel\"\n");
			SW.Write ("\"map\" \"" + mapName + "\"\n");
			SW.Write ("\"landmark\" \"" + mapName + "m\"\n");

			WriteBlock (SW, (p.X - 8).ToString (), (p.Y - 8).ToString (), "16",

				(p.X + 8).ToString (), (p.Y + 8).ToString (), (wallHeight - 16).ToString (),

				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
					TriggerTexture, TriggerTexture },

				BlockTypes.Default);

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_autosave\"\n");

			WriteBlock (SW, (p.X - 32).ToString (), (p.Y - 32).ToString (), "12",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "16",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");

			WriteMapPortal (SW, RelativePosition, true);
			}

		// Метод создаёт портал на карте
		private static void WriteMapPortal (StreamWriter SW, Point RelativePosition, bool Exit)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
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

			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
				(DefaultWallHeight / 2).ToString () + "\"\n");
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
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string xs = p.X.ToString ();
			string ys = p.Y.ToString ();

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
				SW.Write ("\"targetname\" \"" + BuildMapName (MapNumber) + "m\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");

				SW.Write ("}\n{\n");
				SW.Write ("\"classname\" \"trigger_changelevel\"\n");
				SW.Write ("\"map\" \"" + BuildMapName (MapNumber - 1) + "\"\n");
				SW.Write ("\"landmark\" \"" + BuildMapName (MapNumber) + "m\"\n");

				WriteBlock (SW, (p.X - 8).ToString (), (p.Y - 8).ToString (), (wallHeight - 1).ToString (),
					(p.X + 8).ToString (), (p.Y + 8).ToString (), wallHeight.ToString (),
					new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

				SW.Write ("}\n");
				}

			WriteMapPortal (SW, RelativePosition, false);
			WriteMapSound (SW, RelativePosition, "Teleport1", AmbientTypes.None);
			}

		// Метод записывает блок по указанным коориданатм
		private static void WriteBlock (StreamWriter SW, string X1, string Y1, string Z1,
			string X2, string Y2, string Z2, string[] Textures, BlockTypes BlockType)
			{
			// Расчёт параметров
			string texOffsetX, texOffsetY, texScale;
			switch (BlockType)
				{
				case BlockTypes.Default:
				default:
					texOffsetX = texOffsetY = "0";
					texScale = "1 1";
					break;

				case BlockTypes.Crate:
					texOffsetX = texOffsetY = "32";
					texScale = "0.5 0.5";
					break;

				case BlockTypes.Door:
					texOffsetX = "32";
					texOffsetY = "0";
					texScale = "1 1";
					break;

				case BlockTypes.Button:
					texOffsetX = texOffsetY = "16";
					texScale = "0.5 0.5";
					break;
				}

			// Запись
			SW.Write ("{\n");
			SW.Write ("( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				Textures[0] + " [ 1 0 0 " + texOffsetX + " ] [ 0 -1 0 " + texOffsetY + " ] 0 " + texScale + " \n");
			SW.Write ("( " + X2 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X2 + " " + Y2 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				Textures[1] + " [ 1 0 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 -" + texScale + " \n");
			SW.Write ("( " + X1 + " " + Y1 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				Textures[2] + " [ 1 0 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
			SW.Write ("( " + X1 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				Textures[3] + " [ 0 1 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 -" + texScale + " \n");
			SW.Write ("( " + X2 + " " + Y1 + " " + Z1 + " ) " +
				"( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X2 + " " + Y2 + " " + Z2 + " ) " +
				Textures[4] + " [ 0 1 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
			SW.Write ("( " + X2 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z1 + " ) " +
				Textures[5] + " [ 1 0 0 " + texOffsetX + " ] [ 0 -1 0 " + texOffsetY + " ] 0 " + texScale + " \n");
			SW.Write ("}\n");
			}

		// Возможные типы блоков
		private enum BlockTypes
			{
			Default = 0,
			Crate = 1,
			Door = 2,
			Button = 3
			}

		// Стандартная текстура триггера
		private const string TriggerTexture = "AAATRIGGER";

		/// <summary>
		/// Метод добавляет собираемые объекты на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="MapNumber">Номер текущей карты</param>
		/// <param name="AllowSecondFloor">Флаг, разрешающий размещение на внутренних площадках</param>
		public static void WriteMapItem (StreamWriter SW, Point RelativePosition, Random Rnd, uint MapNumber,
			bool AllowSecondFloor)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
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
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
					prngRange = (int)MapNumber + 14;
					break;

				default:
					prngRange = 25;
					break;
				}

			// Обходной вариант с собираемым объектом
			if (!hiddenObjectWritten && (MapNumber % 10 == 0))
				{
				hiddenObjectWritten = true;
				SW.Write ("\"classname\" \"item_antidote\"\n");
				SW.Write ("\"MinimumToTrigger\" \"1\"\n");
				goto finishItem;
				}

			// Запись объекта
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

				// Улей или граната
				case 24:
					if (Rnd.Next (5) > 3)
						SW.Write ("\"classname\" \"weapon_hornetgun\"\n");
					else
						SW.Write ("\"classname\" \"weapon_handgrenade\"\n");
					break;
				}

finishItem:
			int z = 40;
			if (AllowSecondFloor)
				z += (Rnd.Next (2) * DefaultWallHeight);

			SW.Write ("\"angles\" \"0 " + Rnd.Next (360) + " 0\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
				z.ToString () + "\"\n");   // На некоторой высоте над полом
			SW.Write ("}\n");
			}
		private static bool hiddenObjectWritten = false;

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		/// <param name="MapNumber">Номер текущей карты, используемый для создания уникального имени кнопки</param>
		/// <param name="Texture">Текстура секции</param>
		public static void WriteMapButton (StreamWriter SW, Point RelativePosition, string Texture,
			uint MapNumber)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_button\"\n");
			SW.Write ("\"target\" \"Gate" + BuildMapName (MapNumber) + "\"\n");
			SW.Write ("\"spawnflags\" \"1\"\n");
			SW.Write ("\"delay\" \"1\"\n");
			SW.Write ("\"speed\" \"50\"\n");
			SW.Write ("\"sounds\" \"11\"\n");
			SW.Write ("\"wait\" \"-1\"\n");

			WriteBlock (SW, (p.X - 8).ToString (), (p.Y - 8).ToString (), "0",
				(p.X + 8).ToString (), (p.Y + 8).ToString (), "40",

				new string[] { "+A_SWITCH01", Texture, Texture, Texture, Texture, Texture },

				BlockTypes.Button);

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_autosave\"\n");

			WriteBlock (SW, (p.X - 32).ToString (), (p.Y - 32).ToString (), "12",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "16",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает дверь на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура двери</param>
		public static void WriteMapDoor (StreamWriter SW, Point RelativePosition, string Texture)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string x1, y1, x2, y2;

			// Вертикальная
			if (WallsSupport.IsWallVertical (RelativePosition))
				{
				x1 = (p.X - 8).ToString ();
				y1 = (p.Y - 32).ToString ();
				x2 = (p.X + 8).ToString ();
				y2 = (p.Y + 32).ToString ();
				}
			else
				{
				x1 = (p.X - 32).ToString ();
				y1 = (p.Y - 8).ToString ();
				x2 = (p.X + 32).ToString ();
				y2 = (p.Y + 8).ToString ();
				}

			// Запись
			WriteBlock (SW, x1, y1, "0", x2, y2, "96", new string[] { Texture, Texture, Texture,
				Texture, Texture, Texture }, BlockTypes.Door);
			}

		/// <summary>
		/// Метод записывает ящик на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="AllowExplosives">Флаг разрешения ящиков со взрывчаткой</param>
		/// <param name="AllowItems">Флаг разрешения ящиков с жуками и собираемыми объектами</param>
		public static void WriteMapCrate (StreamWriter SW, Point RelativePosition, Random Rnd,
			bool AllowItems, bool AllowExplosives)
			{
			// Контроль
			if (!AllowExplosives && !AllowItems)
				return;

			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string x1 = (p.X - 16).ToString ();
			string y1 = (p.Y - 16).ToString ();
			string x2 = (p.X + 16).ToString ();
			string y2 = (p.Y + 16).ToString ();

			bool explosive = (Rnd.Next (2) == 0);
			string tex = "CRATE01"; // Взрывчатка по умолчанию

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_pushable\"\n");
			SW.Write ("\"health\" \"20\"\n");
			SW.Write ("\"material\" \"1\"\n");
			SW.Write ("\"spawnflags\" \"128\"\n");
			SW.Write ("\"friction\" \"40\"\n");
			SW.Write ("\"buoyancy\" \"60\"\n");
			SW.Write ("\"rendermode\" \"4\"\n");    // Прозрачность для врагов
			SW.Write ("\"renderamt\" \"255\"\n");

			// Разрешение для взрывчатки будет необходимым, но недостаточным условием для её появления:
			// решающим фактором будет ГПСЧ.
			// Запрет на остальные ящики будет достаточным условием для появления взрывчатки.
			// При этом предполагается, что случай обоюдного запрета до этого места не дойдёт
			if (explosive && AllowExplosives || !AllowItems)
				{
				SW.Write ("\"explodemagnitude\" \"" + Rnd.Next (160, 200).ToString () + "\"\n");
				}
			else
				{
				int r = Rnd.Next (4);

				// Враги
				if (r < 3)
					{
					SW.Write ("\"spawnobject\" \"" + (r + 27).ToString () + "\"\n");
					}

				// Пустые или редкие ящики
				else
					{
					// Иногда добавлять случайное оружие или предмет
					if (Rnd.Next (3) == 0)
						SW.Write ("\"spawnobject\" \"" + (Rnd.Next (26) + 1).ToString () + "\"\n");

					// Случайная текстура для ящиков без врагов
					r = Rnd.Next (3);
					}

				switch (r)
					{
					case 0:
						tex = "CRATE04";
						break;

					case 1:
						tex = "CRATE07";
						break;

					case 2:
					default:
						tex = "CRATE08";
						break;
					}
				}

			WriteBlock (SW, x1, y1, "0", x2, y2, "64", new string[] { tex, tex, tex, tex, tex, tex },
				BlockTypes.Crate);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает звуковое сопровождение и эффект помещения на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Ambient">Тип эмбиента</param>
		/// <param name="Sound">Звук</param>
		public static void WriteMapSound (StreamWriter SW, Point RelativePosition, string Sound,
			AmbientTypes Ambient)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string x = p.X.ToString ();
			string y = p.Y.ToString ();

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

			// Запись
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
		/// Метод записывает отметки пути от входа к выходу на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="StartOrFinish">Флаг указывает на начальную или конечную точку пути</param>
		public static void WriteMapPathStone (StreamWriter SW, Point RelativePosition, bool StartOrFinish)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string tex;
			if (StartOrFinish)
				tex = "~Path02";
			else
				tex = "~Path01";

			WriteBlock (SW, (p.X - 8).ToString (), (p.Y - 8).ToString (), "-8",
				(p.X + 8).ToString (), (p.Y + 8).ToString (), "0",
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			}

		// Параметры неба
		private static string[] skyTypes = new string[] {
			"eshq_citday_",
			"eshq_desday_",
			"eshq_desmor_",
			"eshq_out_",
			"eshq_seanig_",
			"eshq_firmor_"
			};
		private static string[] sunColors = new string[] {
			"255 255 128 200",
			"255 255 128 200",
			"255 224 128 180",
			"160 128 96 120",
			"96 128 160 150",
			"128 128 128 150",
			};
		private static string[] sunAngles = new string[] {
			"280 170 0",
			"290 160 0",
			"330 175 0",
			"340 135 0",
			"90 0 0",
			"90 0 0"
			};
		private static int skyIndex;

		/// <summary>
		/// Метод записывает пол и потолок на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="Section">Секция карты</param>
		/// <param name="FloorTexture">Текстура пола</param>
		/// <param name="RoofTexture">Текстура потолка</param>
		/// <param name="RelativeMapHeight">Относительная ширина карты</param>
		/// <param name="RelativeMapWidth">Относительная длина карты</param>
		public static void WriteMapCeilingAndFloor (StreamWriter SW, byte Section, int RelativeMapWidth,
			int RelativeMapHeight, string RoofTexture, string FloorTexture)
			{
			// Расчёт параметров
			bool negX = ((Section & NegativeX) != 0);
			bool negY = ((Section & NegativeY) != 0);
			int realMapWidth = RelativeMapWidth * WallLength;
			int realMapHeight = RelativeMapHeight * WallLength;

			string x1 = (negX ? (-realMapWidth / 2) : 0).ToString ();
			string x2 = (negX ? 0 : (realMapWidth / 2)).ToString ();
			string y1 = (negY ? (-realMapHeight / 2) : 0).ToString ();
			string y2 = (negY ? 0 : (realMapHeight / 2)).ToString ();

			string h2 = (wallHeight + 32).ToString ();
			string h1 = (IsSkyTexture (RoofTexture) ? (wallHeight + 16) : wallHeight).ToString ();

			// Запись
			WriteBlock (SW, x1, y1, "-16", x2, y2, "0", new string[] { FloorTexture, FloorTexture, FloorTexture,
				FloorTexture, FloorTexture, FloorTexture }, BlockTypes.Default);

			WriteBlock (SW, x1, y1, h1, x2, y2, h2, new string[] { RoofTexture, RoofTexture, RoofTexture,
				RoofTexture, RoofTexture, RoofTexture }, BlockTypes.Default);
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
		/// <param name="AddingTheBulb">Флаг указывает, что добавляется лампочка, а не источник света</param>
		/// <param name="SubFloor">Флаг указывает, что свет добавляется к внутренней площадке</param>
		/// <returns>Возвращает true, если добавлен действующий источник света</returns>
		public static bool WriteMapLight (StreamWriter SW, Point RelativePosition, string RoofTexture,
			bool AddingTheBulb, bool SubFloor)
			{
			// Защита
			if (IsSkyTexture (RoofTexture) && !SubFloor && EnvironmentAdded)
				return false;

			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Добавление атмосферного освещения
			// (флаг лампочки требуется контролировать, т.к. они добавляются раньше)
			if (!AddingTheBulb && IsSkyTexture (RoofTexture))
				{
				SW.Write ("{\n");
				SW.Write ("\"classname\" \"light_environment\"\n");
				SW.Write ("\"_fade\" \"1.0\"\n");

				SW.Write ("\"angles\" \"" + sunAngles[skyIndex] + "\"\n");
				SW.Write ("\"_light\" \"" + sunColors[skyIndex] + "\"\n");

				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
					(wallHeight - 8).ToString () + "\"\n");
				SW.Write ("}\n");

				EnvironmentAdded = true;
				return true;
				}

			// Добавление источника света
			int z = wallHeight;
			if (SubFloor)
				z -= (DefaultWallHeight + 12);

			if (!AddingTheBulb)
				{
				SW.Write ("{\n");
				SW.Write ("\"classname\" \"light\"\n");
				SW.Write (SubFloor ? subLightColor : lightColor);
				SW.Write ("\"_fade\" \"1.0\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
					(z - 12).ToString () + "\"\n");
				SW.Write ("}\n");
				}

			// Добавление лампы
			else if (!IsSkyTexture (RoofTexture))
				{
				int d = SubFloor ? 8 : 16;

				WriteBlock (SW, (p.X - d).ToString (), (p.Y - d).ToString (), (z - 4).ToString (),
					(p.X + d).ToString (), (p.Y + d).ToString (), z.ToString (),

					new string[] { RoofTexture, RoofTexture, RoofTexture, RoofTexture, RoofTexture,
						SubFloor ? "~PATH01" : "~LAMP07" },

					BlockTypes.Default);

				return false;
				}

			return true;
			}

		/// <summary>
		/// Возвращает true, если небесный свет уже был добавлен
		/// </summary>
		public static bool EnvironmentAdded = false;
		private static string lightColor = "", subLightColor = "";

		/// <summary>
		/// Метод записывает мебель на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="NearbyWalls">Доступные (с указанной позиции) стены</param>
		/// <param name="WallTexture">Текстура окружающей стены</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="FurnitureType">Индекс мебели</param>
		public static void WriteMapFurniture (StreamWriter SW, Point RelativePosition, FurnitureTypes FurnitureType,
			List<CPResults> NearbyWalls, string WallTexture, Random Rnd)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			CPResults placement = NearbyWalls[Rnd.Next (NearbyWalls.Count)];

			// Расчёт координат
			Furniture f = Furniture.GetFurniture (FurnitureType, placement, Rnd);
			int[] coords = f.Coordinates;

			// Введение смещения
			coords[0] += p.X;
			coords[3] += p.X;
			coords[1] += p.Y;
			coords[4] += p.Y;

			// Сборка линии текстур
			string[] tex = f.Textures;
			for (int i = 0; i < tex.Length; i++)
				{
				if (string.IsNullOrWhiteSpace (tex[i]))
					tex[i] = WallTexture;
				}

			// Запись
			WriteBlock (SW, coords[0].ToString (), coords[1].ToString (), coords[2].ToString (),
				coords[3].ToString (), coords[4].ToString (), coords[5].ToString (), tex, BlockTypes.Door);
			}

		/// <summary>
		/// Метод записывает внутреннюю площадку на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="SubFloorTexture">Текстура внутренней площадки</param>
		public static void WriteMapSubFloor (StreamWriter SW, Point RelativePosition, string SubFloorTexture)
			{
			WriteSubFloor (SW, RelativePosition, SubFloorTexture, null);
			}

		/// <summary>
		/// Метод записывает зацеп к внутренней площадке на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		public static void WriteMapSubFloor (StreamWriter SW, Point RelativePosition, List<CPResults> SurroundingWalls)
			{
			WriteSubFloor (SW, RelativePosition, null, SurroundingWalls);
			}

		// Универсальный метод записи внутренней площадки
		private static void WriteSubFloor (StreamWriter SW, Point RelativePosition, string SubFloorTexture,
			List<CPResults> SurroundingWalls)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Сборка линии текстур
			string[] tex = new string[6];
			for (int i = 0; i < tex.Length; i++)
				tex[i] = (SurroundingWalls != null) ? TriggerTexture : SubFloorTexture;

			// Запись площадки
			if (SurroundingWalls == null)
				{
				WriteBlock (SW, (p.X - 56).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 16).ToString (),
					(p.X + 56).ToString (), (p.Y + 56).ToString (), DefaultWallHeight.ToString (),
					tex, BlockTypes.Door);
				return;
				}

			// Запись лестницы.
			// Обработка функцией для SurroundingWalls имеет скрытое ограничение, заключающееся в том, что
			// такая площадка может появиться над дверью в стене, только если она окружена тремя стенами
			if (!SurroundingWalls.Contains (CPResults.Left))
				{
				SW.Write ("{\n\"classname\" \"func_ladder\"\n");
				WriteBlock (SW, (p.X - 60).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 16).ToString (),
					(p.X - 56).ToString (), (p.Y + 56).ToString (), DefaultWallHeight.ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}

			if (!SurroundingWalls.Contains (CPResults.Right))
				{
				SW.Write ("{\n\"classname\" \"func_ladder\"\n");
				WriteBlock (SW, (p.X + 56).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 16).ToString (),
					(p.X + 60).ToString (), (p.Y + 56).ToString (), DefaultWallHeight.ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}

			if (!SurroundingWalls.Contains (CPResults.Down))
				{
				SW.Write ("{\n\"classname\" \"func_ladder\"\n");
				WriteBlock (SW, (p.X - 56).ToString (), (p.Y - 60).ToString (), (DefaultWallHeight - 16).ToString (),
					(p.X + 56).ToString (), (p.Y - 56).ToString (), DefaultWallHeight.ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}

			if (!SurroundingWalls.Contains (CPResults.Up))
				{
				SW.Write ("{\n\"classname\" \"func_ladder\"\n");
				WriteBlock (SW, (p.X - 56).ToString (), (p.Y + 56).ToString (), (DefaultWallHeight - 16).ToString (),
					(p.X + 56).ToString (), (p.Y + 60).ToString (), DefaultWallHeight.ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}
			}

#if false

		/// <summary>
		/// Метод записывает декаль на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Decal">Текстура декали</param>
		/// <param name="Top">Флаг отрисовки декали на потолке вместо пола</param>
		public static void WriteMapDecal (StreamWriter SW, Point RelativePosition, string Decal, bool Top)
			{
			// Расчёт параметров
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

			// Запись
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"infodecal\"\n");
			SW.Write ("\"texture\" \"" + Decal + "\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " " + (Top ? WallHeightInt.ToString () : "0") + "\"\n");
			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает декали пути от входа к выходу на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="StartOrFinish">Флаг указывает на начальную или конечную точку пути</param>
		public static void WriteMapPathTrace (StreamWriter SW, Point RelativePosition, bool StartOrFinish)
			{
			string decal;
			if (StartOrFinish)
				decal = "{target";
			else if (leftStep)
				decal = "{foot_l";
			else
				decal = "{foot_r";

			WriteMapDecal (SW, RelativePosition, decal, false);

			leftStep = !leftStep;
			}
		private static bool leftStep = false;

#endif
		}
	}
