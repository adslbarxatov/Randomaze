namespace RD_AAOW
	{
	/// <summary>
	/// Структура описывает настраиваемые параметры приложения
	/// </summary>
	public struct ESRMSettings
		{
		/// <summary>
		/// Масштабный коэффициент размера лабиринта
		/// </summary>
		public uint MazeSizeCoefficient;

		/// <summary>
		/// Случайный масштабный коэффициент размера лабиринта
		/// </summary>
		public bool RandomMazeSizeCoefficient;

		/// <summary>
		/// Ограничение коэффициента размера лабиринта
		/// </summary>
		public uint MaximumMazeSizeCoefficient;

		/// <summary>
		/// Коэффициент плотности врагов
		/// </summary>
		public uint EnemiesDensityCoefficient;

		/// <summary>
		/// Случайный коэффициент плотности врагов
		/// </summary>
		public bool RandomEnemiesDensityCoefficient;

		/// <summary>
		/// Ограничение коэффициента плотности врагов
		/// </summary>
		public uint MaximumEnemiesDensityCoefficient;

		/// <summary>
		/// Коэффициент плотности собираемых объектов
		/// </summary>
		public uint ItemsDensityCoefficient;

		/// <summary>
		/// Случайный коэффициент плотности собираемых объектов
		/// </summary>
		public bool RandomItemsDensityCoefficient;

		/// <summary>
		/// Ограничение коэффициента плотности собираемых объектов
		/// </summary>
		public uint MaximumItemsDensityCoefficient;

		/// <summary>
		/// Коэффициент насыщенности лабиринта стенами
		/// </summary>
		public uint WallsDensityCoefficient;

		/// <summary>
		/// Случайный коэффициент насыщенности лабиринта стенами
		/// </summary>
		public bool RandomWallsDensityCoefficient;

		/// <summary>
		/// Ограничение коэффициента насыщенности лабиринта стенами
		/// </summary>
		public uint MaximumWallsDensityCoefficient;

		/// <summary>
		/// Режим блокировки выхода кнопкой
		/// </summary>
		public bool ButtonMode;

		/// <summary>
		/// Коэффициент преобразования врагов в ящики
		/// </summary>
		public uint CratesDensityCoefficient;

		/// <summary>
		/// Случайный коэффициент преобразования врагов в ящики
		/// </summary>
		public bool RandomCratesDensityCoefficient;

		/// <summary>
		/// Ограничение коэффициента преобразования врагов в ящики
		/// </summary>
		public uint MaximumCratesDensityCoefficient;

		/// <summary>
		/// Строка разрешённых врагов
		/// </summary>
		public string EnemiesPermissionLine;

		/// <summary>
		/// Коэффициент освещения карты
		/// </summary>
		public uint LightingCoefficient;

		/// <summary>
		/// Случайный коэффициент освещения карты
		/// </summary>
		public bool RandomLightingCoefficient;

		/// <summary>
		/// Ограничение коэффициента освещения карты
		/// </summary>
		public uint MaximumLightingCoefficient;

		/// <summary>
		/// Тип фильтрации секций карты
		/// </summary>
		public MapSectionTypes SectionType;

		/// <summary>
		/// Флаг двойной высоты карт
		/// </summary>
		public bool TwoFloors;

		/// <summary>
		/// Флаг разрешения размещения собираемых объектов на внутренних площадках
		/// </summary>
		public bool AllowItemsForSecondFloor;

		/// <summary>
		/// Флаг разрешения ящиков с жуками и соибраемыми предметами
		/// </summary>
		public bool AllowItemsCrates;

		/// <summary>
		/// Флаг разрешения ящиков со взрывчаткой
		/// </summary>
		public bool AllowExplosiveCrates;
		}

	/// <summary>
	/// Возможные варианты фильтрации секций карты
	/// </summary>
	public enum MapSectionTypes
		{
		/// <summary>
		/// Все варианты
		/// </summary>
		AllTypes = 0,

		/// <summary>
		/// Только варианты с небом
		/// </summary>
		OnlyUnderSky = 1,

		/// <summary>
		/// Только варианты в помещении
		/// </summary>
		OnlyInside = 2
		}
	}
