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
		/// <param name="Probabilities">Список вероятностей генерации врагов</param>
		/// <param name="SecondFloor">Флаг установки врага на внутренней площадке</param>
		/// <param name="CeilingNotAllowed">Флаг указывает наневозможность ориентации на потолке</param>
		/// <param name="AllowMonsterMakers">Флаг разрешения монстр-мейкеров</param>
		/// <param name="WaterLevel">Флаг указывает, что воды достаточно для плавающих монстров</param>
		public static void WriteMapEnemy (StreamWriter SW, Point RelativePosition,
			byte[] Probabilities, bool SecondFloor, bool CeilingNotAllowed, bool AllowMonsterMakers, uint WaterLevel)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);

			List<CPResults> rWalls = RandomazeForm.GetSurroundingWalls (RelativePosition,
				FurnitureTypes.Computer);
			List<byte> enemiesProbabilityLine = new List<byte> ();

			for (int i = 0; i < Probabilities.Length; i++)
				{
				if ((i == m_hou) && (MapSupport.MapNumber < 3))
					continue;
				if ((i == m_slv) && (MapSupport.MapNumber < 4))
					continue;
				if ((i == m_asn) && (MapSupport.MapNumber < 6))
					continue;
				if ((i == m_tur) && (MapSupport.MapNumber < 7))
					continue;
				if ((i == m_con) && (MapSupport.MapNumber < 8))
					continue;
				if ((i == m_bul) && (MapSupport.MapNumber < 10))
					continue;
				if ((i == m_agr) && (MapSupport.MapNumber < 11))
					continue;
				if ((i == m_brn) && (MapSupport.MapNumber < 12) && !CeilingNotAllowed && MapSupport.TwoFloors)
					continue;
				if ((i == m_min) && (rWalls.Count < 1))
					continue;

				for (int j = 0; j < Probabilities[i]; j++)
					enemiesProbabilityLine.Add ((byte)i);
				}

			// Запись
			SW.Write ("{\n");

			// Диапазон противников задаётся ограничением на верхнюю границу диапазона ГПСЧ
			/*int prngRange;
			switch (MapSupport.MapNumber)
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
					prngRange = (int)MapSupport.MapNumber + 8;
					break;

				default:
					prngRange = 24;
					break;
				}*/

			// Добавление
			int z = SecondFloor ? (MapSupport.DefaultWallHeight - 16) : 0;
			int r = RDGenerics.RND.Next (360);

			int crEnemy;
			if ((enemiesProbabilityLine.Count < 1) || (RDGenerics.RND.Next (enemiesProbabilityLine.Count) == 0))
				crEnemy = m_lee;
			else
				crEnemy = enemiesProbabilityLine[RDGenerics.RND.Next (enemiesProbabilityLine.Count)];

			// Обработка для монстр-мейкеров
			bool noMM = false;
			bool countEnemy = false, countRat = false;
			bool mMaker = awaitingNextMM && (RDGenerics.RND.Next (3) == 0);

			// Выбор врага
			/*retry:*/
			switch (crEnemy)
				{
				// Монстры-заглушки
				default:
				case m_lee:
					if (WaterLevel > 0)
						{
						InitMonster (SW, false, enemies[m_lee]);
						z = 4;
						}
					else
						{
						InitMonster (SW, false, rats[RDGenerics.RND.Next (rats.Count)]);
						}

					// Не могут быть монстрмейкерами
					noMM = true;
					// Обязательно должны пройти через счётчики
					mMaker = false;
					countRat = true;
					break;

				case m_gru:
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_gru]))
						{*/
					InitMonster (SW, mMaker, enemies[m_gru]);
					countEnemy = true;

					if (!mMaker)
						SW.Write ("\"weapons\" \"" +
							gruntWeapons[RDGenerics.RND.Next (gruntWeapons.Length)] + "\"\n");
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Зомби
				case m_zom:
					/*case 16:*/
					z = 0;  // Только на полу
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_zom]))
						{*/
					InitMonster (SW, mMaker, enemies[m_zom]);
					countEnemy = true;

					if (!mMaker)
						SW.Write ("\"skin\" \"" + RDGenerics.RND.Next (2).ToString () + "\"\n");
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Крабы
				case m_hed:
					/*case 17:*/
					z = 0;  // Только на полу
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_hed]))
						{*/
					InitMonster (SW, mMaker, enemies[m_hed]);
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Алиены
				case m_slv:
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_slv]))
						{*/
					InitMonster (SW, mMaker, enemies[m_slv]);
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Куры
				case m_bul:
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_bul]))
						{*/
					InitMonster (SW, mMaker, enemies[m_bul]);
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Ассассины
				case m_asn:
					/*case 13:*/
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_asn]))
						{*/
					InitMonster (SW, mMaker, enemies[m_asn]);
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Турель
				case m_tur:
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_tur]))
						{*/
					int t = RDGenerics.RND.Next (turrets.Count);
					InitMonster (SW, false, turrets[t]);
					bool turret = (t < 2);

					if (MapSupport.TwoFloors && !CeilingNotAllowed && turret && (RDGenerics.RND.Next (2) == 0))
						{
						SW.Write ("\"orientation\" \"1\"\n");
						z = MapSupport.WallHeight;
						}
					else
						{
						SW.Write ("\"orientation\" \"0\"\n");
						z = 0;
						}

					SW.Write ("\"spawnflags\" \"32\"\n");
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Солдаты алиенов
				case m_agr:
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_agr]))
						{*/
					InitMonster (SW, mMaker, enemies[m_agr]);
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Контроллеры
				case m_con:
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_con]))
						{*/
					InitMonster (SW, mMaker, enemies[m_con]);
					countEnemy = true;

					z = MapSupport.WallHeight - 96;    // Ближе к потолку
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Собаки
				/*case 11:*/
				case m_hou:
					z = 0;  // Только на полу
					/*if (Permissions.Contains (EnemiesPermissionsKeys[m_hou]))
						{*/
					InitMonster (SW, mMaker, enemies[m_hou]);
					countEnemy = true;
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Барнаклы
				case m_brn:
					/*if (MapSupport.TwoFloors && Permissions.Contains (EnemiesPermissionsKeys[m_brn]) &&
						!CeilingNotAllowed)
						{*/
					InitMonster (SW, false, enemies[m_brn]);
					countEnemy = true;

					z = MapSupport.WallHeight;  // Только на потолке
					/*}
				else
					{
					goto check;
					}*/
					break;

				// Мины
				/*case 2:*/
				case m_min:
					/*if ((rWalls.Count > 0) && Permissions.Contains (EnemiesPermissionsKeys[m_min]))
						{*/
					InitMonster (SW, false, enemies[m_min]);
					noMM = true;
					// Не учитывается ачивкой, не может формироваться мейкером, не может триггерить мейкер

					SW.Write ("\"spawnflags\" \"1\"\n");
					z = 16 + RDGenerics.RND.Next (2) * 48;
					int off = MapSupport.WallLength / 2 - 16;

					switch (rWalls[RDGenerics.RND.Next (rWalls.Count)])
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
					/*}
				else
					{
					goto check;
					}*/
					break;

					/*// Пиявки
					case 23:
						if ((WaterLevel > 0) && Permissions.Contains (EnemiesPermissionsKeys[m_lee]))
							{
							InitMonster (SW, false, enemies[m_lee]);
							z = 4;
							noMM = true;
							mm = false;
							countRat = true;
							// Учитывается ачивкой, не может формироваться мейкером, не может триггерить мейкер
							}
						else
							{
							goto check;
							}
						break;*/
				}

			/*finishM:*/
			// Обработка монстр-мейкеров или создание ачивки
			if (!mMaker)
				{
				if (AllowMonsterMakers && !noMM && (RDGenerics.RND.Next (3) == 0))
					{
					availableMMNumber++;
					nextMMName = "MM" + MapSupport.BuildMapName () + "T" +
						availableMMNumber.ToString ("D3");

					SW.Write ("\"TriggerTarget\" \"" + nextMMName + "\"\n");
					awaitingNextMM = true;

					SW.Write ("\"TriggerCondition\" \"4\"\n");
					}
				else if (countEnemy || countRat)
					{
					SW.Write ("\"TriggerTarget\" \"Achi" + MapSupport.BuildMapName () +
						 (countRat ? "C2" : "C1") + "\"\n");

					if (countEnemy)
						realEnemiesQuantity++;
					if (countRat)
						realRatsQuantity++;

					SW.Write ("\"TriggerCondition\" \"4\"\n");
					}
				}

			// Общие параметры
			SW.Write ("\"angles\" \"0 " + r.ToString () + " 0\"\n");
			SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " + z.ToString () + "\"\n");
			SW.Write ("}\n");

			// Финализация монстр-мейкера (имя было сброшено методом записи)
			if (mMaker && string.IsNullOrWhiteSpace (nextMMName))
				awaitingNextMM = false;

			/*return;*/

			/*// Проверка возможности выбора другого врага
			check:
			enemy += RDGenerics.RND.Next (3);
			if (enemy >= prngRange)
				{
				if (WaterLevel > 0)
					{
					InitMonster (SW, false, enemies[m_lee]);
					z = 4;
					}
				else
					{
					InitMonster (SW, false, rats[RDGenerics.RND.Next (rats.Count)]);
					}
				countRat = true;
				mm = false;
				noMM = true;

				goto finishM;
				}
			else
				{
				goto retry;
				}*/
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
			SW.Write ("\"teleport_sound\" \"ambience/teleport1.wav\"\n");
			SW.Write ("\"teleport_sprite\" \"sprites/portal1.spr\"\n");

			nextMMName = "";    // Имя использовано
			}

		/// <summary>
		/// Метод добавляет триггер достижения на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="TwoButtons">Флаг указывает на наличие второй кнопки</param>
		/// <param name="Water">Флаг наличия воды на карте</param>
		public static void WriteMapAchievement (StreamWriter SW, Point RelativePosition,
			bool TwoButtons, bool Water)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);
			p.X += 16;
			p.Y += 16;

			string mn = MapSupport.BuildMapName ();

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

				SW.Write ("}\n");

				SW.Write ("{\n");
				MapSupport.AddEntity (SW, "monstermaker");
				SW.Write ("\"monstercount\" \"1\"\n");
				SW.Write ("\"delay\" \"-1\"\n");
				SW.Write ("\"m_imaxlivechildren\" \"1\"\n");
				SW.Write ("\"monstertype\" \"monster_barney\"\n");
				SW.Write ("\"teleport_sound\" \"ambience/teleport1.wav\"\n");
				SW.Write ("\"teleport_sprite\" \"sprites/portal1.spr\"\n");
				SW.Write ("\"angles\" \"0 " + RDGenerics.RND.Next (360).ToString () + " 0\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 0\"\n");

				if (TwoButtons)
					{
					// Иначе Барни застрянет во втором шлюзе
					SW.Write ("\"targetname\" \"Achi" + mn + "R2D\"\n");

					SW.Write ("}\n{\n");
					MapSupport.AddEntity (SW, "trigger_changetarget");
					SW.Write ("\"targetname\" \"Achi" + mn + "R2\"\n");
					SW.Write ("\"target\" \"MGate" + mn + "\"\n");
					SW.Write ("\"m_iszNewTarget\" \"Achi" + mn + "R2D\"\n");
					SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 92\"\n");
					}
				else
					{
					SW.Write ("\"targetname\" \"Achi" + mn + "R2\"\n");
					}

				SW.Write ("}\n{\n");
				MapSupport.AddEntity (SW, "env_message");
				SW.Write ("\"spawnflags\" \"2\"\n");
				SW.Write ("\"targetname\" \"Achi" + mn + "R2\"\n");
				SW.Write ("\"messagesound\" \"items/suitchargeok1.wav\"\n");
				SW.Write ("\"messagevolume\" \"10\"\n");
				SW.Write ("\"messageattenuation\" \"3\"\n");
				SW.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 88\"\n");

				if (Water)
					SW.Write ("\"message\" \"ACHI_ESRM_02_W\"\n");
				else
					SW.Write ("\"message\" \"ACHI_ESRM_02\"\n");

				SW.Write ("}\n");
				}
			}

		// Подстановки номеров оружия для солдат
		private static string[] gruntWeapons = new string[] { "1", "3", "5", "8", "10" };

		/*/// <summary>
		/// Набор ключевых символов разрешений для врагов
		/// </summary>
		public static string[] EnemiesPermissionsKeys = new string[] {
			"a", "b", "c", "e", "g", "h", "l", "m", "n", "r", "s", "t", "z"
			};*/

		/// <summary>
		/// Возвращает число доступных видов врагов
		/// </summary>
		public static uint AvailableEnemiesTypes
			{
			get
				{
				return (uint)enemies.Length;
				}
			}

		// Набор названий классов для врагов
		private static string[] enemies = new string[] {
			"monster_human_assassin",
			"monster_bullchicken",
			"monster_alien_controller",
			"monster_houndeye",
			"monster_human_grunt",
			"monster_headcrab",
			"monster_leech",
			"monster_tripmine",
			"monster_barnacle",
			"monster_alien_grunt",
			"monster_alien_slave",
			"",	// Турели, ручная подстановка
			"monster_zombie"
			};
		private const int m_asn = 0;
		private const int m_bul = 1;
		private const int m_con = 2;
		private const int m_hou = 3;
		private const int m_gru = 4;
		private const int m_hed = 5;
		private const int m_lee = 6;
		private const int m_min = 7;
		private const int m_brn = 8;
		private const int m_agr = 9;
		private const int m_slv = 10;
		private const int m_tur = 11;
		private const int m_zom = 12;

		/// <summary>
		/// Метод удаляет барнакла из разрешающей строки при выключении режима двух этажей
		/// </summary>
		/// <param name="EnemiesPermissionLine">Имеющаяся строка разрешений для врагов</param>
		public static void RemoveBarnacle (ref List<byte> EnemiesPermissionLine)
			{
			/*return EnemiesPermissionLine.Replace (EnemiesPermissionsKeys[m_brn], "");*/
			EnemiesPermissionLine[m_brn] = 0;
			}

		/// <summary>
		/// Метод удаляет барнакла из разрешающей строки при выключении режима двух этажей
		/// </summary>
		/// <param name="EnemiesPermissionLine">Имеющаяся строка разрешений для врагов</param>
		public static void RemoveLeech (ref List<byte> EnemiesPermissionLine)
			{
			/*return EnemiesPermissionLine.Replace (EnemiesPermissionsKeys[m_lee], "");*/
			EnemiesPermissionLine[m_lee] = 0;
			}

		/// <summary>
		/// Метод удаляет барнакла из разрешающей строки при выключении режима двух этажей
		/// </summary>
		/// <param name="EnemiesPermissionLine">Имеющаяся строка разрешений для врагов</param>
		public static void RemoveBarnacle (ref byte[] EnemiesPermissionLine)
			{
			EnemiesPermissionLine[m_brn] = 0;
			}

		/// <summary>
		/// Метод удаляет барнакла из разрешающей строки при выключении режима двух этажей
		/// </summary>
		/// <param name="EnemiesPermissionLine">Имеющаяся строка разрешений для врагов</param>
		public static void RemoveLeech (ref byte[] EnemiesPermissionLine)
			{
			EnemiesPermissionLine[m_lee] = 0;
			}

		/// <summary>
		/// Метод возвращает true, если строка разрешений врагов содержит хедкраба
		/// </summary>
		/// <param name="EnemiesPermissionLine">Имеющаяся строка разрешений для врагов</param>
		public static bool IsHeadcrabAllowed (byte[] EnemiesPermissionLine)
			{
			/*return EnemiesPermissionLine.Contains (EnemiesPermissionsKeys[m_hed]);*/
			return EnemiesPermissionLine[m_hed] != 0;
			}

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
			//"monster_cockroach",
			};
		private static List<string> turrets = new List<string> {
			"monster_turret",
			"monster_miniturret",
			"monster_sentry",
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
