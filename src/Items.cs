using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="MapNumber">Номер текущей карты</param>
		/// <param name="AllowSecondFloor">Флаг, разрешающий размещение на внутренних площадках</param>
		/// <param name="Permissions">Строка разрешений для видов оружия</param>
		public static void WriteMapItem (StreamWriter SW, Point RelativePosition, uint MapNumber,
			bool AllowSecondFloor, string Permissions)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);

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
				MapSupport.AddEntity (SW, "item_antidote");
				SW.Write ("\"MinimumToTrigger\" \"1\"\n");
				goto finishItem;
				}

			// Запись объекта
			string doc = "item_security";
			int item = RDGenerics.RND.Next (prngRange);

			// Выбор предмета
			retry:
			switch (item)
				{
				// Аптечки
				default:
					if (Permissions.Contains (ItemsPermissionsKeys[0]))
						MapSupport.AddEntity (SW, items[0]);
					else
						goto check;
					break;

				// Броня
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
					if (Permissions.Contains (ItemsPermissionsKeys[1]))
						MapSupport.AddEntity (SW, items[1]);
					else
						goto check;
					break;

				// Гранаты
				case 6:
				case 17:
					if (Permissions.Contains (ItemsPermissionsKeys[2]))
						MapSupport.AddEntity (SW, items[2]);
					else
						goto check;
					break;

				// Пистолет
				case 15:
					if (Permissions.Contains (ItemsPermissionsKeys[3]))
						MapSupport.AddEntity (SW, items[3]);
					else
						goto check;
					break;

				// Гранаты с радиоуправлением
				case 16:
					if (Permissions.Contains (ItemsPermissionsKeys[4]))
						MapSupport.AddEntity (SW, items[4]);
					else
						goto check;
					break;

				// .357
				case 5:
				case 18:
					if (Permissions.Contains (ItemsPermissionsKeys[5]))
						MapSupport.AddEntity (SW, items[5]);
					else
						goto check;
					break;

				// Арбалет
				case 19:
				case 20:
					if (Permissions.Contains (ItemsPermissionsKeys[6]))
						MapSupport.AddEntity (SW, items[6]);
					else
						goto check;
					break;

				// Гаусс
				case 21:
				case 22:
					if (Permissions.Contains (ItemsPermissionsKeys[7]))
						MapSupport.AddEntity (SW, items[7]);
					else
						goto check;
					break;

				// Монтировка или радиограната
				case 23:
					if (RDGenerics.RND.Next (5) > 3)
						{
						if (Permissions.Contains (ItemsPermissionsKeys[8]))
							MapSupport.AddEntity (SW, items[8]);
						else
							goto check;
						}
					else
						{
						if (Permissions.Contains (ItemsPermissionsKeys[4]))
							MapSupport.AddEntity (SW, items[4]);
						else
							goto check;
						}
					break;

				// Улей или граната
				case 24:
					if (RDGenerics.RND.Next (5) > 3)
						{
						if (Permissions.Contains (ItemsPermissionsKeys[9]))
							MapSupport.AddEntity (SW, items[9]);
						else
							goto check;
						}
					else
						{
						if (Permissions.Contains (ItemsPermissionsKeys[2]))
							MapSupport.AddEntity (SW, items[2]);
						else
							goto check;
						}
					break;
				}

			finishItem:
			int z = 40;
			if (AllowSecondFloor)
				z += (RDGenerics.RND.Next (2) * MapSupport.DefaultWallHeight);

			SW.Write ("\"angles\" \"0 " + RDGenerics.RND.Next (360) + " 0\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
				z.ToString () + "\"\n");   // На некоторой высоте над полом
			SW.Write ("}\n");
			return;

			// Проверка возможности выбора другого врага
			check:
			item += RDGenerics.RND.Next (3);
			if (item >= prngRange)
				{
				MapSupport.AddEntity (SW, doc);
				goto finishItem;
				}
			else
				{
				goto retry;
				}
			}

		// Флаг наличия записи об объекте-достижении
		private static bool hiddenObjectWritten = false;

		/// <summary>
		/// Набор ключевых символов разрешений для предметов
		/// </summary>
		public static string[] ItemsPermissionsKeys = new string[] {
			// Прямая генерация
			"k", "b", "g", "9", "s", "3", "c", "u", "w", "h", 
			// Ящики
			"r", "o", "p", "t", "n", "e", "a"
			};

		// Набор имён классов предметов
		private static string[] items = new string[] {
			// Прямая генерация
			"item_healthkit",
			"item_battery",
			"weapon_handgrenade",
			"weapon_9mmhandgun",
			"weapon_satchel",
			"weapon_357",
			"weapon_crossbow",
			"weapon_gauss",
			"weapon_crowbar",
			"weapon_hornetgun",

			// Ящики
			"weapon_9mmAR",		// Требует отключения солдат
			"weapon_shotgun",	// Требует отключения солдат
			"weapon_rpg",
			"weapon_tripmine",
			"weapon_snark",
			"weapon_egon",
			"weapon_axe"
			};

		/// <summary>
		/// Метод проверяет допустимость данного объекта для «размещения в ящике»
		/// </summary>
		/// <param name="Permissions">Строка разрешений</param>
		/// <param name="ItemIndex">Индекс объекта для ящика</param>
		/// <returns></returns>
		public static bool IsCrateItemAllowed (string Permissions, int ItemIndex)
			{
			// Контроль
			int idx = weaponEquivalents.IndexOf (ItemIndex);
			if (idx < 0)
				{
				idx = ammoEquivalents.IndexOf (ItemIndex);
				if (idx < 0)
					return true;
				}

			return Permissions.Contains (ItemsPermissionsKeys[idx]);
			}

		// Набор сопоставлений для собираемых объектов при генерации из ящиков
		private static List<int> weaponEquivalents = new List<int> {
			// Прямая генерация
			2, 1, 17, 3, 19, 12, 10, 23, 22, 21, 
			// Ящики
			5, 8, 14, 18, 20, 24, 26
			};

		private static List<int> ammoEquivalents = new List<int> {
			// Прямая генерация
			-1, -1, -1, 4, -1, 13, 11, 16, -1, -1, 
			// Ящики
			6, 9, 15, -1, -1, -1, -1
			};
		}
	}
