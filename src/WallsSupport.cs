using System.Drawing;

namespace RD_AAOW
	{
	/// <summary>
	/// Возможные типы соседних препятствий
	/// </summary>
	public enum WallsNeighborsTypes
		{
		/// <summary>
		/// Обычная стена
		/// </summary>
		Wall,

		/// <summary>
		/// Шлюз
		/// </summary>
		Gate,

		/// <summary>
		/// Окно
		/// </summary>
		Window,

		/// <summary>
		/// Окно под углом к стене
		/// </summary>
		WindowCorner,

		/// <summary>
		/// Свободный проход
		/// </summary>
		Passage,
		}

	/// <summary>
	/// Класс предоставляет обработчики для стен и шлюзов
	/// </summary>
	public static class WallsSupport
		{
		/// <summary>
		/// Метод определяет положение стены
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <returns>Возвращает true, если стена вертикальная</returns>
		public static bool IsWallVertical (Point RelativePosition)
			{
			return (RelativePosition.X % 2 == 0);
			}

		/// <summary>
		/// Метод записывает шлюз, ограничивающий точку входа, на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Frame">Флаг указывает, что записывается рама шлюза вместо него самого</param>
		public static void WriteMapFinishGate (Point RelativePosition, bool Frame)
			{
			WriteGate (RelativePosition, Frame, true);
			}

		/// <summary>
		/// Метод записывает шлюз, закрывающий точку выхода, на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Frame">Флаг указывает, что записывается рама шлюза вместо него самого</param>
		public static void WriteMapGate (Point RelativePosition, bool Frame)
			{
			WriteGate (RelativePosition, Frame, false);
			}

		/// <summary>
		/// Метод записывает стену на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="FrontTexture">Лицевая текстура стены</param>
		/// <param name="BackTexture">Задняя текстура стены</param>
		/// <param name="LeftEnd">Тип левого торца стены</param>
		/// <param name="RightEnd">Тип правого торца стены</param>
		public static void WriteMapWall (Point RelativePosition, string FrontTexture, string BackTexture,
			WallsNeighborsTypes LeftEnd, WallsNeighborsTypes RightEnd)
			{
			neighborLeft = LeftEnd;
			neighborRight = RightEnd;

			WriteMapBarrier (RelativePosition, BarrierTypes.DefaultWall, FrontTexture, BackTexture);
			}

		/// <summary>
		/// Метод записывает раму окна на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="FrontTexture">Лицевая текстура стены для рамы</param>
		/// <param name="BackTexture">Задняя текстура стены для рамы</param>
		/// <param name="FloorsIsolation">Флаг изоляции этажей</param>
		public static void WriteMapWindow (Point RelativePosition, string FrontTexture, string BackTexture,
			bool FloorsIsolation)
			{
			WriteMapBarrier (RelativePosition, BarrierTypes.WindowFrameTop, FrontTexture, BackTexture);
			WriteMapBarrier (RelativePosition, BarrierTypes.WindowFrameBottom, FrontTexture, BackTexture);

			if (FloorsIsolation)
				WriteMapBarrier (RelativePosition, BarrierTypes.WindowFrameMiddle, FrontTexture, BackTexture);
			}

		/// <summary>
		/// Метод записывает разрушаемую часть окна и звуковой эффект перехода между секциями на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="MapBarrier">Тип материала разрушаемой части</param>
		/// <param name="FloorsIsolation">Флаг изоляции этажей</param>
		public static void WriteMapWindow (Point RelativePosition, MapBarriersTypes MapBarrier,
			bool FloorsIsolation)
			{
			// Запись преграды
			bool glass;
			switch (MapBarrier)
				{
				case MapBarriersTypes.OnlyGlass:
				default:
					glass = true;
					break;

				case MapBarriersTypes.OnlyFabric:
					glass = false;
					break;

				case MapBarriersTypes.Both:
					glass = RDGenerics.RND.Next (2) == 0;
					break;
				}

			// Запись
			int fragments = FloorsIsolation ? 2 : 1;

			for (int i = 0; i < fragments; i++)
				{
				MapSupport.Write ("{\n");
				MapSupport.AddEntity (MapClasses.Breakable);

				if (glass)
					{
					MapSupport.Write ("\"rendermode\" \"2\"\n");
					MapSupport.Write ("\"renderamt\" \"80\"\n");
					MapSupport.Write ("\"material\" \"0\"\n");
					}
				else
					{
					MapSupport.Write ("\"rendermode\" \"0\"\n");
					MapSupport.Write ("\"renderamt\" \"0\"\n");
					MapSupport.Write ("\"material\" \"9\"\n");
					}
				MapSupport.Write ("\"health\" \"20\"\n");

				if (!FloorsIsolation)
					WriteMapBarrier (RelativePosition, glass ? BarrierTypes.GlassWindowFull :
						BarrierTypes.FabricWindowFull, null, null);
				else if (i == 0)
					WriteMapBarrier (RelativePosition, glass ? BarrierTypes.GlassWindowFirstFloor :
						BarrierTypes.FabricWindowFirstFloor, null, null);
				else
					WriteMapBarrier (RelativePosition, glass ? BarrierTypes.GlassWindowSecondFloor :
						BarrierTypes.FabricWindowSecondFloor, null, null);

				MapSupport.Write ("}\n");
				}
			}

		/// <summary>
		/// Метод записывает звуковой эффект перехода между секциями на карту
		/// </summary>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Sections">Инициализированные секции карты</param>
		/// <param name="WallsAreRare">Флаг, указывающий на редкость стен (большие внутренние пространства)</param>
		/// <param name="FloorsIsolation">Флаг изоляции этажей</param>
		public static void WriteMapTransitSFX (Point RelativePosition, Section[] Sections,
			bool WallsAreRare, bool FloorsIsolation)
			{
			// Определение необходимости установки звукового триггера
			bool leftSideIsUnderSky, rightSideIsUnderSky;
			if (IsWallVertical (RelativePosition))
				{
				leftSideIsUnderSky = Sections[MapSupport.GetSection
					(new Point (RelativePosition.X - 1, RelativePosition.Y))].IsUnderTheSky;
				rightSideIsUnderSky = Sections[MapSupport.GetSection
					(new Point (RelativePosition.X + 1, RelativePosition.Y))].IsUnderTheSky;
				}
			else
				{
				leftSideIsUnderSky = Sections[MapSupport.GetSection
					(new Point (RelativePosition.X, RelativePosition.Y - 1))].IsUnderTheSky;
				rightSideIsUnderSky = Sections[MapSupport.GetSection
					(new Point (RelativePosition.X, RelativePosition.Y + 1))].IsUnderTheSky;
				}

			if (leftSideIsUnderSky == rightSideIsUnderSky)
				return;

			// Запись звукового триггера
			/*byte leftRT, rightRT;
			byte offset = (byte)((MapSupport.TwoFloors && !FloorsIsolation) ? 1 : 0);
			if (leftSideIsUnderSky)
				leftRT = 0;
			else if (WallsAreRare)
				leftRT = (byte)(18 + offset);
			else
				leftRT = (byte)(17 + offset);

			if (rightSideIsUnderSky)
				rightRT = 0;
			else if (WallsAreRare)
				rightRT = (byte)(18 + offset);
			else
				rightRT = (byte)(17 + offset);*/
			MapEntryPointFlags f = MapEntryPointFlags.None;
			if (leftSideIsUnderSky)
				f |= MapEntryPointFlags.IsUnderSky;
			if (WallsAreRare)
				f |= MapEntryPointFlags.WallsAreRare;
			if (FloorsIsolation)
				f |= MapEntryPointFlags.FloorsIsolation;
			MapSoundTriggerRooms rtl = MapSupport.GetRoomType (f);

			f = MapEntryPointFlags.None;
			if (rightSideIsUnderSky)
				f |= MapEntryPointFlags.IsUnderSky;
			if (WallsAreRare)
				f |= MapEntryPointFlags.WallsAreRare;
			if (FloorsIsolation)
				f |= MapEntryPointFlags.FloorsIsolation;
			MapSoundTriggerRooms rtr = MapSupport.GetRoomType (f);

			MapSupport.WriteMapSoundTrigger (RelativePosition,
				FloorsIsolation ? MapSoundTriggerTypes.WindowOnSecondFloor : MapSoundTriggerTypes.Window, rtl, rtr);
			}

		// Универсальный метод формирования шлюза
		private static void WriteGate (Point RelativePosition, bool Frame, bool Finish)
			{
			// Расчёт параметров
			string tex = Finish ? "MetalGate07" : "MetalGate06";

			// Запись рамы
			if (Frame)
				{
				WriteMapBarrier (RelativePosition, BarrierTypes.GateFrameTop, tex, tex);
				return;
				}

			// Запись шлюза
			MapSupport.Write ("{\n");
			MapSupport.AddEntity (MapClasses.Door);
			MapSupport.Write ("\"angles\" \"90 0 0\"\n");
			MapSupport.Write ("\"speed\" \"70\"\n");
			MapSupport.Write ("\"movesnd\" \"3\"\n");
			MapSupport.Write ("\"stopsnd\" \"1\"\n");
			MapSupport.Write ("\"wait\" \"-1\"\n");
			MapSupport.Write ("\"lip\" \"9\"\n");
			if (Finish)
				MapSupport.Write ("\"targetname\" \"" + MapSupport.FirstGateName + "\"\n");

			WriteMapBarrier (RelativePosition, BarrierTypes.Gate, tex, tex);

			MapSupport.Write ("}\n");
			}

		// Возможные типы препятствий
		private enum BarrierTypes
			{
			// Обычная стена
			DefaultWall,

			// Шлюз
			Gate,

			// Верхняя рама окна
			WindowFrameTop,

			// Нижняя рама окна
			WindowFrameBottom,

			// Средняя рама окна (для изолированных этажей)
			WindowFrameMiddle,

			// Преграды
			GlassWindowFull,
			FabricWindowFull,

			GlassWindowFirstFloor,
			FabricWindowFirstFloor,

			GlassWindowSecondFloor,
			FabricWindowSecondFloor,

			// Верхняя рама шлюза
			GateFrameTop,
			}

		// Общий метод для стен и препятствий
		private static WallsNeighborsTypes neighborLeft, neighborRight;
		private static void WriteMapBarrier (Point RelativePosition, BarrierTypes Type, string FrontTexture,
			string BackTexture)
			{
			// Расчёт параметров
			int x1, y1, x2, y2;
			string xa, xb, xc, xd, ya, yb, yc, yd, z1, z2;
			string[] textures;
			string fTex = FrontTexture;
			string bTex = BackTexture;

			int lDelta = 8, rDelta = 8, mDelta = 8;
			switch (Type)
				{
				case BarrierTypes.DefaultWall:
				default:
					z1 = "0";
					z2 = (MapSupport.WallHeight + 16).ToString ();

					string lTex = fTex;
					string rTex = fTex;
					if (neighborLeft == WallsNeighborsTypes.Gate)
						{
						lTex = "BorderRub01";
						}
					else if (neighborLeft == WallsNeighborsTypes.Window)
						{
						lTex = MapSupport.BlueMetalTexture;
						lDelta = 0;
						}
					else if (neighborLeft == WallsNeighborsTypes.WindowCorner)
						{
						lTex = MapSupport.BlueMetalTexture;
						}
					else if (neighborLeft == WallsNeighborsTypes.Wall)
						{
						lDelta = 0;
						}

					if (neighborRight == WallsNeighborsTypes.Gate)
						{
						rTex = "BorderRub01";
						}
					else if (neighborRight == WallsNeighborsTypes.Window)
						{
						rTex = MapSupport.BlueMetalTexture;
						rDelta = 0;
						}
					else if (neighborRight == WallsNeighborsTypes.WindowCorner)
						{
						rTex = MapSupport.BlueMetalTexture;
						}
					else if (neighborRight == WallsNeighborsTypes.Wall)
						{
						rDelta = 0;
						}

					textures = [MapSupport.SkyTexture, fTex, fTex, bTex, lTex, lTex, rTex, rTex];
					break;

				case BarrierTypes.Gate:
					z1 = "0";
					z2 = "120";
					textures = [MapSupport.BlueMetalTexture, fTex, fTex, fTex, fTex, fTex, fTex, fTex];
					break;

				case BarrierTypes.GlassWindowFull:
				case BarrierTypes.FabricWindowFull:
				case BarrierTypes.GlassWindowFirstFloor:
				case BarrierTypes.FabricWindowFirstFloor:
				case BarrierTypes.GlassWindowSecondFloor:
				case BarrierTypes.FabricWindowSecondFloor:
					string tex;
					uint texIdx = (uint)RDGenerics.RND.Next (2) + 1;

					switch (Type)
						{
						case BarrierTypes.GlassWindowFull:
						case BarrierTypes.GlassWindowFirstFloor:
						case BarrierTypes.GlassWindowSecondFloor:
							tex = MapSupport.GetGlassTexture (texIdx);
							break;

						default:
							tex = MapSupport.GetFabricTexture (texIdx + 2);
							break;
						}

					switch (Type)
						{
						case BarrierTypes.GlassWindowFull:
						case BarrierTypes.FabricWindowFull:
						default:
							z1 = "8";
							z2 = (MapSupport.WallHeight - 8).ToString ();
							break;

						case BarrierTypes.GlassWindowFirstFloor:
						case BarrierTypes.FabricWindowFirstFloor:
							z1 = "8";
							z2 = (MapSupport.BalconyHeight - 18).ToString ();
							break;

						case BarrierTypes.GlassWindowSecondFloor:
						case BarrierTypes.FabricWindowSecondFloor:
							z1 = (MapSupport.BalconyHeight + 8).ToString ();
							z2 = (MapSupport.WallHeight - 8).ToString ();
							break;
						}

					/*z1 = "8";
					z2 = (MapSupport.WallHeight - 8).ToString ();*/
					textures = [tex, tex, tex, tex, tex, tex, tex, tex];
					rDelta = lDelta = 0;
					mDelta = 4;
					break;

				case BarrierTypes.WindowFrameTop:
				case BarrierTypes.GateFrameTop:
					if (Type == BarrierTypes.WindowFrameTop)
						z1 = (MapSupport.WallHeight - 8).ToString ();
					else
						z1 = "120";
					z2 = (MapSupport.WallHeight + 16).ToString ();

					textures = [MapSupport.SkyTexture, MapSupport.BlueMetalTexture, fTex, bTex, fTex, bTex, fTex, bTex];

					if (Type == BarrierTypes.WindowFrameTop)
						rDelta = lDelta = 0;
					break;

				case BarrierTypes.WindowFrameBottom:
					z1 = "0";
					z2 = "8";
					textures = [MapSupport.BlueMetalTexture, fTex, fTex, bTex, fTex, bTex, fTex, bTex];
					rDelta = lDelta = 0;
					break;

				case BarrierTypes.WindowFrameMiddle:
					z1 = (MapSupport.BalconyHeight - 18).ToString ();
					z2 = (MapSupport.BalconyHeight + 8).ToString ();
					textures = [MapSupport.BlueMetalTexture, MapSupport.BlueMetalTexture,
						fTex, bTex, fTex, bTex, fTex, bTex];
					rDelta = lDelta = 0;
					break;
				}

			// Запись
			MapSupport.Write ("{\n");

			// Вертикальная
			if (IsWallVertical (RelativePosition))
				{
				x1 = RelativePosition.X * MapSupport.WallLength / 2;
				y1 = (RelativePosition.Y - 1) * MapSupport.WallLength / 2;
				y2 = (RelativePosition.Y + 1) * MapSupport.WallLength / 2;

				xa = (x1 - mDelta).ToString ();
				xb = x1.ToString ();
				xc = (x1 + mDelta).ToString ();
				ya = y1.ToString ();
				yb = (y1 + lDelta).ToString ();
				yc = (y2 - rDelta).ToString ();
				yd = y2.ToString ();

				// Нижний и верхний торцы
				MapSupport.Write ("( " + xc + " " + yc + " " + z2 + " ) " +
					"( " + xc + " " + yb + " " + z2 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					textures[0] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				MapSupport.Write ("( " + xa + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yb + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z1 + " ) " +
					textures[1] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");

				// Лицевая и задняя сторона
				MapSupport.Write ("( " + xa + " " + yb + " " + z1 + " ) " +
					"( " + xa + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yc + " " + z2 + " ) " +
					textures[2] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				MapSupport.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yb + " " + z1 + " ) " +
					"( " + xc + " " + yb + " " + z2 + " ) " +
					textures[3] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");

				// Верхний торец
				if (lDelta == 0)
					{
					MapSupport.Write ("( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[4] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					MapSupport.Write ("( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					MapSupport.Write ("( " + xc + " " + yb + " " + z1 + " ) " +
						"( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xb + " " + ya + " " + z2 + " ) " +
						textures[5] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}

				// Нижний торец
				if (rDelta == 0)
					{
					MapSupport.Write ("( " + xa + " " + yd + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z2 + " ) " +
						textures[6] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					MapSupport.Write ("( " + xb + " " + yd + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					MapSupport.Write ("( " + xa + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yd + " " + z1 + " ) " +
						"( " + xb + " " + yd + " " + z2 + " ) " +
						textures[7] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				}

			// Горизонтальная
			else
				{
				y1 = RelativePosition.Y * MapSupport.WallLength / 2;
				x1 = (RelativePosition.X - 1) * MapSupport.WallLength / 2;
				x2 = (RelativePosition.X + 1) * MapSupport.WallLength / 2;

				xa = x1.ToString ();
				xb = (x1 + lDelta).ToString ();
				xc = (x2 - rDelta).ToString ();
				xd = x2.ToString ();
				ya = (y1 - mDelta).ToString ();
				yb = y1.ToString ();
				yc = (y1 + mDelta).ToString ();

				// Нижний и верхний торец
				MapSupport.Write ("( " + xc + " " + ya + " " + z2 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					"( " + xa + " " + yb + " " + z2 + " ) " +
					textures[0] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				MapSupport.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xb + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yb + " " + z1 + " ) " +
					textures[1] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");

				// Лицевая и задняя сторона
				MapSupport.Write ("( " + xb + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yc + " " + z2 + " ) " +
					textures[2] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				MapSupport.Write ("( " + xc + " " + ya + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					textures[3] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");

				// Левый торец
				if (lDelta == 0)
					{
					MapSupport.Write ("( " + xa + " " + ya + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					MapSupport.Write ("( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					MapSupport.Write ("( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[5] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}

				// Правый торец
				if (rDelta == 0)
					{
					MapSupport.Write ("( " + xd + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					MapSupport.Write ("( " + xd + " " + yb + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					MapSupport.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xd + " " + yb + " " + z1 + " ) " +
						"( " + xd + " " + yb + " " + z2 + " ) " +
						textures[7] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				}

			MapSupport.Write ("}\n");
			}
		}
	}
