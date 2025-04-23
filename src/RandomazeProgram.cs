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
			RDLocale.InitEncodings ();

			// Проверка запуска единственной копии
			if (!RDGenerics.IsAppInstanceUnique (false))
				return -3;

			// Язык интерфейса и контроль XPUN
			if (!RDLocale.IsXPUNClassAcceptable)
				return -1;

			// Отображение справки и запроса на принятие Политики
			if (!RDInterface.AcceptEULA ())
				return -2;
			RDInterface.ShowAbout (true);

			if (!RDGenerics.AppHasAccessRights (true, false))
				return -4;

			// Запуск
			if (RDLocale.CurrentLanguage != RDLanguages.en_us)
				RDLocale.CurrentLanguage = RDLanguages.en_us;    // Защита от других языков

			Application.Run (new RandomazeForm (args));
			return 0;
			}
		}
	}
