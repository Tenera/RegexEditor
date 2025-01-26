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
	/// Summary description for ContextChoice.
	/// </summary>
	internal class ContextChoice : System.Windows.Forms.Form
	{
		internal EditView editView;
		internal System.Windows.Forms.ListBox ListBoxChoices;

		internal int ItemsPerPage = -1;
		internal Font ItemFont = null;
		internal ArrayList ItemList = null;
		internal ArrayList ItemImageIndexList = null;
		internal ImageList ItemImageList = null;

		private int borderHeight = -1;
		private const int DefaultItemsPerPage = 10;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal ContextChoice(EditView editView)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			SetStyle(ControlStyles.Selectable, false);

			this.editView = editView;
			this.GotFocus += new EventHandler(ContextChoice_GotFocus);
			this.LostFocus += new EventHandler(ContextChoice_LostFocus);
			this.ListBoxChoices.LostFocus += new EventHandler(ContextChoice_LostFocus);

			borderHeight = 2 * SystemInformation.Border3DSize.Height;
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
			this.ListBoxChoices = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// ListBoxChoices
			// 
			this.ListBoxChoices.BackColor = System.Drawing.Color.MintCream;
			this.ListBoxChoices.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ListBoxChoices.CausesValidation = false;
			this.ListBoxChoices.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ListBoxChoices.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.ListBoxChoices.IntegralHeight = false;
			this.ListBoxChoices.Location = new System.Drawing.Point(0, 0);
			this.ListBoxChoices.Name = "ListBoxChoices";
			this.ListBoxChoices.Size = new System.Drawing.Size(130, 174);
			this.ListBoxChoices.TabIndex = 0;
			this.ListBoxChoices.TabStop = false;
			this.ListBoxChoices.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ContextChoice_KeyPress);
			this.ListBoxChoices.DoubleClick += new System.EventHandler(this.ListBoxChoices_DoubleClick);
			this.ListBoxChoices.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.ListBoxChoices_MeasureItem);
			this.ListBoxChoices.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBoxChoices_DrawItem);
			// 
			// ContextChoice
			// 
			this.AutoScale = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(130, 174);
			this.ControlBox = false;
			this.Controls.Add(this.ListBoxChoices);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "ContextChoice";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = true;
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ContextChoice_KeyPress);
			this.VisibleChanged += new System.EventHandler(this.ContextChoice_VisibleChanged);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Handles the GotFocus event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ContextChoice_GotFocus(object sender, EventArgs e)
		{
			if (ListBoxChoices != null)
			{
				ListBoxChoices.Focus();
			}
		}

		/// <summary>
		/// Hides the LostFocus event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ContextChoice_LostFocus(object sender, EventArgs e)
		{
			if (!this.ContainsFocus)
			{
				this.ListBoxChoices.SelectedIndex = -1;
				this.Hide();
			}
		}

		/// <summary>
		/// Handles the KeyPress event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ContextChoice_KeyPress(object sender, KeyPressEventArgs e)
		{
			editView.KeyPressWithContextChoice(sender, e);
		}

		/// <summary>
		/// Handles the DoubleClick event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ListBoxChoices_DoubleClick(object sender, EventArgs e)
		{
			if (this.ListBoxChoices.Text != null)
			{
				if (!editView.IsContextChoiceChar(editView.Edit.GetCurrentWord()[0]))
				{
					editView.Edit.DeleteCurrentWord();
				}
				editView.Edit.Insert(this.ListBoxChoices.Text);
				this.Hide();
			}
		}

		private void ListBoxChoices_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			e.DrawBackground();
			if (ItemImageList == null)
			{
				e.Graphics.FillEllipse(new SolidBrush(Color.BlueViolet), 
					e.Bounds.X + 2, e.Bounds.Y + 2,
					e.Bounds.Height - 2, e.Bounds.Height - 2);
			}
			else
			{
				if ((int)ItemImageIndexList[e.Index] != -1)
				{
					e.Graphics.DrawImage(ItemImageList.
						Images[(int)ItemImageIndexList[e.Index]], 
						e.Bounds.X + 1, e.Bounds.Y + (ItemHeight 
						- ItemImageList.ImageSize.Height)/2);
				}
			}
			e.Graphics.DrawString(ListBoxChoices.Items[e.Index].ToString(), 
				ListBoxFont, new SolidBrush(e.ForeColor), e.Bounds.Location.X 
				+ e.Bounds.Height * 4 / 3, e.Bounds.Location.Y 
				+ (ItemHeight - ListBoxFont.Height)/2);
			e.DrawFocusRectangle();
		}

		private void ListBoxChoices_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
			e.ItemHeight = ItemHeight;
		}

		private void ContextChoice_VisibleChanged(object sender, System.EventArgs e)
		{
			if (this.Visible)
			{
				if (ItemList == null)
				{
					ListBoxChoices.Items.Clear();
					return;
				}
				ListBoxChoices.BeginUpdate();
				ListBoxChoices.Items.Clear();
				for (int i = 0; i < ItemList.Count; i++)
				{
					ListBoxChoices.Items.Add(ItemList[i].ToString());
				}
				ListBoxChoices.EndUpdate();
				this.Width = MaxItemWidth + ItemHeight * 4 / 3 + 
					2 * (SystemInformation.BorderSize.Width 
					+ SystemInformation.VerticalScrollBarWidth);
				this.Height = ItemHeight * ListBoxItemsPerPage + borderHeight;
			}
		}

		private int MaxItemWidth
		{
			get
			{
				int maxItemWidth = 0;
				int widthTemp = 0;
				Graphics g = this.CreateGraphics();
				for (int i = 0; i < ListBoxChoices.Items.Count; i++)
				{
					widthTemp = (int) g.MeasureString(ListBoxChoices.
						Items[i].ToString(), ListBoxFont, 10000).Width;
					if (maxItemWidth < widthTemp)
					{
						maxItemWidth = widthTemp;
					}
				}
				return maxItemWidth;
			}
		}

		private int ItemHeight
		{
			get
			{
				if (ItemImageList == null)
				{
					return (ListBoxFont.Height + 2);
				}
				else
				{
					return (Math.Max(ItemImageList.ImageSize.Height, 
						ListBoxFont.Height) + 2);
				}
			}
		}

		private Font ListBoxFont
		{
			get
			{
				return (ItemFont == null) ? this.Font : ItemFont;
			}
		}

		private int ListBoxItemsPerPage
		{
			get
			{
				return (ItemsPerPage == -1) ? DefaultItemsPerPage : ItemsPerPage;
			}
		}
	}
}
