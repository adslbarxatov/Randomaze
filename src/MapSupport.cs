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
	/// Доступные классы объектов для карты
	/// </summary>
	public enum MapClasses
		{
		/// <summary>
		/// Основной класс-описатель статической геометрии
		/// </summary>
		World,

		/// <summary>
		/// Разрущаемый объект
		/// </summary>
		Breakable,

		/// <summary>
		/// Дверь
		/// </summary>
		Door,

		/// <summary>
		/// Кнопка
		/// </summary>
		Button,

		/// <summary>
		/// Подвижный объект
		/// </summary>
		Pushable,

		/// <summary>
		/// Слой воды
		/// </summary>
		Water,

		/// <summary>
		/// Лестница
		/// </summary>
		Ladder,

		/// <summary>
		/// Генератор врагов
		/// </summary>
		MonsterMaker,

		/// <summary>
		/// Счётчик (триггер)
		/// </summary>
		Counter,

		/// <summary>
		/// Текущий уровень здоровья (триггер)
		/// </summary>
		HealthSetter,

		/// <summary>
		/// Сообщение на экране (триггер)
		/// </summary>
		Message,

		/// <summary>
		/// Переключатель для цели объекта (триггер)
		/// </summary>
		ChangeTarget,

		/// <summary>
		/// Завершение игры (триггер)
		/// </summary>
		GameEnd,

		/// <summary>
		/// Перезапуск игры с отображением сообщения (триггер)
		/// </summary>
		GameRestart,

		/// <summary>
		/// Множественный триггер с отсрочками
		/// </summary>
		MultiTarget,

		/// <summary>
		/// Одинарный триггер с отсрочкой
		/// </summary>
		OnceTarget,

		/// <summary>
		/// Точка телепортации между картами
		/// </summary>
		Landmark,

		/// <summary>
		/// Смена карты (триггер)
		/// </summary>
		ChangeLevel,

		/// <summary>
		/// Автосохранение (триггер)
		/// </summary>
		Autosave,

		/// <summary>
		/// Спрайт
		/// </summary>
		Sprite,

		/// <summary>
		/// Точка входа игрока (для первой карты)
		/// </summary>
		PlayerStart,

		/// <summary>
		/// Гравитация (триггер)
		/// </summary>
		Gravity,

		/// <summary>
		/// Туман (триггер)
		/// </summary>
		Fog,

		/// <summary>
		/// Звуковой эффект (триггер)
		/// </summary>
		SoundEffect,

		/// <summary>
		/// Источник звука
		/// </summary>
		SoundSource,

		/// <summary>
		/// Солнце ("небесное" освещение)
		/// </summary>
		Sun,

		/// <summary>
		/// Источник освещения
		/// </summary>
		LightSource,

		/// <summary>
		/// Точка навигационной сетки
		/// </summary>
		Node,

		/// <summary>
		/// Декаль
		/// </summary>
		Decal,



		/// <summary>
		/// Спрятанный объект
		/// </summary>
		HiddenObject,

		/// <summary>
		/// Собираемый документ
		/// </summary>
		Document,

		/// <summary>
		/// Защитный костюм
		/// </summary>
		Suit,



		/// <summary>
		/// Аптечка
		/// </summary>
		HealthKit,

		/// <summary>
		/// Броня
		/// </summary>
		Armor,

		/// <summary>
		/// Ручная граната (оружие)
		/// </summary>
		HandGrenade,

		/// <summary>
		/// Пистолет (оружие)
		/// </summary>
		HandGun,

		/// <summary>
		/// Взрывпакет (оружие)
		/// </summary>
		Satchel,

		/// <summary>
		/// Револьвер (оружие)
		/// </summary>
		Python,

		/// <summary>
		/// Абралет (оружие)
		/// </summary>
		Crossbow,

		/// <summary>
		/// Пушка Гаусса (оружие)
		/// </summary>
		Gauss,

		/// <summary>
		/// Монтировка (оружие)
		/// </summary>
		Crowbar,

		/// <summary>
		/// Улей (оружие)
		/// </summary>
		HornetGun,

		/// <summary>
		/// Автомат MP5 (оружие)
		/// </summary>
		MachineGun,

		/// <summary>
		/// Дробовик (оружие)
		/// </summary>
		Shotgun,

		/// <summary>
		/// РПГ (оружие)
		/// </summary>
		RPG,

		/// <summary>
		/// Магнитная мина (оружие)
		/// </summary>
		TripMineWeapon,

		/// <summary>
		/// Снарк (оружие)
		/// </summary>
		Snark,

		/// <summary>
		/// Глюонная пушка (оружие)
		/// </summary>
		Egon,

		/// <summary>
		/// Топор (оружие)
		/// </summary>
		Axe,

		/// <summary>
		/// Ящик патронов для автомата и пистолета
		/// </summary>
		AmmoBox,

		/// <summary>
		/// Патроны для дробовика
		/// </summary>
		AmmoShotgun,



		/// <summary>
		/// Ассасин (враг)
		/// </summary>
		Assassin,

		/// <summary>
		/// Булсквид (враг)
		/// </summary>
		Bullsquid,

		/// <summary>
		/// Контроллер (враг)
		/// </summary>
		AlienController,

		/// <summary>
		/// Хаундай (враг)
		/// </summary>
		Houndeye,

		/// <summary>
		/// Солдат (враг)
		/// </summary>
		Soldier,

		/// <summary>
		/// Хедкраб (враг)
		/// </summary>
		HeadCrab,

		/// <summary>
		/// Пиявка (враг)
		/// </summary>
		Leech,

		/// <summary>
		/// Магнитная мина (враг)
		/// </summary>
		TripMineEnemy,

		/// <summary>
		/// Барнакл (враг)
		/// </summary>
		Barnacle,

		/// <summary>
		/// Солдат Xen (враг)
		/// </summary>
		AlienGrunt,

		/// <summary>
		/// Вортигонт (враг)
		/// </summary>
		Vortigaunt,

		/// <summary>
		/// Зомби
		/// </summary>
		Zombie,

		/// <summary>
		/// Большая туррель (враг)
		/// </summary>
		Turret,

		/// <summary>
		/// Малая туррель (враг)
		/// </summary>
		MiniTurret,

		/// <summary>
		/// Автоматический пулемёт (враг)
		/// </summary>
		Sentry,
		


		/// <summary>
		/// Крыса (нейтрал)
		/// </summary>
		Rat,

		/// <summary>
		/// Барни (друг)
		/// </summary>
		Barney,
		};

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
		/// Метод добавляет класс объекта на карту и обновляет счётчик, если необходимо
		/// </summary>
		/// <param name="MapClass">Тип объекта карты</param>
		public static void AddEntity (MapClasses MapClass)
			{
			string[] cls = classes[(int)MapClass];
			Write ("\"classname\" \"" + cls[0] + "\"\n");

			if (string.IsNullOrWhiteSpace (cls[1]))
				entitiesQuantity++;
			}

		// Список поддерживаемых классов объектов
		private static string[][] classes = new string[][] {
			// Общие классы
			new string[] { "worldspawn", "NC" },

			new string[] { "func_breakable", null },
			new string[] { "func_door", null },
			new string[] { "func_button", null },
			new string[] { "func_pushable", null },
			new string[] { "func_water", null },
			new string[] { "func_ladder", null },
			new string[] { "monstermaker", null },
			new string[] { "game_counter", null },
			new string[] { "game_player_set_health", null },
			new string[] { "env_message", null },
			new string[] { "trigger_changetarget", null },
			new string[] { "game_end", null },
			new string[] { "player_loadsaved", null },
			new string[] { "multi_manager", null },
			new string[] { "trigger_once", null },
			new string[] { "info_landmark", null },
			new string[] { "trigger_changelevel", null },
			new string[] { "trigger_autosave", null },
			new string[] { "env_sprite", null },
			new string[] { "info_player_start", null },
			new string[] { "trigger_gravity", null },
			new string[] { "trigger_fog", null },
			new string[] { "trigger_sound", null },
			new string[] { "ambient_generic", null },
			new string[] { "light_environment", "NC" },
			new string[] { "light", "NC" },
			new string[] { "info_node", null },
			new string[] { "infodecal", null },

			// Особые объекты
			new string[] { "item_antidote", null },
			new string[] { "item_security", null },
			new string[] { "item_suit", null },

			// Предметы
			new string[] { "item_healthkit", null },
			new string[] { "item_battery", null },
			new string[] { "weapon_handgrenade", null },
			new string[] { "weapon_9mmhandgun", null },
			new string[] { "weapon_satchel", null },
			new string[] { "weapon_357", null },
			new string[] { "weapon_crossbow", null },
			new string[] { "weapon_gauss", null },
			new string[] { "weapon_crowbar", null },
			new string[] { "weapon_hornetgun", null },

			// Предметы в ящиках
			new string[] { "weapon_9mmAR", null },
			new string[] { "weapon_shotgun", null },
			new string[] { "weapon_rpg", null },
			new string[] { "weapon_tripmine", null },
			new string[] { "weapon_snark", null },
			new string[] { "weapon_egon", null },
			new string[] { "weapon_axe", null },

			new string[] { "ammo_9mmbox", null },
			new string[] { "ammo_buckshot", null },

			// Враги
			new string[] { "monster_human_assassin", null },
			new string[] { "monster_bullchicken", null },
			new string[] { "monster_alien_controller", null },
			new string[] { "monster_houndeye", null },
			new string[] { "monster_human_grunt", null },
			new string[] { "monster_headcrab", null },
			new string[] { "monster_leech", null },
			new string[] { "monster_tripmine", null },
			new string[] { "monster_barnacle", null },
			new string[] { "monster_alien_grunt", null },
			new string[] { "monster_alien_slave", null },
			new string[] { "monster_zombie", null },

			new string[] { "monster_turret", null },
			new string[] { "monster_miniturret", null },
			new string[] { "monster_sentry", null },

			new string[] { "monster_rat", null },
			new string[] { "monster_barney", null },
			};

		/// <summary>
		/// Метод возвращает настоящее имя класса по его типу
		/// </summary>
		/// <param name="MapClass">Тип объекта карты</param>
		/// <returns>Настоящее имя класса</returns>
		public static string GetClassName (MapClasses MapClass)
			{
			return classes[(int)MapClass][0];
			}

		/// <summary>
		/// Возвращает true, если максимально допустимое число сущностей было превышено
		/// </summary>
		public static bool IsEntitiesLimitExceeded
			{
			get
				{
				return (entitiesQuantity > (1 << 11));
				}
			}

		/// <summary>
		/// Метод формирует каноничное имя карты по её номеру с указанным инкрементом
		/// </summary>
		/// <param name="InitialMapNumber">Текущий номер карты</param>
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
		/// Метод записывает закрывающий элемент статической геометрии карты
		/// </summary>
		public static void FinishMapStaticGeometry ()
			{
			Write ("}\n");
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
		private static void WriteMapEndPoint_Finish (Point RelativePosition)
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
			Write ("{\n");
			AddEntity (MapClasses.GameEnd);
			Write ("\"targetname\" \"DevEnd02\"\n");
			Write ("\"origin\" \"" + x1 + " " + y1 + " " + z3 + "\"\n");

			Write ("}\n{\n");
			AddEntity (MapClasses.GameRestart);
			Write ("\"targetname\" \"DevEnd01\"\n");
			Write ("\"holdtime\" \"9\"\n");
			Write ("\"message\" \"DEV_END\"\n");
			Write ("\"duration\" \"1\"\n");
			Write ("\"messagetime\" \"2\"\n");
			Write ("\"loadtime\" \"9\"\n");
			Write ("\"rendercolor\" \"0 0 0\"\n");
			Write ("\"renderamt\" \"255\"\n");
			Write ("\"origin\" \"" + x1 + " " + y2 + " " + z3 + "\"\n");

			Write ("}\n{\n");
			AddEntity (MapClasses.MultiTarget);
			Write ("\"targetname\" \"DevM\"\n");
			Write ("\"DevEnd01\" \"0\"\n");
			Write ("\"DevEnd02\" \"8.5\"\n");
			Write ("\"origin\" \"" + x2 + " " + y1 + " " + z3 + "\"\n");

			Write ("}\n{\n");
			AddEntity (MapClasses.OnceTarget);
			Write ("\"target\" \"DevM\"\n");

			WriteBlock (x1, y1, z1, x2, y2, z2,
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
					TriggerTexture, TriggerTexture }, BlockTypes.Default);

			Write ("}\n");

			WriteMapPortal (RelativePosition, true);
			}

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		/// <param name="TwoButtons">Флаг указывает на наличие второй кнопки</param>
		public static void WriteMapEndPoint (Point RelativePosition, bool TwoButtons)
			{
			// Защита
			if (MapNumber >= MapsLimit)
				{
				WriteMapEndPoint_Finish (RelativePosition);
				return;
				}

			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string mapName = BuildMapName (1);

			// Запись
			Write ("{\n");
			AddEntity (MapClasses.Landmark);
			Write ("\"targetname\" \"" + mapName + "m\"\n");
			Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 40\"\n");

			Write ("}\n{\n");
			AddEntity (MapClasses.ChangeLevel);
			Write ("\"map\" \"" + mapName + "\"\n");
			Write ("\"landmark\" \"" + mapName + "m\"\n");

			WriteBlock ((p.X - 8).ToString (), (p.Y - 8).ToString (), "16",
				(p.X + 8).ToString (), (p.Y + 8).ToString (), (wallHeight - 16).ToString (),

				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
					TriggerTexture, TriggerTexture },

				BlockTypes.Default);

			Write ("}\n{\n");
			AddEntity (MapClasses.Autosave);

			WriteBlock ((p.X - 32).ToString (), (p.Y - 32).ToString (), "12",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "16",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			Write ("}\n");

			WriteMapPortal (RelativePosition, true);
			WriteMapSound (RelativePosition, "alien_cycletone", AmbientTypes.None);

			// Второй шлюз
			if (!TwoButtons)
				return;

			Write ("{\n");
			AddEntity (MapClasses.Door);
			Write ("\"angles\" \"90 0 0\"\n");
			Write ("\"speed\" \"70\"\n");
			Write ("\"movesnd\" \"2\"\n");
			Write ("\"stopsnd\" \"11\"\n");
			Write ("\"wait\" \"-1\"\n");
			Write ("\"lip\" \"1\"\n");
			Write ("\"target\" \"" + EnemiesSupport.IndirectCounterMMNameForSecondCounter + "\"\n");
			if (MapNumber <= MapsLimit)
				Write ("\"targetname\" \"" + SecondGateName + "\"\n");

			string tex = GreenMetalTexture;
			WriteBlock ((p.X - 12).ToString (), (p.Y - 12).ToString (), "0",
				(p.X - 8).ToString (), (p.Y - 8).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			WriteBlock ((p.X + 8).ToString (), (p.Y - 12).ToString (), "0",
				(p.X + 12).ToString (), (p.Y - 8).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			WriteBlock ((p.X - 12).ToString (), (p.Y + 8).ToString (), "0",
				(p.X - 8).ToString (), (p.Y + 12).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);
			WriteBlock ((p.X + 8).ToString (), (p.Y + 8).ToString (), "0",
				(p.X + 12).ToString (), (p.Y + 12).ToString (), WallHeight.ToString (),
				new string[] { tex, tex, tex, tex, tex, tex }, BlockTypes.Default);

			Write ("}\n");
			}

		/// <summary>
		/// Возвращает имя объекта для второго шлюза
		/// </summary>
		public static string SecondGateName
			{
			get
				{
				return "MGate" + BuildMapName ();
				}
			}

		/// <summary>
		/// Возвращает имя объекта для первого шлюза
		/// </summary>
		public static string FirstGateName
			{
			get
				{
				return "Gate" + BuildMapName ();
				}
			}

		// Метод создаёт портал на карте
		private static void WriteMapPortal (Point RelativePosition, bool Exit)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
			Write ("{\n");
			AddEntity (MapClasses.Sprite);
			Write ("\"spawnflags\" \"1\"\n");
			Write ("\"angles\" \"0 0 0\"\n");
			Write ("\"rendermode\" \"5\"\n");
			Write ("\"renderamt\" \"" + (Exit ? "255" : "100") + "\"\n");
			Write ("\"rendercolor\" \"0 0 0\"\n");
			Write ("\"framerate\" \"10.0\"\n");
			Write ("\"model\" \"sprites/" + (Exit ? "exit" : "enter") + "1.spr\"\n");
			Write ("\"scale\" \"1\"\n");

			Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
				(DefaultWallHeight / 2).ToString () + "\"\n");
			Write ("}\n");
			}

		// Метод записывает вариант стартовой позиции для первой карты
		private static void WriteMapEntryPoint_Start (string xs, string ys)
			{
			Write ("{\n");
			AddEntity (MapClasses.PlayerStart);
			Write ("\"angles\" \"0 0 0\"\n");
			Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.Suit);
			Write ("\"spawnflags\" \"1\"\n");
			Write ("\"angles\" \"0 0 0\"\n");
			Write ("\"target\" \"Preset\"\n");
			Write ("\"origin\" \"" + xs + " " + ys + " 32\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.MachineGun);
			Write ("\"origin\" \"" + xs + " " + ys + " 36\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.Shotgun);
			Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.HandGrenade);
			Write ("\"origin\" \"" + xs + " " + ys + " 44\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.HandGrenade);
			Write ("\"origin\" \"" + xs + " " + ys + " 48\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.AmmoBox);
			Write ("\"origin\" \"" + xs + " " + ys + " 52\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.AmmoShotgun);
			Write ("\"origin\" \"" + xs + " " + ys + " 56\"\n");
			Write ("}\n{\n");

			AddEntity (MapClasses.HealthSetter);
			Write ("\"targetname\" \"Preset\"\n");
			Write ("\"dmg\" \"200\"\n");
			Write ("\"origin\" \"" + xs + " " + ys + " 64\"\n");
			Write ("}\n");
			}

		/// <summary>
		/// Метод записывает точку входа на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки входа</param>
		/// <param name="GravityLevel">Уровень гравитации на карте (10 = 100%)</param>
		/// <param name="IsUnderSky">Флаг указывает, расположена ли точка входа под небом</param>
		/// <param name="FogLevel">Уровень тумана на карте (10 = 100%)</param>
		/// <param name="WallsAreRare">Флаг указывает на редкость стен на карте</param>
		public static void WriteMapEntryPoint (Point RelativePosition,
			uint GravityLevel, uint FogLevel, bool IsUnderSky, bool WallsAreRare)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string xs = p.X.ToString ();
			string ys = p.Y.ToString ();

			// Первая карта
			if (MapNumber == 1)
				{
				WriteMapEntryPoint_Start (xs, ys);
				}

			// Все последующие
			else
				{
				Write ("{\n");
				AddEntity (MapClasses.Landmark);
				Write ("\"targetname\" \"" + BuildMapName () + "m\"\n");
				Write ("\"origin\" \"" + xs + " " + ys + " 40\"\n");

				Write ("}\n{\n");
				AddEntity (MapClasses.ChangeLevel);
				Write ("\"map\" \"" + BuildMapName (-1) + "\"\n");
				Write ("\"landmark\" \"" + BuildMapName () + "m\"\n");

				WriteBlock ((p.X - 8).ToString (), (p.Y - 8).ToString (), "-2",
					(p.X + 8).ToString (), (p.Y + 8).ToString (), "-1",
					new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

				Write ("}\n");
				}

			// Гравитационный триггер
			Write ("{\n");
			AddEntity (MapClasses.Gravity);
			Write ("\"gravity\" \"" + (GravityLevel * 80).ToString () + "\"\n");

			WriteBlock ((p.X - 32).ToString (), (p.Y - 32).ToString (), "24",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "28",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			Write ("}\n");

			// Триггер тумана
			Write ("{\n");
			AddEntity (MapClasses.Fog);
			Write ("\"renderamt\" \"" + ((uint)(255.0 * FogLevel / 10.0)).ToString () + "\"\n");
			Write ("\"rendercolor\" \"" + (224 + RDGenerics.RND.Next (32)).ToString () + " " +
				(224 + RDGenerics.RND.Next (32)).ToString () + " " +
				(224 + RDGenerics.RND.Next (32)).ToString () + "\"\n");
			Write ("\"enablingMove\" \"0\"\n");

			WriteBlock ((p.X - 32).ToString (), (p.Y - 32).ToString (), "32",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "36",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			Write ("}\n");

			// Звуковой триггер
			byte rt;
			byte offset = (byte)(TwoFloors ? 1 : 0);
			if (IsUnderSky)
				rt = 0;
			else if (WallsAreRare)
				rt = (byte)(18 + offset);
			else
				rt = (byte)(17 + offset);

			WriteMapSoundTrigger (RelativePosition, false, rt, 0);

			// Остальное
			WriteMapPortal (RelativePosition, false);
			WriteMapSound (RelativePosition, "Teleport1", AmbientTypes.None);
			}

		// Метод записывает блок по указанным коориданатм
		private static void WriteBlock (string X1, string Y1, string Z1,
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
			Write ("{\n");
			Write ("( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				Textures[0] + " [ 1 0 0 " + texOffsetX + " ] [ 0 -1 0 " + texOffsetY + " ] 0 " + texScale + " \n");
			Write ("( " + X2 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X2 + " " + Y2 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				Textures[1] + " [ 1 0 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 -" + texScale + " \n");
			Write ("( " + X1 + " " + Y1 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				Textures[2] + " [ 1 0 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
			Write ("( " + X1 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z2 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z2 + " ) " +
				Textures[3] + " [ 0 1 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 -" + texScale + " \n");
			Write ("( " + X2 + " " + Y1 + " " + Z1 + " ) " +
				"( " + X2 + " " + Y1 + " " + Z2 + " ) " +
				"( " + X2 + " " + Y2 + " " + Z2 + " ) " +
				Textures[4] + " [ 0 1 0 " + texOffsetX + " ] [ 0 0 -1 0 ] 0 " + texScale + " \n");
			Write ("( " + X2 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y2 + " " + Z1 + " ) " +
				"( " + X1 + " " + Y1 + " " + Z1 + " ) " +
				Textures[5] + " [ 1 0 0 " + texOffsetX + " ] [ 0 -1 0 " + texOffsetY + " ] 0 " + texScale + " \n");
			Write ("}\n");
			}

		// Возможные типы блоков
		private enum BlockTypes
			{
			Default = 0,
			Crate = 1,
			Door = 2,
			}

		/// <summary>
		/// Метод записывает звуковой триггер на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки входа</param>
		/// <param name="ForWindow">Флаг двунаправленного триггера для окон</param>
		/// <param name="RoomTypeLeft">Тип окружения слева (для всех)</param>
		/// <param name="RoomTypeRight">Тип окружения справа (для оконных)</param>
		public static void WriteMapSoundTrigger (Point RelativePosition, bool ForWindow,
			byte RoomTypeLeft, byte RoomTypeRight)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			string x1, y1, x2, y2, z1, z2;

			// Запись
			Write ("{\n");
			AddEntity (MapClasses.SoundEffect);
			Write ("\"roomtype\" \"" + RoomTypeLeft.ToString () + "\"\n");
			Write ("\"roomtype2\" \"" + RoomTypeRight.ToString () + "\"\n");
			Write ("\"spawnflags\" \"" + (ForWindow ? "1" : "0") + "\"\n");

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
			WriteBlock (x1, y1, z1, x2, y2, z2, new string[] { TriggerTexture, TriggerTexture,
				TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture }, BlockTypes.Default);

			Write ("}\n");
			}

		/// <summary>
		/// Метод записывает точку выхода с карты
		/// </summary>
		/// <param name="NearbyWalls">Список окружающих стен</param>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		/// <param name="SecondButton">Флаг, указывающий на формирование второй (дополнительной) кнопки</param>
		public static void WriteMapButton (Point RelativePosition, List<CPResults> NearbyWalls,
			bool SecondButton)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			// Запись
			Write ("{\n");
			AddEntity (MapClasses.Button);
			Write ("\"spawnflags\" \"1\"\n");
			Write ("\"delay\" \"1\"\n");
			Write ("\"speed\" \"50\"\n");

			if (SecondButton)
				{
				Write ("\"target\" \"" + SecondGateName + "\"\n");
				Write ("\"sounds\" \"6\"\n");
				}
			else
				{
				Write ("\"target\" \"" + FirstGateName + "\"\n");
				Write ("\"sounds\" \"11\"\n");
				}

			Write ("\"wait\" \"-1\"\n");

			WriteMapFurniture (RelativePosition,
				SecondButton ? FurnitureTypes.ExitTeleportButton : FurnitureTypes.ExitGateButton,
				NearbyWalls, GreenMetalTexture);

			Write ("}\n{\n");
			AddEntity (MapClasses.Autosave);

			WriteBlock ((p.X - 32).ToString (), (p.Y - 32).ToString (), "12",
				(p.X + 32).ToString (), (p.Y + 32).ToString (), "16",
				new string[] { TriggerTexture, TriggerTexture, TriggerTexture, TriggerTexture,
						TriggerTexture, TriggerTexture }, BlockTypes.Default);

			Write ("}\n");

			WriteMapSound (RelativePosition, "electramb1", AmbientTypes.None);
			}

		/// <summary>
		/// Метод записывает дверь на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура двери</param>
		public static void WriteMapDoor (Point RelativePosition, string Texture)
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
			WriteBlock (x1, y1, "0", x2, y2, "96", new string[] { Texture, Texture, Texture,
				Texture, Texture, Texture }, BlockTypes.Door);
			}

		/// <summary>
		/// Метод записывает ящик на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="CratesBalance">Баланс ящиков между предметами и взрывчаткой</param>
		/// <param name="ItemPermissions">Строка разрешений для объектов в ящиках</param>
		/// <param name="EnemiesPermissions">Строка разрешений для врагов в ящиках (крабы, снарки)</param>
		public static void WriteMapCrate (Point RelativePosition,
			int CratesBalance, byte[] ItemPermissions, byte[] EnemiesPermissions)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string x1 = (p.X - 16).ToString ();
			string y1 = (p.Y - 16).ToString ();
			string x2 = (p.X + 16).ToString ();
			string y2 = (p.Y + 16).ToString ();

			bool explosive = CratesBalance >
				RDGenerics.RND.Next (-ESRMSettings.CratesBalanceRange, ESRMSettings.CratesBalanceRange);

			string tex = "CRATE01"; // Взрывчатка по умолчанию

			Write ("{\n");
			AddEntity (MapClasses.Pushable);
			Write ("\"health\" \"20\"\n");
			Write ("\"material\" \"1\"\n");
			Write ("\"spawnflags\" \"128\"\n");
			Write ("\"friction\" \"40\"\n");
			Write ("\"buoyancy\" \"60\"\n");
			Write ("\"rendermode\" \"4\"\n");    // Прозрачность для врагов
			Write ("\"renderamt\" \"255\"\n");

			if (explosive)
				{
				Write ("\"explodemagnitude\" \"" + RDGenerics.RND.Next (160, 200).ToString () + "\"\n");
				}
			else
				{
				bool factor1 = EnemiesSupport.IsHeadcrabAllowed (EnemiesPermissions);
				int r = RDGenerics.RND.Next (factor1 ? 5 : 4);
				int idx;

				// Враги (при запрете хедкрабов увеличивается число ящиков со взрывчаткой)
				if (factor1 && (r < 3) || !factor1 && (r < 2))
					{
					if (!factor1)
						r = 0;  // Только снарки
					Write ("\"spawnobject\" \"" + (r + 27).ToString () + "\"\n");
					}

				// Пустые или редкие ящики
				else
					{
					// Иногда добавлять случайное оружие или предмет
					if (RDGenerics.RND.Next (2) == 0)
						{
						Write ("\"spawnobject\" \"" +
							ItemsSupport.GetRandomItemForCrate (ItemPermissions).ToString () + "\"\n");
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

			WriteBlock (x1, y1, "0", x2, y2, "64", new string[] { tex, tex, tex, tex, tex, tex },
				BlockTypes.Crate);

			Write ("}\n");
			}

		/// <summary>
		/// Метод записывает звуковое сопровождение и эффект помещения на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Ambient">Тип эмбиента</param>
		/// <param name="Sound">Звук</param>
		public static void WriteMapSound (Point RelativePosition, string Sound,
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
			Write ("{\n");
			AddEntity (MapClasses.SoundSource);

			if (Ambient == AmbientTypes.Target)
				{
				Write ("\"spawnflags\" \"49\"\n");
				Write ("\"targetname\" \"" + Sound + "\"\n");
				}
			else
				{
				Write ("\"spawnflags\" \"2\"\n");
				}

			Write ("\"message\" \"ambience/" + Sound + ".wav\"\n");
			Write ("\"health\" \"" + h.ToString () + "\"\n");
			Write ("\"pitch\" \"100\"\n");
			Write ("\"pitchstart\" \"100\"\n");
			Write ("\"origin\" \"" + x + " " + y + " 88\"\n");
			Write ("}\n");
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
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="StartOrFinish">Флаг указывает на начальную или конечную точку пути</param>
		public static void WriteMapPathStone (Point RelativePosition, bool StartOrFinish)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			string tex;
			if (StartOrFinish)
				tex = "~Path02";
			else
				tex = "~Path01";

			WriteBlock ((p.X - 8).ToString (), (p.Y - 8).ToString (), "-8",
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
		/// <param name="Section">Секция карты</param>
		/// <param name="FloorTexture">Текстура пола</param>
		/// <param name="RoofTexture">Текстура потолка</param>
		/// <param name="RelativeMapHeight">Относительная ширина карты</param>
		/// <param name="RelativeMapWidth">Относительная длина карты</param>
		public static void WriteMapCeilingAndFloor (byte Section, int RelativeMapWidth,
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
			WriteBlock (x1, y1, "-16", x2, y2, "0", new string[] { FloorTexture, FloorTexture, FloorTexture,
				FloorTexture, FloorTexture, FloorTexture }, BlockTypes.Default);

			WriteBlock (x1, y1, h1, x2, y2, h2, new string[] { RoofTexture, RoofTexture, RoofTexture,
				RoofTexture, RoofTexture, RoofTexture }, BlockTypes.Default);
			}

		/// <summary>
		/// Метод записывает пол и потолок на карту
		/// </summary>
		/// <param name="WaterLevel">Уровень воды (в долях от единицы)</param>
		/// <param name="RelativeMapHeight">Относительная ширина карты</param>
		/// <param name="RelativeMapWidth">Относительная длина карты</param>
		public static void WriteMapWater (float WaterLevel, int RelativeMapWidth,
			int RelativeMapHeight)
			{
			// Расчёт параметров
			int realMapWidth = RelativeMapWidth * WallLength;
			int realMapHeight = RelativeMapHeight * WallLength;
			string tex = waterTextures[RDGenerics.RND.Next (waterTextures.Length)];
			string h = ((int)(DefaultWallHeight * WaterLevel)).ToString ();
			string amt = (60 + RDGenerics.RND.Next (100)).ToString ();

			for (int i = 0; i < 4; i++)
				{
				// Рандомный пропуск на низком уровне затопления
				if ((WaterLevel == 0.05f) && (RDGenerics.RND.Next (2) == 0))
					continue;

				bool negX = ((i & NegativeX) != 0);
				bool negY = ((i & NegativeY) != 0);
				string x1 = (negX ? (-realMapWidth / 2) : 0).ToString ();
				string x2 = (negX ? 0 : (realMapWidth / 2)).ToString ();
				string y1 = (negY ? (-realMapHeight / 2) : 0).ToString ();
				string y2 = (negY ? 0 : (realMapHeight / 2)).ToString ();

				// Запись
				Write ("{\n");
				AddEntity (MapClasses.Water);
				Write ("\"renderamt\" \"" + amt + "\"\n");
				Write ("\"rendermode\" \"2\"\n");
				Write ("\"wait\" \"-1\"\n");
				Write ("\"skin\" \"-3\"\n");
				Write ("\"WaveHeight\" \"0.1\"\n");

				WriteBlock (x1, y1, "0", x2, y2, h, new string[] { tex, tex, tex,
					tex, tex, tex }, BlockTypes.Default);

				Write ("}\n");
				}
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
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="RoofTexture">Текстура потолка</param>
		/// <param name="AddingTheBulb">Флаг указывает, что добавляется лампочка, а не источник света</param>
		/// <param name="SubFloor">Флаг указывает, что свет добавляется к внутренней площадке</param>
		/// <returns>Возвращает true, если добавлен действующий источник света</returns>
		public static bool WriteMapLight (Point RelativePosition, string RoofTexture,
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
				Write ("{\n");
				AddEntity (MapClasses.Sun);
				Write ("\"_fade\" \"1.0\"\n");

				Write ("\"angles\" \"" + sunAngles[skyIndex] + "\"\n");
				Write ("\"_light\" \"" + sunColors[skyIndex] + "\"\n");

				Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
					(wallHeight - 8).ToString () + "\"\n");
				Write ("}\n");

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
				Write ("{\n");
				AddEntity (MapClasses.LightSource);
				Write (SubFloor ? subLightColor : lightColor);
				Write ("\"_fade\" \"1.0\"\n");
				Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " " +
					(z - 8).ToString () + "\"\n");
				Write ("}\n");
				}

			// Добавление лампы
			else if (!IsSkyTexture (RoofTexture))
				{
				int d = SubFloor ? 8 : 16;

				WriteBlock ((p.X - d).ToString (), (p.Y - d).ToString (), (z - 0).ToString (),
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
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="NearbyWalls">Доступные (с указанной позиции) стены</param>
		/// <param name="WallTexture">Текстура окружающей стены</param>
		/// <param name="FurnitureType">Индекс мебели</param>
		public static void WriteMapFurniture (Point RelativePosition, FurnitureTypes FurnitureType,
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
				WriteBlock (coords[0].ToString (), coords[1].ToString (), coords[2].ToString (),
					coords[3].ToString (), coords[4].ToString (), coords[5].ToString (), tex, BlockTypes.Door);
				}
			}

		/// <summary>
		/// Метод записывает внутреннюю площадку на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="SubFloorTexture">Текстура внутренней площадки</param>
		public static void WriteMapSubFloor (Point RelativePosition, string SubFloorTexture)
			{
			WriteSubFloor (RelativePosition, SubFloorTexture, null);
			}

		/// <summary>
		/// Метод записывает зацеп к внутренней площадке на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		public static void WriteMapSubFloor (Point RelativePosition,
			List<CPResults> SurroundingWalls)
			{
			WriteSubFloor (RelativePosition, null, SurroundingWalls);
			}

		// Универсальный метод записи внутренней площадки
		private static void WriteSubFloor (Point RelativePosition, string SubFloorTexture,
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
				WriteBlock ((p.X - 56).ToString (), (p.Y - 56).ToString (), (DefaultWallHeight - 32).ToString (),
					(p.X + 56).ToString (), (p.Y + 56).ToString (), (DefaultWallHeight - 16).ToString (),
					tex, BlockTypes.Door);
				return;
				}

			// Запись лестницы.
			// Обработка функцией для SurroundingWalls имеет скрытое ограничение, заключающееся в том, что
			// такая площадка может появиться над дверью в стене, только если она окружена тремя стенами
			List<int> offsets = new List<int> ();
			if (!SurroundingWalls.Contains (CPResults.Left))
				{
				offsets.Add (-60);
				offsets.Add (-56);
				offsets.Add (-56);
				offsets.Add (56);
				}

			if (!SurroundingWalls.Contains (CPResults.Right))
				{
				offsets.Add (56);
				offsets.Add (-56);
				offsets.Add (60);
				offsets.Add (56);
				}

			if (!SurroundingWalls.Contains (CPResults.Down))
				{
				offsets.Add (-56);
				offsets.Add (-60);
				offsets.Add (56);
				offsets.Add (-56);
				}

			if (!SurroundingWalls.Contains (CPResults.Up))
				{
				offsets.Add (-56);
				offsets.Add (56);
				offsets.Add (56);
				offsets.Add (60);
				}

			for (int i = 0; i < offsets.Count; i += 4)
				{
				Write ("{\n");
				AddEntity (MapClasses.Ladder);
				WriteBlock ((p.X + offsets[i + 0]).ToString (), (p.Y + offsets[i + 1]).ToString (),
					(DefaultWallHeight - 32).ToString (),
					(p.X + offsets[i + 2]).ToString (), (p.Y + offsets[i + 3]).ToString (),
					(DefaultWallHeight - 16).ToString (),
					tex, BlockTypes.Door);
				Write ("}\n");
				}
			}

		/// <summary>
		/// Метод формирует файл скрипта-описателя мода
		/// </summary>
		/// <param name="MapName">Название первой карты мода</param>
		/// <returns>Возвращает true в случае успеха</returns>
		public static bool WriteGameScript (string MapName)
			{
			// Создание файла
			FileStream FSsc = null;
			try
				{
				FSsc = new FileStream (RDGenerics.AppStartupPath + "liblist.gam", FileMode.Create);
				}
			catch
				{
				return false;
				}
			StreamWriter SWsc = new StreamWriter (FSsc, RDGenerics.GetEncoding (RDEncodings.UTF8));

			SWsc.Write ("game \"" + ProgramDescription.AssemblyTitle + "\"\n");
			SWsc.Write ("type \"singleplayer_only\"\n");
			SWsc.Write ("version \"" + ProgramDescription.AssemblyVersion + "\"\n");
			//SWsc.Write ("noskills \"1\"\n");

			SWsc.Write ("startmap \"" + MapName + "\"\n");
			SWsc.Write ("creditsmap \"" + MapName + "\"\n");
			SWsc.Write ("edicts \"1500\"\n");

			SWsc.Write ("cldll \"1\"\n");
			SWsc.Write ("gamedll \"dlls\\hl.dll\"\n");
			SWsc.Write ("spentity \"info_landmark\"\n");

			SWsc.Write ("developer_url \"" + RDGenerics.DPArrayStorageLink + "\"\n");
			SWsc.Write ("url_info \"http://moddb.com/mods/esrm\"\n");

			SWsc.Close ();
			FSsc.Close ();
			return true;
			}

		/// <summary>
		/// Метод записывает межстенный заполнитель (для удаления недоступных пространств из компилируемой зоны)
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		public static void WriteMapFiller (Point RelativePosition)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);
			WriteBlock ((p.X - WallLength / 2).ToString (), (p.Y - WallLength / 2).ToString (), "0",
				(p.X + WallLength / 2).ToString (), (p.Y + WallLength / 2).ToString (), (WallHeight + 32).ToString (),
				new string[] { "BLACK", "BLACK", "BLACK", "BLACK", "BLACK", "BLACK" }, BlockTypes.Default);
			}

		/// <summary>
		/// Метод записывает точку навигационной сетки на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки выхода</param>
		public static void WriteMapNode (Point RelativePosition)
			{
			// Расчёт параметров
			Point p = EvaluateAbsolutePosition (RelativePosition);

			Write ("{\n");
			AddEntity (MapClasses.Node);
			Write ("\"origin\" \"" + p.X.ToString () + " " + p.Y.ToString () + " 16\"\n");
			Write ("}\n");
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

		/// <summary>
		/// Метод создаёт (инициализирует) новый файл карты и создаёт дескриптор записи
		/// </summary>
		/// <param name="AppSettings">Инициализированный экземпляр настроек программы</param>
		/// <param name="MapPath">Путь для сохранения карты</param>
		/// <returns>Возвращает true, если создание файла завершилось успехом</returns>
		public static bool OpenMapFile (string MapPath, ESRMSettings AppSettings)
			{
			try
				{
				FS2 = new FileStream (MapPath, FileMode.Create);
				}
			catch
				{
				return false;
				}
			SW2 = new StreamWriter (FS2, RDGenerics.GetEncoding (RDEncodings.UTF8));
			mapOpened = true;

			// Начало карты
			Write ("{\n");
			AddEntity (MapClasses.World);
			Write ("\"message\" \"ES: Randomaze map " + BuildMapName () + " by FDL\"\n");
			Write ("\"mapversion\" \"220\"\n");

			// Инициализация неба
			skyIndex = RDGenerics.RND.Next (skyTypes.Length / 2);

			float inc = (AppSettings.OutsideLightingCoefficient - 1.0f) /
				(ESRMSettings.MaximumOutsideLightingCoefficient - 1.0f);
			if (inc < 0.0f)
				inc = 0.0f;
			if (inc > 1.0f)
				inc = 1.0f;
			inc = (1.0f - inc) * skyTypes.Length / 2.0f;

			skyIndex += (int)inc;
			Write ("\"skyname\" \"" + skyTypes[skyIndex] + "\"\n");

			// Параметры карты
			Write ("\"MaxRange\" \"3000\"\n");
			Write ("\"light\" \"1\"\n");
			Write ("\"sounds\" \"1\"\n");
			Write ("\"WaveHeight\" \"0.1\"\n");
			Write ("\"newunit\" \"1\"\n");
			Write ("\"wad\" \"" + RandomazeForm.MainWAD + "\"\n");

			// Параметры первой карты
			if (MapNumber == 1)
				{
				Write ("\"chaptertitle\" \"" + ProgramDescription.AssemblyTitle + "\"\n");
				Write ("\"startdark\" \"1\"\n");
				Write ("\"gametitle\" \"1\"\n");
				}
			else
				{
				Write ("\"chaptertitle\" \"Map #" + MapNumber.ToString ("D3") + "\"\n");
				}

			// Создание цвета ламп
			if (string.IsNullOrWhiteSpace (lightColor))
				{
				lightColor = "\"_light\" \"" + (224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(112 + RDGenerics.RND.Next (32)).ToString () + " " + (AppSettings.TwoFloors ? "200" : "150") + "\"\n";
				subLightColor = "\"_light\" \"" + (224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(224 + RDGenerics.RND.Next (32)).ToString () + " " +
					(112 + RDGenerics.RND.Next (32)).ToString () + " 100\"\n";
				}

			// Выбор высоты карты
			if (AppSettings.TwoFloors)
				wallHeight = DoubleWallHeight;
			else
				wallHeight = DefaultWallHeight;

			// Завершено
			return mapOpened;
			}
		private static FileStream FS2;
		private static StreamWriter SW2;
		private static bool mapOpened = false;

		/// <summary>
		/// Метод записывает указанный текст в открытый файл карты
		/// </summary>
		/// <param name="Data">Текстовые данные для записи</param>
		/// <returns>Возвращает false, если файл не был предварительно создан и открыт</returns>
		public static bool Write (string Data)
			{
			if (!mapOpened)
				return false;

			SW2.Write (Data);
			return true;
			}

		/// <summary>
		/// Метод закрывает открытый ранее файл карты
		/// </summary>
		public static void CloseMapFile ()
			{
			if (!mapOpened)
				return;

			SW2.Close ();
			FS2.Close ();
			mapOpened = false;
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

		// Текстуры

		/// <summary>
		/// Возвращает имя текстуры неба
		/// </summary>
		public const string SkyTexture = "sky";

		/// <summary>
		/// Возвращает имя стандартной текстуры триггера
		/// </summary>
		public const string TriggerTexture = "AAATRIGGER";

		/// <summary>
		/// Возвращает текстуру дерева
		/// </summary>
		/// <param name="Number">Номер (от 1 до 4)</param>
		public static string GetWoodTexture (uint Number)
			{
			int i = (int)Number - 1;
			if ((i >= 0) && (i < woodTextures.Length))
				return woodTextures[i];

			return woodTextures[0];
			}
		private static string[] woodTextures = new string[]
			{
			"Wood01",
			"Wood02",
			"Wood03",
			"Wood04",
			};

		/// <summary>
		/// Возвращает текстуру красного металла
		/// </summary>
		public const string RedMetalTexture = "BorderMet05";

		/// <summary>
		/// Возвращает текстуру светло-зелёного металла
		/// </summary>
		public const string LimeMetalTexture = "Metal01";

		/// <summary>
		/// Возвращает текстуру зелёного металла
		/// </summary>
		public const string GreenMetalTexture = "Metal06";

		/// <summary>
		/// Возвращает текстуру серого металла
		/// </summary>
		public const string GreyMetalTexture = "Metal05";

		/// <summary>
		/// Возвращает текстуру голубого металла
		/// </summary>
		public const string BlueMetalTexture = "Metal08";

		/// <summary>
		/// Возвращает текстуру ткани
		/// </summary>
		/// <param name="Number">Номер (от 1 до 4)</param>
		public static string GetFabricTexture (uint Number)
			{
			int i = (int)Number - 1;
			if ((i >= 0) && (i < fabricTextures.Length))
				return fabricTextures[i];

			return fabricTextures[0];
			}
		private static string[] fabricTextures = new string[]
			{
			"Fabric01",
			"Fabric02",
			"Fabric03",
			"Fabric04",
			};

		/// <summary>
		/// Возвращает текстуру стекла
		/// </summary>
		/// <param name="Number">Номер (от 1 до 2)</param>
		public static string GetGlassTexture (uint Number)
			{
			int i = (int)Number - 1;
			if ((i >= 0) && (i < glassTextures.Length))
				return glassTextures[i];

			return glassTextures[0];
			}
		private static string[] glassTextures = new string[]
			{
			"Glass01",
			"Glass03",
			};

		private static string[] waterTextures = new string[] {
			"!_DirtyWater01",
			"!_ToxWater02",
			"!_Water01",
			"!_Water02",
			};
		}
	}
