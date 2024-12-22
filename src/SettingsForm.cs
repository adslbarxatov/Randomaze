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
		// Переменные
		private ESRMSettings settings;

		// Переменные
		private List<Label> enemiesLabels = new List<Label> ();
		private List<TrackBar> enemiesTracks = new List<TrackBar> ();
		private List<string> enemiesNames = new List<string> ();
		private byte[] enemies;
		private List<bool> enemiesLocks = new List<bool> ();

		private List<Label> itemsLabels = new List<Label> ();
		private List<TrackBar> itemsTracks = new List<TrackBar> ();
		private List<string> itemsNames = new List<string> ();
		private byte[] items;
		private List<bool> itemsLocks = new List<bool> ();

		private Color enabledColor = Color.FromArgb (0, 200, 0);
		private Color disabledColor = Color.FromArgb (200, 200, 200);

		/// <summary>
		/// Конструктор. Запускает форму
		/// </summary>
		/// <param name="OldSettings">Параметры, полученные из файла настроек</param>
		public SettingsForm (ESRMSettings OldSettings)
			{
			// Инициализация и локализация формы
			InitializeComponent ();

			RDLocale.SetControlsText (this);
			AbortButton.Text = RDLocale.GetDefaultText (RDLDefaultTexts.Button_Cancel);
			ApplyButton.Text = RDLocale.GetDefaultText (RDLDefaultTexts.Button_OK);

			GenericTab.Text = RDLocale.GetControlText (this.Name, GenericTab.Name);
			RDLocale.SetControlsText (GenericTab);

			Generic2Tab.Text = RDLocale.GetControlText (this.Name, Generic2Tab.Name);
			RDLocale.SetControlsText (Generic2Tab);

			EnemiesTab.Text = RDLocale.GetControlText (this.Name, EnemiesTab.Name);
			RDLocale.SetControlsText (EnemiesTab);

			ItemsTab.Text = RDLocale.GetControlText (this.Name, ItemsTab.Name);
			RDLocale.SetControlsText (ItemsTab);

			MazeSizeFlag.Text = EnemiesDensityFlag.Text = ItemsDensityFlag.Text =
				CratesDensityFlag.Text = WallsDensityFlag.Text = InsideLightingFlag.Text =
				GravityFlag.Text = RandomizeFloorsFlag.Text = FogFlag.Text = WaterFlag.Text =
				OutsideLightingFlag.Text = RDLocale.GetText ("SettingsForm_Random");

			this.TopMost = true;
			this.Text = ProgramDescription.AssemblyTitle + ": " + RDLocale.GetText ("SettingsForm_T");
			this.CancelButton = AbortButton;
			this.AcceptButton = ApplyButton;

			// Разбор настроек
			settings = OldSettings;

			MazeSizeTrack.Maximum = (int)ESRMSettings.MaximumMazeSizeCoefficient;
			MazeSizeTrack.Value = (int)settings.MazeSizeCoefficient;
			MazeSizeFlag.Checked = MazeSizeFlag.Enabled = false;    // settings.RandomMazeSizeCoefficient;
			MazeSizeFlag_CheckedChanged (null, null);

			EnemiesDensityTrack.Maximum = (int)ESRMSettings.MaximumEnemiesDensityCoefficient;
			EnemiesDensityTrack.Value = (int)settings.EnemiesDensityCoefficient;
			EnemiesDensityFlag.Checked = settings.RandomEnemiesDensityCoefficient;
			EnemiesDensityFlag_CheckedChanged (null, null);

			ItemsDensityTrack.Maximum = (int)ESRMSettings.MaximumItemsDensityCoefficient;
			ItemsDensityTrack.Value = (int)settings.ItemsDensityCoefficient;
			ItemsDensityFlag.Checked = settings.RandomItemsDensityCoefficient;
			ItemsDensityFlag_CheckedChanged (null, null);

			WallsDensityTrack.Maximum = (int)ESRMSettings.MaximumWallsDensityCoefficient;
			WallsDensityTrack.Value = (int)settings.WallsDensityCoefficient;
			WallsDensityFlag.Checked = WallsDensityFlag.Enabled = false;
			WallsDensityFlag_CheckedChanged (null, null);

			InsideLightingTrack.Maximum = (int)ESRMSettings.MaximumInsideLightingCoefficient;
			InsideLightingTrack.Value = (int)settings.InsideLightingCoefficient;
			InsideLightingFlag.Checked = settings.RandomInsideLightingCoefficient;
			InsideLightingFlag_CheckedChanged (null, null);

			OutsideLightingTrack.Maximum = (int)ESRMSettings.MaximumOutsideLightingCoefficient;
			OutsideLightingTrack.Value = (int)settings.OutsideLightingCoefficient;
			OutsideLightingFlag.Checked = settings.RandomOutsideLightingCoefficient;
			OutsideLightingFlag_CheckedChanged (null, null);

			GravityTrack.Maximum = (int)ESRMSettings.MaximumGravityCoefficient;
			GravityTrack.Value = (int)settings.GravityCoefficient;
			GravityFlag.Checked = settings.RandomGravityCoefficient;
			GravityFlag_CheckedChanged (null, null);

			FogTrack.Maximum = (int)ESRMSettings.MaximumFogCoefficient;
			FogTrack.Value = (int)settings.FogCoefficient + 1;
			FogFlag.Checked = settings.RandomFogCoefficient;
			FogFlag_CheckedChanged (null, null);

			WaterTrack.Maximum = (int)ESRMSettings.MaximumWaterLevel;
			WaterTrack.Value = (int)settings.WaterLevel + 1;
			WaterFlag.Checked = settings.RandomWaterLevel;
			WaterFlag_CheckedChanged (null, null);

			MonsterMakerFlag.Checked = settings.AllowMonsterMakers;

			for (int i = 0; i < 5; i++)
				{
				string idx = "Enemy" + i.ToString ("D2");
				enemiesLabels.Add ((Label)this.Controls.Find (idx + "Label", true)[0]);

				enemiesTracks.Add ((TrackBar)this.Controls.Find (idx + "Track", true)[0]);
				enemiesTracks[i].Maximum = ESRMSettings.MaximumEnemiesProbability;
				}

			for (int i = 0; i < EnemiesSupport.AvailableEnemiesTypes; i++)
				{
				string idx = "Enemy" + i.ToString ("D2");
				enemiesNames.Add (RDLocale.GetText (idx));
				enemiesLocks.Add (true);
				}

			EnemyScroll.Maximum = (int)EnemiesSupport.AvailableEnemiesTypes - enemiesLabels.Count;

			enemies = settings.EnemiesPermissionLine;
			EnemyScroll_Scroll (null, null);

			for (int i = 0; i < 7; i++)
				{
				string idx = "Item" + i.ToString ("D2");
				itemsLabels.Add ((Label)this.Controls.Find (idx + "Label", true)[0]);

				itemsTracks.Add ((TrackBar)this.Controls.Find (idx + "Track", true)[0]);
				itemsTracks[i].Maximum = ESRMSettings.MaximumItemsProbability;
				}

			for (int i = 0; i < ItemsSupport.AvailableItemsTypes; i++)
				{
				string idx = "Item" + i.ToString ("D2");
				itemsNames.Add (RDLocale.GetText (idx));
				itemsLocks.Add (true);
				}

			ItemScroll.Maximum = (int)ItemsSupport.AvailableItemsTypes - itemsLabels.Count;

			items = settings.ItemsPermissionLine;
			ItemScroll_Scroll (null, null);
			WaterTrack_Scroll (null, null);

			for (int i = (int)MapSectionTypes.AllTypes; i <= (int)MapSectionTypes.OnlyInside; i++)
				SkyCombo.Items.Add (RDLocale.GetText ("GenericTab_SkyCombo" + i.ToString ("D2")));
			SkyCombo.SelectedIndex = (int)settings.SectionType - 1;

			for (int i = (int)MapBarriersTypes.OnlyGlass; i <= (int)MapBarriersTypes.Both; i++)
				BarrierCombo.Items.Add (RDLocale.GetText ("GenericTab_BarrierCombo" + i.ToString ("D2")));
			BarrierCombo.SelectedIndex = (int)settings.BarriersType - 1;

			for (int i = (int)MapButtonsTypes.NoButtons; i <= (int)MapButtonsTypes.MainAndAdditional; i++)
				ButtonCombo.Items.Add (RDLocale.GetText ("Generic2Tab_ButtonCombo" + i.ToString ("D2")));
			ButtonCombo.SelectedIndex = (int)settings.ButtonMode;

			AllowItemsForSecondFloor.Checked = settings.AllowItemsForSecondFloor;
			TwoFloorsFlag.Checked = settings.TwoFloors;
			RandomizeFloorsFlag.Checked = settings.RandomizeFloorsQuantity;
			TwoFloorsFlag_CheckedChanged (null, null);

			/*CratesDensityTrack.Minimum = 0;*/
			CratesDensityTrack.Maximum = (int)ESRMSettings.MaximumCratesDensityCoefficient2;
			CratesDensityTrack.Value = (int)settings.CratesDensityCoefficient2+1;
			CratesDensityFlag.Checked = settings.RandomCratesDensityCoefficient2;

			/*AllowExplosiveCratesFlag.Checked = settings.AllowExplosiveCrates;
			AllowItemsCratesFlag.Checked = settings.AllowItemsCrates;
			AllowExplosiveCratesFlag_CheckedChanged (null, null);*/
			CratesBalanceTrack.Minimum = -ESRMSettings.CratesBalanceRange;
			CratesBalanceTrack.Maximum = ESRMSettings.CratesBalanceRange;
			CratesBalanceTrack.Value = settings.CratesBalance;
			CratesDensityFlag_CheckedChanged (null, null);

			NodesFlag.Checked = settings.UseMapNodes;

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

			settings.InsideLightingCoefficient = (uint)InsideLightingTrack.Value;
			settings.RandomInsideLightingCoefficient = InsideLightingFlag.Checked;

			settings.OutsideLightingCoefficient = (uint)OutsideLightingTrack.Value;
			settings.RandomOutsideLightingCoefficient = OutsideLightingFlag.Checked;

			settings.GravityCoefficient = (uint)GravityTrack.Value;
			settings.RandomGravityCoefficient = GravityFlag.Checked;

			settings.FogCoefficient = (uint)FogTrack.Value - 1;
			settings.RandomFogCoefficient = FogFlag.Checked;

			settings.WaterLevel = (uint)WaterTrack.Value - 1;
			settings.RandomWaterLevel = WaterFlag.Checked;

			settings.AllowMonsterMakers = MonsterMakerFlag.Checked;
			settings.EnemiesPermissionLine = enemies;
			settings.ItemsPermissionLine = items;

			settings.SectionType = (MapSectionTypes)(SkyCombo.SelectedIndex + 1);
			settings.BarriersType = (MapBarriersTypes)(BarrierCombo.SelectedIndex + 1);
			settings.ButtonMode = (MapButtonsTypes)ButtonCombo.SelectedIndex;

			settings.TwoFloors = TwoFloorsFlag.Checked;
			settings.AllowItemsForSecondFloor = AllowItemsForSecondFloor.Checked;
			settings.RandomizeFloorsQuantity = RandomizeFloorsFlag.Checked;

			/*settings.AllowExplosiveCrates = AllowExplosiveCratesFlag.Checked;
			settings.AllowItemsCrates = AllowItemsCratesFlag.Checked;*/
			settings.CratesDensityCoefficient2 = (uint)CratesDensityTrack.Value - 1;
			settings.RandomCratesDensityCoefficient2 = CratesDensityFlag.Checked;
			settings.CratesBalance = CratesBalanceTrack.Value;

			settings.UseMapNodes = NodesFlag.Checked;

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
			CratesDensityTrack.Enabled = !CratesDensityFlag.Checked /*&& (AllowExplosiveCratesFlag.Checked ||
				AllowItemsCratesFlag.Checked)*/;
			CratesDensityTrack.BackColor = CratesDensityTrack.Enabled ? enabledColor : disabledColor;
			}

		private void InsideLightingFlag_CheckedChanged (object sender, EventArgs e)
			{
			InsideLightingTrack.Enabled = !InsideLightingFlag.Checked;
			InsideLightingTrack.BackColor = InsideLightingTrack.Enabled ? enabledColor : disabledColor;
			}

		private void OutsideLightingFlag_CheckedChanged (object sender, EventArgs e)
			{
			OutsideLightingTrack.Enabled = !OutsideLightingFlag.Checked;
			OutsideLightingTrack.BackColor = OutsideLightingTrack.Enabled ? enabledColor : disabledColor;
			}

		private void GravityFlag_CheckedChanged (object sender, EventArgs e)
			{
			GravityTrack.Enabled = ResetGravityButton.Enabled = !GravityFlag.Checked;
			GravityTrack.BackColor = GravityTrack.Enabled ? enabledColor : disabledColor;
			}

		private void FogFlag_CheckedChanged (object sender, EventArgs e)
			{
			FogTrack.Enabled = !FogFlag.Checked;
			FogTrack.BackColor = FogTrack.Enabled ? enabledColor : disabledColor;
			}

		private void WaterFlag_CheckedChanged (object sender, EventArgs e)
			{
			WaterTrack.Enabled = !WaterFlag.Checked;
			WaterTrack.BackColor = WaterTrack.Enabled ? enabledColor : disabledColor;
			}

		// Ограничение суммарного коэффициента размерности лабиринта и плотности стен
		private int sizeWallsDifferenceLimit = 5;
		private void MazeSizeTrack_Scroll (object sender, EventArgs e)
			{
			int coeff = (int)ESRMSettings.MaximumWallsDensityCoefficient - WallsDensityTrack.Value + MazeSizeTrack.Value;
			int limit = (int)(ESRMSettings.MaximumMazeSizeCoefficient + ESRMSettings.MaximumWallsDensityCoefficient);

			if (coeff < limit - sizeWallsDifferenceLimit)
				return;

			TrackBar tb = (TrackBar)sender;
			if (tb.Name == MazeSizeTrack.Name)
				WallsDensityTrack.Value = (int)ESRMSettings.MaximumWallsDensityCoefficient -
					(limit - sizeWallsDifferenceLimit - MazeSizeTrack.Value);
			else
				MazeSizeTrack.Value = limit - sizeWallsDifferenceLimit -
					((int)ESRMSettings.MaximumWallsDensityCoefficient - WallsDensityTrack.Value);
			}

		/*// Включение / выключение ящиков
		private void AllowExplosiveCratesFlag_CheckedChanged (object sender, EventArgs e)
			{
			Label05.Enabled = CratesDensityFlag.Enabled = (AllowExplosiveCratesFlag.Checked ||
				AllowItemsCratesFlag.Checked);
			CratesDensityFlag_CheckedChanged (null, null);
			}*/

		// Включение дополнительных монстров
		private void TwoFloorsFlag_CheckedChanged (object sender, EventArgs e)
			{
			// barnacle зависит от высоты карты
			if (TwoFloorsFlag.Checked || RandomizeFloorsFlag.Checked)
				{
				enemiesLocks[8] = AllowItemsForSecondFloor.Enabled = true;
				}
			else
				{
				enemiesLocks[8] = AllowItemsForSecondFloor.Enabled = AllowItemsForSecondFloor.Checked = false;
				enemies[8] = 0;
				}

			TwoFloorsFlag.Enabled = !RandomizeFloorsFlag.Checked;

			// Подгрузка новых значений
			EnemyScroll_Scroll (null, null);
			}

		// Сброс гравитации до нормального значения
		private void ResetGravityButton_Click (object sender, EventArgs e)
			{
			GravityTrack.Value = GravityTrack.Maximum / 2;
			}

		// Изменение уровня воды
		private void WaterTrack_Scroll (object sender, EventArgs e)
			{
			// leech зависит от уровня воды
			enemiesLocks[6] = false;
			if ((WaterTrack.Value > WaterTrack.Minimum) || WaterFlag.Checked)
				enemies[6] = (byte)(enemiesTracks[0].Maximum / 2);
			else
				enemies[6] = 0;

			// Подгрузка новых значений
			EnemyScroll_Scroll (null, null);
			}

		// Прокрутка списка врагов
		private void EnemyScroll_Scroll (object sender, ScrollEventArgs e)
			{
			scroll = true;

			for (int i = 0; i < enemiesLabels.Count; i++)
				{
				int v = i + EnemyScroll.Value;
				enemiesLabels[i].Text = enemiesNames[v];
				enemiesTracks[i].Value = enemies[v];
				enemiesTracks[i].Enabled = enemiesLocks[v];
				}

			scroll = false;
			}
		private bool scroll = false;

		// Изменение вероятности генерации врагов
		private void Enemy00Track_Scroll (object sender, EventArgs e)
			{
			if (scroll)
				return;

			int idx = enemiesTracks.IndexOf ((TrackBar)sender);
			enemies[EnemyScroll.Value + idx] = (byte)enemiesTracks[idx].Value;
			}

		// Прокрутка списка предметов
		private void ItemScroll_Scroll (object sender, ScrollEventArgs e)
			{
			scroll = true;

			for (int i = 0; i < itemsLabels.Count; i++)
				{
				int v = i + ItemScroll.Value;
				itemsLabels[i].Text = itemsNames[v];
				if (ItemsSupport.ItemsOnlyFromCrates.Contains (v))
					itemsLabels[i].Text += " *";

				itemsTracks[i].Value = items[v];
				itemsTracks[i].Enabled = itemsLocks[v];
				}

			scroll = false;
			}

		// Изменение вероятности генерации предметов
		private void Item00Track_Scroll (object sender, EventArgs e)
			{
			if (scroll)
				return;

			int idx = itemsTracks.IndexOf ((TrackBar)sender);

			// Аптечки и броня не могут быть удалены совсем
			if ((ItemScroll.Value + idx < 2) && (itemsTracks[idx].Value < 1))
				itemsTracks[idx].Value = 1;

			items[ItemScroll.Value + idx] = (byte)itemsTracks[idx].Value;
			}

		// Изменение значение плотности ящиков
		private void CratesDensityTrack_Scroll (object sender, EventArgs e)
			{
			CratesBalanceLabel.Enabled = CratesBalanceTrack.Enabled = (CratesDensityTrack.Value >
				CratesDensityTrack.Minimum);
			}
		}
	}
