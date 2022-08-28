using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RD_AAOW
	{
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
		private SupportedLanguages al;

		/// <summary>
		/// Конструктор. Запускает форму
		/// </summary>
		/// <param name="OldSettings">Параметры, полученные из файла настроек</param>
		/// <param name="InterfaceLanguage">Язык интерфейса</param>
		public SettingsForm (ESRMSettings OldSettings, SupportedLanguages InterfaceLanguage)
			{
			// Инициализация и локализация формы
			InitializeComponent ();

			al = InterfaceLanguage;
			Localization.SetControlsText (this, al);
			Localization.SetControlsText (EnemiesContainer, al);
			MazeSizeFlag.Text = EnemiesDensityFlag.Text = ItemsDensityFlag.Text =
				CratesDensityFlag.Text = WallsDensityFlag.Text = LightingFlag.Text =
				Localization.GetText ("SettingsForm_Random", al);

			this.TopMost = true;
			this.Text = ProgramDescription.AssemblyTitle + ": " + Localization.GetText ("SettingsForm_T", al);
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

			LightingTrack.Maximum = (int)settings.MaximumLightingCoefficient;
			LightingTrack.Value = (int)settings.LightingCoefficient;
			LightingFlag.Checked = settings.RandomLightingCoefficient;
			LightingFlag_CheckedChanged (null, null);

			ButtonModeFlag.Checked = settings.ButtonMode;

			for (int i = 0; i < MapSupport.EnemiesPermissionsKeys.Length; i++)
				enemiesFlags.Insert (0, (CheckBox)EnemiesContainer.Controls[i]);

			for (int i = 0; i < MapSupport.EnemiesPermissionsKeys.Length; i++)
				enemiesFlags[i].Checked = settings.EnemiesPermissionLine.Contains (MapSupport.EnemiesPermissionsKeys[i]);

			switch (settings.SectionType)
				{
				default:
				case MapSectionTypes.AllTypes:
					AllTypesRadio.Checked = true;
					break;

				case MapSectionTypes.OnlyUnderSky:
					OnlySkyRadio.Checked = true;
					break;

				case MapSectionTypes.OnlyInside:
					OnlyInsideRadio.Checked = true;
					break;
				}

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

			settings.LightingCoefficient = (uint)LightingTrack.Value;
			settings.RandomLightingCoefficient = LightingFlag.Checked;

			settings.ButtonMode = ButtonModeFlag.Checked;

			settings.EnemiesPermissionLine = "";
			for (int i = 0; i < MapSupport.EnemiesPermissionsKeys.Length; i++)
				{
				if (enemiesFlags[i].Checked)
					settings.EnemiesPermissionLine += MapSupport.EnemiesPermissionsKeys[i];
				else
					settings.EnemiesPermissionLine += "-";
				}

			if (OnlyInsideRadio.Checked)
				settings.SectionType = MapSectionTypes.OnlyInside;
			else if (OnlySkyRadio.Checked)
				settings.SectionType = MapSectionTypes.OnlyUnderSky;
			else
				settings.SectionType = MapSectionTypes.AllTypes;

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

		private void LightingFlag_CheckedChanged (object sender, EventArgs e)
			{
			LightingTrack.Enabled = !LightingFlag.Checked;
			LightingTrack.BackColor = LightingTrack.Enabled ? enabledColor : disabledColor;
			}

		// Ограничение суммарного коэффициента размерности лабиринта и плотности стен
		private int sizeWallsDifferenceLimit = 6;
		private void MazeSizeTrack_Scroll (object sender, EventArgs e)
			{
			int coeff = (int)settings.MaximumWallsDensityCoefficient - WallsDensityTrack.Value + MazeSizeTrack.Value;
			int limit = (int)(settings.MaximumMazeSizeCoefficient + settings.MaximumWallsDensityCoefficient);

			if (coeff < limit - sizeWallsDifferenceLimit)
				return;

			TrackBar tb = (TrackBar)sender;
			if (tb.Name == MazeSizeTrack.Name)
				WallsDensityTrack.Value = (int)settings.MaximumWallsDensityCoefficient -
					(limit - sizeWallsDifferenceLimit - MazeSizeTrack.Value);
			else
				MazeSizeTrack.Value = limit - sizeWallsDifferenceLimit -
					((int)settings.MaximumWallsDensityCoefficient - WallsDensityTrack.Value);
			}
		}
	}
