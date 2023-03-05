using System;
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
		/// <param name="AllowMonsterMakers">Флаг разрешения монстр-мейкеров</param>
		public static void WriteMapEnemy (StreamWriter SW, Point RelativePosition, Random Rnd,
			uint MapNumber, string Permissions, bool SecondFloor, bool UnderSky, bool AllowMonsterMakers)
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
			int z = SecondFloor ? (MapSupport.DefaultWallHeight - 16) : 0;
			int r = Rnd.Next (360);
			int enemy = Rnd.Next (prngRange);

			// Обработка для монстр-мейкеров
			bool mm = (awaitingNextMM && (Rnd.Next (3) == 0));
			bool countEnemy = false, countRat = false;

// Выбор врага
retry:
			switch (enemy)
				{
				// Солдаты
				default:
					if (Permissions.Contains (EnemiesPermissionsKeys[4]))
						{
						InitMonster (SW, mm, enemies[4]);
						countEnemy = true;

						if (!mm)
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
						InitMonster (SW, mm, enemies[11]);
						countEnemy = true;

						if (!mm)
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
						{
						InitMonster (SW, mm, enemies[5]);
						countEnemy = true;
						}
					else
						{
						goto check;
						}
					break;

				// Алиены
				case 10:
					if (Permissions.Contains (EnemiesPermissionsKeys[9]))
						{
						InitMonster (SW, mm, enemies[9]);
						countEnemy = true;
						}
					else
						{
						goto check;
						}
					break;

				// Куры
				case 19:
					if (Permissions.Contains (EnemiesPermissionsKeys[1]))
						{
						InitMonster (SW, mm, enemies[1]);
						countEnemy = true;
						}
					else
						{
						goto check;
						}
					break;

				// Ассассины
				case 12:
				case 13:
					if (Permissions.Contains (EnemiesPermissionsKeys[0]))
						{
						InitMonster (SW, mm, enemies[0]);
						countEnemy = true;
						}
					else
						{
						goto check;
						}
					break;

				// Турель
				case 14:
					z = 0;  // Только на полу
					if (Permissions.Contains (EnemiesPermissionsKeys[10]))
						{
						if (mm)
							goto check; // Недопустим для монстрмейкера

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
						// Не учитывается ачивкой
						}
					else
						{
						goto check;
						}
					break;

				// Солдаты алиенов
				case 18:
					if (Permissions.Contains (EnemiesPermissionsKeys[8]))
						{
						InitMonster (SW, mm, enemies[8]);
						countEnemy = true;
						}
					else
						{
						goto check;
						}
					break;

				// Контроллеры
				case 15:
					if (Permissions.Contains (EnemiesPermissionsKeys[2]))
						{
						InitMonster (SW, mm, enemies[2]);
						countEnemy = true;

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
						{
						InitMonster (SW, mm, enemies[3]);
						countEnemy = true;
						}
					else
						{
						goto check;
						}
					break;

				// Барнаклы
				case 21:
					if (MapSupport.TwoFloors && Permissions.Contains (EnemiesPermissionsKeys[7]) && !UnderSky)
						{
						if (mm)
							goto check; // Недопустим для монстрмейкера

						MapSupport.AddEntity (SW, enemies[7]);
						countEnemy = true;

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
						if (mm)
							goto check; // Недопустим для монстрмейкера

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

						// Не учитывается ачивкой
						}
					else
						{
						goto check;
						}
					break;
				}

finishM:
// Обработка монстр-мейкеров или создание ачивки
			if (!mm)
				{
				if (AllowMonsterMakers && (Rnd.Next (3) == 0))
					{
					availableMMNumber++;
					nextMMName = "MM" + MapSupport.BuildMapName (MapNumber) + "T" +
						availableMMNumber.ToString ("D3");

					SW.Write ("\"TriggerTarget\" \"" + nextMMName + "\"\n");
					awaitingNextMM = true;
					}
				else if (countEnemy || countRat)
					{
					SW.Write ("\"TriggerTarget\" \"Achi" + MapSupport.BuildMapName (MapNumber) +
						 (countRat ? "C2" : "C1") + "\"\n");

					if (countEnemy)
						realEnemiesQuantity++;
					if (countRat)
						realRatsQuantity++;
					}

				SW.Write ("\"TriggerCondition\" \"4\"\n");
				}

			// Общие параметры
			SW.Write ("\"angles\" \"0 " + r.ToString () + " 0\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " + z.ToString () + "\"\n");
			SW.Write ("}\n");

			// Финализация монстр-мейкера (имя было сброшено методом записи)
			if (mm && string.IsNullOrWhiteSpace (nextMMName))
				{
				/*if (availableMMNumber == 1)
					MapSupport.WriteMapSound (SW, RelativePosition, "Teleport1", MapSupport.AmbientTypes.Target);*/

				awaitingNextMM = false;
				}

			return;

// Проверка возможности выбора другого врага
check:
			enemy += Rnd.Next (3);
			if (enemy >= prngRange)
				{
				InitMonster (SW, mm, rats[Rnd.Next (rats.Count)]);
				countRat = true;

				goto finishM;
				}
			else
				{
				goto retry;
				}
			}

		// Состояние генерации монстр-мейкеров
		private static uint availableMMNumber = 0;
		private static bool awaitingNextMM = false;
		private static string nextMMName;

		// Метод обрабоатывает вилку между монстром и монст-мейкером
		private static void InitMonster (StreamWriter SW, bool AsMonsterMaker, string MonsterType)
			{
			// Как реальный NPC
			if (!AsMonsterMaker)
				{
				MapSupport.AddEntity (SW, MonsterType);
				return;
				}

			// Как монстрмейкер
			MapSupport.AddEntity (SW, "monstermaker");
			SW.Write ("\"targetname\" \"" + nextMMName + "\"\n");
			SW.Write ("\"monstercount\" \"1\"\n");
			SW.Write ("\"delay\" \"-1\"\n");
			SW.Write ("\"m_imaxlivechildren\" \"1\"\n");
			SW.Write ("\"monstertype\" \"" + MonsterType + "\"\n");
			/*SW.Write ("\"target\" \"Teleport1\"\n");*/
			SW.Write ("\"teleport_sound\" \"ambience/teleport1.wav\"\n");
			SW.Write ("\"teleport_sprite\" \"sprites/portal1.spr\"\n");

			nextMMName = "";    // Имя использовано
			}

		/// <summary>
		/// Метод добавляет триггер достижения на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="MapNumber">Номер карты, позволяющий выполнять наполнение с прогрессом</param>
		public static void WriteMapAchievement (StreamWriter SW, Point RelativePosition, uint MapNumber)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);
			string mn = MapSupport.BuildMapName (MapNumber);

			if (realEnemiesQuantity > 0)
				{
				SW.Write ("{\n");
				MapSupport.AddEntity (SW, "game_counter");
				SW.Write ("\"targetname\" \"Achi" + mn + "C1\"\n");
				SW.Write ("\"target\" \"Achi" + mn + "R1\"\n");
				SW.Write ("\"health\" \"" + realEnemiesQuantity.ToString () + "\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 64\"\n");

				SW.Write ("}\n{\n");
				MapSupport.AddEntity (SW, "game_player_set_health");
				SW.Write ("\"targetname\" \"Achi" + mn + "R1\"\n");
				SW.Write ("\"dmg\" \"200\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 72\"\n");

				SW.Write ("}\n{\n");
				MapSupport.AddEntity (SW, "env_message");
				SW.Write ("\"spawnflags\" \"2\"\n");
				SW.Write ("\"targetname\" \"Achi" + mn + "R1\"\n");
				SW.Write ("\"messagesound\" \"items/suitchargeok1.wav\"\n");
				SW.Write ("\"messagevolume\" \"10\"\n");
				SW.Write ("\"messageattenuation\" \"3\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 76\"\n");

				if (availableMMNumber > 0)
					SW.Write ("\"message\" \"ACHI_ESRM_03\"\n");
				else
					SW.Write ("\"message\" \"ACHI_ESRM_01\"\n");

				SW.Write ("}\n");
				}

			if (realRatsQuantity > 0)
				{
				SW.Write ("{\n");

				MapSupport.AddEntity (SW, "game_counter");
				SW.Write ("\"targetname\" \"Achi" + mn + "C2\"\n");
				SW.Write ("\"target\" \"Achi" + mn + "R2\"\n");
				SW.Write ("\"health\" \"" + realRatsQuantity.ToString () + "\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 80\"\n");

				SW.Write ("}\n{\n");
				MapSupport.AddEntity (SW, "env_message");
				SW.Write ("\"spawnflags\" \"2\"\n");
				SW.Write ("\"targetname\" \"Achi" + mn + "R2\"\n");
				SW.Write ("\"messagesound\" \"items/suitchargeok1.wav\"\n");
				SW.Write ("\"messagevolume\" \"10\"\n");
				SW.Write ("\"messageattenuation\" \"3\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 88\"\n");

				if (availableMMNumber > 0)
					SW.Write ("\"message\" \"ACHI_ESRM_04\"\n");
				else
					SW.Write ("\"message\" \"ACHI_ESRM_02\"\n");

				SW.Write ("}\n");
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
			"",	// Турели, ручная подстановка
			"monster_zombie"
			};

		// Счётчики реально добавленных сущностей
		private static uint realEnemiesQuantity = 0;
		private static uint realRatsQuantity = 0;

		/// <summary>
		/// Метод сбрасывает счётчики в случае необходимости генерации новой карты
		/// </summary>
		public static void ResetCounters ()
			{
			realEnemiesQuantity = 0;
			realRatsQuantity = 0;
			}

		// Перечень монстров-заглушек
		private static List<string> rats = new List<string> {
			"monster_rat" ,
			"monster_cockroach"
			};

		/// <summary>
		/// Возвращает количество реально добавленных на карту крыс и тараканов
		/// </summary>
		public static uint RatsQuantity
			{
			get
				{
				return realRatsQuantity;
				}
			}

		/// <summary>
		/// Возвращает количество врагов, замещённых монстрмейкерами
		/// </summary>
		public static uint MonsterMakersQuantity
			{
			get
				{
				return availableMMNumber;
				}
			}
		}
	}
