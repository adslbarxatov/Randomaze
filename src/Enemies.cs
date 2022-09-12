﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс предоставляет обработчики для сущностей противников
	/// </summary>
	public static class EnemiesSupport
		{
		/// <summary>
		/// Метод добавляет врагов на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Rnd">ГПСЧ</param>
		/// <param name="MapNumber">Номер карты, позволяющий выполнять наполнение с прогрессом</param>
		/// <param name="Permissions">Строка флагов разрешённых врагов</param>
		/// <param name="SecondFloor">Флаг установки врага на внутренней площадке</param>
		/// <param name="UnderSky">Флаг расположения под небом</param>
		public static void WriteMapEnemy (StreamWriter SW, Point RelativePosition, Random Rnd,
			uint MapNumber, string Permissions, bool SecondFloor, bool UnderSky)
			{
			// Расчёт параметров
			int x, y;
			MapSupport.EvaluateAbsolutePosition (RelativePosition, out x, out y);

			// Запись
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
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
					prngRange = (int)MapNumber + 8;
					break;

				default:
					prngRange = 24;
					break;
				}

			// Добавление
			string rat = Rnd.Next (2) == 0 ? "\"classname\" \"monster_rat\"\n" :
				"\"classname\" \"monster_cockroach\"\n";
			int z = SecondFloor ? MapSupport.DefaultWallHeight : 0;
			int r = Rnd.Next (360);

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
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[11]))
						{
						SW.Write ("\"classname\" \"" + enemies[11] + "\"\n");
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
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[5]))
						SW.Write ("\"classname\" \"" + enemies[5] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Алиены
				case 10:
					if (Permissions.Contains (EnemiesPermissionsKeys[9]))
						SW.Write ("\"classname\" \"" + enemies[9] + "\"\n");
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
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[10]))
						{
						SW.Write ("\"classname\" \"" + enemies[10] + "\"\n");
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
					if (Permissions.Contains (EnemiesPermissionsKeys[8]))
						SW.Write ("\"classname\" \"" + enemies[8] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Контроллеры
				case 15:
					if (Permissions.Contains (EnemiesPermissionsKeys[2]))
						{
						SW.Write ("\"classname\" \"" + enemies[2] + "\"\n");
						z = MapSupport.WallHeight - 96;    // Ближе к потолку
						}
					else
						{
						SW.Write (rat);
						}
					break;

				// Собаки
				case 11:
				case 20:
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[3]))
						SW.Write ("\"classname\" \"" + enemies[3] + "\"\n");
					else
						SW.Write (rat);
					break;

				// Барнаклы
				case 21:
					if (MapSupport.TwoFloors && Permissions.Contains (EnemiesPermissionsKeys[7]) && !UnderSky)
						{
						SW.Write ("\"classname\" \"" + enemies[7] + "\"\n");
						z = MapSupport.WallHeight;  // Только на потолке
						}
					else
						{
						SW.Write (rat);
						}
					break;

				// Мины
				case 22:
				case 23:
					List<CPResults> rWalls = RandomazeForm.GetSurroundingWalls (RelativePosition, FurnitureTypes.Computer);
					if ((rWalls.Count > 0) && Permissions.Contains (EnemiesPermissionsKeys[6]))
						{
						SW.Write ("\"classname\" \"" + enemies[6] + "\"\n");
						SW.Write ("\"spawnflags\" \"1\"\n");
						z = 16 + Rnd.Next (2) * 48;
						int off = MapSupport.WallLength / 2 - 16;

						switch (rWalls[Rnd.Next (rWalls.Count)])
							{
							default:
							case CPResults.Left:
								r = 0;
								x -= off;
								break;

							case CPResults.Right:
								r = 180;
								x += off;
								break;

							case CPResults.Down:
								r = 90;
								y -= off;
								break;

							case CPResults.Up:
								r = 270;
								y += off;
								break;
							}
						}
					else
						{
						SW.Write (rat);
						}
					break;
				}

			SW.Write ("\"angles\" \"0 " + r.ToString () + " 0\"\n");
			SW.Write ("\"origin\" \"" + x.ToString () + " " + y.ToString () + " " + z.ToString () + "\"\n");
			SW.Write ("}\n");
			}

		// Подстановки номеров оружия для солдат
		private static string[] gruntWeapons = new string[] { "1", "3", "5", "8", "10" };

		/// <summary>
		/// Набор ключевых символов разрешений для врагов
		/// </summary>
		public static string[] EnemiesPermissionsKeys = new string[] {
			"a", "b", "c", "e", "g", "h", "m", "n", "r", "s", "t", "z"
			};

		private static string[] enemies = new string[] {
			"monster_human_assassin",
			"monster_bullchicken",
			"monster_alien_controller",
			"monster_houndeye",
			"monster_human_grunt",
			"monster_headcrab",
			"monster_tripmine",
			"monster_barnacle",
			"monster_alien_grunt",
			"monster_alien_slave",
			"monster_miniturret",
			"monster_zombie"
			};
		}
	}
