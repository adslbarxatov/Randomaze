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
			// Расчёт параметров
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			string x1 = (x - 8).ToString ();
			string y1 = (y - 8).ToString ();
			string x2 = (x + 8).ToString ();
			string y2 = (y + 8).ToString ();
			string z1 = "16";
			string z2 = (WallHeightInt - 16).ToString ();
			string z3 = (WallHeightInt - 8).ToString ();

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
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			string mapName = Program.MainAlias + (MapNumber + 1).ToString (MapsNumbersFormat);

			// Запись
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"info_landmark\"\n");
			SW.Write ("\"targetname\" \"" + mapName + "m\"\n");
			SW.Write ("\"origin\" \"" + x.ToString () + " " + y.ToString () + " 40\"\n");

			SW.Write ("}\n{\n");
			SW.Write ("\"classname\" \"trigger_changelevel\"\n");
			SW.Write ("\"map\" \"" + mapName + "\"\n");
			SW.Write ("\"landmark\" \"" + mapName + "m\"\n");

			WriteBlock (SW, (x - 8).ToString (), (y - 8).ToString (), "16",

				(x + 8).ToString (), (y + 8).ToString (), (WallHeightInt - 16).ToString (),

				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
					TriggerTexture, TriggerTexture },

				BlockTypes.Default);

			SW.Write ("}\n");

			WriteMapPortal (SW, RelativePosition, true);
			}

		// Метод создаёт портал на карте
		private static void WriteMapPortal (StreamWriter SW, Point RelativePosition, bool Exit)
			{
			// Расчёт параметров
			string x = (RelativePosition.X * WallLength / 2).ToString ();
			string y = (RelativePosition.Y * WallLength / 2).ToString ();

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
			// Расчёт параметров
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			string xs = x.ToString ();
			string ys = y.ToString ();
			string x1 = (x - 8).ToString ();
			string y1 = (y - 8).ToString ();
			string x2 = (x + 8).ToString ();
			string y2 = (y + 8).ToString ();
			string z1 = (WallHeightInt - 1).ToString ();
			string z2 = WallHeightInt.ToString ();

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
				SW.Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");

				SW.Write ("}\n{\n");
				SW.Write ("\"classname\" \"trigger_changelevel\"\n");
				SW.Write ("\"map\" \"" + Program.MainAlias + (MapNumber - 1).ToString (MapsNumbersFormat) + "\"\n");
				SW.Write ("\"landmark\" \"" + Program.MainAlias + MapNumber.ToString (MapsNumbersFormat) + "m\"\n");

				WriteBlock (SW, x1, y1, z1, x2, y2, z2,
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
				Textures[1] + " [ 1 0 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
			SW.Write ("( " + X1 + " " + Y1 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				Textures[2] + " [ 1 0 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
			SW.Write ("( " + X1 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				Textures[3] + " [ 0 1 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
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
		/// <param name="Permissions">Строка флагов разрешённых врагов</param>
		public static void WriteMapEnemy (StreamWriter SW, Point RelativePosition, Random Rnd,
			uint MapNumber, string Permissions)
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
				/*prngRange = 11;
				break;*/

				case 4:
				/*prngRange = 12;
				break;*/

				case 5:
				/*prngRange = 13;
				break;*/

				case 6:
				/*prngRange = 14;
				break;*/

				case 7:
				/*prngRange = 15;
				break;*/

				case 8:
				/*prngRange = 16;
				break;*/

				case 9:
				/*prngRange = 17;
				break;*/

				case 10:
				/*prngRange = 18;
				break;*/

				case 11:
				/*prngRange = 19;
				break;*/

				case 12:
					prngRange = (int)MapNumber + 8;
					break;

				default:
					prngRange = 21;
					break;
				}

			// Добавление
			const string rat = "\"classname\" \"monster_rat\"\n";
			string z = "0";

			switch (Rnd.Next (prngRange))
				{
				// Солдаты
				default:
					if (Permissions.Contains (EnemiesPermissionsKeys[4]))
						{
						SW.Write ("\"classname\" \"" + enemies[4] + "\"\n");
						SW.Write ("\"weapons\" \"" + gruntWeapons[Rnd.Next (gruntWeapons.Length)] + "\"\n");
						}
					else
						{
						SW.Write (rat);
						}
					break;

				// Зомби
				case 0:
				case 16:
					if (Permissions.Contains (EnemiesPermissionsKeys[9]))
						{
						SW.Write ("\"classname\" \"" + enemies[9] + "\"\n");
						SW.Write ("\"skin\" \"" + Rnd.Next (2).ToString () + "\"\n");
						}
					else
						{
						SW.Write (rat);
						}
					break;

				// Крабы
				case 1:
				case 17:
					if (Permissions.Contains (EnemiesPermissionsKeys[5]))
						SW.Write ("\"classname\" \"" + enemies[5] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Алиены
				case 10:
					if (Permissions.Contains (EnemiesPermissionsKeys[7]))
						SW.Write ("\"classname\" \"" + enemies[7] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Куры
				case 19:
					if (Permissions.Contains (EnemiesPermissionsKeys[1]))
						SW.Write ("\"classname\" \"" + enemies[1] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Ассассины
				case 12:
				case 13:
					if (Permissions.Contains (EnemiesPermissionsKeys[0]))
						SW.Write ("\"classname\" \"" + enemies[0] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Турель
				case 14:
					if (Permissions.Contains (EnemiesPermissionsKeys[8]))
						{
						SW.Write ("\"classname\" \"" + enemies[8] + "\"\n");
						SW.Write ("\"spawnflags\" \"32\"\n");
						SW.Write ("\"orientation\" \"0\"\n");
						}
					else
						{
						SW.Write (rat);
						}
					break;

				// Солдаты алиенов
				case 18:
					if (Permissions.Contains (EnemiesPermissionsKeys[6]))
						SW.Write ("\"classname\" \"" + enemies[6] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Контроллеры
				case 15:
					if (Permissions.Contains (EnemiesPermissionsKeys[2]))
						{
						SW.Write ("\"classname\" \"" + enemies[2] + "\"\n");
						z = "32";
						}
					else
						{
						SW.Write (rat);
						}
					break;

				// Собаки
				case 11:
				case 20:
					if (Permissions.Contains (EnemiesPermissionsKeys[3]))
						{
						SW.Write ("\"classname\" \"" + enemies[3] + "\"\n");
						}
					else
						{
						SW.Write (rat);
						}
					break;
				}

			SW.Write ("\"angles\" \"0 " + Rnd.Next (360) + " 0\"\n");
			SW.Write ("\"origin\" \"" + x + " " + y + " " + z + "\"\n");
			SW.Write ("}\n");
			}

		// Подстановки номеров оружия для солдат
		private static string[] gruntWeapons = new string[] { "1", "3", "5", "8", "10" };

		/// <summary>
		/// Примечание к строке разрешений для врагов
		/// </summary>
		public const string EnemiesPermissionsMessage = "Permitted enemies (" +
			"Assassins, " +
			"Bullchickens, " +
			"alien Controllers, " +
			"houndEyes, " +
			"human Grunts, " +
			"Headcrabs, " +
			"alien gRunts, " +
			"Slaves, " +
			"Turrets, " +
			"Zombies" +
			"):";

		/// <summary>
		/// Набор ключевых символов разрешений для врагов
		/// </summary>
		public static string[] EnemiesPermissionsKeys = new string[] {
			"a", "b", "c", "e", "g", "h", "r", "s", "t", "z"
			};

		private static string[] enemies = new string[] {
			"monster_human_assassin",
			"monster_bullchicken",
			"monster_alien_controller",
			"monster_houndeye",
			"monster_human_grunt",
			"monster_headcrab",
			"monster_alien_grunt",
			"monster_alien_slave",
			"monster_miniturret",
			"monster_zombie"
			};

		/// <summary>
		/// Метод записывает шлюз, ограничивающий точку входа, на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Frame">Флаг указывает, что записывается рама шлюза вместо него самого</param>
		public static void WriteMapGate (StreamWriter SW, Point RelativePosition, bool Frame)
			{
			WriteGate (SW, RelativePosition, Frame, MapsLimit + 1);
			}

		/// <summary>
		/// Метод записывает шлюз, закрывающий точку выхода, на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Frame">Флаг указывает, что записывается рама шлюза вместо него самого</param>
		/// <param name="MapNumber">Номер карты, используемый для создания уникального имени шлюза</param>
		public static void WriteMapGate (StreamWriter SW, Point RelativePosition, bool Frame, uint MapNumber)
			{
			WriteGate (SW, RelativePosition, Frame, MapNumber);
			}

		// Универсальный метод формирования шлюза
		private static void WriteGate (StreamWriter SW, Point RelativePosition, bool Frame, uint MapNumber)
			{
			// Расчёт параметров
			string tex = (MapNumber > MapsLimit) ? "MetalGate06" : "MetalGate07";

			// Запись рамы
			if (Frame)
				{
				WriteMapBarrier (SW, RelativePosition, BarrierTypes.GateFrameTop, tex);
				return;
				}

			// Запись шлюза
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_door\"\n");
			SW.Write ("\"angles\" \"90 0 0\"\n");
			SW.Write ("\"speed\" \"100\"\n");
			SW.Write ("\"movesnd\" \"3\"\n");
			SW.Write ("\"stopsnd\" \"1\"\n");
			SW.Write ("\"wait\" \"-1\"\n");
			SW.Write ("\"lip\" \"9\"\n");
			if (MapNumber <= MapsLimit)
				SW.Write ("\"targetname\" \"Gate" + MapNumber.ToString (MapsNumbersFormat) + "\"\n");

			WriteMapBarrier (SW, RelativePosition, BarrierTypes.Gate, tex);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		/// <param name="MapNumber">Номер текущей карты, используемый для создания уникального имени кнопки</param>
		/// <param name="Texture">Текстура секции</param>
		public static void WriteMapButton (StreamWriter SW, Point RelativePosition, string Texture, uint MapNumber)
			{
			// Расчёт параметров
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			// Запись
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_button\"\n");
			SW.Write ("\"target\" \"Gate" + MapNumber.ToString (MapsNumbersFormat) + "\"\n");
			SW.Write ("\"spawnflags\" \"1\"\n");
			SW.Write ("\"delay\" \"1\"\n");
			SW.Write ("\"speed\" \"50\"\n");
			SW.Write ("\"sounds\" \"11\"\n");
			SW.Write ("\"wait\" \"-1\"\n");

			WriteBlock (SW, (x - 8).ToString (), (y - 8).ToString (), "0",

				(x + 8).ToString (), (y + 8).ToString (), "40",

				new string[] { "+A_SWITCH01", Texture, Texture, Texture, Texture, Texture },

				BlockTypes.Button);

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
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;
			string x1, y1, x2, y2;

			// Вертикальная
			if (RelativePosition.X % 2 == 0)
				{
				x1 = (x - 8).ToString ();
				y1 = (y - 32).ToString ();
				x2 = (x + 8).ToString ();
				y2 = (y + 32).ToString ();
				}
			else
				{
				x1 = (x - 32).ToString ();
				y1 = (y - 8).ToString ();
				x2 = (x + 32).ToString ();
				y2 = (y + 8).ToString ();
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
		public static void WriteMapCrate (StreamWriter SW, Point RelativePosition, Random Rnd)
			{
			// Расчёт параметров
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			string x1 = (x - 16).ToString ();
			string y1 = (y - 16).ToString ();
			string x2 = (x + 16).ToString ();
			string y2 = (y + 16).ToString ();

			bool explosive = (Rnd.Next (2) == 0);
			string tex = (explosive ? "CRATE01" : "CRATE08");

			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_pushable\"\n");
			SW.Write ("\"health\" \"20\"\n");
			SW.Write ("\"material\" \"1\"\n");
			SW.Write ("\"spawnflags\" \"128\"\n");
			SW.Write ("\"friction\" \"40\"\n");
			SW.Write ("\"buoyancy\" \"60\"\n");

			if (explosive)
				{
				SW.Write ("\"explodemagnitude\" \"" + Rnd.Next (160, 200).ToString () + "\"\n");
				}
			else
				{
				int r = Rnd.Next (4);
				if (r > 0)
					SW.Write ("\"spawnobject\" \"" + (r + 25).ToString () + "\"\n");
				}

			WriteBlock (SW, x1, y1, "0", x2, y2, "64", new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Crate);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает стену на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура стены</param>
		/// <param name="LeftEnd">Тип левого торца стены</param>
		/// <param name="RightEnd">Тип правого торца стены</param>
		public static void WriteMapWall (StreamWriter SW, Point RelativePosition, string Texture,
			NeighborsTypes LeftEnd, NeighborsTypes RightEnd)
			{
			neighborLeft = LeftEnd;
			neighborRight = RightEnd;

			WriteMapBarrier (SW, RelativePosition, BarrierTypes.DefaultWall, Texture);
			}

		// Общий метод для стен и препятствий
		private static NeighborsTypes neighborLeft, neighborRight;
		private static void WriteMapBarrier (StreamWriter SW, Point RelativePosition, BarrierTypes Type, string Texture)
			{
			// Расчёт параметров
			int x1, y1, x2, y2;
			string xa, xb, xc, xd, ya, yb, yc, yd, z1, z2;
			string[] textures;

			int lDelta = 8, rDelta = 8, mDelta = 8;
			switch (Type)
				{
				case BarrierTypes.DefaultWall:
				default:
					z1 = "0";
					z2 = (WallHeightInt + 16).ToString ();

					string lTex = Texture;
					if (neighborLeft == NeighborsTypes.Gate)
						{
						lTex = "BorderRub01";
						}
					else if (neighborLeft == NeighborsTypes.Window)
						{
						lTex = "Metal08";
						lDelta = 0;
						}
					else if (neighborLeft == NeighborsTypes.WindowCorner)
						{
						lTex = "Metal08";
						}
					else if (neighborLeft == NeighborsTypes.Wall)
						{
						lDelta = 0;
						}

					string rTex = Texture;
					if (neighborRight == NeighborsTypes.Gate)
						{
						rTex = "BorderRub01";
						}
					else if (neighborRight == NeighborsTypes.Window)
						{
						rTex = "Metal08";
						rDelta = 0;
						}
					else if (neighborRight == NeighborsTypes.WindowCorner)
						{
						rTex = "Metal08";
						}
					else if (neighborRight == NeighborsTypes.Wall)
						{
						rDelta = 0;
						}

					textures = new string[] { Texture, Texture, Texture, Texture,
						lTex, lTex, rTex, rTex };
					break;

				case BarrierTypes.Gate:
					z1 = "0";
					z2 = (WallHeightInt - 8).ToString ();
					textures = new string[] { "Metal08", Texture, Texture, Texture,
						Texture, Texture, Texture, Texture };
					break;

				case BarrierTypes.Window:
					z1 = "8";
					z2 = (WallHeightInt - 8).ToString ();
					textures = new string[] { "Glass01", "Glass01", "Glass01", "Glass01",
						"Glass01", "Glass01", "Glass01", "Glass01" };
					rDelta = lDelta = 0;
					mDelta = 4;
					break;

				case BarrierTypes.WindowFrameTop:
				case BarrierTypes.GateFrameTop:
					z1 = (WallHeightInt - 8).ToString ();
					z2 = (WallHeightInt + 16).ToString ();
					textures = new string[] { Texture, "Metal08", Texture, Texture,
						Texture, Texture, Texture, Texture };

					if (Type == BarrierTypes.WindowFrameTop)
						rDelta = lDelta = 0;
					break;

				case BarrierTypes.WindowFrameBottom:
					z1 = "0";
					z2 = "8";
					textures = new string[] { "Metal08", Texture, Texture, Texture,
						Texture, Texture, Texture, Texture };
					rDelta = lDelta = 0;
					break;
				}

			// Запись
			SW.Write ("{\n");

			// Вертикальная
			if (RelativePosition.X % 2 == 0)
				{
				x1 = RelativePosition.X * WallLength / 2;
				y1 = (RelativePosition.Y - 1) * WallLength / 2;
				y2 = (RelativePosition.Y + 1) * WallLength / 2;

				xa = (x1 - mDelta).ToString ();
				xb = x1.ToString ();
				xc = (x1 + mDelta).ToString ();
				ya = y1.ToString ();
				yb = (y1 + lDelta).ToString ();
				yc = (y2 - rDelta).ToString ();
				yd = y2.ToString ();

				// Нижний и верхний торцы
				SW.Write ("( " + xc + " " + yc + " " + z2 + " ) " +
					"( " + xc + " " + yb + " " + z2 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					textures[0] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + xa + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yb + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z1 + " ) " +
					textures[1] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");

				// Лицевая и задняя сторона
				SW.Write ("( " + xa + " " + yb + " " + z1 + " ) " +
					"( " + xa + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yc + " " + z2 + " ) " +
					textures[2] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yb + " " + z1 + " ) " +
					"( " + xc + " " + yb + " " + z2 + " ) " +
					textures[3] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");

				// Верхний торец
				if (lDelta == 0)
					{
					SW.Write ("( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[4] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xc + " " + yb + " " + z1 + " ) " +
						"( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xb + " " + ya + " " + z2 + " ) " +
						textures[5] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}

				// Нижний торец
				if (rDelta == 0)
					{
					SW.Write ("( " + xa + " " + yd + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z2 + " ) " +
						textures[6] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xb + " " + yd + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xa + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yd + " " + z1 + " ) " +
						"( " + xb + " " + yd + " " + z2 + " ) " +
						textures[7] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				}

			// Горизонтальная
			else
				{
				y1 = RelativePosition.Y * WallLength / 2;
				x1 = (RelativePosition.X - 1) * WallLength / 2;
				x2 = (RelativePosition.X + 1) * WallLength / 2;

				xa = x1.ToString ();
				xb = (x1 + lDelta).ToString ();
				xc = (x2 - rDelta).ToString ();
				xd = x2.ToString ();
				ya = (y1 - mDelta).ToString ();
				yb = y1.ToString ();
				yc = (y1 + mDelta).ToString ();

				// Нижний и верхний торец
				SW.Write ("( " + xc + " " + ya + " " + z2 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					"( " + xa + " " + yb + " " + z2 + " ) " +
					textures[0] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xb + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yb + " " + z1 + " ) " +
					textures[1] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");

				// Лицевая и задняя сторона
				SW.Write ("( " + xb + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yc + " " + z2 + " ) " +
					textures[2] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + xc + " " + ya + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					textures[3] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");

				// Левый торец
				if (lDelta == 0)
					{
					SW.Write ("( " + xa + " " + ya + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[5] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}

				// Правый торец
				if (rDelta == 0)
					{
					SW.Write ("( " + xd + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xd + " " + yb + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xd + " " + yb + " " + z1 + " ) " +
						"( " + xd + " " + yb + " " + z2 + " ) " +
						textures[7] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				}

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает раму окна на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура стены для рамы</param>
		public static void WriteMapWindow (StreamWriter SW, Point RelativePosition, string Texture)
			{
			WriteMapBarrier (SW, RelativePosition, BarrierTypes.WindowFrameTop, Texture);
			WriteMapBarrier (SW, RelativePosition, BarrierTypes.WindowFrameBottom, Texture);
			}

		/// <summary>
		/// Метод записывает стекло окна на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		public static void WriteMapWindow (StreamWriter SW, Point RelativePosition)
			{
			SW.Write ("{\n");
			SW.Write ("\"classname\" \"func_breakable\"\n");
			SW.Write ("\"rendermode\" \"2\"\n");
			SW.Write ("\"renderamt\" \"80\"\n");
			SW.Write ("\"health\" \"20\"\n");
			SW.Write ("\"material\" \"0\"\n");

			WriteMapBarrier (SW, RelativePosition, BarrierTypes.Window, null);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Возможные типы препятствий
		/// </summary>
		public enum BarrierTypes
			{
			/// <summary>
			/// Обычная стена
			/// </summary>
			DefaultWall,

			/// <summary>
			/// Шлюз
			/// </summary>
			Gate,

			/// <summary>
			/// Верхняя рама окна
			/// </summary>
			WindowFrameTop,

			/// <summary>
			/// Нижняя рама окна
			/// </summary>
			WindowFrameBottom,

			/// <summary>
			/// Окно
			/// </summary>
			Window,

			/// <summary>
			/// Верхняя рама шлюза
			/// </summary>
			GateFrameTop
			}

		/// <summary>
		/// Возможные типы соседних препятствий
		/// </summary>
		public enum NeighborsTypes
			{
			/// <summary>
			/// Обычная стена
			/// </summary>
			Wall,

			/// <summary>
			/// Шлюз
			/// </summary>
			Gate,

			/// <summary>
			/// Окно
			/// </summary>
			Window,

			/// <summary>
			/// Окно под углом к стене
			/// </summary>
			WindowCorner,

			/// <summary>
			/// Свободный проход
			/// </summary>
			Passage
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
		/// Метод записывает звуковое сопровождение и эффект помещения на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Ambient">Тип эмбиента</param>
		/// <param name="Sound">Звук</param>
		public static void WriteMapSound (StreamWriter SW, Point RelativePosition, string Sound, AmbientTypes Ambient)
			{
			// Расчёт параметров
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
			SW.Write ("\"wad\" \"" + Program.MainWAD + "\"\n");

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
			// Расчёт параметров
			bool negX = ((Section & NegativeX) != 0);
			bool negY = ((Section & NegativeY) != 0);

			string x1 = (negX ? (-RealMapWidth / 2) : 0).ToString ();
			string x2 = (negX ? 0 : (RealMapWidth / 2)).ToString ();
			string y1 = (negY ? (-RealMapHeight / 2) : 0).ToString ();
			string y2 = (negY ? 0 : (RealMapHeight / 2)).ToString ();

			string h2 = (WallHeightInt + 32).ToString ();
			string h1 = (IsSkyTexture (RoofTexture) ? WallHeightInt + 16 : WallHeightInt).ToString ();

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
					(112 + rnd.Next (32)).ToString () + " 150\"\n";
				}

			// Расчёт параметров
			int x = RelativePosition.X * WallLength / 2;
			int y = RelativePosition.Y * WallLength / 2;

			// Добавление атмосферного освещения
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

			// Добавление источника света
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

			// Добавление лампы
			else if (!IsSkyTexture (RoofTexture))
				{
				WriteBlock (SW, (x - 16).ToString (), (y - 16).ToString (), (WallHeightInt - 4).ToString (),
					(x + 16).ToString (), (y + 16).ToString (), WallHeightInt.ToString (),
					new string[] { RoofTexture, RoofTexture, RoofTexture, RoofTexture, RoofTexture, "~LAMP07" },
					BlockTypes.Default);

				return false;
				}

			return true;
			}

		/// <summary>
		/// Возвращает true, если небесный свет уже был добавлен
		/// </summary>
		public static bool EnvironmentAdded = false;
		private static string lightColor = "";
		}
	}
