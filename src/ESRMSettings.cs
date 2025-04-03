using System;
using System.Collections.Generic;

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
					case allowMonsterMakersPar:

					// Значения, начинающиеся с нуля
					case fogCoefficientPar:
					case waterLevelPar:
					case buttonModePar:
					case cratesDensityCoefficientPar:
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
			_ = CratesDensityCoefficient2;
			_ = EnemiesPermissionLine;
			_ = InsideLightingCoefficient;
			_ = OutsideLightingCoefficient;
			_ = SectionType;
			_ = TwoFloors;
			_ = AllowItemsForSecondFloor;
			_ = ItemsPermissionLine;
			_ = GravityCoefficient;
			_ = FogCoefficient;
			_ = AllowMonsterMakers;
			_ = BarriersType;
			_ = CleanupOldMaps;
			_ = WaterLevel;
			_ = CratesBalance;
			_ = UseNeonLights;

			// Защита
			if (!TwoFloors && !RandomizeFloorsQuantity)
				EnemiesSupport.RemoveBarnacle (ref enemiesPermissionLine);
			if ((WaterLevel < 1) && !RandomWaterLevel)
				EnemiesSupport.RemoveLeech (ref enemiesPermissionLine);
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
		/// Возвращает или задаёт набор вероятностей генерации врагов
		/// </summary>
		public byte[] EnemiesPermissionLine
			{
			get
				{
				// Отсечка
				if (enemiesPermissionLine != null)
					return enemiesPermissionLine.ToArray ();

				// Присвоение с перезаписью
				string line = "";
				if (settingFromEngineToken == enemiesPermissionLinePar)
					{
					line = settingFromEngineValue;
					RDGenerics.SetAppRegistryValue (enemiesPermissionLinePar, line);
					}

				// Простое присвоение
				else
					{
					line = RDGenerics.GetAppRegistryValue (enemiesPermissionLinePar);
					}

				// Забой недостающих элементов
				bool defValues = false;
				for (int i = line.Length; i < EnemiesSupport.AvailableEnemiesTypes; i++)
					{
					line += MaximumEnemiesProbability.ToString ();
					defValues = true;
					}

				// Сплит
				enemiesPermissionLine = new List<byte> ();
				for (int i = 0; i < EnemiesSupport.AvailableEnemiesTypes; i++)
					{
					try
						{
						string s = line[i].ToString ();
						enemiesPermissionLine.Add (byte.Parse (s));
						if (enemiesPermissionLine[i] > MaximumEnemiesProbability)
							enemiesPermissionLine[i] = MaximumEnemiesProbability;
						}
					catch
						{
						enemiesPermissionLine.Add (1);
						defValues = true;
						}
					}

				if (defValues)
					{
					EnemiesSupport.RemoveBarnacle (ref enemiesPermissionLine);
					EnemiesSupport.RemoveLeech (ref enemiesPermissionLine);
					EnemiesPermissionLine = enemiesPermissionLine.ToArray ();
					}

				return enemiesPermissionLine.ToArray ();
				}
			set
				{
				enemiesPermissionLine = new List<byte> (value);
				string line = "";
				for (int i = 0; i < enemiesPermissionLine.Count; i++)
					line += enemiesPermissionLine[i].ToString ();

				RDGenerics.SetAppRegistryValue (enemiesPermissionLinePar, line);
				}
			}
		private List<byte> enemiesPermissionLine;
		private const string enemiesPermissionLinePar = "EP";

		/// <summary>
		/// Возвращает строку разрешённых врагов
		/// </summary>
		public string EnemiesPermissionLineAsString
			{
			get
				{
				return RDGenerics.GetAppRegistryValue (enemiesPermissionLinePar);
				}
			}

		/// <summary>
		/// Возвращает ограничение вероятности генерации врагов
		/// </summary>
		public const byte MaximumEnemiesProbability = 5;



		/// <summary>
		/// Возвращает или задаёт набор вероятностей генерации предметов
		/// </summary>
		public byte[] ItemsPermissionLine
			{
			get
				{
				// Отсечка
				if (itemsPermissionLine != null)
					return itemsPermissionLine.ToArray ();

				// Присвоение с перезаписью
				string line = "";
				if (settingFromEngineToken == itemsPermissionLinePar)
					{
					line = settingFromEngineValue;
					RDGenerics.SetAppRegistryValue (itemsPermissionLinePar, line);
					}

				// Простое присвоение
				else
					{
					line = RDGenerics.GetAppRegistryValue (itemsPermissionLinePar);
					}

				// Забой недостающих элементов
				bool defValues = false;
				for (int i = line.Length; i < ItemsSupport.AvailableItemsTypes; i++)
					{
					line += MaximumItemsProbability.ToString ();
					defValues = true;
					}

				// Сплит
				itemsPermissionLine = new List<byte> ();
				for (int i = 0; i < ItemsSupport.AvailableItemsTypes; i++)
					{
					try
						{
						string s = line[i].ToString ();
						itemsPermissionLine.Add (byte.Parse (s));
						if (itemsPermissionLine[i] > MaximumItemsProbability)
							itemsPermissionLine[i] = MaximumItemsProbability;
						}
					catch
						{
						itemsPermissionLine.Add (1);
						defValues = true;
						}
					}

				if (defValues)
					ItemsPermissionLine = itemsPermissionLine.ToArray ();

				return itemsPermissionLine.ToArray ();
				}
			set
				{
				itemsPermissionLine = new List<byte> (value);
				string line = "";
				for (int i = 0; i < itemsPermissionLine.Count; i++)
					line += itemsPermissionLine[i].ToString ();

				RDGenerics.SetAppRegistryValue (itemsPermissionLinePar, line);
				}
			}
		private List<byte> itemsPermissionLine;
		private const string itemsPermissionLinePar = "IP";

		/// <summary>
		/// Возвращает строку разрешённых предметов
		/// </summary>
		public string ItemsPermissionLineAsString
			{
			get
				{
				return RDGenerics.GetAppRegistryValue (itemsPermissionLinePar);
				}
			}

		/// <summary>
		/// Возвращает ограничение вероятности генерации предметов
		/// </summary>
		public const byte MaximumItemsProbability = 5;



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
		/// Возвращает ограничение уровня воды (25% максимум)
		/// </summary>
		public const uint MaximumWaterLevel = 6;



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



		/// <summary>
		/// Возвращает или задаёт флаг разрешения генерации навигационной сетки
		/// </summary>
		public bool UseMapNodes
			{
			get
				{
				return GetSettingsValue (useMapNodesPar, 2, 1, ref useMapNodes) > 1;
				}
			set
				{
				SetSettingsValue (useMapNodesPar, ref useMapNodes, (uint)(value ? 2 : 1));
				}
			}
		private int useMapNodes = int.MaxValue;
		private const string useMapNodesPar = "MN";



		/// <summary>
		/// Возвращает или задаёт баланс ящиков между предметами и взывчаткой
		/// [-CratesBalanceRange; +CratesBalanceRange]
		/// </summary>
		public int CratesBalance
			{
			get
				{
				return (int)GetSettingsValue (cratesBalancePar, MaximumCratesBalance, CratesBalanceRange + 1,
					ref cratesBalance) - CratesBalanceRange - 1;
				}
			set
				{
				SetSettingsValue (cratesBalancePar, ref cratesBalance, (uint)(value + CratesBalanceRange + 1));
				}
			}
		private int cratesBalance = int.MaxValue;
		private const string cratesBalancePar = "CB";

		/// <summary>
		/// Возвращает ограничение баланса ящиков (соответствует границе +3)
		/// </summary>
		private const uint MaximumCratesBalance = 2 * CratesBalanceRange + 1;

		/// <summary>
		/// Возвращает границу диапазона баланса ящиков
		/// </summary>
		public const int CratesBalanceRange = 3;



		/// <summary>
		/// Возвращает или задаёт коэффициент преобразования врагов в ящики
		/// [0; 5]
		/// </summary>
		public uint CratesDensityCoefficient2
			{
			get
				{
				return GetSettingsValue (cratesDensityCoefficientPar,
					MaximumCratesDensityCoefficient2, 3, ref cratesDensityCoefficient) - 1;
				}
			set
				{
				SetSettingsValue (cratesDensityCoefficientPar, ref cratesDensityCoefficient, value + 1);
				}
			}
		private int cratesDensityCoefficient = int.MaxValue;
		private const string cratesDensityCoefficientPar = "CD";

		/// <summary>
		/// Возвращает или задаёт флаг случайного коэффициента преобразования врагов в ящики
		/// </summary>
		public bool RandomCratesDensityCoefficient2
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
		public const uint MaximumCratesDensityCoefficient2 = 6;



		/// <summary>
		/// Возвращает или задаёт флаг добавления неоновых ламп на карты
		/// </summary>
		public bool UseNeonLights
			{
			get
				{
				return GetSettingsValue (useNeonLightsPar, 2, 1, ref useNeonLights) > 1;
				}
			set
				{
				SetSettingsValue (useNeonLightsPar, ref useNeonLights, (uint)(value ? 2 : 1));
				}
			}
		private int useNeonLights = int.MaxValue;
		private const string useNeonLightsPar = "NL";
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
