using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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
		}

	/// <summary>
	/// Класс описывает форму выбора параметров приложения
	/// </summary>
	public partial class SettingsForm:Form
		{
		/// <summary>
		/// Возвращает изменённые настройки приложения
		/// </summary>
		public ESRMSettings Settings
			{
			get
				{
				return settings;
				}
			}
		private ESRMSettings settings;

		// Переменные
		private List<CheckBox> enemiesFlags = new List<CheckBox> ();
		private Color enabledColor = Color.FromArgb (0, 200, 0),
			disabledColor = Color.FromArgb (200, 200, 200);

		/// <summary>
		/// Конструктор. Запускает форму
		/// </summary>
		/// <param name="OldSettings">Параметры, полученные из файла настроек</param>
		public SettingsForm (ESRMSettings OldSettings)
			{
			// Инициализация и локализация формы
			InitializeComponent ();

			this.TopMost = true;
			this.Text = ProgramDescription.AssemblyTitle + ": settings";
			this.CancelButton = AbortButton;
			this.AcceptButton = ApplyButton;

			// Разбор настроек
			settings = OldSettings;

			MazeSizeTrack.Maximum = (int)settings.MaximumMazeSizeCoefficient;
			MazeSizeTrack.Value = (int)settings.MazeSizeCoefficient;
			MazeSizeFlag.Checked = settings.RandomMazeSizeCoefficient;
			MazeSizeFlag_CheckedChanged (null, null);

			EnemiesDensityTrack.Maximum = (int)settings.MaximumEnemiesDensityCoefficient;
			EnemiesDensityTrack.Value = (int)settings.EnemiesDensityCoefficient;
			EnemiesDensityFlag.Checked = settings.RandomEnemiesDensityCoefficient;
			EnemiesDensityFlag_CheckedChanged (null, null);

			ItemsDensityTrack.Maximum = (int)settings.MaximumItemsDensityCoefficient;
			ItemsDensityTrack.Value = (int)settings.ItemsDensityCoefficient;
			ItemsDensityFlag.Checked = settings.RandomItemsDensityCoefficient;
			ItemsDensityFlag_CheckedChanged (null, null);

			WallsDensityTrack.Maximum = (int)settings.MaximumWallsDensityCoefficient;
			WallsDensityTrack.Value = (int)settings.WallsDensityCoefficient;
			WallsDensityFlag.Checked = settings.RandomWallsDensityCoefficient;
			WallsDensityFlag_CheckedChanged (null, null);

			CratesDensityTrack.Maximum = (int)settings.MaximumCratesDensityCoefficient;
			CratesDensityTrack.Value = (int)settings.CratesDensityCoefficient;
			CratesDensityFlag.Checked = settings.RandomCratesDensityCoefficient;
			CratesDensityFlag_CheckedChanged (null, null);

			ButtonModeFlag.Checked = settings.ButtonMode;

			for (int i = 0; i < MapSupport.EnemiesPermissionsKeys.Length; i++)
				enemiesFlags.Insert (0, (CheckBox)EnemiesContainer.Controls[i]);

			for (int i = 0; i < MapSupport.EnemiesPermissionsKeys.Length; i++)
				enemiesFlags[i].Checked = settings.EnemiesPermissionLine.Contains (MapSupport.EnemiesPermissionsKeys[i]);

			// Запуск
			this.ShowDialog ();
			}

		// Отмена изменений
		private void BAbort_Click (object sender, EventArgs e)
			{
			this.Close ();
			}

		/// <summary>
		/// Возвращает флаг, сообщающий об отмене операции
		/// </summary>
		public bool Cancelled
			{
			get
				{
				return cancelled;
				}
			}
		private bool cancelled = true;

		// Применение настроек
		private void ApplyButton_Click (object sender, EventArgs e)
			{
			// Сохранение
			settings.MazeSizeCoefficient = (uint)MazeSizeTrack.Value;
			settings.RandomMazeSizeCoefficient = MazeSizeFlag.Checked;

			settings.EnemiesDensityCoefficient = (uint)EnemiesDensityTrack.Value;
			settings.RandomEnemiesDensityCoefficient = EnemiesDensityFlag.Checked;

			settings.ItemsDensityCoefficient = (uint)ItemsDensityTrack.Value;
			settings.RandomItemsDensityCoefficient = ItemsDensityFlag.Checked;

			settings.WallsDensityCoefficient = (uint)WallsDensityTrack.Value;
			settings.RandomWallsDensityCoefficient = WallsDensityFlag.Checked;

			settings.CratesDensityCoefficient = (uint)CratesDensityTrack.Value;
			settings.RandomCratesDensityCoefficient = CratesDensityFlag.Checked;

			settings.ButtonMode = ButtonModeFlag.Checked;

			settings.EnemiesPermissionLine = "";
			for (int i = 0; i < MapSupport.EnemiesPermissionsKeys.Length; i++)
				{
				if (enemiesFlags[i].Checked)
					settings.EnemiesPermissionLine += MapSupport.EnemiesPermissionsKeys[i];
				else
					settings.EnemiesPermissionLine += "-";
				}

			// Выход
			cancelled = false;
			this.Close ();
			}

		// Переключение состояний
		private void MazeSizeFlag_CheckedChanged (object sender, EventArgs e)
			{
			MazeSizeTrack.Enabled = !MazeSizeFlag.Checked;
			MazeSizeTrack.BackColor = MazeSizeTrack.Enabled ? enabledColor : disabledColor;
			}

		private void EnemiesDensityFlag_CheckedChanged (object sender, EventArgs e)
			{
			EnemiesDensityTrack.Enabled = !EnemiesDensityFlag.Checked;
			EnemiesDensityTrack.BackColor = EnemiesDensityTrack.Enabled ? enabledColor : disabledColor;
			}

		private void ItemsDensityFlag_CheckedChanged (object sender, EventArgs e)
			{
			ItemsDensityTrack.Enabled = !ItemsDensityFlag.Checked;
			ItemsDensityTrack.BackColor = ItemsDensityTrack.Enabled ? enabledColor : disabledColor;
			}

		private void WallsDensityFlag_CheckedChanged (object sender, EventArgs e)
			{
			WallsDensityTrack.Enabled = !WallsDensityFlag.Checked;
			WallsDensityTrack.BackColor = WallsDensityTrack.Enabled ? enabledColor : disabledColor;
			}

		private void CratesDensityFlag_CheckedChanged (object sender, EventArgs e)
			{
			CratesDensityTrack.Enabled = !CratesDensityFlag.Checked;
			CratesDensityTrack.BackColor = CratesDensityTrack.Enabled ? enabledColor : disabledColor;
			}
		}
	}
