using System.Drawing;
using System.IO;

namespace RD_AAOW
	{
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
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Frame">Флаг указывает, что записывается рама шлюза вместо него самого</param>
		public static void WriteMapGate (StreamWriter SW, Point RelativePosition, bool Frame)
			{
			WriteGate (SW, RelativePosition, Frame, MapSupport.MapsLimit + 1);
			}

		/// <summary>
		/// Метод записывает шлюз, закрывающий точку выхода, на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Frame">Флаг указывает, что записывается рама шлюза вместо него самого</param>
		/// <param name="MapNumber">Номер карты, используемый для создания уникального имени шлюза</param>
		public static void WriteMapGate (StreamWriter SW, Point RelativePosition, bool Frame, uint MapNumber)
			{
			WriteGate (SW, RelativePosition, Frame, MapNumber);
			}

		/// <summary>
		/// Метод записывает стену на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура стены</param>
		/// <param name="LeftEnd">Тип левого торца стены</param>
		/// <param name="RightEnd">Тип правого торца стены</param>
		public static void WriteMapWall (StreamWriter SW, Point RelativePosition, string Texture,
			WallsNeighborsTypes LeftEnd, WallsNeighborsTypes RightEnd)
			{
			neighborLeft = LeftEnd;
			neighborRight = RightEnd;

			WriteMapBarrier (SW, RelativePosition, BarrierTypes.DefaultWall, Texture);
			}

		/// <summary>
		/// Метод записывает раму окна на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Texture">Текстура стены для рамы</param>
		public static void WriteMapWindow (StreamWriter SW, Point RelativePosition, string Texture)
			{
			WriteMapBarrier (SW, RelativePosition, BarrierTypes.WindowFrameTop, Texture);
			WriteMapBarrier (SW, RelativePosition, BarrierTypes.WindowFrameBottom, Texture);
			}

		/// <summary>
		/// Метод записывает стекло окна и звуковой эффект перехода между секциями на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		public static void WriteMapWindow (StreamWriter SW, Point RelativePosition, MapBarriersTypes MapBarrier)
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

			SW.Write ("{\n");
			MapSupport.AddEntity (SW, "func_breakable");

			if (glass)
				{
				SW.Write ("\"rendermode\" \"2\"\n");
				SW.Write ("\"renderamt\" \"80\"\n");
				SW.Write ("\"material\" \"0\"\n");
				}
			else
				{
				SW.Write ("\"rendermode\" \"0\"\n");
				SW.Write ("\"renderamt\" \"0\"\n");
				SW.Write ("\"material\" \"3\"\n");
				}
			SW.Write ("\"health\" \"20\"\n");

			WriteMapBarrier (SW, RelativePosition, glass ? BarrierTypes.GlassWindow :
				BarrierTypes.FabricWindow, null);

			SW.Write ("}\n");
			}

		/// <summary>
		/// Метод записывает звуковой эффект перехода между секциями на карту
		/// </summary>
		/// <param name="SW">Дескриптор файла карты</param>
		/// <param name="RelativePosition">Относительная позиция точки создания</param>
		/// <param name="Sections">Инициализированные секции карты</param>
		/// <param name="WallsAreRare">Флаг, указывающий на редкость стен (большие внутренние пространства)</param>
		public static void WriteMapTransitSFX (StreamWriter SW, Point RelativePosition, Section[] Sections,
			bool WallsAreRare)
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
			byte leftRT, rightRT;
			byte offset = (byte)(MapSupport.TwoFloors ? 1 : 0);
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
				rightRT = (byte)(17 + offset);

			MapSupport.WriteMapSoundTrigger (SW, RelativePosition, true, leftRT, rightRT);
			}

		// Универсальный метод формирования шлюза
		private static void WriteGate (StreamWriter SW, Point RelativePosition, bool Frame, uint MapNumber)
			{
			// Расчёт параметров
			string tex = (MapNumber > MapSupport.MapsLimit) ? "MetalGate06" : "MetalGate07";

			// Запись рамы
			if (Frame)
				{
				WriteMapBarrier (SW, RelativePosition, BarrierTypes.GateFrameTop, tex);
				return;
				}

			// Запись шлюза
			SW.Write ("{\n");
			MapSupport.AddEntity (SW, "func_door");
			SW.Write ("\"angles\" \"90 0 0\"\n");
			SW.Write ("\"speed\" \"100\"\n");
			SW.Write ("\"movesnd\" \"3\"\n");
			SW.Write ("\"stopsnd\" \"1\"\n");
			SW.Write ("\"wait\" \"-1\"\n");
			SW.Write ("\"lip\" \"9\"\n");
			if (MapNumber <= MapSupport.MapsLimit)
				SW.Write ("\"targetname\" \"Gate" + MapSupport.BuildMapName (MapNumber) + "\"\n");

			WriteMapBarrier (SW, RelativePosition, BarrierTypes.Gate, tex);

			SW.Write ("}\n");
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

			// Преграды
			GlassWindow,
			FabricWindow,

			// Верхняя рама шлюза
			GateFrameTop
			}

		// Общий метод для стен и препятствий
		private static WallsNeighborsTypes neighborLeft, neighborRight;
		private static void WriteMapBarrier (StreamWriter SW, Point RelativePosition, BarrierTypes Type,
			string Texture)
			{
			// Расчёт параметров
			int x1, y1, x2, y2;
			string xa, xb, xc, xd, ya, yb, yc, yd, z1, z2;
			string[] textures;

			int lDelta = 8, rDelta = 8, mDelta = 8;
			switch (Type)
				{
				case BarrierTypes.DefaultWall:
				default:
					z1 = "0";
					z2 = (MapSupport.WallHeight + 16).ToString ();

					string lTex = Texture;
					if (neighborLeft == WallsNeighborsTypes.Gate)
						{
						lTex = "BorderRub01";
						}
					else if (neighborLeft == WallsNeighborsTypes.Window)
						{
						lTex = "Metal08";
						lDelta = 0;
						}
					else if (neighborLeft == WallsNeighborsTypes.WindowCorner)
						{
						lTex = "Metal08";
						}
					else if (neighborLeft == WallsNeighborsTypes.Wall)
						{
						lDelta = 0;
						}

					string rTex = Texture;
					if (neighborRight == WallsNeighborsTypes.Gate)
						{
						rTex = "BorderRub01";
						}
					else if (neighborRight == WallsNeighborsTypes.Window)
						{
						rTex = "Metal08";
						rDelta = 0;
						}
					else if (neighborRight == WallsNeighborsTypes.WindowCorner)
						{
						rTex = "Metal08";
						}
					else if (neighborRight == WallsNeighborsTypes.Wall)
						{
						rDelta = 0;
						}

					textures = new string[] { MapSupport.SkyTexture, Texture, Texture, Texture,
						lTex, lTex, rTex, rTex };
					break;

				case BarrierTypes.Gate:
					z1 = "0";
					z2 = "120";
					textures = new string[] { "Metal08", Texture, Texture, Texture,
						Texture, Texture, Texture, Texture };
					break;

				case BarrierTypes.GlassWindow:
				case BarrierTypes.FabricWindow:
					string tex = (Type == BarrierTypes.GlassWindow) ? "Glass01" : "Fabric03";
					z1 = "8";
					z2 = (MapSupport.WallHeight - 8).ToString ();
					textures = new string[] { tex, tex, tex, tex, tex, tex, tex, tex };
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

					textures = new string[] { MapSupport.SkyTexture, "Metal08", Texture, Texture,
						Texture, Texture, Texture, Texture };

					if (Type == BarrierTypes.WindowFrameTop)
						rDelta = lDelta = 0;
					break;

				case BarrierTypes.WindowFrameBottom:
					z1 = "0";
					z2 = "8";
					textures = new string[] { "Metal08", Texture, Texture, Texture,
						Texture, Texture, Texture, Texture };
					rDelta = lDelta = 0;
					break;
				}

			// Запись
			SW.Write ("{\n");

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
				SW.Write ("( " + xc + " " + yc + " " + z2 + " ) " +
					"( " + xc + " " + yb + " " + z2 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					textures[0] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + xa + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yb + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z1 + " ) " +
					textures[1] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");

				// Лицевая и задняя сторона
				SW.Write ("( " + xa + " " + yb + " " + z1 + " ) " +
					"( " + xa + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yc + " " + z2 + " ) " +
					textures[2] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yb + " " + z1 + " ) " +
					"( " + xc + " " + yb + " " + z2 + " ) " +
					textures[3] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");

				// Верхний торец
				if (lDelta == 0)
					{
					SW.Write ("( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[4] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xc + " " + yb + " " + z1 + " ) " +
						"( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xb + " " + ya + " " + z2 + " ) " +
						textures[5] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}

				// Нижний торец
				if (rDelta == 0)
					{
					SW.Write ("( " + xa + " " + yd + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z2 + " ) " +
						textures[6] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xb + " " + yd + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + yc + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xa + " " + yc + " " + z1 + " ) " +
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
				SW.Write ("( " + xc + " " + ya + " " + z2 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					"( " + xa + " " + yb + " " + z2 + " ) " +
					textures[0] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");
				SW.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xb + " " + yc + " " + z1 + " ) " +
					"( " + xa + " " + yb + " " + z1 + " ) " +
					textures[1] + " [ 1 0 0 0 ] [ 0 -1 0 0 ] 0 1 1 \n");

				// Лицевая и задняя сторона
				SW.Write ("( " + xb + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yc + " " + z1 + " ) " +
					"( " + xc + " " + yc + " " + z2 + " ) " +
					textures[2] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
				SW.Write ("( " + xc + " " + ya + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z1 + " ) " +
					"( " + xb + " " + ya + " " + z2 + " ) " +
					textures[3] + " [ 1 0 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");

				// Левый торец
				if (lDelta == 0)
					{
					SW.Write ("( " + xa + " " + ya + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z1 + " ) " +
						"( " + xb + " " + yc + " " + z2 + " ) " +
						textures[4] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xb + " " + ya + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z1 + " ) " +
						"( " + xa + " " + yb + " " + z2 + " ) " +
						textures[5] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}

				// Правый торец
				if (rDelta == 0)
					{
					SW.Write ("( " + xd + " " + yc + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				else
					{
					SW.Write ("( " + xd + " " + yb + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z1 + " ) " +
						"( " + xc + " " + ya + " " + z2 + " ) " +
						textures[6] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					SW.Write ("( " + xc + " " + yc + " " + z1 + " ) " +
						"( " + xd + " " + yb + " " + z1 + " ) " +
						"( " + xd + " " + yb + " " + z2 + " ) " +
						textures[7] + " [ 0 1 0 0 ] [ 0 0 -1 0 ] 0 1 1 \n");
					}
				}

			SW.Write ("}\n");
			}
		}

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
		Passage
		}
	}
