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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Resources;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Summary description for OptionsDlg.
	/// </summary>
	internal class OptionsDlg : System.Windows.Forms.Form
	{
		internal EditSettings editSettings;
		private EditControl edit;
		private string currentPage = string.Empty;
		private System.Windows.Forms.TreeNode treeNodeGeneral;
		private System.Windows.Forms.TreeNode treeNodeFontsColors;
		private System.Windows.Forms.TreeNode treeNodeMisc;

		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;

		private ArrayList optionsArray = new ArrayList();
		private System.Windows.Forms.TreeView editSettingsView;
		private System.Windows.Forms.Label labelLine;
		internal OptionsGeneral pageGeneral;
		internal OptionsFontColor pageFontColor;
		internal OptionsMiscellaneous pageMiscellaneous;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.Panel panelDummy;
		private System.Windows.Forms.Button buttonApply;
		private System.ComponentModel.IContainer components;

		internal OptionsDlg(EditControl edit, EditSettings opt)
		{
			this.edit = edit;
			this.editSettings = opt;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			treeNodeGeneral = new System.Windows.Forms.TreeNode(edit.GetResourceString("DialogItemGeneral"));
			treeNodeFontsColors = new System.Windows.Forms.TreeNode(edit.GetResourceString("DialogItemFontsColors"));
			treeNodeMisc = new System.Windows.Forms.TreeNode(edit.GetResourceString("DialogItemMisc"));

			this.editSettingsView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] 
				{treeNodeGeneral, treeNodeFontsColors, treeNodeMisc});

			pageGeneral = new OptionsGeneral(edit);
			pageGeneral.Location = panelDummy.Location;
			pageGeneral.Size = panelDummy.Size;
			pageGeneral.Visible = false;
			pageFontColor = new OptionsFontColor(edit);
			pageFontColor.Location = panelDummy.Location;
			pageFontColor.Size = panelDummy.Size;
			pageFontColor.Visible = false;
			pageMiscellaneous = new OptionsMiscellaneous(edit);
			pageMiscellaneous.Location = panelDummy.Location;
			pageMiscellaneous.Size = panelDummy.Size;
			pageMiscellaneous.Visible = false;		

			pageGeneral.IndicatorMarginVisible = edit.IndicatorMarginVisible;
			pageGeneral.LineNumberMarginVisible = edit.LineNumberMarginVisible;
			pageGeneral.SelectionMarginVisible = edit.SelectionMarginVisible;
			pageGeneral.VScrollBarVisible = edit.VScrollBarVisible;
			pageGeneral.HScrollBarVisible = edit.HScrollBarVisible;
			pageGeneral.StatusBarVisible = edit.StatusBarVisible;
			pageGeneral.UserMarginVisible = edit.UserMarginVisible;
			pageGeneral.ContextMenuVisible = edit.ContextMenuVisible;
			pageGeneral.WhiteSpaceVisible = edit.WhiteSpaceVisible;
			pageGeneral.GridLinesVisible = edit.GridLinesVisible;
			pageGeneral.ReadOnly = edit.ReadOnly;

			pageGeneral.Invalidate();

			this.Controls.Add(pageGeneral);
			this.Controls.Add(pageFontColor);
			this.Controls.Add(pageMiscellaneous);
			SwitchPage(currentPage);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionsDlg));
			this.labelLine = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.editSettingsView = new System.Windows.Forms.TreeView();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.buttonOK = new System.Windows.Forms.Button();
			this.panelDummy = new System.Windows.Forms.Panel();
			this.buttonApply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelLine
			// 
			this.labelLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.labelLine.Location = new System.Drawing.Point(152, 304);
			this.labelLine.Name = "labelLine";
			this.labelLine.Size = new System.Drawing.Size(400, 2);
			this.labelLine.TabIndex = 1;
			this.labelLine.Text = "label1";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(388, 312);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			// 
			// editSettingsView
			// 
			this.editSettingsView.ImageList = this.imageList1;
			this.editSettingsView.Location = new System.Drawing.Point(5, 5);
			this.editSettingsView.Name = "editSettingsView";
			this.editSettingsView.ShowLines = false;
			this.editSettingsView.ShowPlusMinus = false;
			this.editSettingsView.ShowRootLines = false;
			this.editSettingsView.Size = new System.Drawing.Size(139, 299);
			this.editSettingsView.TabIndex = 0;
			this.editSettingsView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelect_Handler);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(300, 312);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.OnOK_Click);
			// 
			// panelDummy
			// 
			this.panelDummy.Location = new System.Drawing.Point(152, 8);
			this.panelDummy.Name = "panelDummy";
			this.panelDummy.Size = new System.Drawing.Size(400, 288);
			this.panelDummy.TabIndex = 1;
			// 
			// buttonApply
			// 
			this.buttonApply.Location = new System.Drawing.Point(476, 312);
			this.buttonApply.Name = "buttonApply";
			this.buttonApply.TabIndex = 4;
			this.buttonApply.Text = "Apply";
			this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
			// 
			// OptionsDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(562, 343);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonApply,
																		  this.panelDummy,
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.labelLine,
																		  this.editSettingsView});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDlg";
			this.Text = "Options";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.OptionsDlg_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Handles the selection of the tree items.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AfterSelect_Handler(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			SwitchPage(e.Node.Text);
		}

		private void SwitchPage(string strPage)
		{
			panelDummy.Visible = false;
			pageGeneral.Visible = false;
			pageFontColor.Visible = false;
			pageMiscellaneous.Visible = false;

			if (strPage == "General")
			{
				pageGeneral.Visible = true;
			}
			else if (strPage == "Fonts and Colors")
			{
				pageFontColor.Visible = true;
			}
			else if (strPage == "Misc")
			{
				pageMiscellaneous.Visible = true;
			}
			else
			{
				pageGeneral.Visible = true;
			}
			currentPage = strPage;
		}

		private void OnOK_Click(object sender, System.EventArgs e)
		{
			UpdateEdit();
		}

		private void buttonApply_Click(object sender, System.EventArgs e)
		{
			UpdateEdit();
		}

		private void UpdateEdit()
		{
			edit.Font = editSettings.GetFont(pageFontColor.comboBoxFont.SelectedItem.ToString(), 
				Int32.Parse(pageFontColor.comboBoxSize.SelectedItem.ToString()).ToString(), 
				pageFontColor.checkBoxBold.Checked ? "1" : "0");

			edit.IndicatorMarginVisible = pageGeneral.IndicatorMarginVisible;
			edit.LineNumberMarginVisible = pageGeneral.LineNumberMarginVisible;
			edit.SelectionMarginVisible = pageGeneral.SelectionMarginVisible;
			edit.VScrollBarVisible = pageGeneral.VScrollBarVisible;
			edit.HScrollBarVisible = pageGeneral.HScrollBarVisible;
			edit.StatusBarVisible = pageGeneral.StatusBarVisible;
			edit.UserMarginVisible = pageGeneral.UserMarginVisible;
			edit.ContextMenuVisible = pageGeneral.ContextMenuVisible;
			edit.WhiteSpaceVisible = pageGeneral.WhiteSpaceVisible;
			edit.GridLinesVisible = pageGeneral.GridLinesVisible;
			edit.ReadOnly = pageGeneral.ReadOnly;
			edit.UpdateCharWidth();
			edit.UpdateScrollBars();
			edit.UpdateAll();		
		}

		private void OptionsDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}

		/// <summary>
		/// Sets text for dialog items.
		/// </summary>
		internal void SetDialogItemText(EditControl edit)
		{
			this.Text = edit.GetResourceString("DialogItemOptions"); // "Options";
			treeNodeGeneral.Text = edit.GetResourceString("DialogItemGeneral"); // "General";
			treeNodeFontsColors.Text = edit.GetResourceString("DialogItemFontsColors"); // "Fonts and Colors";
			treeNodeMisc.Text = edit.GetResourceString("DialogItemMisc"); // "Misc";
			pageGeneral.SetDialogItemText(edit);
			pageFontColor.SetDialogItemText(edit);
			pageMiscellaneous.SetDialogItemText(edit);
		}
	}
}
