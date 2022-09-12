using System;
using System.Windows.Forms;

namespace RD_AAOW
	{
	/// <summary>
	/// Основной класс программы
	/// </summary>
	public class Program
		{

		/// <summary>
		/// Точка входа приложения
		/// </summary>
		/// <param name="args">Аргументы командной строки</param>
		[STAThread]
		public static int Main (string[] args)
			{
			// Инициализация
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);

			// Отображение справки и запроса на принятие Политики
			if (!ProgramDescription.AcceptEULA ())
				return -1;
			ProgramDescription.ShowAbout (true);

			// Запуск
			Application.Run (new RandomazeForm (args));
			return 0;
			}
		}
	}
