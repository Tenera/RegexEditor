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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Displays a dialog for the user to input a line number.
	/// </summary>
	internal class GoToDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelLineNumber;
		private System.Windows.Forms.TextBox textBoxLineNumber;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Imported native method to beep.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool MessageBeep(int uType);

		internal GoToDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.AcceptButton = buttonOK;

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelLineNumber = new System.Windows.Forms.Label();
			this.textBoxLineNumber = new System.Windows.Forms.TextBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(56, 72);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(72, 23);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "OK";
			// 
			// labelLineNumber
			// 
			this.labelLineNumber.Location = new System.Drawing.Point(16, 16);
			this.labelLineNumber.Name = "labelLineNumber";
			this.labelLineNumber.Size = new System.Drawing.Size(192, 16);
			this.labelLineNumber.TabIndex = 0;
			this.labelLineNumber.Text = "Line number :";
			// 
			// textBoxLineNumber
			// 
			this.textBoxLineNumber.Location = new System.Drawing.Point(16, 40);
			this.textBoxLineNumber.Name = "textBoxLineNumber";
			this.textBoxLineNumber.Size = new System.Drawing.Size(200, 20);
			this.textBoxLineNumber.TabIndex = 1;
			this.textBoxLineNumber.Text = "textBox1";
			this.textBoxLineNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxLineNumber_KeyPress);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(144, 72);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(72, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			// 
			// GoToDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(234, 106);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonCancel,
																		  this.buttonOK,
																		  this.textBoxLineNumber,
																		  this.labelLineNumber});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GoToDlg";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Go To Line";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Handles the KeyPress event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyPressEventArgs that contains the event data.
		/// </param>
		private void textBoxLineNumber_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)27)
			{
				this.Close();
			}
			else if (((e.KeyChar < '0') || (e.KeyChar > '9')) && (e.KeyChar != '\b'))
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
			this.Text = edit.GetResourceString("DialogItemGoToLine"); // "Go To Line";
			labelLineNumber.Text = edit.GetResourceString("DialogItemLineNumber"); // "Line number :";
			buttonOK.Text = edit.GetResourceString("DialogItemOK"); // "OK";
			buttonCancel.Text = edit.GetResourceString("DialogItemCancel"); // "Cancel";
		}

		/// <summary>
		/// The line number in the textbox field.
		/// </summary>
		internal int LineNumber
		{
			get
			{
				if (textBoxLineNumber.Text != string.Empty)
				{
					return Int32.Parse(textBoxLineNumber.Text);
				}
				else
				{
					return -1;
				}
			}
			set
			{
				textBoxLineNumber.Text = value.ToString();
				textBoxLineNumber.SelectionStart = 0;
				textBoxLineNumber.SelectionLength = textBoxLineNumber.Text.Length;
			}
		}

		/// <summary>
		/// The label for the line number textbox field. It could be changed to 
		/// show the range of line numbers.
		/// </summary>
		internal string Label
		{
			set
			{
				labelLineNumber.Text = value;
			}
		}
	}
}
