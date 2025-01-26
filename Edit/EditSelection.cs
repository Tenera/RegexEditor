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

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditSelection class represents the selected text in EditControl.
	/// </summary>
	internal class EditSelection : EditLocationRange
	{
		#region Data Members

		/// <summary>
		/// A value indicating the type of the selection: linewise or columnwise.
		/// </summary>
		private bool isLinewise;
		/// <summary>
		/// A value indicating whether selecting is in process.
		/// </summary>
		private bool isSelecting;

		#endregion

		#region Methods

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal EditSelection() 
		{
			UnSelect();
		}

		/// <summary>
		/// Clears the selection.
		/// </summary>
		internal void UnSelect()
		{
			this.Start.L = EditLocation.None.L;
			this.Start.C = EditLocation.None.C;
			this.End.L = EditLocation.None.L;
			this.End.C = EditLocation.None.C;
			this.isLinewise = true;
			this.isSelecting = false;
		}

		/// <summary>
		/// Gets the real starting location of the selection.
		/// </summary>
		/// <returns>The real starting location of the selection.</returns>
		internal EditLocation GetStart()
		{
			return (this.Start <= this.End) ? this.Start : this.End;
		}

		/// <summary>
		/// Gets the real ending location of the selection.
		/// </summary>
		/// <returns>The real ending location of the selection.</returns>
		internal EditLocation GetEnd()
		{
			return (this.Start <= this.End) ? this.End : this.Start;
		}

		/// <summary>
		/// Initiates the selecting process with the specified starting location 
		/// and type.
		/// </summary>
		/// <param name="lc">The starting location of the selection.</param>
		/// <param name="isLinewise">The selection type.</param>
		internal void StartSelecting(EditLocation lc, bool isLinewise)
		{
			StartSelecting(lc.L, lc.C, isLinewise);
		}

		/// <summary>
		/// Initiates the selecting process with the specified starting location 
		/// and type.
		/// </summary>
		/// <param name="ln">The starting line of the selection.</param>
		/// <param name="ch">The starting char of the selection.</param>
		/// <param name="isLinewise">The selection type.</param>
		internal void StartSelecting(int ln, int ch, bool isLinewise)
		{
			this.Start.L = ln;
			this.Start.C = ch;
			this.End.L = ln;
			this.End.C = ch;
			this.isLinewise = isLinewise;
			this.isSelecting = true;
		}

		/// <summary>
		/// Terminates the current selecting process.
		/// </summary>
		internal void StopSelecting()
		{
			this.isSelecting = false;
		}

		/// <summary>
		/// Extends the selection to the specified ending location.
		/// </summary>
		/// <param name="ln">The ending line to be extended to.</param>
		/// <param name="ch">The ending char to be extended to.</param>
		internal void ExtendTo(int ln, int ch)
		{
			this.End.L = ln;
			this.End.C = ch;
		}

		/// <summary>
		/// Extends the selection to the specified ending location.
		/// </summary>
		/// <param name="lc">The ending location to be extended to.</param>
		internal void ExtendTo(EditLocation lc)
		{
			ExtendTo(lc.L, lc.C);
		}

		/// <summary>
		/// Sets the selection to be the specified range and type.
		/// </summary>
		/// <param name="startLn">The starting line of the selection.</param>
		/// <param name="startCh">The starting char of the selection.</param>
		/// <param name="endLn">The ending line of the selection.</param>
		/// <param name="endCh">The ending char of the selection.</param>
		/// <param name="isLinewise">The selection type.</param>
		internal void Select(int startLn, int startCh, int endLn, int endCh, 
			bool isLinewise)
		{
			this.Start.L = startLn;
			this.Start.C = startCh;
			this.End.L = endLn;
			this.End.C = endCh;
			this.isLinewise = isLinewise;
		}

		/// <summary>
		/// Sets the selection to be the specified range and type.
		/// </summary>
		/// <param name="start">The starting location of the selection.</param>
		/// <param name="end">The ending location of the selection.</param>
		/// <param name="isLinewise">The selection type.</param>
		internal void Select(EditLocation start, EditLocation end, bool isLinewise)
		{
			Select(start.L, start.C, end.L, end.C, isLinewise);
		}

		/// <summary>
		/// Sets the selection to be the specified range and type.
		/// </summary>
		/// <param name="lcr">The location range of the selection.</param>
		/// <param name="isLinewise">The selection type.</param>
		internal void Select(EditLocationRange lcr, bool isLinewise)
		{
			Select(lcr.Start.L, lcr.Start.C, lcr.End.L, lcr.End.C, isLinewise);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether the selection is linewise.
		/// </summary>
		internal bool IsLinewise
		{
			get
			{
				return isLinewise;
			}
		}

		/// <summary>
		/// Gets a value indicating whether selecting is in progress.
		/// </summary>
		internal bool IsSelecting
		{
			get
			{
				return isSelecting;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any selected text.
		/// </summary>
		internal bool HasSelection
		{
			get
			{
				return ((this.Start != this.End) && 
					(this.Start.L > 0) && (this.End.L > 0));
			}
		}

		#endregion
	}
}
