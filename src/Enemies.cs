using System.Collections.Generic;
using System.Drawing;

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
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Probabilities">Список вероятностей генерации врагов</param>
		/// <param name="AllowSecondFloor">Флаг разрешения установки врага на внутренней площадке</param>
		/// <param name="CeilingNotAllowed">Флаг указывает на невозможность ориентации на потолке</param>
		/// <param name="AllowMonsterMakers">Флаг разрешения монстр-мейкеров</param>
		/// <param name="WaterLevel">Флаг указывает, что воды достаточно для плавающих монстров</param>
		public static void WriteMapEnemy (/*StreamWriter SW,*/ Point RelativePosition,
			byte[] Probabilities, bool AllowSecondFloor, bool CeilingNotAllowed,
			bool AllowMonsterMakers, uint WaterLevel)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);

			List<CPResults> rWalls = RandomazeForm.GetSurroundingWalls (RelativePosition,
				FurnitureTypes.Computer);
			List<byte> enemiesProbabilityLine = new List<byte> ();
			enemiesProbabilityLine.Add (255);   // Заглушка для крыс

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

				if (i == m_brn)
					if ((MapSupport.MapNumber < 12) || CeilingNotAllowed || !MapSupport.TwoFloors)
						continue;

				if (i == m_min)
					if (rWalls.Count < 1)
						continue;

				for (int j = 0; j < Probabilities[i]; j++)
					enemiesProbabilityLine.Add ((byte)i);
				}

			// Запись
			MapSupport.Write ("{\n");

			// Добавление
			int z;
			if (AllowSecondFloor && (RDGenerics.RND.Next (4) != 0))
				z = MapSupport.DefaultWallHeight - 16;
			else
				z = 0;
			int r = RDGenerics.RND.Next (360);

			int crEnemy = RDGenerics.RND.Next (enemiesProbabilityLine.Count);
			if ((enemiesProbabilityLine.Count < 1) || (crEnemy == 0))
				crEnemy = m_lee;
			else
				crEnemy = enemiesProbabilityLine[crEnemy];

			// Обработка для монстр-мейкеров
			bool noMM = false;
			bool countEnemy = false, countRat = false;
			bool mMaker = awaitingNextMM && (RDGenerics.RND.Next (3) == 0);

			// Выбор врага
			switch (crEnemy)
				{
				// Монстры-заглушки
				default:
				case m_lee:
					if (WaterLevel > 0)
						{
						z = 4;  // Чуть выше пола для разблокировки плавания
						InitMonster (false, enemies[m_lee]);
						}
					else
						{
						z = 0;  // Только на полу
						InitMonster (false, /*rats[RDGenerics.RND.Next (rats.Count)]*/ MapClasses.Rat);
						}

					// Не могут быть монстрмейкерами
					noMM = true;
					// Обязательно должны пройти через счётчики
					mMaker = false;
					countRat = true;
					break;

				case m_gru:
					InitMonster (mMaker, enemies[m_gru]);
					countEnemy = true;

					if (!mMaker)
						MapSupport.Write ("\"weapons\" \"" +
							gruntWeapons[RDGenerics.RND.Next (gruntWeapons.Length)] + "\"\n");
					break;

				// Зомби
				case m_zom:
					z = 0;  // Только на полу
					InitMonster (mMaker, enemies[m_zom]);
					countEnemy = true;

					if (!mMaker)
						MapSupport.Write ("\"skin\" \"" + RDGenerics.RND.Next (2).ToString () + "\"\n");
					break;

				// Крабы
				case m_hed:
					z = 0;  // Только на полу
					InitMonster (mMaker, enemies[m_hed]);
					countEnemy = true;
					break;

				// Алиены
				case m_slv:
				// Куры
				case m_bul:
				// Ассассины
				case m_asn:
				// Солдаты алиенов
				case m_agr:
					InitMonster (mMaker, enemies[crEnemy]);
					countEnemy = true;
					break;

				// Турель
				case m_tur:
					int t = RDGenerics.RND.Next (turrets.Length);
					InitMonster (false, turrets[t]);
					bool turret = (t < 2);

					if (!MapSupport.TwoFloors)
						{
						z = 0;
						if (turret)
							MapSupport.Write ("\"orientation\" \"0\"\n");
						}
					else
						{
						bool chance = (RDGenerics.RND.Next (2) == 0);
						if (turret && !CeilingNotAllowed && chance)
							{
							z = MapSupport.WallHeight;
							MapSupport.Write ("\"orientation\" \"1\"\n");
							}
						else if (!turret && AllowSecondFloor && chance)
							{
							z = MapSupport.DefaultWallHeight - 16;
							}
						else
							{
							z = 0;
							if (turret)
								MapSupport.Write ("\"orientation\" \"0\"\n");
							}
						}

					MapSupport.Write ("\"spawnflags\" \"32\"\n");
					countEnemy = true;
					break;

				// Контроллеры
				case m_con:
					z = MapSupport.WallHeight - 96;    // Ближе к потолку
					InitMonster (mMaker, enemies[m_con]);
					countEnemy = true;
					break;

				// Собаки
				case m_hou:
					z = 0;  // Только на полу
					InitMonster (mMaker, enemies[m_hou]);
					countEnemy = true;
					break;

				// Барнаклы
				case m_brn:
					z = MapSupport.WallHeight;  // Только на потолке
					InitMonster (false, enemies[m_brn]);
					countEnemy = true;
					break;

				// Мины
				case m_min:
					InitMonster (false, enemies[m_min]);
					noMM = true;

					MapSupport.Write ("\"spawnflags\" \"1\"\n");
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
					break;
				}

			// Обработка монстр-мейкеров или создание ачивки
			if (!mMaker)
				{
				if (AllowMonsterMakers && !noMM && (RDGenerics.RND.Next (3) == 0))
					{
					availableMMNumber++;
					nextMMName = "MM" + MapSupport.BuildMapName () + "T" +
						availableMMNumber.ToString ("D3");

					MapSupport.Write ("\"TriggerTarget\" \"" + nextMMName + "\"\n");
					awaitingNextMM = true;

					MapSupport.Write ("\"TriggerCondition\" \"4\"\n");
					}
				else if (countEnemy || countRat)
					{
					MapSupport.Write ("\"TriggerTarget\" \"" + (countRat ? SecondCounterName : FirstCounterName) +
						"\"\n");

					if (countEnemy)
						realEnemiesQuantity++;
					if (countRat)
						realRatsQuantity++;

					MapSupport.Write ("\"TriggerCondition\" \"4\"\n");
					}
				}

			// Общие параметры
			MapSupport.Write ("\"angles\" \"0 " + r.ToString () + " 0\"\n");
			MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " + z.ToString () + "\"\n");
			MapSupport.Write ("}\n");

			// Финализация монстр-мейкера (имя было сброшено методом записи)
			if (mMaker && string.IsNullOrWhiteSpace (nextMMName))
				awaitingNextMM = false;
			}

		// Состояние генерации монстр-мейкеров
		private static uint availableMMNumber = 0;
		private static bool awaitingNextMM = false;
		private static string nextMMName;

		// Метод обрабоатывает вилку между монстром и монст-мейкером
		private static void InitMonster (bool AsMonsterMaker, MapClasses MonsterType)
			{
			// Как реальный NPC
			if (!AsMonsterMaker)
				{
				MapSupport.AddEntity (MonsterType);
				return;
				}

			// Как монстрмейкер
			MapSupport.AddEntity (MapClasses.MonsterMaker);
			MapSupport.Write ("\"targetname\" \"" + nextMMName + "\"\n");
			MapSupport.Write ("\"monstercount\" \"1\"\n");
			MapSupport.Write ("\"delay\" \"-1\"\n");
			MapSupport.Write ("\"m_imaxlivechildren\" \"1\"\n");
			MapSupport.Write ("\"monstertype\" \"" + MapSupport.GetClassName (MonsterType) + "\"\n");
			MapSupport.Write ("\"teleport_sound\" \"ambience/teleport1.wav\"\n");
			MapSupport.Write ("\"teleport_sprite\" \"sprites/portal1.spr\"\n");

			nextMMName = "";    // Имя использовано
			}

		/// <summary>
		/// Возвращает имя объекта для первого счётчика достижений
		/// </summary>
		public static string FirstCounterName
			{
			get
				{
				return "Achi" + MapSupport.BuildMapName () + "C1";
				}
			}

		/// <summary>
		/// Возвращает имя объекта для второго счётчика достижений
		/// </summary>
		public static string SecondCounterName
			{
			get
				{
				return "Achi" + MapSupport.BuildMapName () + "C2";
				}
			}

		/// <summary>
		/// Возвращает имя монстрмейкера для случая прямого вызова от второго счётчика
		/// </summary>
		public static string DirectMMNameForSecondCounter
			{
			get
				{
				return "Achi" + MapSupport.BuildMapName () + "R2";
				}
			}

		/// <summary>
		/// Возвращает имя монстрмейкера для случая вызова от второго шлюза
		/// после сбора всех крыс по факту нажатия кнопки
		/// </summary>
		public static string IndirectGateMMNameForSecondCounter
			{
			get
				{
				return "Achi" + MapSupport.BuildMapName () + "R2D";
				}
			}

		/// <summary>
		/// Возвращает имя монстрмейкера для случая вызова от второго счётчика
		/// по факту сбора всех крыс при уже нажатой кнопке
		/// </summary>
		public static string IndirectCounterMMNameForSecondCounter
			{
			get
				{
				return "Achi" + MapSupport.BuildMapName () + "R2R";
				}
			}

		/// <summary>
		/// Метод добавляет триггер достижения на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="TwoButtons">Флаг указывает на наличие второй кнопки</param>
		/// <param name="Water">Флаг наличия воды на карте</param>
		public static void WriteMapAchievement (/*StreamWriter SW,*/ Point RelativePosition,
			bool TwoButtons, bool Water)
			{
			// Расчёт параметров
			Point p = MapSupport.EvaluateAbsolutePosition (RelativePosition);
			p.X += 16;
			p.Y += 16;

			string mn = MapSupport.BuildMapName ();

			if (realEnemiesQuantity > 0)
				{
				MapSupport.Write ("{\n");
				MapSupport.AddEntity (MapClasses.Counter);
				MapSupport.Write ("\"targetname\" \"" + FirstCounterName + "\"\n");
				MapSupport.Write ("\"target\" \"Achi" + mn + "R1\"\n");
				MapSupport.Write ("\"health\" \"" + realEnemiesQuantity.ToString () + "\"\n");
				MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 64\"\n");

				MapSupport.Write ("}\n{\n");
				MapSupport.AddEntity (MapClasses.HealthSetter);
				MapSupport.Write ("\"targetname\" \"Achi" + mn + "R1\"\n");
				MapSupport.Write ("\"dmg\" \"200\"\n");
				MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 72\"\n");

				MapSupport.Write ("}\n{\n");
				MapSupport.AddEntity (MapClasses.Message);
				MapSupport.Write ("\"spawnflags\" \"2\"\n");
				MapSupport.Write ("\"targetname\" \"Achi" + mn + "R1\"\n");
				MapSupport.Write ("\"messagesound\" \"items/suitchargeok1.wav\"\n");
				MapSupport.Write ("\"messagevolume\" \"10\"\n");
				MapSupport.Write ("\"messageattenuation\" \"3\"\n");
				MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 76\"\n");

				if (availableMMNumber > 0)
					MapSupport.Write ("\"message\" \"ACHI_ESRM_03\"\n");
				else
					MapSupport.Write ("\"message\" \"ACHI_ESRM_01\"\n");

				MapSupport.Write ("}\n");
				}

			if (realRatsQuantity > 0)
				{
				MapSupport.Write ("{\n");

				MapSupport.AddEntity (/*"game_counter"*/ MapClasses.Counter);
				MapSupport.Write ("\"targetname\" \"" + SecondCounterName + "\"\n");
				MapSupport.Write ("\"target\" \"" + DirectMMNameForSecondCounter + "\"\n");
				MapSupport.Write ("\"health\" \"" + realRatsQuantity.ToString () + "\"\n");
				MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 80\"\n");

				MapSupport.Write ("}\n{\n");
				MapSupport.AddEntity (/*"monstermaker"*/ MapClasses.MonsterMaker);
				MapSupport.Write ("\"monstercount\" \"1\"\n");
				MapSupport.Write ("\"delay\" \"-1\"\n");
				MapSupport.Write ("\"m_imaxlivechildren\" \"1\"\n");
				MapSupport.Write ("\"monstertype\" \"monster_barney\"\n");
				MapSupport.Write ("\"teleport_sound\" \"ambience/teleport1.wav\"\n");
				MapSupport.Write ("\"teleport_sprite\" \"sprites/portal1.spr\"\n");
				MapSupport.Write ("\"angles\" \"0 " + RDGenerics.RND.Next (360).ToString () + " 0\"\n");
				MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 0\"\n");

				if (TwoButtons)
					{
					// Иначе Барни застрянет во втором шлюзе
					MapSupport.Write ("\"target\" \"Achi" + mn + "R2M\"\n");
					MapSupport.Write ("\"targetname\" \"" + IndirectGateMMNameForSecondCounter + "\"\n");

					// Триггер на случай, если кнопка найдена ПОСЛЕ сбора всех крыс.
					// Тогда второй шлюз при нажатии кнопки инициирует мейкер,
					// а страховочный триггер отсекается
					MapSupport.Write ("}\n{\n");
					MapSupport.AddEntity (MapClasses.ChangeTarget);
					MapSupport.Write ("\"targetname\" \"" + DirectMMNameForSecondCounter + "\"\n");
					MapSupport.Write ("\"target\" \"" + MapSupport.SecondGateName + "\"\n");
					MapSupport.Write ("\"m_iszNewTarget\" \"" + IndirectGateMMNameForSecondCounter + "\"\n");
					MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 92\"\n");

					// Триггер на случай, если кнопка найдена ДО сбора всех крыс.
					// Тогда счётчик крыс инициирует мейкер, а первый триггер отсекается
					MapSupport.Write ("}\n{\n");
					MapSupport.AddEntity (/*"trigger_changetarget"*/ MapClasses.ChangeTarget);
					MapSupport.Write ("\"targetname\" \"" + IndirectCounterMMNameForSecondCounter + "\"\n");
					MapSupport.Write ("\"target\" \"" + SecondCounterName + "\"\n");
					MapSupport.Write ("\"m_iszNewTarget\" \"" + IndirectGateMMNameForSecondCounter + "\"\n");
					MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 96\"\n");
					}
				else
					{
					MapSupport.Write ("\"targetname\" \"" + DirectMMNameForSecondCounter + "\"\n");
					}

				MapSupport.Write ("}\n{\n");
				MapSupport.AddEntity (/*"env_message"*/ MapClasses.Message);
				MapSupport.Write ("\"spawnflags\" \"2\"\n");

				if (TwoButtons)
					MapSupport.Write ("\"targetname\" \"Achi" + mn + "R2M\"\n");
				else
					MapSupport.Write ("\"targetname\" \"" + DirectMMNameForSecondCounter + "\"\n");

				MapSupport.Write ("\"messagesound\" \"items/suitchargeok1.wav\"\n");
				MapSupport.Write ("\"messagevolume\" \"10\"\n");
				MapSupport.Write ("\"messageattenuation\" \"3\"\n");
				MapSupport.Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 88\"\n");

				if (Water)
					MapSupport.Write ("\"message\" \"ACHI_ESRM_02_W\"\n");
				else
					MapSupport.Write ("\"message\" \"ACHI_ESRM_02\"\n");

				MapSupport.Write ("}\n");
				}
			}

		// Подстановки номеров оружия для солдат
		private static string[] gruntWeapons = new string[] { "1", "3", "5", "8", "10" };

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
		private static MapClasses[] enemies = new MapClasses[] {
			/*"monster_human_assassin",
			"monster_bullchicken",
			"monster_alien_controller",
			"monster_houndeye",
			"monster_human_grunt",*/
			MapClasses.Assassin,
			MapClasses.Bullsquid,
			MapClasses.AlienController,
			MapClasses.Houndeye,
			MapClasses.Soldier,

			/*"monster_headcrab",
			"monster_leech",
			"monster_tripmine",
			"monster_barnacle",
			"monster_alien_grunt",*/
			MapClasses.HeadCrab,
			MapClasses.Leech,
			MapClasses.TripMineEnemy,
			MapClasses.Barnacle,
			MapClasses.AlienGrunt,

			/*"monster_alien_slave",
			"",	// Турели, ручная подстановка
			"monster_zombie"*/
			MapClasses.Vortigaunt,
			MapClasses.Turret,	// Подменяются при создании
			MapClasses.Zombie,
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
			EnemiesPermissionLine[m_brn] = 0;
			}

		/// <summary>
		/// Метод удаляет барнакла из разрешающей строки при выключении режима двух этажей
		/// </summary>
		/// <param name="EnemiesPermissionLine">Имеющаяся строка разрешений для врагов</param>
		public static void RemoveLeech (ref List<byte> EnemiesPermissionLine)
			{
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
		/*private static List<string> rats = new List<string> {
			"monster_rat" ,
			//"monster_cockroach",
			};*/
		private static MapClasses[] turrets = new MapClasses[] {
			/*"monster_turret",
			"monster_miniturret",
			"monster_sentry",*/
			MapClasses.Turret,
			MapClasses.MiniTurret,
			MapClasses.Sentry,
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
