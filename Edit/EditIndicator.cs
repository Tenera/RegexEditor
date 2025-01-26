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
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditIndicator class defines the base class for indicators 
	/// in EditControl.
	/// </summary>
	public abstract class EditIndicator : IComparable
	{
		/// <summary>
		/// Helper constant for ordering indicators.
		/// </summary>
		public const int BookmarkDisplayingOrder = 2;

		/// <summary>
		/// Gets the name of the indicator.
		/// </summary>
		/// <returns></returns>
		abstract public string GetName();

		/// <summary>
		/// Gets the displaying order number of the indicator.
		/// </summary>
		/// <returns></returns>
		abstract public int GetDisplayingOrder();

		/// <summary>
		/// Draws the indicator.
		/// </summary>
		/// <returns></returns>
		abstract public void Draw(Graphics g, Rectangle rect, 
			Color foreColor, Color backColor);

		/// <summary>
		/// Compares the displaying order number of the current EditIndicator
		/// object with that of the specified EditIndicator object.
		/// </summary>
		/// <param name="o">An EditIndicator object to compare.</param>
		/// <returns>A signed number indicating the relative displaying 
		/// order of the current EditIndicator object and the specified 
		/// EditIndicator object:  
		/// -1 - the current EditIndicator object displays in front of 
		/// the specified EditIndicator object;
		/// 0 - the current EditIndicator object displays exchangeablely 
		/// with the specified EditIndicator object;
		/// 1 - the current EditIndicator object displays behind the 
		/// specified EditIndicator object.</returns>
		int IComparable.CompareTo(object o)
		{
			return this.GetDisplayingOrder().CompareTo(
				((EditIndicator)o).GetDisplayingOrder());
		}
	}

	/// <summary>
	/// The Bookmark indicator.
	/// </summary>
	public class EditBookmark : EditIndicator
	{
		/// <summary>
		/// Gets the name of the indicator.
		/// </summary>
		/// <returns></returns>
		public override string GetName()
		{
			return ("Bookmark");
		}

		/// <summary>
		/// Gets the displaying order number of the indicator.
		/// </summary>
		/// <returns></returns>
		public override int GetDisplayingOrder()
		{
			return BookmarkDisplayingOrder;
		}

		/// <summary>
		/// Draws the indicator.
		/// </summary>
		public override void Draw(Graphics g, Rectangle rect, 
			Color foreColor, Color backColor)
		{
			int YCoord = rect.Top;
			int cornerWidth = rect.Width/8;
			int spaceHeight = rect.Height/8;
			Pen pen = new Pen(foreColor, 1);
			Point[] points = {
				new Point(rect.Left, YCoord + rect.Height 
								 - spaceHeight - cornerWidth),
				new Point(rect.Left, YCoord + spaceHeight + cornerWidth),
				new Point(rect.Left + cornerWidth, YCoord + spaceHeight),
				new Point(rect.Left + rect.Width - 2 - cornerWidth, 
								 YCoord + spaceHeight),
				new Point(rect.Left + rect.Width - 2, YCoord 
								 + spaceHeight + cornerWidth),
				new Point(rect.Left + rect.Width - 2, YCoord + rect.Height 
								 - spaceHeight - cornerWidth),
				new Point(rect.Left + rect.Width - 2 - cornerWidth, 
								 YCoord + rect.Height - spaceHeight),
				new Point(rect.Left + cornerWidth, YCoord + rect.Height 
								 - spaceHeight),
			};
			g.FillPolygon(new SolidBrush(backColor), points);
			g.DrawPolygon(pen, points);
		}
	}

	/// <summary>
	/// The Breakpoint indicator.
	/// </summary>
	public class EditBreakpoint : EditIndicator
	{
		/// <summary>
		/// Gets the name of the indicator.
		/// </summary>
		/// <returns></returns>
		public override string GetName()
		{
			return ("Breakpoint");
		}

		/// <summary>
		/// Gets the displaying order number of the indicator.
		/// </summary>
		/// <returns></returns>
		public override int GetDisplayingOrder()
		{
			return BookmarkDisplayingOrder - 1;
		}

		/// <summary>
		/// Draws the indicator.
		/// </summary>
		public override void Draw(Graphics g, Rectangle rect, 
			Color foreColor, Color backColor)
		{
			int diameter = Math.Min(rect.Width - 1, rect.Height);
			Rectangle boxRectangle = new Rectangle(rect.Left + 
				(rect.Width - 1 - diameter)/2, rect.Top, 
				diameter, diameter);
			g.FillEllipse(new SolidBrush(backColor), boxRectangle);
		}
	}
}
