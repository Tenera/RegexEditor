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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditButton class represents the splitter button at the top of
	/// the vertical scrollbar in EditView.
	/// </summary>
	internal class EditButton : System.Windows.Forms.Button
	{
		#region Data Members

		/// <summary>
		/// A value indicating whether theme is supported.
		/// </summary>
		private bool bThemeSupported;
		/// <summary>
		/// A pointer to the theme data handle.
		/// </summary>
		private IntPtr hTheme = IntPtr.Zero;

		#endregion

		#region Imported Methods

		/// <summary>
		/// Tests if a visual style for the current application is active.
		/// </summary>
		[DllImport("uxtheme.dll")]
		public static extern int IsThemeActive();
		/// <summary>
		/// Opens the theme data for a window and its associated class.
		/// </summary>
		[DllImport("uxtheme.dll")]
		public static extern IntPtr OpenThemeData(IntPtr hWnd, [MarshalAs(UnmanagedType.LPTStr)] string classList);
		/// <summary>
		/// Closes the theme data handle.
		/// </summary>
		[DllImport("uxtheme.dll")]
		public static extern void CloseThemeData(IntPtr hTheme);
		/// <summary>
		/// Draws the background image defined by the visual style for the 
		/// specified control part.
		/// </summary>
		[DllImport("uxtheme.dll")]
		public static extern void DrawThemeBackground(IntPtr hTheme, IntPtr hDC, int partId, int stateId, ref RECT rect, ref RECT clipRect);
		/// <summary>
		/// Draws the part of a parent control that is covered by a partially
		/// -transparent or alpha-blended child control.
		/// </summary>
		[DllImport("uxtheme.dll")]
		public static extern void DrawThemeParentBackground(IntPtr hWnd, IntPtr hDC, ref RECT rect);

		#endregion

		#region Methods

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal EditButton()
		{
			SetStyle(ControlStyles.Selectable, false);
			bThemeSupported = ((Environment.OSVersion.Version.Major >= 5)
				&& (Environment.OSVersion.Version.Minor >= 1)
				&& (Environment.OSVersion.Platform == PlatformID.Win32NT));
		}

		/// <summary> 
		/// Cleans up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if (disposing)
			{
				if (hTheme != IntPtr.Zero)
				{
					CloseThemeData(this.hTheme);
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Draws the whole content area.
		/// </summary>
		/// <param name="pe">A PaintEventArgs that contains the event data.
		/// </param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe); 
			if (bThemeSupported)
			{
				if ((IsThemeActive() == 1)  && (this.hTheme == IntPtr.Zero))
				{
					this.hTheme = OpenThemeData(this.Handle, "SCROLLBAR");
				}

				if (this.hTheme != IntPtr.Zero)
				{
					IntPtr hDC = pe.Graphics.GetHdc();
					RECT rect = new RECT(this.ClientRectangle);
					DrawThemeParentBackground(this.Handle, hDC, ref rect);
					DrawThemeBackground(hTheme, hDC, 3, 1, ref rect, ref rect);
					pe.Graphics.ReleaseHdc(hDC);
				}
			}
		}

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m">The Windows Message to process.</param>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == 794) 
			{
				if (this.hTheme != IntPtr.Zero) 
				{
					CloseThemeData(this.hTheme);
					this.hTheme = IntPtr.Zero; 
				}
				base.Invalidate();
			}
		}

		#endregion
	}

	#region RECT structure

	/// <summary>
	/// The RECT structure defines the coordinates of the upper-left and 
	/// lower-right corners of a rectangle. 
	/// </summary>
	[StructLayout(LayoutKind.Sequential)] 
	internal struct RECT
	{
		public int Left;
		public int Top;
		public int Right;
		public int Bottom;

		/// <summary>
		/// Creates a new RECT object from the given values of data members.
		/// </summary>
		/// <param name="left">The x-coordinate of the upper-left corner 
		/// of the rectangle.</param>
		/// <param name="top">The y-coordinate of the upper-left corner 
		/// of the rectangle.</param>
		/// <param name="right">The x-coordinate of the lower-right corner 
		/// of the rectangle.</param>
		/// <param name="bottom">The y-coordinate of the lower-right corner 
		/// of the rectangle.</param>
		public RECT(int left, int top, int right, int bottom) 
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		/// <summary>
		/// Creates a new RECT object from a Rectangle.
		/// </summary>
		/// <param name="rect">The Rectangle object from which the new RECT 
		/// object is obtained.</param>
		public RECT(Rectangle rect) 
		{
			this.Left = rect.Left; 
			this.Top = rect.Top;
			this.Right = rect.Right;
			this.Bottom = rect.Bottom;
		}

		/// <summary>
		/// Gets a Rectangle object from the current RECT object.
		/// </summary>
		/// <returns>The Rectangle converted from the current RECT object.
		/// </returns>
		public Rectangle ToRectangle() 
		{
			return new Rectangle(Left, Top, Right, Bottom);
		}

		/// <summary>
		/// Returns a string that represents the current RECT object.
		/// </summary>
		/// <returns>A string that contains the list of the data members 
		/// of the current RECT object.</returns>
		public override string ToString()
		{
			return "Left " + Left + "; " + "Right " + Right + "; "
				+ "Top " + Top + "; " + "Bottom " + Bottom;
		}
	}

	#endregion
}
