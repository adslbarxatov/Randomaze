using System;

namespace RD_AAOW
	{
	/// <summary>
	/// Структура описывает настраиваемые параметры приложения
	/// </summary>
	public class ESRMSettings
		{
		/// <summary>
		/// Конструктор. Инициализирует хранилище настроек и передаёт ему команду, полученную от движка
		/// </summary>
		/// <param name="SettingFromEngineToken">Псевдоним параметра, переданный от движка</param>
		/// <param name="SettingFromEngineValue">Значение параметра, переданное от движка</param>
		public ESRMSettings (string SettingFromEngineToken, string SettingFromEngineValue)
			{
			// Сохранение параметров
			if (!string.IsNullOrWhiteSpace (SettingFromEngineToken) &&
				!string.IsNullOrWhiteSpace (SettingFromEngineValue))
				{
				settingFromEngineToken = SettingFromEngineToken;
				settingFromEngineValue = SettingFromEngineValue;

				// Подстройка диапазонов
				switch (settingFromEngineToken)
					{
					// Флаги
					case twoFloorsPar:
					case allowItemsForSecondFloorPar:
					case allowExplosiveCratesPar:
					case allowItemsCratesPar:
					case allowMonsterMakersPar:

					// Значения, начинающиеся с нуля
					case fogCoefficientPar:
					case waterLevelPar:
					case buttonModePar:
						try
							{
							settingFromEngineValue = (uint.Parse (settingFromEngineValue) + 1).ToString ();
							}
						catch { }
						break;
					}
				}

			// Прогрузка значений
			_ = MazeSizeCoefficient;
			_ = EnemiesDensityCoefficient;
			_ = ItemsDensityCoefficient;
			_ = WallsDensityCoefficient;
			_ = ButtonMode;
			_ = CratesDensityCoefficient;
			_ = EnemiesPermissionLine;
			_ = InsideLightingCoefficient;
			_ = OutsideLightingCoefficient;
			_ = SectionType;
			_ = TwoFloors;
			_ = AllowExplosiveCrates;
			_ = AllowItemsCrates;
			_ = AllowItemsForSecondFloor;
			_ = ItemsPermissionLine;
			_ = GravityCoefficient;
			_ = FogCoefficient;
			_ = AllowMonsterMakers;
			_ = BarriersType;
			_ = CleanupOldMaps;
			_ = WaterLevel;

			// Защита
			if (!TwoFloors && !RandomizeFloorsQuantity)
				EnemiesPermissionLine = EnemiesSupport.RemoveBarnacle (EnemiesPermissionLine);
			if ((WaterLevel < 1) && !RandomWaterLevel)
				EnemiesPermissionLine = EnemiesSupport.RemoveLeech (EnemiesPermissionLine);
			}
		private string settingFromEngineToken = "";
		private string settingFromEngineValue = "";

		/// <summary>
		/// Возвращает или задаёт масштабный коэффициент размера лабиринта
		/// </summary>
		public uint MazeSizeCoefficient
			{
			get
				{
				return GetSettingsValue (mazeSizeCoefficientPar,
					MaximumMazeSizeCoefficient, 4, ref mazeSizeCoefficient);
				}
			set
				{
				SetSettingsValue (mazeSizeCoefficientPar, ref mazeSizeCoefficient, value);
				}
			}
		private int mazeSizeCoefficient = int.MaxValue;
		private const string mazeSizeCoefficientPar = "MS";

		// Метод загружает настройку и контролирует её вхождение в диапазон
		private uint GetSettingsValue (string ValueToken, uint ValueMaximum, uint DefaultValue, ref int Value)
			{
			// Отсечка, если загрузка настройки уже выполнялась
			if (Value < int.MaxValue)
				return (uint)Math.Abs (Value);

			// Получение настройки
			int v;
			int e = 0;
			try
				{
				v = int.Parse (RDGenerics.GetAppRegistryValue (ValueToken));
				}
			catch
				{
				v = (int)DefaultValue;
				}

			try
				{
				if (settingFromEngineToken == ValueToken)
					e = int.Parse (settingFromEngineValue);
				}
			catch { }

			// Получение значения от движка с сохранением
			if ((settingFromEngineToken == ValueToken) && (e > 0))
				{
				Value = (int)MapSupport.InboundValue (e, 1, ValueMaximum);
				SetSettingsValue (ValueToken, ref Value, uint.MaxValue);
				}

			// Рандомизация
			else if (v < 0)
				{
				Value = -RDGenerics.RND.Next (1, (int)ValueMaximum + 1);
				}

			// Простое присвоение
			else
				{
				Value = (int)MapSupport.InboundValue (v, 1, ValueMaximum);
				}

			return (uint)Math.Abs (Value);
			}

		// Метод сохраняет настройку
		private void SetSettingsValue (string ValueToken, ref int Value, uint NewValue)
			{
			if (NewValue < uint.MaxValue)
				{
				if (Value < 0)
					Value = -(int)NewValue;
				else
					Value = (int)NewValue;
				}

			RDGenerics.SetAppRegistryValue (ValueToken, Value.ToString ());
			}

		/// <summary>
		/// Возвращает или задаёт флаг случайного масштабного коэффициента размера лабиринта
		/// </summary>
		public bool RandomMazeSizeCoefficient
			{
			get
				{
				return (mazeSizeCoefficient < 0);
				}
			set
				{
				SetSettingsValue (mazeSizeCoefficientPar, ref mazeSizeCoefficient, value);
				}
			}

		// Метод сохраняет рандомизацию настройки
		private void SetSettingsValue (string ValueToken, ref int Value, bool Randomize)
			{
			Value = Math.Abs (Value) * (Randomize ? -1 : 1);
			SetSettingsValue (ValueToken, ref Value, uint.MaxValue);
			}

		/// <summary>
		/// Возвращает ограничение коэффициента размера лабиринта
		/// </summary>
		public const uint MaximumMazeSizeCoefficient = 8;



		/// <summary>
		/// Возвращает или задаёт коэффициент плотности врагов
		/// </summary>
		public uint EnemiesDensityCoefficient
			{
			get
				{
				return GetSettingsValue (enemiesDensityCoefficientPar,
					MaximumEnemiesDensityCoefficient, 4, ref enemiesDensityCoefficient);
				}
			set
				{
				SetSettingsValue (enemiesDensityCoefficientPar, ref enemiesDensityCoefficient, value);
				}
			}
		private int enemiesDensityCoefficient = int.MaxValue;
		private const string enemiesDensityCoefficientPar = "DF";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента плотности врагов
		/// </summary>
		public bool RandomEnemiesDensityCoefficient
			{
			get
				{
				return (enemiesDensityCoefficient < 0);
				}
			set
				{
				SetSettingsValue (enemiesDensityCoefficientPar, ref enemiesDensityCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента плотности врагов
		/// </summary>
		public const uint MaximumEnemiesDensityCoefficient = 8;



		/// <summary>
		/// Возвращает или задаёт коэффициент плотности собираемых объектов
		/// </summary>
		public uint ItemsDensityCoefficient
			{
			get
				{
				return GetSettingsValue (itemsDensityCoefficientPar,
					MaximumItemsDensityCoefficient, 5, ref itemsDensityCoefficient);
				}
			set
				{
				SetSettingsValue (itemsDensityCoefficientPar, ref itemsDensityCoefficient, value);
				}
			}
		private int itemsDensityCoefficient = int.MaxValue;
		private const string itemsDensityCoefficientPar = "ID";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента плотности собираемых объектов
		/// </summary>
		public bool RandomItemsDensityCoefficient
			{
			get
				{
				return (itemsDensityCoefficient < 0);
				}
			set
				{
				SetSettingsValue (itemsDensityCoefficientPar, ref itemsDensityCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента плотности собираемых объектов
		/// </summary>
		public const uint MaximumItemsDensityCoefficient = 8;



		/// <summary>
		/// Возвращает или задаёт коэффициент насыщенности лабиринта стенами
		/// </summary>
		public uint WallsDensityCoefficient
			{
			get
				{
				return GetSettingsValue (wallsDensityCoefficientPar,
					MaximumWallsDensityCoefficient, 5, ref wallsDensityCoefficient);
				}
			set
				{
				SetSettingsValue (wallsDensityCoefficientPar, ref wallsDensityCoefficient, value);
				}
			}
		private int wallsDensityCoefficient = int.MaxValue;
		private const string wallsDensityCoefficientPar = "WD";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента насыщенности лабиринта стенами
		/// </summary>
		public bool RandomWallsDensityCoefficient
			{
			get
				{
				return (wallsDensityCoefficient < 0);
				}
			set
				{
				SetSettingsValue (wallsDensityCoefficientPar, ref wallsDensityCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента насыщенности лабиринта стенами
		/// </summary>
		public const uint MaximumWallsDensityCoefficient = 12;



		/// <summary>
		/// Возвращает или задаёт режим блокировки выхода кнопками
		/// </summary>
		public MapButtonsTypes ButtonMode
			{
			get
				{
				return (MapButtonsTypes)(GetSettingsValue (buttonModePar, 3, 1, ref buttonMode) - 1);
				}
			set
				{
				SetSettingsValue (buttonModePar, ref buttonMode, (uint)value + 1);
				}
			}
		private const string buttonModePar = "BM";
		private int buttonMode = int.MaxValue;



		/// <summary>
		/// Возвращает или задаёт коэффициент преобразования врагов в ящики
		/// </summary>
		public uint CratesDensityCoefficient
			{
			get
				{
				return GetSettingsValue (cratesDensityCoefficientPar,
					MaximumCratesDensityCoefficient, 2, ref cratesDensityCoefficient);
				}
			set
				{
				SetSettingsValue (cratesDensityCoefficientPar, ref cratesDensityCoefficient, value);
				}
			}
		private int cratesDensityCoefficient = int.MaxValue;
		private const string cratesDensityCoefficientPar = "CD";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента преобразования врагов в ящики
		/// </summary>
		public bool RandomCratesDensityCoefficient
			{
			get
				{
				return (cratesDensityCoefficient < 0);
				}
			set
				{
				SetSettingsValue (cratesDensityCoefficientPar, ref cratesDensityCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента преобразования врагов в ящики
		/// </summary>
		public const uint MaximumCratesDensityCoefficient = 5;



		/// <summary>
		/// Возвращает или задаёт строку разрешённых врагов
		/// </summary>
		public string EnemiesPermissionLine
			{
			get
				{
				// Отсечка
				if (!string.IsNullOrWhiteSpace (enemiesPermissionLine))
					return enemiesPermissionLine;

				// Присвоение с перезаписью
				if (settingFromEngineToken == enemiesPermissionLinePar)
					EnemiesPermissionLine = settingFromEngineValue;

				// Простое присвоение
				else
					enemiesPermissionLine = RDGenerics.GetAppRegistryValue (enemiesPermissionLinePar);

				// По умолчанию
				if (string.IsNullOrWhiteSpace (enemiesPermissionLine))
					{
					for (int i = 0; i < EnemiesSupport.EnemiesPermissionsKeys.Length; i++)
						enemiesPermissionLine += EnemiesSupport.EnemiesPermissionsKeys[i];
					enemiesPermissionLine = EnemiesSupport.RemoveBarnacle (enemiesPermissionLine);
					}

				return enemiesPermissionLine;
				}
			set
				{
				enemiesPermissionLine = value;
				RDGenerics.SetAppRegistryValue (enemiesPermissionLinePar, enemiesPermissionLine);
				}
			}
		private string enemiesPermissionLine = "";
		private const string enemiesPermissionLinePar = "EP";



		/// <summary>
		/// Возвращает или задаёт коэффициент искусственного освещения карты
		/// </summary>
		public uint InsideLightingCoefficient
			{
			get
				{
				return GetSettingsValue (insideLightingCoefficientPar,
					MaximumInsideLightingCoefficient, 10, ref insideLightingCoefficient);
				}
			set
				{
				SetSettingsValue (insideLightingCoefficientPar, ref insideLightingCoefficient, value);
				}
			}
		private int insideLightingCoefficient = int.MaxValue;
		private const string insideLightingCoefficientPar = "LI";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента искусственного освещения карты
		/// </summary>
		public bool RandomInsideLightingCoefficient
			{
			get
				{
				return (insideLightingCoefficient < 0);
				}
			set
				{
				SetSettingsValue (insideLightingCoefficientPar, ref insideLightingCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента искусственного освещения карты
		/// </summary>
		public const uint MaximumInsideLightingCoefficient = 10;



		/// <summary>
		/// Возвращает или задаёт коэффициент естественного освещения карты
		/// </summary>
		public uint OutsideLightingCoefficient
			{
			get
				{
				return GetSettingsValue (outsideLightingCoefficientPar,
					MaximumOutsideLightingCoefficient, 5, ref outsideLightingCoefficient);
				}
			set
				{
				SetSettingsValue (outsideLightingCoefficientPar, ref outsideLightingCoefficient, value);
				}
			}
		private int outsideLightingCoefficient = int.MaxValue;
		private const string outsideLightingCoefficientPar = "LO";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента естественного освещения карты
		/// </summary>
		public bool RandomOutsideLightingCoefficient
			{
			get
				{
				return (outsideLightingCoefficient < 0);
				}
			set
				{
				SetSettingsValue (outsideLightingCoefficientPar, ref outsideLightingCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента естественного освещения карты
		/// </summary>
		public const uint MaximumOutsideLightingCoefficient = 6;



		/// <summary>
		/// Возвращает или задаёт тип фильтрации секций карты
		/// </summary>
		public MapSectionTypes SectionType
			{
			get
				{
				return (MapSectionTypes)GetSettingsValue (sectionTypePar,
					(uint)MapSectionTypes.OnlyInside, (uint)MapSectionTypes.AllTypes,
					ref sectionType);
				}
			set
				{
				SetSettingsValue (sectionTypePar, ref sectionType, (uint)value);
				}
			}
		private const string sectionTypePar = "ST";
		private int sectionType = int.MaxValue;



		/// <summary>
		/// Возвращает или задаёт флаг двойной высоты карт
		/// </summary>
		public bool TwoFloors
			{
			get
				{
				return GetSettingsValue (twoFloorsPar, 2, 1, ref twoFloors) > 1;
				}
			set
				{
				SetSettingsValue (twoFloorsPar, ref twoFloors, (uint)(value ? 2 : 1));
				}
			}
		private int twoFloors = int.MaxValue;
		private const string twoFloorsPar = "TF";

		/// <summary>
		/// Возвращает или задаёт флаг рандомизации двойной высоты карт
		/// </summary>
		public bool RandomizeFloorsQuantity
			{
			get
				{
				return (twoFloors < 0);
				}
			set
				{
				SetSettingsValue (twoFloorsPar, ref twoFloors, value);
				}
			}



		/// <summary>
		/// Возвращает или задаёт флаг разрешения для собираемых объектов на внутренних площадках
		/// </summary>
		public bool AllowItemsForSecondFloor
			{
			get
				{
				return TwoFloors && (GetSettingsValue (allowItemsForSecondFloorPar, 2, 2,
					ref allowItemsForSecondFloor) > 1);
				}
			set
				{
				SetSettingsValue (allowItemsForSecondFloorPar, ref allowItemsForSecondFloor, (uint)(value ? 2 : 1));
				}
			}
		private int allowItemsForSecondFloor = int.MaxValue;
		private const string allowItemsForSecondFloorPar = "SF";



		/// <summary>
		/// Возвращает или задаёт флаг разрешения ящиков с жуками и собираемыми предметами
		/// </summary>
		public bool AllowItemsCrates
			{
			get
				{
				return GetSettingsValue (allowItemsCratesPar, 2, 2, ref allowItemsCrates) > 1;
				}
			set
				{
				SetSettingsValue (allowItemsCratesPar, ref allowItemsCrates, (uint)(value ? 2 : 1));
				}
			}
		private int allowItemsCrates = int.MaxValue;
		private const string allowItemsCratesPar = "IC";



		/// <summary>
		/// Возвращает или задаёт флаг разрешения ящиков со взрывчаткой
		/// </summary>
		public bool AllowExplosiveCrates
			{
			get
				{
				return GetSettingsValue (allowExplosiveCratesPar, 2, 2, ref allowExplosiveCrates) > 1;
				}
			set
				{
				SetSettingsValue (allowExplosiveCratesPar, ref allowExplosiveCrates, (uint)(value ? 2 : 1));
				}
			}
		private int allowExplosiveCrates = int.MaxValue;
		private const string allowExplosiveCratesPar = "XC";



		/// <summary>
		/// Возвращает или задаёт строку разрешённых собираемых предметов
		/// </summary>
		public string ItemsPermissionLine
			{
			get
				{
				// Отсечка
				if (!string.IsNullOrWhiteSpace (itemsPermissionLine))
					return itemsPermissionLine;

				// Присвоение с перезаписью
				if (settingFromEngineToken == itemsPermissionLinePar)
					itemsPermissionLine = settingFromEngineValue;

				// Простое присвоение
				else
					itemsPermissionLine = RDGenerics.GetAppRegistryValue (itemsPermissionLinePar);

				// По умолчанию
				if (string.IsNullOrWhiteSpace (itemsPermissionLine))
					{
					for (int i = 0; i < ItemsSupport.ItemsPermissionsKeys.Length; i++)
						itemsPermissionLine += ItemsSupport.ItemsPermissionsKeys[i];
					}

				return itemsPermissionLine;
				}
			set
				{
				itemsPermissionLine = value;
				RDGenerics.SetAppRegistryValue (itemsPermissionLinePar, itemsPermissionLine);
				}
			}
		private string itemsPermissionLine = "";
		private const string itemsPermissionLinePar = "IP";



		/// <summary>
		/// Возвращает или задаёт коэффициент гравитации (в десятках процентов)
		/// </summary>
		public uint GravityCoefficient
			{
			get
				{
				return GetSettingsValue (gravityCoefficientPar,
					MaximumGravityCoefficient, 10, ref gravityCoefficient);
				}
			set
				{
				SetSettingsValue (gravityCoefficientPar, ref gravityCoefficient, value);
				}
			}
		private int gravityCoefficient = int.MaxValue;
		private const string gravityCoefficientPar = "GR";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента гравитации
		/// </summary>
		public bool RandomGravityCoefficient
			{
			get
				{
				return (gravityCoefficient < 0);
				}
			set
				{
				SetSettingsValue (gravityCoefficientPar, ref gravityCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента гравитации
		/// </summary>
		public const uint MaximumGravityCoefficient = 20;



		/// <summary>
		/// Возвращает или задаёт флаг разрешения монстр-мейкеров
		/// </summary>
		public bool AllowMonsterMakers
			{
			get
				{
				return GetSettingsValue (allowMonsterMakersPar, 2, 1, ref allowMonsterMakers) > 1;
				}
			set
				{
				SetSettingsValue (allowMonsterMakersPar, ref allowMonsterMakers, (uint)(value ? 2 : 1));
				}
			}
		private int allowMonsterMakers = int.MaxValue;
		private const string allowMonsterMakersPar = "MM";



		/// <summary>
		/// Возвращает или задаёт тип фильтрации перегородок между секциями карты
		/// </summary>
		public MapBarriersTypes BarriersType
			{
			get
				{
				return (MapBarriersTypes)GetSettingsValue (barriersTypePar,
					(uint)MapBarriersTypes.Both, (uint)MapBarriersTypes.Both,
					ref barriersType);
				}
			set
				{
				SetSettingsValue (barriersTypePar, ref barriersType, (uint)value);
				}
			}
		private const string barriersTypePar = "BT";
		private int barriersType = int.MaxValue;



		/// <summary>
		/// Возвращает или задаёт коэффициент тумана (в десятках процентов)
		/// </summary>
		public uint FogCoefficient
			{
			get
				{
				return GetSettingsValue (fogCoefficientPar,
					MaximumFogCoefficient, 1, ref fogCoefficient) - 1;
				}
			set
				{
				SetSettingsValue (fogCoefficientPar, ref fogCoefficient, value + 1);
				}
			}
		private int fogCoefficient = int.MaxValue;
		private const string fogCoefficientPar = "FC";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента тумана
		/// </summary>
		public bool RandomFogCoefficient
			{
			get
				{
				return (fogCoefficient < 0);
				}
			set
				{
				SetSettingsValue (fogCoefficientPar, ref fogCoefficient, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение коэффициента тумана
		/// </summary>
		public const uint MaximumFogCoefficient = 11;



		/// <summary>
		/// Возвращает или задаёт уровень воды (в пятёрках процентов)
		/// </summary>
		public uint WaterLevel
			{
			get
				{
				return GetSettingsValue (waterLevelPar, MaximumWaterLevel, 1, ref waterLevel) - 1;
				}
			set
				{
				SetSettingsValue (waterLevelPar, ref waterLevel, value + 1);
				}
			}
		private int waterLevel = int.MaxValue;
		private const string waterLevelPar = "WL";

		/// <summary>
		/// Возвращает или задаёт флаг случайного уровня воды
		/// </summary>
		public bool RandomWaterLevel
			{
			get
				{
				return (waterLevel < 0);
				}
			set
				{
				SetSettingsValue (waterLevelPar, ref waterLevel, value);
				}
			}

		/// <summary>
		/// Возвращает ограничение уровня воды (45% максимум)
		/// </summary>
		public const uint MaximumWaterLevel = 10;



		/// <summary>
		/// Возвращает или задаёт флаг разрешения на очистку старых карт (интерфейсная опция)
		/// </summary>
		public bool CleanupOldMaps
			{
			get
				{
				return GetSettingsValue (cleanupOldMapsPar, 2, 1, ref cleanupOldMaps) > 1;
				}
			set
				{
				SetSettingsValue (cleanupOldMapsPar, ref cleanupOldMaps, (uint)(value ? 2 : 1));
				}
			}
		private int cleanupOldMaps = int.MaxValue;
		private const string cleanupOldMapsPar = "OM";
		}

	/// <summary>
	/// Возможные варианты фильтрации секций карты
	/// </summary>
	public enum MapSectionTypes
		{
		/// <summary>
		/// Все варианты
		/// </summary>
		AllTypes = 1,

		/// <summary>
		/// Только варианты с небом
		/// </summary>
		OnlyUnderSky = 2,

		/// <summary>
		/// Только варианты в помещении
		/// </summary>
		OnlyInside = 3,
		}

	/// <summary>
	/// Возможные варианты перегородок между секциями
	/// </summary>
	public enum MapBarriersTypes
		{
		/// <summary>
		/// Только стеклянные
		/// </summary>
		OnlyGlass = 1,

		/// <summary>
		/// Только тканевые
		/// </summary>
		OnlyFabric = 2,

		/// <summary>
		/// Оба варианта
		/// </summary>
		Both = 3,
		}

	/// <summary>
	/// Возможные варианты кнопок открытия выхода с карты
	/// </summary>
	public enum MapButtonsTypes
		{
		/// <summary>
		/// Без кнопок
		/// </summary>
		NoButtons = 0,

		/// <summary>
		/// Одна кнопка
		/// </summary>
		SingleButton = 1,

		/// <summary>
		/// Основная и дополнительная конпка
		/// </summary>
		MainAndAdditional = 2,
		}
	}
