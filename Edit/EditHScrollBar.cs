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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditHScrollBar class represents the horizontal scrollbar in EditView.
	/// </summary>
	internal class EditHScrollBar : System.Windows.Forms.HScrollBar
	{
		/// <summary>
		/// Default constructor. Creates a new EditHScrollBar object.
		/// </summary>
		internal EditHScrollBar()
		{
			SetStyle(ControlStyles.Selectable, false);
		}

		/// <summary>
		/// Moves the thumb box of the scrollbar to the specified location.
		/// </summary>
		/// <param name="X">The X-coordinate of the desired location.</param>
		/// <param name="Y">The Y-coordinate of the desired location.</param>
		/// <returns>true if the thumb box has been moved; otherwise, false.
		/// </returns>
		internal bool ScrollHere(int X, int Y)
		{
			if ((X == -1) && (Y == -1))
			{
				return false;
			}
			if (this.Maximum == this.Minimum)
			{
				return false;
			}
			int aw = SystemInformation.HorizontalScrollBarArrowWidth;
			int cw = this.ClientSize.Width;
			int thumbBoxSize = (Math.Min(this.LargeChange, this.Maximum) 
				- this.Minimum) * (cw - 2*aw) / (this.Maximum - this.Minimum);
			if (X <= (aw + thumbBoxSize/2))
			{
				this.Value = this.Minimum;
			}
			else if (X >= (cw - aw - thumbBoxSize/2))
			{
				this.Value = this.Minimum + this.Maximum - this.LargeChange;
			}
			else
			{
				this.Value = this.Minimum + (X - aw) 
					* (this.Maximum - this.Minimum) / (cw - 2*aw) 
					- this.LargeChange/2;
			}
			return true;
		}
	}
}
