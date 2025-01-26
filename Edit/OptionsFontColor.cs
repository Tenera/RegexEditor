//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
//  Copyright Syncfusion Inc. 2001 - 2003. All rights reserved. Use of this code is subject to the terms of our 
//  license. A copy of the current license can be obtained at any time by e-mailing licensing@syncfusion.com. 
//  Re-distribution in any form is strictly prohibited. Any infringement will be prosecuted under applicable laws. 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
/*******************************************************************************
*                    Essential Edit - A syntax coloring edit                   *
*                                Author: B. Wu                                 *
********************************************************************************/

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Data;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The OptionsFontColor class provides a pane for the Options dialog.
	/// </summary>
	internal class OptionsFontColor : System.Windows.Forms.UserControl
	{
		private EditControl edit;
		internal System.Windows.Forms.ComboBox comboBoxFont;
		internal System.Windows.Forms.ComboBox comboBoxSize;
		internal System.Windows.Forms.ComboBox comboBoxForeColor;
		internal System.Windows.Forms.ComboBox comboBoxBackColor;
		private System.Windows.Forms.Button buttonForeColor;
		private System.Windows.Forms.Button buttonBackColor;
		internal System.Windows.Forms.CheckBox checkBoxBold;
		private System.Windows.Forms.Panel panelSample;
		private System.Windows.Forms.ListBox listBoxDisplayItems;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Label labelForegroundColor;
		private System.Windows.Forms.Label labelBackgroundColor;
		private System.Windows.Forms.Label labelSample;
		private System.Windows.Forms.Label labelFont;
		private System.Windows.Forms.Label labelDisplayItems;
		private System.Windows.Forms.Label labelSize;

		private static string [] DefaultColorNames = 
		{
			"Automatic",
			"Black",
			"White",
			"Maroon",
			"Dark green",
			"Olive",
			"Dark blue",
			"Purple",
			"Dark cyan",
			"Light gray",
			"Gray",
			"Red",
			"Green",
			"Yellow",
			"Blue",
			"Magenta",
			"Cyan",
			"Custom"
		};

		internal OptionsFontColor(EditControl edit)
		{
			this.edit = edit;
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			SetDialogItemText(edit);

			// TODO: Add any initialization after the InitForm call
			InstalledFontCollection fonts = new InstalledFontCollection();
			comboBoxFont.BeginUpdate();
			for (int i = 0; i < fonts.Families.Length; i++)
			{
				comboBoxFont.Items.Add(fonts.Families[i].Name);
			}
			comboBoxFont.SelectedItem = edit.Font.FontFamily.Name;
			comboBoxFont.EndUpdate();
			comboBoxSize.BeginUpdate();
			for (int i = 6; i <= 24; i++)
			{
				comboBoxSize.Items.Add(i.ToString());
			}
			comboBoxSize.SelectedItem = edit.Font.Size.ToString();
			comboBoxSize.EndUpdate();
			comboBoxForeColor.BeginUpdate();
			for (int i = 0; i < DefaultColorNames.Length; i++)
			{
				comboBoxForeColor.Items.Add(DefaultColorNames[i]);
			}
			comboBoxForeColor.SelectedItem = "Automatic";
			comboBoxForeColor.EndUpdate();
			comboBoxBackColor.BeginUpdate();
			for (int i = 0; i < DefaultColorNames.Length; i++)
			{
				comboBoxBackColor.Items.Add(DefaultColorNames[i]);
			}
			comboBoxBackColor.SelectedItem = "Automatic";
			comboBoxBackColor.EndUpdate();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelForegroundColor = new System.Windows.Forms.Label();
			this.labelBackgroundColor = new System.Windows.Forms.Label();
			this.labelSample = new System.Windows.Forms.Label();
			this.comboBoxForeColor = new System.Windows.Forms.ComboBox();
			this.labelFont = new System.Windows.Forms.Label();
			this.checkBoxBold = new System.Windows.Forms.CheckBox();
			this.labelDisplayItems = new System.Windows.Forms.Label();
			this.listBoxDisplayItems = new System.Windows.Forms.ListBox();
			this.comboBoxBackColor = new System.Windows.Forms.ComboBox();
			this.comboBoxFont = new System.Windows.Forms.ComboBox();
			this.labelSize = new System.Windows.Forms.Label();
			this.panelSample = new System.Windows.Forms.Panel();
			this.comboBoxSize = new System.Windows.Forms.ComboBox();
			this.buttonBackColor = new System.Windows.Forms.Button();
			this.buttonForeColor = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelForegroundColor
			// 
			this.labelForegroundColor.Location = new System.Drawing.Point(184, 72);
			this.labelForegroundColor.Name = "labelForegroundColor";
			this.labelForegroundColor.Size = new System.Drawing.Size(100, 14);
			this.labelForegroundColor.TabIndex = 7;
			this.labelForegroundColor.Text = "Foreground color:";
			// 
			// labelBackgroundColor
			// 
			this.labelBackgroundColor.Location = new System.Drawing.Point(184, 120);
			this.labelBackgroundColor.Name = "labelBackgroundColor";
			this.labelBackgroundColor.Size = new System.Drawing.Size(100, 14);
			this.labelBackgroundColor.TabIndex = 10;
			this.labelBackgroundColor.Text = "Background color:";
			// 
			// labelSample
			// 
			this.labelSample.Location = new System.Drawing.Point(8, 184);
			this.labelSample.Name = "labelSample";
			this.labelSample.Size = new System.Drawing.Size(100, 14);
			this.labelSample.TabIndex = 13;
			this.labelSample.Text = "Sample:";
			// 
			// comboBoxForeColor
			// 
			this.comboBoxForeColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.comboBoxForeColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxForeColor.DropDownWidth = 121;
			this.comboBoxForeColor.Location = new System.Drawing.Point(184, 88);
			this.comboBoxForeColor.Name = "comboBoxForeColor";
			this.comboBoxForeColor.Size = new System.Drawing.Size(121, 21);
			this.comboBoxForeColor.TabIndex = 8;
			this.comboBoxForeColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxForeColor_SelectedIndexChanged);
			this.comboBoxForeColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ColorListDrawItem);
			// 
			// labelFont
			// 
			this.labelFont.Location = new System.Drawing.Point(8, 0);
			this.labelFont.Name = "labelFont";
			this.labelFont.Size = new System.Drawing.Size(100, 14);
			this.labelFont.TabIndex = 0;
			this.labelFont.Text = "Font:";
			// 
			// checkBoxBold
			// 
			this.checkBoxBold.Location = new System.Drawing.Point(248, 16);
			this.checkBoxBold.Name = "checkBoxBold";
			this.checkBoxBold.Size = new System.Drawing.Size(52, 24);
			this.checkBoxBold.TabIndex = 2;
			this.checkBoxBold.Text = "Bold";
			this.checkBoxBold.CheckedChanged += new System.EventHandler(this.checkBoxBold_CheckedChanged);
			// 
			// labelDisplayItems
			// 
			this.labelDisplayItems.Location = new System.Drawing.Point(8, 48);
			this.labelDisplayItems.Name = "labelDisplayItems";
			this.labelDisplayItems.Size = new System.Drawing.Size(100, 14);
			this.labelDisplayItems.TabIndex = 5;
			this.labelDisplayItems.Text = "Display items:";
			// 
			// listBoxDisplayItems
			// 
			this.listBoxDisplayItems.Location = new System.Drawing.Point(8, 64);
			this.listBoxDisplayItems.Name = "listBoxDisplayItems";
			this.listBoxDisplayItems.Size = new System.Drawing.Size(168, 108);
			this.listBoxDisplayItems.TabIndex = 6;
			this.listBoxDisplayItems.SelectedIndexChanged += new System.EventHandler(this.listBoxDisplayItems_SelectedIndexChanged);
			// 
			// comboBoxBackColor
			// 
			this.comboBoxBackColor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.comboBoxBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxBackColor.DropDownWidth = 121;
			this.comboBoxBackColor.Location = new System.Drawing.Point(184, 136);
			this.comboBoxBackColor.Name = "comboBoxBackColor";
			this.comboBoxBackColor.Size = new System.Drawing.Size(121, 21);
			this.comboBoxBackColor.TabIndex = 11;
			this.comboBoxBackColor.SelectedIndexChanged += new System.EventHandler(this.comboBoxBackColor_SelectedIndexChanged);
			this.comboBoxBackColor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ColorListDrawItem);
			// 
			// comboBoxFont
			// 
			this.comboBoxFont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxFont.DropDownWidth = 228;
			this.comboBoxFont.Location = new System.Drawing.Point(8, 16);
			this.comboBoxFont.Name = "comboBoxFont";
			this.comboBoxFont.Size = new System.Drawing.Size(228, 21);
			this.comboBoxFont.TabIndex = 1;
			this.comboBoxFont.SelectedIndexChanged += new System.EventHandler(this.comboBoxFont_SelectedIndexChanged);
			// 
			// labelSize
			// 
			this.labelSize.Location = new System.Drawing.Point(312, 0);
			this.labelSize.Name = "labelSize";
			this.labelSize.Size = new System.Drawing.Size(48, 14);
			this.labelSize.TabIndex = 3;
			this.labelSize.Text = "Size:";
			// 
			// panelSample
			// 
			this.panelSample.Location = new System.Drawing.Point(8, 200);
			this.panelSample.Name = "panelSample";
			this.panelSample.Size = new System.Drawing.Size(384, 80);
			this.panelSample.TabIndex = 14;
			this.panelSample.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSample_Paint);
			// 
			// comboBoxSize
			// 
			this.comboBoxSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxSize.DropDownWidth = 80;
			this.comboBoxSize.Location = new System.Drawing.Point(312, 16);
			this.comboBoxSize.Name = "comboBoxSize";
			this.comboBoxSize.Size = new System.Drawing.Size(80, 21);
			this.comboBoxSize.TabIndex = 4;
			this.comboBoxSize.SelectedIndexChanged += new System.EventHandler(this.comboBoxSize_SelectedIndexChanged);
			// 
			// buttonBackColor
			// 
			this.buttonBackColor.Location = new System.Drawing.Point(312, 136);
			this.buttonBackColor.Name = "buttonBackColor";
			this.buttonBackColor.Size = new System.Drawing.Size(80, 23);
			this.buttonBackColor.TabIndex = 12;
			this.buttonBackColor.Text = "Custom...";
			this.buttonBackColor.Click += new System.EventHandler(this.buttonBackColor_Click);
			// 
			// buttonForeColor
			// 
			this.buttonForeColor.Location = new System.Drawing.Point(312, 88);
			this.buttonForeColor.Name = "buttonForeColor";
			this.buttonForeColor.Size = new System.Drawing.Size(80, 23);
			this.buttonForeColor.TabIndex = 9;
			this.buttonForeColor.Text = "Custom...";
			this.buttonForeColor.Click += new System.EventHandler(this.buttonForeColor_Click);
			// 
			// OptionsFontColor
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.panelSample,
																		  this.labelSample,
																		  this.checkBoxBold,
																		  this.buttonBackColor,
																		  this.buttonForeColor,
																		  this.comboBoxBackColor,
																		  this.labelBackgroundColor,
																		  this.comboBoxForeColor,
																		  this.labelForegroundColor,
																		  this.listBoxDisplayItems,
																		  this.labelDisplayItems,
																		  this.labelSize,
																		  this.comboBoxSize,
																		  this.labelFont,
																		  this.comboBoxFont});
			this.Name = "OptionsFontColor";
			this.Size = new System.Drawing.Size(400, 288);
			this.VisibleChanged += new System.EventHandler(this.OptionsFontColor_VisibleChanged);
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonForeColor_Click(object sender, System.EventArgs e)
		{
			CustomizeForeColor();
		}

		private void CustomizeForeColor()
		{
			ColorDialog colorDlg = new ColorDialog();
			colorDlg.FullOpen = true;
			if (colorDlg.ShowDialog() != DialogResult.Cancel)
			{
				comboBoxForeColor.SelectedItem = "Custom";
				EditColorGroup cg =  edit.Settings.ColorGroupList.
					GetColorGroup(listBoxDisplayItems.Text);
				cg.ForeColor = colorDlg.Color;
				cg.IsAutoForeColor = false;
			}
		}

		private void buttonBackColor_Click(object sender, System.EventArgs e)
		{
			CustomizeBackColor();
		}

		private void CustomizeBackColor()
		{
			ColorDialog colorDlg = new ColorDialog();
			colorDlg.FullOpen = true;
			if (colorDlg.ShowDialog() != DialogResult.Cancel)
			{
				comboBoxBackColor.SelectedItem = "Custom";
				EditColorGroup cg =  edit.Settings.ColorGroupList.
					GetColorGroup(listBoxDisplayItems.Text);
				cg.BackColor = colorDlg.Color;
				cg.IsAutoBackColor = false;
			}
		}

		private void panelSample_Paint(object sender, 
			System.Windows.Forms.PaintEventArgs e)
		{
			string colorName = comboBoxForeColor.Text.Replace(" ", "");
			panelSample.ForeColor = GetItemForeColor(colorName);
			colorName = comboBoxBackColor.Text.Replace(" ", "");
			panelSample.BackColor = GetItemBackColor(colorName);
			if (comboBoxFont.Created && comboBoxSize.Created 
				&& checkBoxBold.Created)
			{
				panelSample.Font = edit.Settings.GetFont(comboBoxFont.Text,
					comboBoxSize.Text, checkBoxBold.Checked ? "1" : "0");
				e.Graphics.DrawRectangle(new Pen(ForeColor, 1), 
					new Rectangle(panelSample.ClientRectangle.Top, 
					panelSample.ClientRectangle.Left, 
					panelSample.ClientRectangle.Width - 1, 
					panelSample.ClientRectangle.Height - 1));
				SizeF stringSize = e.Graphics.MeasureString("AaBbCcXxYyZz", 
					panelSample.Font);
				e.Graphics.DrawString("AaBbCcXxYyZz", panelSample.Font, 
					new SolidBrush(panelSample.ForeColor), 
					panelSample.ClientRectangle.Left +
					(panelSample.ClientRectangle.Width - stringSize.Width)/2, 
					panelSample.ClientRectangle.Top +
					(panelSample.ClientRectangle.Height - stringSize.Height)/2);
			}
		}

		private void SetItemForeColor(EditColorGroup cg, string colorName)
		{
			if (colorName == "Automatic")
			{
				if (cg.GroupName == "Text")
				{
					cg.ForeColor = SystemColors.WindowText;
				}
				else if (cg.GroupName == "Selected Text")
				{
					cg.ForeColor = SystemColors.HighlightText;
				}
				else if (cg.GroupName == "Inactive Selected Text")
				{
					cg.ForeColor = SystemColors.InactiveCaptionText;
				}
				else
				{
					cg.ForeColor = GetItemForeColor("Text");
				}
				cg.IsAutoForeColor = true;
			}
			else
			{
				cg.ForeColor = Color.FromName(colorName);
				cg.IsAutoForeColor = false;
			}

			//set the settings appropriately
			int count = edit.Settings.ColorGroupInfoList.Count;
			int cgLoc = -1;
			for(int i = 0; i < count; i ++)
			{
				if(((EditGroupInfo)edit.Settings.ColorGroupInfoList[i]).GroupName == cg.GroupName)
				{
					cgLoc = i;
					break;
				}
			}
			if(cgLoc < 0)
			{
				EditGroupInfo gInfo = new EditGroupInfo();
				gInfo.BackColorAutomatic = cg.IsAutoBackColor?"0":"1";
				gInfo.Background = cg.BackColor.R.ToString() + "," + cg.BackColor.G.ToString() + "," + cg.BackColor.B.ToString();
				gInfo.ForeColorAutomatic = cg.IsAutoForeColor?"0":"1";
				gInfo.Foreground = cg.ForeColor.R.ToString() + "," + cg.ForeColor.G.ToString() + "," + cg.ForeColor.B.ToString();
				gInfo.GroupName = cg.GroupName;
				gInfo.GroupType = cg.GroupType.ToString();

				edit.Settings.ColorGroupInfoList.Add(gInfo);
			}
			else
			{
				((EditGroupInfo)edit.Settings.ColorGroupInfoList[cgLoc]).Foreground = cg.ForeColor.R.ToString() + "," + cg.ForeColor.G.ToString() + "," + cg.ForeColor.B.ToString();
			}
		}

		private Color GetItemForeColor(string colorName)
		{
			if (colorName == "Automatic")
			{
				if (listBoxDisplayItems.Text == "Text")
				{
					return SystemColors.WindowText;
				}
				else if (listBoxDisplayItems.Text == "Selected Text")
				{
					return SystemColors.HighlightText;
				}
				else if (listBoxDisplayItems.Text == "Inactive Selected Text")
				{
					return SystemColors.InactiveCaptionText;
				}
				else
				{
					return edit.Settings.GetColorGroupForeColor("Text");
				}
			}
			else if (colorName == "Custom")
			{
				return edit.Settings.GetColorGroupForeColor(listBoxDisplayItems.Text);
			}
			else
			{
				return Color.FromName(colorName);
			}
		}

		private void SetItemBackColor(EditColorGroup cg, string colorName)
		{
			if (colorName == "Automatic")
			{
				if (cg.GroupName == "Text")
				{
					cg.BackColor = SystemColors.Window;
				}
				else if (cg.GroupName == "Selected Text")
				{
					cg.BackColor = SystemColors.Highlight;
				}
				else if (cg.GroupName == "Inactive Selected Text")
				{
					cg.BackColor = SystemColors.InactiveCaption;
				}
				else
				{
					cg.BackColor = GetItemBackColor("Text");
				}
				cg.IsAutoBackColor = true;
			}
			else
			{
				cg.BackColor = Color.FromName(colorName);
				cg.IsAutoBackColor = false;
			}
			//set the settings appropriately
			int count = edit.Settings.ColorGroupInfoList.Count;
			int cgLoc = -1;
			for(int i = 0; i < count; i ++)
			{
				if(((EditGroupInfo)edit.Settings.ColorGroupInfoList[i]).GroupName == cg.GroupName)
				{
					cgLoc = i;
					break;
				}
			}
			if(cgLoc < 0)
			{
				EditGroupInfo gInfo = new EditGroupInfo();
				gInfo.BackColorAutomatic = cg.IsAutoBackColor?"0":"1";
				gInfo.Background = cg.BackColor.R.ToString() + "," + cg.BackColor.G.ToString() + "," + cg.BackColor.B.ToString();
				gInfo.ForeColorAutomatic = cg.IsAutoForeColor?"0":"1";
				gInfo.Foreground = cg.ForeColor.R.ToString() + "," + cg.ForeColor.G.ToString() + "," + cg.ForeColor.B.ToString();
				gInfo.GroupName = cg.GroupName;
				gInfo.GroupType = cg.GroupType.ToString();

				edit.Settings.ColorGroupInfoList.Add(gInfo);
			}
			else
			{
				((EditGroupInfo)edit.Settings.ColorGroupInfoList[cgLoc]).Background = cg.BackColor.R.ToString() + "," + cg.BackColor.G.ToString() + "," + cg.BackColor.B.ToString();
			}

		}

		private Color GetItemBackColor(string colorName)
		{
			if (colorName == "Automatic")
			{
				if (listBoxDisplayItems.Text == "Text")
				{
					return SystemColors.Window;
				}
				else if (listBoxDisplayItems.Text == "Selected Text")
				{
					return SystemColors.Highlight;
				}
				else if (listBoxDisplayItems.Text == "Inactive Selected Text")
				{
					return SystemColors.InactiveCaption;
				}
				else
				{
					return edit.Settings.GetColorGroupBackColor("Text");
				}
			}
			else if (colorName == "Custom")
			{
				return edit.Settings.GetColorGroupBackColor(listBoxDisplayItems.Text);
			}
			else
			{
				return Color.FromName(colorName);
			}
		}
		
		private void comboBoxFont_SelectedIndexChanged(object sender, 
			System.EventArgs e)
		{
			if (panelSample.Created)
			{
				panelSample.Invalidate();
			}
		}

		private void comboBoxSize_SelectedIndexChanged(object sender, 
			System.EventArgs e)
		{
			if (panelSample.Created)
			{
				panelSample.Invalidate();
			}
		}

		private void comboBoxForeColor_SelectedIndexChanged(object sender, 
			System.EventArgs e)
		{
			if (panelSample.Created)
			{
				panelSample.Invalidate();
			}
			if (listBoxDisplayItems.Created)
			{
				EditColorGroup cg =  edit.Settings.ColorGroupList.
					GetColorGroup(listBoxDisplayItems.Text);
				string colorName = comboBoxForeColor.SelectedItem.
					ToString().Replace(" ", "");
				if (colorName == "Custom")
				{
//					CustomizeForeColor();
				}
				else
				{
					SetItemForeColor(cg, colorName);
				}
			}
		}

		private void comboBoxBackColor_SelectedIndexChanged(object sender, 
			System.EventArgs e)
		{
			if (panelSample.Created)
			{
				panelSample.Invalidate();
			}
			if (listBoxDisplayItems.Created)
			{
				EditColorGroup cg = edit.Settings.ColorGroupList.
					GetColorGroup(listBoxDisplayItems.Text);
				string colorName = comboBoxBackColor.SelectedItem.
					ToString().Replace(" ", "");
				if (colorName == "Custom")
				{
//					CustomizeBackColor();
				}
				else
				{
					SetItemBackColor(cg, colorName);
				}
			}
		}

		private void checkBoxBold_CheckedChanged(object sender, 
			System.EventArgs e)
		{
			if (panelSample.Created)
			{
				panelSample.Invalidate();
			}
		}

		private void ColorListDrawItem(object sender, DrawItemEventArgs e)
		{
			ComboBox comboBoxSelected = (ComboBox)sender;
			e.DrawBackground();
			Rectangle colorRect = new Rectangle(e.Bounds.Location.X + 1, 
				e.Bounds.Location.Y + 1, 
				e.Bounds.Height - 3, e.Bounds.Height - 3);
			string colorName = comboBoxSelected.Items[e.Index].
				ToString().Replace(" ", "");
			Color boxColor;
			if(comboBoxSelected == comboBoxForeColor)
			{
				boxColor = GetItemForeColor(colorName);
			}
			else
			{
				boxColor = GetItemBackColor(colorName);
			}
			e.Graphics.FillRectangle(new SolidBrush(boxColor), colorRect);
			e.Graphics.DrawRectangle(new Pen(Color.Black, 1), colorRect);
			e.Graphics.DrawString(comboBoxSelected.Items[e.Index].ToString(), 
				e.Font, new SolidBrush(e.ForeColor), e.Bounds.Location.X 
				+ colorRect.Width + colorRect.Width/2, e.Bounds.Location.Y);
			e.DrawFocusRectangle();
		}

		private void listBoxDisplayItems_SelectedIndexChanged(object sender, 
			System.EventArgs e)
		{
			EditColorGroup cg = edit.Settings.ColorGroupList.
				GetColorGroup(listBoxDisplayItems.Text);
			string colorName;
			if (cg.IsAutoForeColor)
			{
				comboBoxForeColor.Text = "Automatic";
			}
			else
			{
				bool bMatch = false;
				for (int i = 0; i < comboBoxForeColor.Items.Count; i++)
				{
					colorName = comboBoxForeColor.Items[i].ToString().Replace(" ", "");
					if (cg.ForeColor.ToArgb() == Color.FromName(colorName).ToArgb())
					{
						comboBoxForeColor.SelectedIndex = i;
						bMatch = true;
						break;
					}
				}
				if (!bMatch)
				{
					comboBoxForeColor.Text = "Custom";
				}
			}
			if (cg.IsAutoBackColor)
			{
				comboBoxBackColor.Text = "Automatic";
			}
			else
			{
				bool bMatch = false;
				for (int i = 0; i < comboBoxBackColor.Items.Count; i++)
				{
					colorName = comboBoxBackColor.Items[i].ToString().Replace(" ", "");
					if (cg.BackColor.ToArgb() == Color.FromName(colorName).ToArgb())
					{
						comboBoxBackColor.SelectedIndex = i;
						bMatch = true;
						break;
					}
				}
				if (!bMatch)
				{
					comboBoxBackColor.Text = "Custom";
				}
			}
			comboBoxForeColor.Invalidate();
			comboBoxBackColor.Invalidate();
			panelSample.Invalidate();
		}

		private void OptionsFontColor_VisibleChanged(object sender, System.EventArgs e)
		{
			if (this.Visible)
			{
				listBoxDisplayItems.BeginUpdate();
				listBoxDisplayItems.Items.Clear();
				for (int i = 0; i < edit.Settings.ColorGroupList.Count; i++)
				{
					listBoxDisplayItems.Items.
						Add(edit.Settings.ColorGroupList[i].GroupName);
				}
				listBoxDisplayItems.SelectedIndex = 0;
				listBoxDisplayItems.EndUpdate();
			}
		}

		/// <summary>
		/// Sets text for dialog items.
		/// </summary>
		internal void SetDialogItemText(EditControl edit)
		{
			labelForegroundColor.Text = edit.GetResourceString("DialogItemForegroundColor"); // "Foreground color:"
			labelBackgroundColor.Text = edit.GetResourceString("DialogItemBackgroundColor"); // "Background color:"
			labelSample.Text = edit.GetResourceString("DialogItemSample"); // "Sample:"
			labelFont.Text = edit.GetResourceString("DialogItemFont"); // "Font:"
			labelDisplayItems.Text = edit.GetResourceString("DialogItemDisplayItems"); // "Display items:"
			labelSize.Text = edit.GetResourceString("DialogItemSize"); // "Size:"
			buttonForeColor.Text = edit.GetResourceString("DialogItemCustom"); // "Custom..."
			buttonBackColor.Text = edit.GetResourceString("DialogItemCustom"); // "Custom..."
			checkBoxBold.Text = edit.GetResourceString("DialogItemBold"); // "Bold"
		}
	}
}
