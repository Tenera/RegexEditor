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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Summary description for ContextPrompt.
	/// </summary>
	internal class ContextPrompt : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EditView editView;
		private int currentPrompt = 0;
		private ArrayList promptList = null;
		private int leftMargin = 4;
		private int topMargin = 4;
		private int rectHeight = 12;
		private int rectWidth = 8;

		internal ContextPrompt(EditView editView)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SetStyle(ControlStyles.Selectable, false);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
	
			this.editView = editView;
			this.LostFocus += new EventHandler(ContextPrompt_LostFocus);
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
			// 
			// ContextPrompt
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Info;
			this.ClientSize = new System.Drawing.Size(300, 24);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ContextPrompt";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "ContextPrompt";
			this.TopMost = true;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ContextPrompt_MouseDown);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ContextPrompt_KeyPress);

		}
		#endregion

		/// <summary>
		/// Hides the popup selection when focus is lost.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ContextPrompt_LostFocus(object sender, EventArgs e)
		{
			this.Hide();
		}

		/// <summary>
		/// Draws the whole popup prompt object.
		/// </summary>
		/// <param name="pe">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			Rectangle upArrowRect = new Rectangle(ClientRectangle.Left + leftMargin,
				ClientRectangle.Top + topMargin + (Font.Height - rectHeight)/2, 
				rectWidth, rectHeight);
			pe.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Control),
				upArrowRect);
			Point [] upArrow = {
								   new Point (upArrowRect.Left,
								   ClientRectangle.Top + topMargin + rectHeight*2/3 + 1),
								   new Point (upArrowRect.Left + rectWidth/2,
								   ClientRectangle.Top + topMargin + rectHeight/3),
								   new Point (upArrowRect.Left + rectWidth,
								   ClientRectangle.Top + topMargin + rectHeight*2/3 + 1)
							   };
			pe.Graphics.FillPolygon(new SolidBrush(Color.Black), upArrow);
			string strTemp1 = (currentPrompt + 1).ToString() + " of " 
				+ TotalChoices.ToString();
			int strWidth1 = (int)pe.Graphics.MeasureString(strTemp1, Font).Width 
				+ 2 * leftMargin;
			pe.Graphics.DrawString(strTemp1, Font, new SolidBrush(ForeColor), 
				ClientRectangle.Left + leftMargin + upArrowRect.Width + leftMargin,
				ClientRectangle.Top + topMargin);
			Rectangle downArrowRect = new Rectangle(ClientRectangle.Left + leftMargin + 
				upArrowRect.Width + strWidth1,
				ClientRectangle.Top + topMargin + (Font.Height - rectHeight)/2,  
				rectWidth, rectHeight);
			pe.Graphics.FillRectangle(new SolidBrush(System.Drawing.SystemColors.Control),
				downArrowRect);
			Point [] downArrow = {
									 new Point (downArrowRect.Left,
									 ClientRectangle.Top + topMargin + rectHeight/3),
									 new Point (downArrowRect.Left + rectWidth/2,
									 ClientRectangle.Top + topMargin + rectHeight*2/3),
									 new Point (downArrowRect.Left + rectWidth,
									 ClientRectangle.Top + topMargin + rectHeight/3),
			};
			pe.Graphics.FillPolygon(new SolidBrush(Color.Black), downArrow);
			if ((promptList == null) || (promptList.Count == 0))
			{
				pe.Graphics.DrawString("No information to show.", Font, 
					new SolidBrush(ForeColor), downArrowRect.Right, 
					ClientRectangle.Top + topMargin);
				return;
			}
			SizeF size = pe.Graphics.MeasureString((string)promptList[currentPrompt], 
				Font, ClientRectangle.Width - 2 * leftMargin - downArrowRect.Right);
			this.Height = (int)size.Height + 2 * topMargin;
			RectangleF rextRect = new RectangleF(downArrowRect.Right + leftMargin,
				ClientRectangle.Top + topMargin,
				ClientRectangle.Width - 2 * leftMargin - downArrowRect.Right,
				size.Height);
			pe.Graphics.DrawString((string)promptList[currentPrompt], Font, 
				new SolidBrush(ForeColor), rextRect);
			pe.Graphics.DrawRectangle(new Pen(Color.Black, 1),
				ClientRectangle.X, ClientRectangle.Y, 
				ClientRectangle.Width - 1, ClientRectangle.Height - 1);
		}

		/// <summary>
		/// Handles the MouseDown event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextPrompt_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.X <= ClientRectangle.Left + leftMargin + rectWidth)
			{
				PreviousChoice();
			}
			else
			{
				NextChoice();
			}
		}

		/// <summary>
		/// Handles the KeyPress event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContextPrompt_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			editView.KeyPressWithContextPrompt(sender, e);
		}

		/// <summary>
		/// Processes arrow keys.
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Up)
			{
				PreviousChoice();
				return true;
			}
			else if (keyData == Keys.Down)
			{
				NextChoice();
				return true;
			}
			else if	(keyData == Keys.Left)
			{
				editView.LeftArrowKey();
				return true;
			}
			else if (keyData == Keys.Right)
			{
				editView.RightArrowKey();
				return true;
			}
			else
			{
				return base.ProcessDialogKey(keyData);
			}
		}

		/// <summary>
		/// Displays the next choice.
		/// </summary>
		private void NextChoice()
		{
			if (currentPrompt == TotalChoices - 1)
			{
				currentPrompt = 0;
			}
			else
			{
				currentPrompt++;
			}
			Invalidate();
		}

		/// <summary>
		/// Displays the previous choice.
		/// </summary>
		private void PreviousChoice()
		{
			if (currentPrompt == 0)
			{
				currentPrompt = TotalChoices - 1;
			}
			else
			{
				currentPrompt--;
			}
			Invalidate();
		}

		/// <summary>
		/// Gets the total number of choices.
		/// </summary>
		internal int TotalChoices
		{
			get
			{
				if (promptList != null)
				{
					return promptList.Count;
				}
				return 0;
			}
		}

		/// <summary>
		/// Sets the items for the listBoxChoices.
		/// </summary>
		/// <param name="al"></param>
		internal void SetPrompts(ArrayList al)
		{
			promptList = al;
		}
	}
}
