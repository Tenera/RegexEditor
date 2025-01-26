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
using System.Data;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Summary description for OptionsGeneral.
	/// </summary>
	internal class OptionsGeneral : System.Windows.Forms.UserControl
	{
		private EditControl edit;
		private System.Windows.Forms.CheckBox checkBoxReadOnly;
		private System.Windows.Forms.CheckBox checkBoxGridLines;
		private System.Windows.Forms.CheckBox checkBoxStatusBar;
		private System.Windows.Forms.CheckBox checkBoxWhiteSpace;
		private System.Windows.Forms.CheckBox checkBoxContextMenu;
		private System.Windows.Forms.CheckBox checkBoxUserMargin;
		private System.Windows.Forms.CheckBox checkBoxLineNumbers;
		private System.Windows.Forms.CheckBox checkBoxHorizontalScrollBar;
		private System.Windows.Forms.CheckBox checkBoxVerticalScrollBar;
		private System.Windows.Forms.CheckBox checkBoxIndicatorMargin;
		private System.Windows.Forms.CheckBox checkBoxSelectionMargin;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal OptionsGeneral(EditControl edit)
		{
			this.edit = edit;

			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			SetDialogItemText(edit);
			// TODO: Add any initialization after the InitForm call
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
			this.checkBoxReadOnly = new System.Windows.Forms.CheckBox();
			this.checkBoxGridLines = new System.Windows.Forms.CheckBox();
			this.checkBoxStatusBar = new System.Windows.Forms.CheckBox();
			this.checkBoxWhiteSpace = new System.Windows.Forms.CheckBox();
			this.checkBoxContextMenu = new System.Windows.Forms.CheckBox();
			this.checkBoxUserMargin = new System.Windows.Forms.CheckBox();
			this.checkBoxLineNumbers = new System.Windows.Forms.CheckBox();
			this.checkBoxHorizontalScrollBar = new System.Windows.Forms.CheckBox();
			this.checkBoxVerticalScrollBar = new System.Windows.Forms.CheckBox();
			this.checkBoxIndicatorMargin = new System.Windows.Forms.CheckBox();
			this.checkBoxSelectionMargin = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkBoxReadOnly
			// 
			this.checkBoxReadOnly.Location = new System.Drawing.Point(16, 244);
			this.checkBoxReadOnly.Name = "checkBoxReadOnly";
			this.checkBoxReadOnly.Size = new System.Drawing.Size(164, 18);
			this.checkBoxReadOnly.TabIndex = 10;
			this.checkBoxReadOnly.Text = "Read Only";
			// 
			// checkBoxGridLines
			// 
			this.checkBoxGridLines.Location = new System.Drawing.Point(16, 220);
			this.checkBoxGridLines.Name = "checkBoxGridLines";
			this.checkBoxGridLines.Size = new System.Drawing.Size(164, 18);
			this.checkBoxGridLines.TabIndex = 9;
			this.checkBoxGridLines.Text = "Grid Lines";
			// 
			// checkBoxStatusBar
			// 
			this.checkBoxStatusBar.Location = new System.Drawing.Point(16, 148);
			this.checkBoxStatusBar.Name = "checkBoxStatusBar";
			this.checkBoxStatusBar.Size = new System.Drawing.Size(164, 18);
			this.checkBoxStatusBar.TabIndex = 6;
			this.checkBoxStatusBar.Text = "Status Bar";
			// 
			// checkBoxWhiteSpace
			// 
			this.checkBoxWhiteSpace.Location = new System.Drawing.Point(16, 196);
			this.checkBoxWhiteSpace.Name = "checkBoxWhiteSpace";
			this.checkBoxWhiteSpace.Size = new System.Drawing.Size(164, 18);
			this.checkBoxWhiteSpace.TabIndex = 8;
			this.checkBoxWhiteSpace.Text = "White Space";
			// 
			// checkBoxContextMenu
			// 
			this.checkBoxContextMenu.Location = new System.Drawing.Point(16, 172);
			this.checkBoxContextMenu.Name = "checkBoxContextMenu";
			this.checkBoxContextMenu.Size = new System.Drawing.Size(164, 18);
			this.checkBoxContextMenu.TabIndex = 7;
			this.checkBoxContextMenu.Text = "Context Menu";
			// 
			// checkBoxUserMargin
			// 
			this.checkBoxUserMargin.Location = new System.Drawing.Point(16, 76);
			this.checkBoxUserMargin.Name = "checkBoxUserMargin";
			this.checkBoxUserMargin.Size = new System.Drawing.Size(164, 18);
			this.checkBoxUserMargin.TabIndex = 3;
			this.checkBoxUserMargin.Text = "User Margin";
			// 
			// checkBoxLineNumbers
			// 
			this.checkBoxLineNumbers.Location = new System.Drawing.Point(16, 28);
			this.checkBoxLineNumbers.Name = "checkBoxLineNumbers";
			this.checkBoxLineNumbers.Size = new System.Drawing.Size(164, 18);
			this.checkBoxLineNumbers.TabIndex = 1;
			this.checkBoxLineNumbers.Text = "Line Number Margin";
			// 
			// checkBoxHorizontalScrollBar
			// 
			this.checkBoxHorizontalScrollBar.Location = new System.Drawing.Point(16, 124);
			this.checkBoxHorizontalScrollBar.Name = "checkBoxHorizontalScrollBar";
			this.checkBoxHorizontalScrollBar.Size = new System.Drawing.Size(164, 18);
			this.checkBoxHorizontalScrollBar.TabIndex = 5;
			this.checkBoxHorizontalScrollBar.Text = "Horizontal ScrollBar";
			// 
			// checkBoxVerticalScrollBar
			// 
			this.checkBoxVerticalScrollBar.Location = new System.Drawing.Point(16, 100);
			this.checkBoxVerticalScrollBar.Name = "checkBoxVerticalScrollBar";
			this.checkBoxVerticalScrollBar.Size = new System.Drawing.Size(164, 18);
			this.checkBoxVerticalScrollBar.TabIndex = 4;
			this.checkBoxVerticalScrollBar.Text = "Vertical ScrollBar";
			// 
			// checkBoxIndicatorMargin
			// 
			this.checkBoxIndicatorMargin.Location = new System.Drawing.Point(16, 4);
			this.checkBoxIndicatorMargin.Name = "checkBoxIndicatorMargin";
			this.checkBoxIndicatorMargin.Size = new System.Drawing.Size(164, 18);
			this.checkBoxIndicatorMargin.TabIndex = 0;
			this.checkBoxIndicatorMargin.Text = "Indicator Margin";
			// 
			// checkBoxSelectionMargin
			// 
			this.checkBoxSelectionMargin.Location = new System.Drawing.Point(16, 52);
			this.checkBoxSelectionMargin.Name = "checkBoxSelectionMargin";
			this.checkBoxSelectionMargin.Size = new System.Drawing.Size(164, 18);
			this.checkBoxSelectionMargin.TabIndex = 2;
			this.checkBoxSelectionMargin.Text = "Selection Margin";
			// 
			// OptionsGeneral
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBoxReadOnly,
																		  this.checkBoxGridLines,
																		  this.checkBoxStatusBar,
																		  this.checkBoxWhiteSpace,
																		  this.checkBoxContextMenu,
																		  this.checkBoxUserMargin,
																		  this.checkBoxLineNumbers,
																		  this.checkBoxHorizontalScrollBar,
																		  this.checkBoxVerticalScrollBar,
																		  this.checkBoxIndicatorMargin,
																		  this.checkBoxSelectionMargin});
			this.Name = "OptionsGeneral";
			this.Size = new System.Drawing.Size(400, 288);
			this.ResumeLayout(false);

		}
		#endregion

		internal bool IndicatorMarginVisible
		{
			get
			{
				return checkBoxIndicatorMargin.Checked;
			}
			set
			{
				checkBoxIndicatorMargin.Checked = value;
			}
		}

		internal bool LineNumberMarginVisible
		{
			get
			{
				return checkBoxLineNumbers.Checked;
			}
			set
			{
				checkBoxLineNumbers.Checked = value;
			}
		}

		internal bool SelectionMarginVisible
		{
			get
			{
				return checkBoxSelectionMargin.Checked;
			}
			set
			{
				checkBoxSelectionMargin.Checked = value;
			}
		}

		internal bool VScrollBarVisible
		{
			get
			{
				return checkBoxVerticalScrollBar.Checked;
			}
			set
			{
				checkBoxVerticalScrollBar.Checked = value;
			}
		}
	
		internal bool HScrollBarVisible
		{
			get
			{
				return checkBoxHorizontalScrollBar.Checked;
			}
			set
			{
				checkBoxHorizontalScrollBar.Checked = value;
			}
		}

		internal bool UserMarginVisible
		{
			get
			{
				return checkBoxUserMargin.Checked;
			}
			set
			{
				checkBoxUserMargin.Checked = value;
			}
		}

		internal bool StatusBarVisible
		{
			get
			{
				return checkBoxStatusBar.Checked;
			}
			set
			{
				checkBoxStatusBar.Checked = value;
			}
		}

		internal bool ContextMenuVisible
		{
			get
			{
				return checkBoxContextMenu.Checked;
			}
			set
			{
				checkBoxContextMenu.Checked = value;
			}
		}

		internal bool WhiteSpaceVisible
		{
			get
			{
				return checkBoxWhiteSpace.Checked;
			}
			set
			{
				checkBoxWhiteSpace.Checked = value;
			}
		}

		internal bool GridLinesVisible
		{
			get
			{
				return checkBoxGridLines.Checked;
			}
			set
			{
				checkBoxGridLines.Checked = value;
			}
		}

		internal bool ReadOnly
		{
			get
			{
				return checkBoxReadOnly.Checked;
			}
			set
			{
				checkBoxReadOnly.Checked = value;
			}
		}

		/// <summary>
		/// Sets text for dialog items.
		/// </summary>
		internal void SetDialogItemText(EditControl edit)
		{
			checkBoxReadOnly.Text = edit.GetResourceString("DialogItemReadOnly"); // "Read Only"
			checkBoxGridLines.Text = edit.GetResourceString("DialogItemGridLines"); // "Grid Lines"
			checkBoxStatusBar.Text = edit.GetResourceString("DialogItemStatusBar"); // "Status Bar"
			checkBoxWhiteSpace.Text = edit.GetResourceString("DialogItemWhiteSpace"); // "White Space"
			checkBoxContextMenu.Text = edit.GetResourceString("DialogItemContextMenu"); // "Context Menu"
			checkBoxUserMargin.Text = edit.GetResourceString("DialogItemUserMargin"); // "User Margin"
			checkBoxLineNumbers.Text = edit.GetResourceString("DialogItemLineNumbers"); // "Line Number Margin"
			checkBoxHorizontalScrollBar.Text = edit.GetResourceString("DialogItemHorizontalScrollBar"); // "Horizontal ScrollBar"
			checkBoxVerticalScrollBar.Text = edit.GetResourceString("DialogItemVerticalScrollBar"); // "Vertical ScrollBar"
			checkBoxIndicatorMargin.Text = edit.GetResourceString("DialogItemIndicatorMargin"); // "Indicator Margin"
			checkBoxSelectionMargin.Text = edit.GetResourceString("DialogItemSelectionMargin"); // "Selection Margin"
		}
	}
}
