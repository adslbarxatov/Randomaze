using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

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
		/// Возвращает двойную высоту стен лабиринта
		/// </summary>
		public const int DoubleWallHeight = 224;

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
		/// Возвращает или задаёт номер генерируемой карты
		/// </summary>
		public static uint MapNumber
			{
			get
				{
				return mapNumber;
				}
			set
				{
				mapNumber = value;
				}
			}
		private static uint mapNumber = 0;

		/// <summary>
		/// Стандартная длина стен на картах
		/// </summary>
		public const int WallLength = 128;

		/// <summary>
		/// Возвращает максимально допустимое количество карт
		/// </summary>
		public const int MapsLimit = 999;

		/// <summary>
		/// Возвращает общее количество активных сущностей на карте
		/// </summary>
		public static uint EntitiesQuantity
			{
			get
				{
				return entitiesQuantity;
				}
			}
		private static uint entitiesQuantity = 0;

		/// <summary>
		/// Метод сбрасывает счётчик сущностей
		/// </summary>
		public static void ResetEntitiesCounter ()
			{
			entitiesQuantity = 0;
			environmentAdded = false;
			}

		/// <summary>
		/// Метод добавляет одну сущность в счётчик
		/// </summary>
		/// <param name="ClassName">Название класса сущности</param>
		/// <param name="SW">Дескриптор записи в файл</param>
		public static void AddEntity (StreamWriter SW, string ClassName)
			{
			AddEntity (SW, ClassName, true);
			}

		/// <summary>
		/// Метод добавляет одну сущность в счётчик
		/// </summary>
		/// <param name="ClassName">Название класса сущности</param>
		/// <param name="SW">Дескриптор записи в файл</param>
		/// <param name="Count">Флаг указывает на необходимость нарастить счётчик сущностей
		/// при создании этого экземпляра</param>
		public static void AddEntity (StreamWriter SW, string ClassName, bool Count)
			{
			SW.Write ("\"classname\" \"" + ClassName + "\"\n");
			if (Count)
				entitiesQuantity++;
			}

		/// <summary>
		/// Возвращает true, если максимально допустимое число сущностей было превышено
		/// </summary>
		public static bool IsEntitiesLimitExceeded
			{
			get
				{
				return (entitiesQuantity > 1024);
				}
			}

		/// <summary>
		/// Метод формирует каноничное имя карты по её номеру с указанным инкрементом
		/// </summary>
		/// <param name="Offset">Инкремент номера карты</param>
		/// <returns>Строка с название карты</returns>
		public static string BuildMapName (uint InitialMapNumber, int Offset)
			{
			return RandomazeForm.MainAlias + (InitialMapNumber + Offset).ToString ("D3");
			}

		/// <summary>
		/// Метод формирует каноничное имя карты по её номеру с указанным инкрементом
		/// </summary>
		/// <param name="Offset">Инкремент номера карты</param>
		/// <returns>Строка с название карты</returns>
		public static string BuildMapName (int Offset)
			{
			return BuildMapName (mapNumber, Offset);
			}

		/// <summary>
		/// Метод формирует каноничное имя карты по её номеру
		/// </summary>
		/// <returns>Строка с название карты</returns>
		public static string BuildMapName ()
			{
			return BuildMapName (0);
			}

		/// <summary>
		/// Метод записывает заголовок карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="Lightness">Уровень затемнения неба (0.0 – 1.0)</param>
		/// <param name="TwoFloors">Инициализация двухэтажной карты</param>
		public static void WriteMapHeader (StreamWriter SW, float Lightness, bool TwoFloors)
			{
			// Начало карты
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"worldspawn\"\n");
			SW.Write ("\"message\" \"ES: Randomaze map " + BuildMapName () + " by FDL\"\n");
			SW.Write ("\"mapversion\" \"220\"\n");

			// Инициализация неба
			skyIndex = RDGenerics.RND.Next (skyTypes.Length / 2);

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
			else
				{
				SW.Write ("\"chaptertitle\" \"Map #" + MapNumber.ToString ("D3") + "\"\n");
				}

			// Создание цвета ламп
			if (string.IsNullOrWhiteSpace (lightColor))
				{
				lightColor = "\"_light\" \"" + (224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(112 + RDGenerics.RND.Next (32)).ToString () + " " + (TwoFloors ? "200" : "150") + "\"\n";
				subLightColor = "\"_light\" \"" + (224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(112 + RDGenerics.RND.Next (32)).ToString () + " 100\"\n";
				}

			// Выбор высоты карты
			if (TwoFloors)
				wallHeight = DoubleWallHeight;
			else
				wallHeight = DefaultWallHeight;
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
			AddEntity (SW, "game_end");
			SW.Write ("\"targetname\" \"DevEnd02\"\n");
			SW.Write ("\"origin\" \"" + x1 + " " + y1 + " " + z3 + "\"\n");

			SW.Write ("}\n{\n");
			AddEntity (SW, "player_loadsaved");
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
			AddEntity (SW, "multi_manager");
			SW.Write ("\"targetname\" \"DevM\"\n");
			SW.Write ("\"DevEnd01\" \"0\"\n");
			SW.Write ("\"DevEnd02\" \"5.5\"\n");
			SW.Write ("\"origin\" \"" + x2 + " " + y1 + " " + z3 + "\"\n");

			SW.Write ("}\n{\n");
			AddEntity (SW, "trigger_once");
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
		/// <param name="TeleportGate">Флаг указывает на наличие второго шлюза перед выходом</param>
		public static void WriteMapEndPoint (StreamWriter SW, Point RelativePosition, bool TeleportGate)
			{
			// Защита
			if (MapNumber >= MapsLimit)
				{
				WriteMapEndPoint_Finish (SW, RelativePosition);
				return;
				}

			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string mapName = BuildMapName (1);

			// Запись
			SW.Write ("{\n");
			AddEntity (SW, "info_landmark");
			SW.Write ("\"targetname\" \"" + mapName + "m\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 40\"\n");

			SW.Write ("}\n{\n");
			AddEntity (SW, "trigger_changelevel");
			SW.Write ("\"map\" \"" + mapName + "\"\n");
			SW.Write ("\"landmark\" \"" + mapName + "m\"\n");

			WriteBlock (SW, (p.X - 8).ToString (), (p.Y - 8).ToString (), "16",
				(p.X + 8).ToString (), (p.Y + 8).ToString (), (wallHeight - 16).ToString (),

				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
					TriggerTexture, TriggerTexture },

				BlockTypes.Default);

			SW.Write ("}\n{\n");
			AddEntity (SW, "trigger_autosave");

			WriteBlock (SW, (p.X - 32).ToString (), (p.Y - 32).ToString (), "12",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "16",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");

			WriteMapPortal (SW, RelativePosition, true);

			// Второй шлюз
			if (!TeleportGate)
				return;

			SW.Write ("{\n");
			AddEntity (SW, "func_door");
			SW.Write ("\"angles\" \"90 0 0\"\n");
			SW.Write ("\"speed\" \"70\"\n");
			SW.Write ("\"movesnd\" \"2\"\n");
			SW.Write ("\"stopsnd\" \"11\"\n");
			SW.Write ("\"wait\" \"-1\"\n");
			SW.Write ("\"lip\" \"1\"\n");
			if (MapNumber <= MapsLimit)
				SW.Write ("\"targetname\" \"MGate" + BuildMapName () + "\"\n");

			string tex = "Metal06";
			WriteBlock (SW, (p.X - 12).ToString (), (p.Y - 12).ToString (), "0",
				(p.X - 8).ToString (), (p.Y - 8).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			WriteBlock (SW, (p.X + 8).ToString (), (p.Y - 12).ToString (), "0",
				(p.X + 12).ToString (), (p.Y - 8).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			WriteBlock (SW, (p.X - 12).ToString (), (p.Y + 8).ToString (), "0",
				(p.X - 8).ToString (), (p.Y + 12).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			WriteBlock (SW, (p.X + 8).ToString (), (p.Y + 8).ToString (), "0",
				(p.X + 12).ToString (), (p.Y + 12).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);

			SW.Write ("}\n");
			}

		// Метод создаёт портал на карте
		private static void WriteMapPortal (StreamWriter SW, Point RelativePosition, bool Exit)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
			SW.Write ("{\n");
			AddEntity (SW, "env_sprite");
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
		/// <param name="GravityLevel">Уровень гравитации на карте (10 = 100%)</param>
		/// <param name="IsUnderSky">Флаг указывает, расположена ли точка входа под небом</param>
		/// <param name="FogLevel">Уровень тумана на карте (10 = 100%)</param>
		/// <param name="WallsAreRare">Флаг указывает на редкость стен на карте</param>
		public static void WriteMapEntryPoint (StreamWriter SW, Point RelativePosition,
			uint GravityLevel, uint FogLevel, bool IsUnderSky, bool WallsAreRare)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string xs = p.X.ToString ();
			string ys = p.Y.ToString ();

			// Первая карта
			if (MapNumber == 1)
				{
				SW.Write ("{\n");
				AddEntity (SW, "info_player_start");
				SW.Write ("\"angles\" \"0 0 0\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "item_suit");
				SW.Write ("\"spawnflags\" \"1\"\n");
				SW.Write ("\"angles\" \"0 0 0\"\n");
				SW.Write ("\"target\" \"Preset\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 32\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "weapon_9mmAR");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 36\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "weapon_shotgun");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "weapon_handgrenade");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 44\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "weapon_handgrenade");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 48\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "ammo_9mmbox");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 52\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "ammo_buckshot");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 56\"\n");
				SW.Write ("}\n{\n");

				AddEntity (SW, "game_player_set_health");
				SW.Write ("\"targetname\" \"Preset\"\n");
				SW.Write ("\"dmg\" \"200\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 64\"\n");
				SW.Write ("}\n");
				}

			// Все последующие
			else
				{
				SW.Write ("{\n");
				AddEntity (SW, "info_landmark");
				SW.Write ("\"targetname\" \"" + BuildMapName () + "m\"\n");
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");

				SW.Write ("}\n{\n");
				AddEntity (SW, "trigger_changelevel");
				SW.Write ("\"map\" \"" + BuildMapName (-1) + "\"\n");
				SW.Write ("\"landmark\" \"" + BuildMapName () + "m\"\n");

				WriteBlock (SW, (p.X - 8).ToString (), (p.Y - 8).ToString (), "-2",
					(p.X + 8).ToString (), (p.Y + 8).ToString (), "-1",
					new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

				SW.Write ("}\n");
				}

			// Гравитационный триггер
			SW.Write ("{\n");
			AddEntity (SW, "trigger_gravity");
			SW.Write ("\"gravity\" \"" + (GravityLevel * 80).ToString () + "\"\n");

			WriteBlock (SW, (p.X - 32).ToString (), (p.Y - 32).ToString (), "24",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "28",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");

			// Триггер тумана
			SW.Write ("{\n");
			AddEntity (SW, "trigger_fog");
			SW.Write ("\"renderamt\" \"" + ((uint)(255.0 * FogLevel / 10.0)).ToString () + "\"\n");
			SW.Write ("\"rendercolor\" \"" + (224 + RDGenerics.RND.Next (32)).ToString () + " " +
				(224 + RDGenerics.RND.Next (32)).ToString () + " " +
				(224 + RDGenerics.RND.Next (32)).ToString () + "\"\n");
			SW.Write ("\"enablingMove\" \"0\"\n");

			WriteBlock (SW, (p.X - 32).ToString (), (p.Y - 32).ToString (), "32",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "36",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");

			// Звуковой триггер
			byte rt;
			byte offset = (byte)(TwoFloors ? 1 : 0);
			if (IsUnderSky)
				rt = 0;
			else if (WallsAreRare)
				rt = (byte)(18 + offset);
			else
				rt = (byte)(17 + offset);

			WriteMapSoundTrigger (SW, RelativePosition, false, rt, 0);

			// Остальное
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
			}

		// Стандартная текстура триггера
		private const string TriggerTexture = "AAATRIGGER";

		/// <summary>
		/// Метод записывает звуковой триггер на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки входа</param>
		/// <param name="ForWindow">Флаг двунаправленного триггера для окон</param>
		/// <param name="RoomTypeLeft">Тип окружения слева (для всех)</param>
		/// <param name="RoomTypeRight">Тип окружения справа (для оконных)</param>
		public static void WriteMapSoundTrigger (StreamWriter SW, Point RelativePosition, bool ForWindow,
			byte RoomTypeLeft, byte RoomTypeRight)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string x1, y1, x2, y2, z1, z2;

			// Запись
			SW.Write ("{\n");
			AddEntity (SW, "trigger_sound");
			SW.Write ("\"roomtype\" \"" + RoomTypeLeft.ToString () + "\"\n");
			SW.Write ("\"roomtype2\" \"" + RoomTypeRight.ToString () + "\"\n");
			SW.Write ("\"spawnflags\" \"" + (ForWindow ? "1" : "0") + "\"\n");

			// Вертикальная
			if (ForWindow)
				{
				z1 = "16";
				z2 = (WallHeight - 16).ToString ();

				if (WallsSupport.IsWallVertical (RelativePosition))
					{
					x1 = (p.X - 4).ToString ();
					y1 = (p.Y - 56).ToString ();
					x2 = (p.X + 4).ToString ();
					y2 = (p.Y + 56).ToString ();
					}
				else
					{
					x1 = (p.X - 56).ToString ();
					y1 = (p.Y - 4).ToString ();
					x2 = (p.X + 56).ToString ();
					y2 = (p.Y + 4).ToString ();
					}
				}
			else
				{
				x1 = (p.X - 32).ToString ();
				y1 = (p.Y - 32).ToString ();
				x2 = (p.X + 32).ToString ();
				y2 = (p.Y + 32).ToString ();
				z1 = "16";
				z2 = "20";
				}

			// Запись
			WriteBlock (SW, x1, y1, z1, x2, y2, z2, new string[] { TriggerTexture, TriggerTexture,
				TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture }, BlockTypes.Default);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="NearbyWalls">Список окружающих стен</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		/// <param name="TeleportButton">Флаг, указывающий на второй тип кнопки (включение телепорта)</param>
		public static void WriteMapButton (StreamWriter SW, Point RelativePosition, List<CPResults> NearbyWalls,
			bool TeleportButton)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
			SW.Write ("{\n");
			AddEntity (SW, "func_button");
			SW.Write ("\"spawnflags\" \"1\"\n");
			SW.Write ("\"delay\" \"1\"\n");
			SW.Write ("\"speed\" \"50\"\n");

			if (TeleportButton)
				{
				SW.Write ("\"target\" \"MGate" + BuildMapName () + "\"\n");
				SW.Write ("\"sounds\" \"8\"\n");
				}
			else
				{
				SW.Write ("\"target\" \"Gate" + BuildMapName () + "\"\n");
				SW.Write ("\"sounds\" \"11\"\n");
				}

			SW.Write ("\"wait\" \"-1\"\n");

			WriteMapFurniture (SW, RelativePosition,
				TeleportButton ? FurnitureTypes.ExitTeleportButton : FurnitureTypes.ExitGateButton,
				NearbyWalls, "Metal06");

			SW.Write ("}\n{\n");
			AddEntity (SW, "trigger_autosave");

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
		/// <param name="AllowExplosives">Флаг разрешения ящиков со взрывчаткой</param>
		/// <param name="AllowItems">Флаг разрешения ящиков с жуками и собираемыми объектами</param>
		/// <param name="ItemPermissions">Строка разрешений для объектов в ящиках</param>
		/// <param name="EnemiesPermissions">Строка разрешений для врагов в ящиках (крабы, снарки)</param>
		public static void WriteMapCrate (StreamWriter SW, Point RelativePosition,
			bool AllowItems, bool AllowExplosives, string ItemPermissions, string EnemiesPermissions)
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

			bool explosive = (RDGenerics.RND.Next (2) == 0);
			string tex = "CRATE01"; // Взрывчатка по умолчанию

			SW.Write ("{\n");
			AddEntity (SW, "func_pushable");
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
				SW.Write ("\"explodemagnitude\" \"" + RDGenerics.RND.Next (160, 200).ToString () + "\"\n");
				}
			else
				{
				int r = RDGenerics.RND.Next (4);
				bool factor1 = EnemiesSupport.IsHeadcrabAllowed (EnemiesPermissions);
				int idx;

				// Враги (при запрете хедкрабов увеличивается число ящиков со взрывчаткой)
				if (factor1 && (r < 3) || !factor1 && (r < 2))
					{
					if (!factor1)
						r = 0;  // Только снарки
					SW.Write ("\"spawnobject\" \"" + (r + 27).ToString () + "\"\n");
					}

				// Пустые или редкие ящики
				else
					{
					// Иногда добавлять случайное оружие или предмет
					if (RDGenerics.RND.Next (3) == 0)
						{
						idx = RDGenerics.RND.Next (26) + 1;
						while ((idx <= 26) && !ItemsSupport.IsCrateItemAllowed (ItemPermissions, idx))
							idx += (RDGenerics.RND.Next (3) + 1);

						if (idx <= 26)
							SW.Write ("\"spawnobject\" \"" + idx.ToString () + "\"\n");
						}

					// Случайная текстура для ящиков без врагов
					r = RDGenerics.RND.Next (3);
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
					h = 1;
					break;

				case AmbientTypes.Sky:
					h = 3;
					break;

				case AmbientTypes.Target:
					h = 4;
					break;

				default:
					h = 7;
					break;
				}

			// Запись
			SW.Write ("{\n");
			AddEntity (SW, "ambient_generic");

			if (Ambient == AmbientTypes.Target)
				{
				SW.Write ("\"spawnflags\" \"49\"\n");
				SW.Write ("\"targetname\" \"" + Sound + "\"\n");
				}
			else
				{
				SW.Write ("\"spawnflags\" \"2\"\n");
				}

			SW.Write ("\"message\" \"ambience/" + Sound + ".wav\"\n");
			SW.Write ("\"health\" \"" + h.ToString () + "\"\n");
			SW.Write ("\"pitch\" \"100\"\n");
			SW.Write ("\"pitchstart\" \"100\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " 88\"\n");
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
			None,

			/// <summary>
			/// Звук, запускаемый событием
			/// </summary>
			Target
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
			"255 255 128 180",
			"255 255 128 180",
			"224 192 128 120",
			"96 64 32 90",
			"32 64 96 120",
			"64 64 64 120",
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
		/// Метод записывает пол и потолок на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="WaterLevel">Уровень воды (в долях от единицы)</param>
		/// <param name="RelativeMapHeight">Относительная ширина карты</param>
		/// <param name="RelativeMapWidth">Относительная длина карты</param>
		public static void WriteMapWater (StreamWriter SW, float WaterLevel, int RelativeMapWidth,
			int RelativeMapHeight)
			{
			// Расчёт параметров
			int realMapWidth = RelativeMapWidth * WallLength;
			int realMapHeight = RelativeMapHeight * WallLength;
			string tex = waterTextures[RDGenerics.RND.Next (waterTextures.Length)];
			string h = ((int)(DefaultWallHeight * WaterLevel)).ToString ();
			string amt = (70 + RDGenerics.RND.Next (130)).ToString ();

			for (int i = 0; i < 4; i++)
				{
				bool negX = ((i & NegativeX) != 0);
				bool negY = ((i & NegativeY) != 0);
				string x1 = (negX ? (-realMapWidth / 2) : 0).ToString ();
				string x2 = (negX ? 0 : (realMapWidth / 2)).ToString ();
				string y1 = (negY ? (-realMapHeight / 2) : 0).ToString ();
				string y2 = (negY ? 0 : (realMapHeight / 2)).ToString ();

				// Запись
				SW.Write ("{\n");
				AddEntity (SW, "func_water");
				SW.Write ("\"renderamt\" \"" + amt + "\"\n");
				SW.Write ("\"rendermode\" \"2\"\n");
				SW.Write ("\"wait\" \"-1\"\n");
				SW.Write ("\"skin\" \"-3\"\n");
				SW.Write ("\"WaveHeight\" \"0.1\"\n");

				WriteBlock (SW, x1, y1, "0", x2, y2, h, new string[] { tex, tex, tex,
					tex, tex, tex }, BlockTypes.Default);

				SW.Write ("}\n");
				}
			}
		private static string[] waterTextures = new string[] {
			"!_DirtyWater01",
			"!_Ether01",
			"!_Water01",
			"!_Water02",
			};

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
			if (IsSkyTexture (RoofTexture) && !SubFloor && environmentAdded)
				return false;

			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Добавление атмосферного освещения
			// (флаг лампочки требуется контролировать, т.к. они добавляются раньше)
			if (!AddingTheBulb && IsSkyTexture (RoofTexture))
				{
				SW.Write ("{\n");
				AddEntity (SW, "light_environment", false);
				SW.Write ("\"_fade\" \"1.0\"\n");

				SW.Write ("\"angles\" \"" + sunAngles[skyIndex] + "\"\n");
				SW.Write ("\"_light\" \"" + sunColors[skyIndex] + "\"\n");

				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
					(wallHeight - 8).ToString () + "\"\n");
				SW.Write ("}\n");

				environmentAdded = true;
				return true;
				}

			// Добавление источника света
			int z;
			if (SubFloor)
				z = DefaultWallHeight - 32;
			else
				z = wallHeight;

			if (!AddingTheBulb)
				{
				SW.Write ("{\n");
				AddEntity (SW, "light", false);
				SW.Write (SubFloor ? subLightColor : lightColor);
				SW.Write ("\"_fade\" \"1.0\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
					(z - 8).ToString () + "\"\n");
				SW.Write ("}\n");
				}

			// Добавление лампы
			else if (!IsSkyTexture (RoofTexture))
				{
				int d = SubFloor ? 8 : 16;

				WriteBlock (SW, (p.X - d).ToString (), (p.Y - d).ToString (), (z - 0).ToString (),
					(p.X + d).ToString (), (p.Y + d).ToString (), (z + 4).ToString (),

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
		public static bool EnvironmentAdded
			{
			get
				{
				return environmentAdded;
				}
			}
		private static bool environmentAdded = false;
		private static string lightColor = "", subLightColor = "";

		/// <summary>
		/// Метод записывает мебель на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="NearbyWalls">Доступные (с указанной позиции) стены</param>
		/// <param name="WallTexture">Текстура окружающей стены</param>
		/// <param name="FurnitureType">Индекс мебели</param>
		public static void WriteMapFurniture (StreamWriter SW, Point RelativePosition, FurnitureTypes FurnitureType,
			List<CPResults> NearbyWalls, string WallTexture)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			CPResults placement = NearbyWalls[RDGenerics.RND.Next (NearbyWalls.Count)];

			// Расчёт координат
			Furniture f = Furniture.GetFurniture (FurnitureType, placement);
			for (uint b = 0; b < f.BlocksCount; b++)
				{
				int[] coords = f.GetCoordinates (b);

				// Введение смещения
				coords[0] += p.X;
				coords[3] += p.X;
				coords[1] += p.Y;
				coords[4] += p.Y;

				// Сборка линии текстур
				string[] tex = f.GetTextures (b);
				for (int i = 0; i < tex.Length; i++)
					{
					if (string.IsNullOrWhiteSpace (tex[i]))
						tex[i] = WallTexture;
					}

				// Запись
				WriteBlock (SW, coords[0].ToString (), coords[1].ToString (), coords[2].ToString (),
					coords[3].ToString (), coords[4].ToString (), coords[5].ToString (), tex, BlockTypes.Door);
				}
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
		public static void WriteMapSubFloor (StreamWriter SW, Point RelativePosition,
			List<CPResults> SurroundingWalls)
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
				WriteBlock (SW, (p.X - 56).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 32).ToString (),
					(p.X + 56).ToString (), (p.Y + 56).ToString (), (DefaultWallHeight - 16).ToString (),
					tex, BlockTypes.Door);
				return;
				}

			// Запись лестницы.
			// Обработка функцией для SurroundingWalls имеет скрытое ограничение, заключающееся в том, что
			// такая площадка может появиться над дверью в стене, только если она окружена тремя стенами
			if (!SurroundingWalls.Contains (CPResults.Left))
				{
				SW.Write ("{\n");
				AddEntity (SW, "func_ladder");
				WriteBlock (SW, (p.X - 60).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 32).ToString (),
					(p.X - 56).ToString (), (p.Y + 56).ToString (), (DefaultWallHeight - 16).ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}

			if (!SurroundingWalls.Contains (CPResults.Right))
				{
				SW.Write ("{\n");
				AddEntity (SW, "func_ladder");
				WriteBlock (SW, (p.X + 56).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 32).ToString (),
					(p.X + 60).ToString (), (p.Y + 56).ToString (), (DefaultWallHeight - 16).ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}

			if (!SurroundingWalls.Contains (CPResults.Down))
				{
				SW.Write ("{\n");
				AddEntity (SW, "func_ladder");
				WriteBlock (SW, (p.X - 56).ToString (), (p.Y - 60).ToString (), (DefaultWallHeight - 32).ToString (),
					(p.X + 56).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 16).ToString (),
					tex, BlockTypes.Door);
				SW.Write ("}\n");
				}

			if (!SurroundingWalls.Contains (CPResults.Up))
				{
				SW.Write ("{\n");
				AddEntity (SW, "func_ladder");
				WriteBlock (SW, (p.X - 56).ToString (), (p.Y + 56).ToString (), (DefaultWallHeight - 32).ToString (),
					(p.X + 56).ToString (), (p.Y + 60).ToString (), (DefaultWallHeight - 16).ToString (),
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

		/// <summary>
		/// Метод формирует файл скрипта-описателя мода
		/// </summary>
		/// <param name="MapName">Название первой карты мода</param>
		/// <returns>Возвращает true в случае успеха</returns>
		public static bool WriteGameScript (string MapName)
			{
			// Создание файла
			FileStream FS = null;
			try
				{
				FS = new FileStream (RDGenerics.AppStartupPath + "liblist.gam", FileMode.Create);
				}
			catch
				{
				return false;
				}
			StreamWriter SW = new StreamWriter (FS, RDGenerics.GetEncoding (RDEncodings.UTF8));

			SW.Write ("game \"" + ProgramDescription.AssemblyTitle + "\"\n");
			SW.Write ("type \"singleplayer_only\"\n");
			SW.Write ("version \"" + ProgramDescription.AssemblyVersion + "\"\n");
			SW.Write ("noskills \"1\"\n");

			SW.Write ("startmap \"" + MapName + "\"\n");
			SW.Write ("creditsmap \"" + MapName + "\"\n");
			SW.Write ("edicts \"1500\"\n");

			SW.Write ("cldll \"1\"\n");
			SW.Write ("gamedll \"dlls\\hl.dll\"\n");
			SW.Write ("spentity \"info_landmark\"\n");

			SW.Write ("developer_url \"" + RDGenerics.DPArrayStorageLink + "\"\n");
			SW.Write ("url_info \"http://moddb.com/mods/esrm\"\n");

			SW.Close ();
			FS.Close ();
			return true;
			}

		/// <summary>
		/// Метод записывает межстенный заполнитель (для удаления недоступных пространств из компилируемой зоны)
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		public static void WriteMapFiller (StreamWriter SW, Point RelativePosition)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			WriteBlock (SW, (p.X - WallLength / 2).ToString (), (p.Y - WallLength / 2).ToString (), "0",
				(p.X + WallLength / 2).ToString (), (p.Y + WallLength / 2).ToString (), (WallHeight + 32).ToString (),
				new string[] { "BLACK", "BLACK", "BLACK", "BLACK", "BLACK", "BLACK" }, BlockTypes.Default);
			}

		/// <summary>
		/// Метод ограничивает указанное значение
		/// </summary>
		/// <param name="Value">Исходное значение</param>
		/// <param name="Minimum">Требуемый минимум</param>
		/// <param name="Maximum">Требуемый максимум</param>
		/// <returns>Возвращает значение, входящее в требуемый диапазон</returns>
		public static uint InboundValue (int Value, uint Minimum, uint Maximum)
			{
			uint v = (uint)Value;

			if (v < Minimum)
				v = Minimum;
			if (v > Maximum)
				v = Maximum;

			return v;
			}

		// Код, обеспечивающий блокировку кнопки закрытия окна
		[DllImport ("user32.dll")]
		private static extern IntPtr GetSystemMenu (IntPtr Hwnd, bool Revert);

		[DllImport ("user32.dll")]
		private static extern int EnableMenuItem (IntPtr tMenu, int targetItem, int targetStatus);

		/// <summary>
		/// Метод отключает кнопку закрытия окна
		/// </summary>
		/// <param name="WindowHandle">Дескриптор окна</param>
		public static void DisableClosingButton (IntPtr WindowHandle)
			{
			// SC_CLOSE, MF_GRAYED
			EnableMenuItem (GetSystemMenu (WindowHandle, false), 0xF060, 0x0001);
			}
		}
	}
