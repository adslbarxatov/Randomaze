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
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);

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
					prngRange = (int)MapNumber + 8;
					break;

				default:
					prngRange = 23;
					break;
				}

			// Добавление
			string rat2 = Rnd.Next (2) == 0 ? "monster_rat" : "monster_cockroach";
			int z = SecondFloor ? (MapSupport.DefaultWallHeight - 16) : 0;
			int r = Rnd.Next (360);
			int enemy = Rnd.Next (prngRange);

// Выбор врага
retry:
			switch (enemy)
				{
				// Солдаты
				default:
					if (Permissions.Contains (EnemiesPermissionsKeys[4]))
						{
						MapSupport.AddEntity (SW, enemies[4]);
						SW.Write ("\"weapons\" \"" + gruntWeapons[Rnd.Next (gruntWeapons.Length)] + "\"\n");
						}
					else
						{
						goto check;
						}
					break;

				// Зомби
				case 0:
				case 16:
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[11]))
						{
						MapSupport.AddEntity (SW, enemies[11]);
						SW.Write ("\"skin\" \"" + Rnd.Next (2).ToString () + "\"\n");
						}
					else
						{
						goto check;
						}
					break;

				// Крабы
				case 1:
				case 17:
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[5]))
						MapSupport.AddEntity (SW, enemies[5]);
					else
						goto check;
					break;

				// Алиены
				case 10:
					if (Permissions.Contains (EnemiesPermissionsKeys[9]))
						MapSupport.AddEntity (SW, enemies[9]);
					else
						goto check;
					break;

				// Куры
				case 19:
					if (Permissions.Contains (EnemiesPermissionsKeys[1]))
						MapSupport.AddEntity (SW, enemies[1]);
					else
						goto check;
					break;

				// Ассассины
				case 12:
				case 13:
					if (Permissions.Contains (EnemiesPermissionsKeys[0]))
						MapSupport.AddEntity (SW, enemies[0]);
					else
						goto check;
					break;

				// Турель
				case 14:
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[10]))
						{
						switch (Rnd.Next (3))
							{
							case 0:
								MapSupport.AddEntity (SW, "monster_turret");
								break;

							case 1:
								MapSupport.AddEntity (SW, "monster_miniturret");
								break;

							case 2:
								MapSupport.AddEntity (SW, "monster_sentry");
								break;
							}

						SW.Write ("\"spawnflags\" \"32\"\n");
						SW.Write ("\"orientation\" \"0\"\n");
						}
					else
						{
						goto check;
						}
					break;

				// Солдаты алиенов
				case 18:
					if (Permissions.Contains (EnemiesPermissionsKeys[8]))
						MapSupport.AddEntity (SW, enemies[8]);
					else
						goto check;
					break;

				// Контроллеры
				case 15:
					if (Permissions.Contains (EnemiesPermissionsKeys[2]))
						{
						MapSupport.AddEntity (SW, enemies[2]);
						z = MapSupport.WallHeight - 96;    // Ближе к потолку
						}
					else
						{
						goto check;
						}
					break;

				// Собаки
				case 11:
				case 20:
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[3]))
						MapSupport.AddEntity (SW, enemies[3]);
					else
						goto check;
					break;

				// Барнаклы
				case 21:
					if (MapSupport.TwoFloors && Permissions.Contains (EnemiesPermissionsKeys[7]) && !UnderSky)
						{
						MapSupport.AddEntity (SW, enemies[7]);
						z = MapSupport.WallHeight;  // Только на потолке
						}
					else
						{
						goto check;
						}
					break;

				// Мины
				case 2:
				case 22:
					List<CPResults> rWalls = RandomazeForm.GetSurroundingWalls (RelativePosition,
						FurnitureTypes.Computer);
					if ((rWalls.Count > 0) && Permissions.Contains (EnemiesPermissionsKeys[6]))
						{
						MapSupport.AddEntity (SW, enemies[6]);
						SW.Write ("\"spawnflags\" \"1\"\n");
						z = 16 + Rnd.Next (2) * 48;
						int off = MapSupport.WallLength / 2 - 16;

						switch (rWalls[Rnd.Next (rWalls.Count)])
							{
							default:
							case CPResults.Left:
								r = 0;
								p.X -= off;
								break;

							case CPResults.Right:
								r = 180;
								p.X += off;
								break;

							case CPResults.Down:
								r = 90;
								p.Y -= off;
								break;

							case CPResults.Up:
								r = 270;
								p.Y += off;
								break;
							}
						}
					else
						{
						goto check;
						}
					break;
				}

// Завершение записи
finish:
			SW.Write ("\"angles\" \"0 " + r.ToString () + " 0\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " + z.ToString () + "\"\n");
			SW.Write ("}\n");
			return;

// Проверка возможности выбора другого врага
check:
			enemy += Rnd.Next (3);
			if (enemy >= prngRange)
				{
				MapSupport.AddEntity (SW, rat2);
				goto finish;
				}
			else
				{
				goto retry;
				}
			}

		// Подстановки номеров оружия для солдат
		private static string[] gruntWeapons = new string[] { "1", "3", "5", "8", "10" };

		/// <summary>
		/// Набор ключевых символов разрешений для врагов
		/// </summary>
		public static string[] EnemiesPermissionsKeys = new string[] {
			"a", "b", "c", "e", "g", "h", "m", "n", "r", "s", "t", "z"
			};

		// Набор названий классов для врагов
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
			"",	// Ручная подстановка
			"monster_zombie"
			};
		}
	}
