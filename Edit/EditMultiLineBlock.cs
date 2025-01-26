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
	/// The EditMultiLineBlock class represents multiline tagged blocks.
	/// </summary>
	internal class EditMultiLineBlock : EditLocationRange
	{
		#region Data Members

		/// <summary>
		/// The index of the color group.
		/// </summary>
		internal short ColorGroupIndex;
		/// <summary>
		/// The index of the multiline tag.
		/// </summary>
		internal short TagIndex;
		/// <summary>
		/// A value indicating whether the block is tagged by an advanced tag.
		/// </summary>
		internal bool IsAdvTag;

		#endregion

		#region Methods

		/// <summary>
		/// Overloaded constructor. Creates an EditMultiLineBlock object with 
		/// the specified values for data members.
		/// </summary>
		/// <param name="lcStart">The starting location.</param>
		/// <param name="lcEnd">The ending location.</param>
		/// <param name="colorGroupIndex">The index of the color group.</param>
		/// <param name="tagIndex">The index of the multiline tag.</param>
		/// <param name="isAdvTag">The value indicating whether the block is 
		/// tagged by an advanced tag.</param>
		internal EditMultiLineBlock(EditLocation lcStart, EditLocation lcEnd, 
			short colorGroupIndex, short tagIndex, bool isAdvTag)
		{
			this.Start = lcStart;
			this.End = lcEnd;
			this.ColorGroupIndex = colorGroupIndex;
			this.TagIndex = tagIndex;
			this.IsAdvTag = isAdvTag;
		}

		/// <summary>
		/// Overloaded constructor. Creates an EditMultiLineBlock object 
		/// with the specified values for data members.
		/// </summary>
		/// <param name="lcr">The location range.</param>
		/// <param name="colorGroupIndex">The index of the color group.</param>
		/// <param name="tagIndex">The index of the multiline tag.</param>
		/// <param name="isAdvTag">The value indicating whether the block is 
		/// tagged by an advanced tag.</param>
		internal EditMultiLineBlock(EditLocationRange lcr, short colorGroupIndex,
			short tagIndex, bool isAdvTag)
		{
			this.Start = lcr.Start;
			this.End = lcr.End;
			this.ColorGroupIndex = colorGroupIndex;
			this.TagIndex = tagIndex;
			this.IsAdvTag = isAdvTag;
		}

		#endregion
	}
}
