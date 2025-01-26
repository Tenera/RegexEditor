//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
//  Copyright Syncfusion Inc. 2001 - 2003. All rights reserved. Use of this code is subject to the terms of our 
//  license. A copy of the current license can be obtained at any time by e-mailing licensing@syncfusion.com. 
//  Re-distribution in any form is strictly prohibited. Any infringement will be prosecuted under applicable laws. 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
/*******************************************************************************
*                    Essential Edit - A syntax coloring edit                   *
*                                Author: B. Wu                                 *
********************************************************************************/

using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Summary description for OptionsMiscellaneous.
	/// </summary>
	internal class OptionsMiscellaneous : System.Windows.Forms.UserControl
	{
		EditControl edit;
		private System.Windows.Forms.RadioButton radioButtonIndentingNone;
		private System.Windows.Forms.RadioButton radioButtonIndentingBlock;
		private System.Windows.Forms.RadioButton radioButtonIndentingSmart;
		private System.Windows.Forms.TextBox textBoxTabSize;
		private System.Windows.Forms.RadioButton radioButtonKeepTabs;
		private System.Windows.Forms.RadioButton radioButtonInsertSpaces;
		private System.Windows.Forms.TextBox textBoxIndentSize;
		private System.Windows.Forms.GroupBox groupBoxIndenting;
		private System.Windows.Forms.GroupBox groupBoxTabs;
		private System.Windows.Forms.Label labelIndentSize;
		private System.Windows.Forms.Label labelTabSize;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Imported method to beep.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool MessageBeep(int uType);

		internal OptionsMiscellaneous(EditControl edit)
		{
			this.edit = edit;

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			SetDialogItemText(edit);

			textBoxTabSize.Text = edit.TabSize.ToString();
			textBoxIndentSize.Text = edit.IndentSize.ToString();

			switch(edit.IndentType)
			{
				case EditIndentType.None:
					radioButtonIndentingNone.Checked = true;
					break;
				case EditIndentType.Block:
					radioButtonIndentingBlock.Checked = true;
					break;
				case EditIndentType.Smart:
					radioButtonIndentingSmart.Checked = true;
					break;
				default:
					break;
			}

			if (edit.KeepTabs)
			{
				radioButtonKeepTabs.Checked = true;
			}
			else 
			{
				radioButtonInsertSpaces.Checked = true;
			}
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
			this.radioButtonIndentingBlock = new System.Windows.Forms.RadioButton();
			this.radioButtonInsertSpaces = new System.Windows.Forms.RadioButton();
			this.textBoxTabSize = new System.Windows.Forms.TextBox();
			this.radioButtonKeepTabs = new System.Windows.Forms.RadioButton();
			this.radioButtonIndentingNone = new System.Windows.Forms.RadioButton();
			this.groupBoxIndenting = new System.Windows.Forms.GroupBox();
			this.radioButtonIndentingSmart = new System.Windows.Forms.RadioButton();
			this.groupBoxTabs = new System.Windows.Forms.GroupBox();
			this.textBoxIndentSize = new System.Windows.Forms.TextBox();
			this.labelIndentSize = new System.Windows.Forms.Label();
			this.labelTabSize = new System.Windows.Forms.Label();
			this.groupBoxIndenting.SuspendLayout();
			this.groupBoxTabs.SuspendLayout();
			this.SuspendLayout();
			// 
			// radioButtonIndentingBlock
			// 
			this.radioButtonIndentingBlock.Location = new System.Drawing.Point(12, 44);
			this.radioButtonIndentingBlock.Name = "radioButtonIndentingBlock";
			this.radioButtonIndentingBlock.Size = new System.Drawing.Size(68, 18);
			this.radioButtonIndentingBlock.TabIndex = 9;
			this.radioButtonIndentingBlock.Text = "Block";
			this.radioButtonIndentingBlock.CheckedChanged += new System.EventHandler(this.radioButtonIndentingBlock_CheckedChanged);
			// 
			// radioButtonInsertSpaces
			// 
			this.radioButtonInsertSpaces.Location = new System.Drawing.Point(12, 72);
			this.radioButtonInsertSpaces.Name = "radioButtonInsertSpaces";
			this.radioButtonInsertSpaces.Size = new System.Drawing.Size(92, 18);
			this.radioButtonInsertSpaces.TabIndex = 5;
			this.radioButtonInsertSpaces.Text = "Insert spaces";
			this.radioButtonInsertSpaces.CheckedChanged += new System.EventHandler(this.radioButtonInsertSpaces_CheckedChanged);
			// 
			// textBoxTabSize
			// 
			this.textBoxTabSize.Location = new System.Drawing.Point(76, 20);
			this.textBoxTabSize.Name = "textBoxTabSize";
			this.textBoxTabSize.Size = new System.Drawing.Size(28, 20);
			this.textBoxTabSize.TabIndex = 2;
			this.textBoxTabSize.Text = "";
			this.textBoxTabSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTabSize_KeyPress);
			this.textBoxTabSize.TextChanged += new System.EventHandler(this.textBoxTabSize_TextChanged);
			// 
			// radioButtonKeepTabs
			// 
			this.radioButtonKeepTabs.Location = new System.Drawing.Point(12, 96);
			this.radioButtonKeepTabs.Name = "radioButtonKeepTabs";
			this.radioButtonKeepTabs.Size = new System.Drawing.Size(92, 18);
			this.radioButtonKeepTabs.TabIndex = 6;
			this.radioButtonKeepTabs.Text = "Keep tabs";
			this.radioButtonKeepTabs.CheckedChanged += new System.EventHandler(this.radioButtonKeepTabs_CheckedChanged);
			// 
			// radioButtonIndentingNone
			// 
			this.radioButtonIndentingNone.Location = new System.Drawing.Point(12, 20);
			this.radioButtonIndentingNone.Name = "radioButtonIndentingNone";
			this.radioButtonIndentingNone.Size = new System.Drawing.Size(68, 18);
			this.radioButtonIndentingNone.TabIndex = 8;
			this.radioButtonIndentingNone.Text = "None";
			this.radioButtonIndentingNone.CheckedChanged += new System.EventHandler(this.radioButtonIndentingNone_CheckedChanged);
			// 
			// groupBoxIndenting
			// 
			this.groupBoxIndenting.Controls.AddRange(new System.Windows.Forms.Control[] {
																							this.radioButtonIndentingSmart,
																							this.radioButtonIndentingBlock,
																							this.radioButtonIndentingNone});
			this.groupBoxIndenting.Location = new System.Drawing.Point(8, 136);
			this.groupBoxIndenting.Name = "groupBoxIndenting";
			this.groupBoxIndenting.Size = new System.Drawing.Size(116, 92);
			this.groupBoxIndenting.TabIndex = 7;
			this.groupBoxIndenting.TabStop = false;
			this.groupBoxIndenting.Text = "Indenting";
			// 
			// radioButtonIndentingSmart
			// 
			this.radioButtonIndentingSmart.Location = new System.Drawing.Point(12, 68);
			this.radioButtonIndentingSmart.Name = "radioButtonIndentingSmart";
			this.radioButtonIndentingSmart.Size = new System.Drawing.Size(68, 18);
			this.radioButtonIndentingSmart.TabIndex = 10;
			this.radioButtonIndentingSmart.Text = "Smart";
			this.radioButtonIndentingSmart.CheckedChanged += new System.EventHandler(this.radioButtonIndentingSmart_CheckedChanged);
			// 
			// groupBoxTabs
			// 
			this.groupBoxTabs.Controls.AddRange(new System.Windows.Forms.Control[] {
																					   this.radioButtonKeepTabs,
																					   this.radioButtonInsertSpaces,
																					   this.textBoxIndentSize,
																					   this.labelIndentSize,
																					   this.textBoxTabSize,
																					   this.labelTabSize});
			this.groupBoxTabs.Location = new System.Drawing.Point(8, 0);
			this.groupBoxTabs.Name = "groupBoxTabs";
			this.groupBoxTabs.Size = new System.Drawing.Size(116, 124);
			this.groupBoxTabs.TabIndex = 0;
			this.groupBoxTabs.TabStop = false;
			this.groupBoxTabs.Text = "Tabs";
			// 
			// textBoxIndentSize
			// 
			this.textBoxIndentSize.Location = new System.Drawing.Point(76, 44);
			this.textBoxIndentSize.Name = "textBoxIndentSize";
			this.textBoxIndentSize.Size = new System.Drawing.Size(28, 20);
			this.textBoxIndentSize.TabIndex = 4;
			this.textBoxIndentSize.Text = "";
			this.textBoxIndentSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIndentSize_KeyPress);
			this.textBoxIndentSize.TextChanged += new System.EventHandler(this.textBoxIndentSize_TextChanged);
			// 
			// labelIndentSize
			// 
			this.labelIndentSize.Location = new System.Drawing.Point(8, 44);
			this.labelIndentSize.Name = "labelIndentSize";
			this.labelIndentSize.Size = new System.Drawing.Size(64, 14);
			this.labelIndentSize.TabIndex = 3;
			this.labelIndentSize.Text = "Indent size:";
			// 
			// labelTabSize
			// 
			this.labelTabSize.Location = new System.Drawing.Point(8, 20);
			this.labelTabSize.Name = "labelTabSize";
			this.labelTabSize.Size = new System.Drawing.Size(64, 14);
			this.labelTabSize.TabIndex = 1;
			this.labelTabSize.Text = "Tab size:";
			// 
			// OptionsMiscellaneous
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.groupBoxTabs,
																		  this.groupBoxIndenting});
			this.Name = "OptionsMiscellaneous";
			this.Size = new System.Drawing.Size(400, 288);
			this.groupBoxIndenting.ResumeLayout(false);
			this.groupBoxTabs.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void textBoxTabSize_TextChanged(object sender, System.EventArgs e)
		{
			if (this.textBoxTabSize.Text != string.Empty)
			{
				edit.TabSize = Int32.Parse(textBoxTabSize.Text);
			}
		}

		private void textBoxIndentSize_TextChanged(object sender, System.EventArgs e)
		{
			if (this.textBoxIndentSize.Text != string.Empty)
			{
				edit.IndentSize = Int32.Parse(textBoxIndentSize.Text);
			}
		}

		private void radioButtonIndentingNone_CheckedChanged(object sender, System.EventArgs e)
		{
			edit.IndentType = EditIndentType.None;
		}

		private void radioButtonIndentingBlock_CheckedChanged(object sender, System.EventArgs e)
		{
			edit.IndentType = EditIndentType.Block;
		}

		private void radioButtonIndentingSmart_CheckedChanged(object sender, System.EventArgs e)
		{
			edit.IndentType = EditIndentType.Smart;
		}

		private void radioButtonInsertSpaces_CheckedChanged(object sender, System.EventArgs e)
		{
			edit.KeepTabs = false;
		}

		private void radioButtonKeepTabs_CheckedChanged(object sender, System.EventArgs e)
		{
			edit.KeepTabs = true;
		}

		private void textBoxTabSize_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (((e.KeyChar < '0') || (e.KeyChar > '9')) 
				&& (e.KeyChar != '\b'))
			{
				MessageBeep(-1);
				e.Handled = true;
			}
		}

		private void textBoxIndentSize_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (((e.KeyChar < '0') || (e.KeyChar > '9')) 
				&& (e.KeyChar != '\b'))
			{
				MessageBeep(-1);
				e.Handled = true;
			}
		}

		/// <summary>
		/// Sets text for dialog items.
		/// </summary>
		internal void SetDialogItemText(EditControl edit)
		{
			radioButtonIndentingNone.Text = edit.GetResourceString("DialogItemNone"); // "None"
			radioButtonIndentingBlock.Text = edit.GetResourceString("DialogItemBlock"); // "Block"
			radioButtonIndentingSmart.Text = edit.GetResourceString("DialogItemSmart"); // "Smart"
			radioButtonKeepTabs.Text = edit.GetResourceString("DialogItemKeepTabs"); // "Keep tabs"
			radioButtonInsertSpaces.Text = edit.GetResourceString("DialogItemInsertSpaces"); // "Insert spaces"
			groupBoxIndenting.Text = edit.GetResourceString("DialogItemIndenting"); // "Indenting"
			groupBoxTabs.Text = edit.GetResourceString("DialogItemTabs"); // "Tabs"
			labelIndentSize.Text = edit.GetResourceString("DialogItemIndentSize"); // "Indent size"
			labelTabSize.Text = edit.GetResourceString("DialogItemTabSize"); // "Tab size"
		}
	}
}
