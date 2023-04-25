using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RD_AAOW
	{
	/// <summary>
	/// Класс описывает форму выбора параметров приложения
	/// </summary>
	public partial class SettingsForm: Form
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
		private List<CheckBox> itemsFlags = new List<CheckBox> ();
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

			Localization.SetControlsText (this);
			AbortButton.Text = Localization.GetDefaultText (LzDefaultTextValues.Button_Cancel);
			ApplyButton.Text = Localization.GetDefaultText (LzDefaultTextValues.Button_OK);

			GenericTab.Text = Localization.GetControlText (this.Name, GenericTab.Name);
			Localization.SetControlsText (GenericTab);

			EnemiesTab.Text = Localization.GetControlText (this.Name, EnemiesTab.Name);
			Localization.SetControlsText (EnemiesTab);

			ItemsTab.Text = Localization.GetControlText (this.Name, ItemsTab.Name);
			Localization.SetControlsText (ItemsTab);

			MazeSizeFlag.Text = EnemiesDensityFlag.Text = ItemsDensityFlag.Text =
				CratesDensityFlag.Text = WallsDensityFlag.Text = LightingFlag.Text =
				GravityFlag.Text = RandomizeFloorsFlag.Text = Localization.GetText ("SettingsForm_Random");

			this.TopMost = true;
			this.Text = ProgramDescription.AssemblyTitle + ": " + Localization.GetText ("SettingsForm_T");
			this.CancelButton = AbortButton;
			this.AcceptButton = ApplyButton;

			// Разбор настроек
			settings = OldSettings;

			MazeSizeTrack.Maximum = (int)settings.MaximumMazeSizeCoefficient;
			MazeSizeTrack.Value = (int)settings.MazeSizeCoefficient;
			MazeSizeFlag.Checked = MazeSizeFlag.Enabled = false;    // settings.RandomMazeSizeCoefficient;
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
			WallsDensityFlag.Checked = WallsDensityFlag.Enabled = false;    // settings.RandomWallsDensityCoefficient;
			WallsDensityFlag_CheckedChanged (null, null);

			LightingTrack.Maximum = (int)settings.MaximumLightingCoefficient;
			LightingTrack.Value = (int)settings.LightingCoefficient;
			LightingFlag.Checked = settings.RandomLightingCoefficient;
			LightingFlag_CheckedChanged (null, null);

			GravityTrack.Maximum = (int)settings.MaximumGravityCoefficient;
			GravityTrack.Value = (int)settings.GravityCoefficient;
			GravityFlag.Checked = settings.RandomGravityCoefficient;
			GravityFlag_CheckedChanged (null, null);

			ButtonModeFlag.Checked = settings.ButtonMode;
			MonsterMakerFlag.Checked = settings.MonsterMakers;

			for (int i = 0; i < EnemiesSupport.EnemiesPermissionsKeys.Length; i++)
				enemiesFlags.Add ((CheckBox)this.Controls.Find ("EnemyFlag" + (i + 1).ToString ("D2"), true)[0]);

			for (int i = 0; i < EnemiesSupport.EnemiesPermissionsKeys.Length; i++)
				enemiesFlags[i].Checked =
					settings.EnemiesPermissionLine.Contains (EnemiesSupport.EnemiesPermissionsKeys[i]);

			for (int i = 0; i < ItemsSupport.ItemsPermissionsKeys.Length; i++)
				itemsFlags.Add ((CheckBox)this.Controls.Find ("ItemFlag" + (i + 1).ToString ("D2"), true)[0]);

			for (int i = 0; i < ItemsSupport.ItemsPermissionsKeys.Length; i++)
				{
				string key = ItemsSupport.ItemsPermissionsKeys[i];
				itemsFlags[i].Checked = settings.ItemsPermissionLine.Contains (key);

				if ((key == "k") || (key == "r") || (key == "o"))
					itemsFlags[i].Enabled = false;
				if (key == "k")
					itemsFlags[i].Checked = true;
				}
			EnemyFlag05_CheckedChanged (null, null);

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

			AllowItemsForSecondFloor.Checked = settings.AllowItemsForSecondFloor;
			TwoFloorsFlag.Checked = settings.TwoFloors;
			RandomizeFloorsFlag.Checked = settings.RandomizeFloorsQuantity;
			TwoFloorsFlag_CheckedChanged (null, null);

			CratesDensityTrack.Maximum = (int)settings.MaximumCratesDensityCoefficient;
			CratesDensityTrack.Value = (int)settings.CratesDensityCoefficient;
			CratesDensityFlag.Checked = settings.RandomCratesDensityCoefficient;

			AllowExplosiveCratesFlag.Checked = settings.AllowExplosiveCrates;
			AllowItemsCratesFlag.Checked = settings.AllowItemsCrates;
			AllowExplosiveCratesFlag_CheckedChanged (null, null);

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

			settings.LightingCoefficient = (uint)LightingTrack.Value;
			settings.RandomLightingCoefficient = LightingFlag.Checked;

			settings.GravityCoefficient = (uint)GravityTrack.Value;
			settings.RandomGravityCoefficient = GravityFlag.Checked;

			settings.ButtonMode = ButtonModeFlag.Checked;
			settings.MonsterMakers = MonsterMakerFlag.Checked;

			settings.EnemiesPermissionLine = "";
			for (int i = 0; i < EnemiesSupport.EnemiesPermissionsKeys.Length; i++)
				{
				if (enemiesFlags[i].Checked)
					settings.EnemiesPermissionLine += EnemiesSupport.EnemiesPermissionsKeys[i];
				else
					settings.EnemiesPermissionLine += "-";
				}

			settings.ItemsPermissionLine = "";
			for (int i = 0; i < ItemsSupport.ItemsPermissionsKeys.Length; i++)
				{
				if (itemsFlags[i].Checked)
					settings.ItemsPermissionLine += ItemsSupport.ItemsPermissionsKeys[i];
				else
					settings.ItemsPermissionLine += "-";
				}

			if (OnlyInsideRadio.Checked)
				settings.SectionType = MapSectionTypes.OnlyInside;
			else if (OnlySkyRadio.Checked)
				settings.SectionType = MapSectionTypes.OnlyUnderSky;
			else
				settings.SectionType = MapSectionTypes.AllTypes;

			settings.TwoFloors = TwoFloorsFlag.Checked;
			settings.AllowItemsForSecondFloor = AllowItemsForSecondFloor.Checked;
			settings.RandomizeFloorsQuantity = RandomizeFloorsFlag.Checked;

			settings.AllowExplosiveCrates = AllowExplosiveCratesFlag.Checked;
			settings.AllowItemsCrates = AllowItemsCratesFlag.Checked;
			settings.CratesDensityCoefficient = (uint)CratesDensityTrack.Value;
			settings.RandomCratesDensityCoefficient = CratesDensityFlag.Checked;

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
			CratesDensityTrack.Enabled = !CratesDensityFlag.Checked && (AllowExplosiveCratesFlag.Checked ||
				AllowItemsCratesFlag.Checked);
			CratesDensityTrack.BackColor = CratesDensityTrack.Enabled ? enabledColor : disabledColor;
			}

		private void LightingFlag_CheckedChanged (object sender, EventArgs e)
			{
			LightingTrack.Enabled = !LightingFlag.Checked;
			LightingTrack.BackColor = LightingTrack.Enabled ? enabledColor : disabledColor;
			}

		private void GravityFlag_CheckedChanged (object sender, EventArgs e)
			{
			GravityTrack.Enabled = ResetGravityButton.Enabled = !GravityFlag.Checked;
			GravityTrack.BackColor = GravityTrack.Enabled ? enabledColor : disabledColor;
			}

		// Ограничение суммарного коэффициента размерности лабиринта и плотности стен
		private int sizeWallsDifferenceLimit = 5;
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

		// Включение / выключение ящиков
		private void AllowExplosiveCratesFlag_CheckedChanged (object sender, EventArgs e)
			{
			Label05.Enabled = CratesDensityFlag.Enabled = (AllowExplosiveCratesFlag.Checked ||
				AllowItemsCratesFlag.Checked);
			CratesDensityFlag_CheckedChanged (null, null);
			}

		// Включение дополнительных монстров
		private void TwoFloorsFlag_CheckedChanged (object sender, EventArgs e)
			{
			if (TwoFloorsFlag.Checked && !RandomizeFloorsFlag.Checked)
				EnemyFlag08.Enabled = AllowItemsForSecondFloor.Enabled = true;
			else
				EnemyFlag08.Enabled = EnemyFlag08.Checked = AllowItemsForSecondFloor.Enabled =
					AllowItemsForSecondFloor.Checked = false;

			TwoFloorsFlag.Enabled = !RandomizeFloorsFlag.Checked;
			}

		// Сброс гравитации до нормального значения
		private void ResetGravityButton_Click (object sender, EventArgs e)
			{
			GravityTrack.Value = GravityTrack.Maximum / 2;
			}

		// Контроль оружия, относящегося к солдатам
		private void EnemyFlag05_CheckedChanged (object sender, EventArgs e)
			{
			ItemFlag11.Checked = ItemFlag12.Checked = EnemyFlag05.Checked;
			}
		}
	}
