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

			// Проверка запуска единственной копии
			if (!RDGenerics.IsAppInstanceUnique (false))
				return -3;

			// Язык интерфейса и контроль XPUN
			if (!Localization.IsXPUNClassAcceptable)
				return -1;

			// Отображение справки и запроса на принятие Политики
			if (!RDGenerics.AcceptEULA ())
				return -2;
			RDGenerics.ShowAbout (true);

			// Запуск
			if (Localization.CurrentLanguage != SupportedLanguages.en_us)
				Localization.CurrentLanguage = SupportedLanguages.en_us;    // Защита от других языков

#if DBG
			string ar = "";
			for (int i = 0; i < args.Length; i++)
				ar += args[i] + " ";

			if (!string.IsNullOrWhiteSpace (ar))
				if (RDGenerics.MessageBox (RDMessageTypes.Information_Center, ar, ">", "X") !=
					RDMessageButtons.ButtonOne)
					return 1;
#endif

			Application.Run (new RandomazeForm (args));
			return 0;
			}
		}
	}
