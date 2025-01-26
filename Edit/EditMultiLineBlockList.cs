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

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditMultiLineBlockList class manages EditMultiLineBlock objects 
	/// for EditData.
	/// </summary>
	internal class EditMultiLineBlockList
	{
		#region Data Members

		/// <summary>
		/// The asociated EditData object.
		/// </summary>
		private EditData editData;
		/// <summary>
		/// The index of the multiline tag that is currently being processed.
		/// </summary>
		private short multiLineTagIndex = -1;
		/// <summary>
		/// The location that has been updated to.
		/// </summary>
		private EditLocation locationUpdated = new EditLocation(0, 0);
		/// <summary>
		/// The arraylist of EditMultiLineBlock objects.
		/// </summary>
		private ArrayList multiLineBlockList = new ArrayList();

		#endregion

		#region Methods

		/// <summary>
		/// Constructor. Creates an EditMultiLineBlockList object associated 
		/// with the specified EditData object.
		/// </summary>
		/// <param name="editData">The associated EditData object.</param>
		internal EditMultiLineBlockList(EditData editData)
		{
			this.editData = editData;
		}

		/// <summary>
		/// Adds an EditMultiLineBlock object with the specified values for 
		/// data members.
		/// </summary>
		/// <param name="lcStart">The starting location of the EditMultiLineBlock 
		/// object.</param>
		/// <param name="lcEnd">The ending location of the EditMultiLineBlock 
		/// object.</param>
		/// <param name="colorGroupIndex">The color group index of the 
		/// EditMultiLineBlock object.</param>
		/// <param name="tagIndex">The index of the multiline tag of the 
		/// EditMultiLineBlock object.</param>
		/// <param name="isAdvTag">A value indicating whether the 
		/// EditMultiLineBlock object is tagged by an advanced tag.</param>
		/// <returns></returns>
		internal int Add(EditLocation lcStart, EditLocation lcEnd, 
			short colorGroupIndex, short tagIndex, bool isAdvTag)
		{
			return Add(new EditMultiLineBlock(lcStart, lcEnd, 
				colorGroupIndex, tagIndex, isAdvTag));
		}

		/// <summary>
		/// Adds an EditMultiLineBlock object.
		/// </summary>
		/// <param name="mlcb">The EditMultiLineBlock object to be added.
		/// </param>
		/// <returns>The index in the internal arrayList at which the 
		/// EditMultiLineBlock object has been added.</returns>
		internal int Add(EditMultiLineBlock mlb)
		{
			int index = multiLineBlockList.BinarySearch(mlb);
			if (index < 0)
			{
				index = ~index;
				multiLineBlockList.Insert(index, mlb);
			}
			return index;
		}

		/// <summary>
		/// Gets the index of the EditMultiLineBlock object containing the 
		/// specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>The index of the EditMultiLineBlock object containing the 
		/// location.</returns>
		internal int GetMultiLineBlockIndex(int ln, int ch)
		{
			int iMin = 0;
			int iMax = multiLineBlockList.Count - 1;
			int iMid;
			while (iMin <= iMax)
			{
				iMid = (iMin + iMax)/2;
				if (((EditMultiLineBlock)multiLineBlockList[iMid]).Contains(ln, ch))
				{
					return iMid;
				}
				else if (((EditMultiLineBlock)multiLineBlockList[iMid]).Start.GreaterThan(ln, ch))
				{
					iMax = iMid - 1;
				}
				else
				{
					iMin = iMid + 1;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the index of the EditMultiLineBlock object locating at the 
		/// specified line.
		/// </summary>
		/// <param name="ln">The line at which the EditMultiLineBlock object is 
		/// to be obtained.</param>
		/// <returns>The index of the EditMultiLineBlock object locating at the 
		/// line.</returns>
		internal int GetMultiLineBlockIndex(int ln)
		{
			int iMin = 0;
			int iMax = multiLineBlockList.Count - 1;
			int iMid;
			while (iMin <= iMax)
			{
				iMid = (iMin + iMax)/2;
				if ((((EditMultiLineBlock)multiLineBlockList[iMid]).Start.L <= ln)
					&&((((EditMultiLineBlock)multiLineBlockList[iMid]).End.L >= ln)
					||(((EditMultiLineBlock)multiLineBlockList[iMid]).End.L == -1)))
				{
					return iMid;
				}
				else if (((EditMultiLineBlock)multiLineBlockList[iMid]).Start.L > ln)
				{
					iMax = iMid - 1;
				}
				else
				{
					iMin = iMid + 1;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets information of the EditMultiLineBlock object locating at the 
		/// specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <param name="startCh">The starting char of the EditMultiLineBlock
		/// object at the same line.</param>
		/// <param name="endCh">The ending char of the EditMultiLineBlock 
		/// object at the same line.</param>
		/// <param name="tagIndex">The index of the multiline tag of the 
		/// EditMultiLineBlock object.</param>
		/// <param name="colorGroupIndex">The color group index of the 
		/// EditMultiLineBlock object.</param>
		/// <param name="bAdvTag">A value indicating whether the 
		/// EditMultiLineBlock object is tagged by an advanced tag.</param>
		/// <param name="bStartLn">A value indicating whether the location is 
		/// at the starting line of the EditMultiLineBlock object.</param>
		/// <param name="bEndLn">A value indicating whether the location is 
		/// at the ending line of the EditMultiLineBlock object.</param>
		/// <returns>true if an EditMultiLineBlock object is found; otherwise, 
		/// false.</returns>
		internal bool GetMultiLineColorInfo(int ln, int ch, out short startCh, 
			out short endCh, out short tagIndex, out short colorGroupIndex, 
			out bool bAdvTag, out bool bStartLn, out bool bEndLn)
		{
			int mlbi = GetMultiLineBlockIndex(ln, ch);
			if (mlbi == -1)
			{
				startCh = -1;
				endCh = -1;
				tagIndex = -1;
				colorGroupIndex = -1;
				bAdvTag = false;
				bStartLn = false;
				bEndLn = false;
				return false;
			}
			else
			{
				if ((((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.L < ln) && 
					(((((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L > ln)) ||
					(((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L == -1)))
				{
					startCh = 1;
					endCh = (short)editData.GetLineLength(ln);
					bStartLn = false;
					bEndLn = false;
				}
				else if ((((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.L < ln) && 
					(((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L == ln))
				{
					startCh = 1;
					endCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).End.C;
					bStartLn = false;
					bEndLn = true;
				}
				else if ((((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.L == ln) && 
					(((((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L > ln)) ||
					(((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L == -1)))
				{
					startCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.C;
					endCh = (short)editData.GetLineLength(ln);
					bStartLn = true;
					bEndLn = false;
				}
				else
				{
					startCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.C;
					endCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).End.C;
					bStartLn = true;
					bEndLn = true;
				}
				tagIndex = ((EditMultiLineBlock)multiLineBlockList[mlbi]).TagIndex;
				colorGroupIndex = ((EditMultiLineBlock)multiLineBlockList[mlbi]).ColorGroupIndex;
				bAdvTag = ((EditMultiLineBlock)multiLineBlockList[mlbi]).IsAdvTag;
				return true;
			}
		}

		/// <summary>
		/// Gets information of the EditMultiLineBlock object locating at the 
		/// specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <param name="startCh">The starting char of the EditMultiLineBlock 
		/// object at the same line.</param>
		/// <param name="endCh">The ending char of the EditMultiLineBlock 
		/// object at the same line.</param>
		/// <param name="tagIndex">The index of the multiline tag of the 
		/// EditMultiLineBlock object.</param>
		/// <returns>true if an EditMultiLineBlock object is found; otherwise, 
		/// false.</returns>
		internal bool GetMultiLineBlockInfo(int ln, int ch, out short startCh, 
			out short endCh, out short tagIndex)
		{
			int mlbi = GetMultiLineBlockIndex(ln, ch);
			if (mlbi == -1)
			{
				startCh = -1;
				endCh = -1;
				tagIndex = -1;
				return false;
			}
			else
			{
				if ((((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.L < ln) && 
					(((((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L > ln)) ||
					(((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L == -1)))
				{
					startCh = 1;
					endCh = (short)editData.GetLineLength(ln);
				}
				else if ((((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.L < ln) && 
					(((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L == ln))
				{
					startCh = 1;
					endCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).End.C;
				}
				else if ((((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.L == ln) && 
					(((((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L > ln)) ||
					(((EditMultiLineBlock)multiLineBlockList[mlbi]).End.L == -1)))
				{
					startCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.C;
					endCh = (short)editData.GetLineLength(ln);
				}
				else
				{
					startCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).Start.C;
					endCh = (short)((EditMultiLineBlock)multiLineBlockList[mlbi]).End.C;
				}
				tagIndex = ((EditMultiLineBlock)multiLineBlockList[mlbi]).TagIndex;
				return true;
			}
		}

		/// <summary>
		/// Clears all the EditMultiLineBlock objects in the internal arraylist.
		/// </summary>
		internal void Clear()
		{
			multiLineBlockList.Clear();
			multiLineBlockList.TrimToSize();
			multiLineTagIndex = -1;
			locationUpdated.L = 0;
			locationUpdated.C = 0;
		}

		/// <summary>
		/// Clears all the EditMultiLineBlock objects past the specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		internal void ClearFrom(int ln, int ch)
		{
			if (locationUpdated.LessThan(ln, ch))
			{
				return;
			}
			if (multiLineBlockList.Count == 0)
			{
				multiLineTagIndex = -1;
				locationUpdated.L = 0;
				locationUpdated.C = 0;
				return;
			}
			EditLocation lcPrev = editData.GetPreviousLineChar(ln, ch);
			if (editData.Edit.FirstLineChar.GreaterThanOrEqualTo(ln, ch))
			{
				multiLineBlockList.RemoveRange(0, multiLineBlockList.Count);
				multiLineTagIndex = -1;
				locationUpdated.L = 0;
				locationUpdated.C = 0;
				return;
			}
			if (((EditMultiLineBlock)multiLineBlockList[0]).Start.GreaterThan(ln, ch))
			{
				multiLineTagIndex = -1;
				locationUpdated = lcPrev;
				multiLineBlockList.RemoveRange(0, multiLineBlockList.Count);
				return;
			}	
			if (((EditMultiLineBlock)multiLineBlockList[multiLineBlockList.Count - 1])
				.End.LessThan(ln, ch))
			{
				multiLineTagIndex = -1;
				locationUpdated = lcPrev;
				return;
			}
			int iMin = 0;
			int iMax = multiLineBlockList.Count - 1;
			int iMid;
			while (iMin <= iMax)
			{
				iMid = (iMin + iMax)/2;
				if (((EditMultiLineBlock)multiLineBlockList[iMid]).Contains(ln, ch))
				{
					if (((EditMultiLineBlock)multiLineBlockList[iMid]).Start.EqualTo(ln, ch))
					{
						multiLineTagIndex = -1;
						locationUpdated = lcPrev;
						multiLineBlockList.RemoveRange(iMid, multiLineBlockList.Count - iMid);
					}
					else
					{
						multiLineTagIndex = ((EditMultiLineBlock)multiLineBlockList[iMid]).TagIndex;
						locationUpdated = lcPrev;
						((EditMultiLineBlock)multiLineBlockList[iMid]).End = EditLocation.Infinite;
						multiLineBlockList.RemoveRange(iMid + 1, multiLineBlockList.Count - iMid - 1);
					}
					return;
				}
				else if (((EditMultiLineBlock)multiLineBlockList[iMid]).Start.GreaterThan(ln, ch))
				{
					if (iMid == 0)
					{
						multiLineTagIndex = -1;
						locationUpdated = lcPrev;
						multiLineBlockList.RemoveRange(iMid, multiLineBlockList.Count - iMid);
						return;
					}
					else if (((EditMultiLineBlock)multiLineBlockList[iMid - 1]).End.LessThan(ln, ch))
					{
						multiLineTagIndex = -1;
						locationUpdated = lcPrev;
						multiLineBlockList.RemoveRange(iMid, multiLineBlockList.Count - iMid);
						return;
					}
					iMax = iMid - 1;
				}
				else
				{
					if (iMid == multiLineBlockList.Count - 1)
					{
						multiLineTagIndex = -1;
						locationUpdated = lcPrev;
						return;
					}
					else if (((EditMultiLineBlock)multiLineBlockList[iMid + 1]).Start.GreaterThan(ln, ch))
					{
						multiLineTagIndex = -1;
						locationUpdated = lcPrev;
						multiLineBlockList.RemoveRange(iMid + 1, multiLineBlockList.Count - iMid - 1);
						return;
					}
					iMin = iMid + 1;
				}
			}
			multiLineTagIndex = -1;
			locationUpdated.L = 0;
			locationUpdated.C = 0;
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the EditMultiLineBlock object at the specified index.
		/// </summary>
		internal EditMultiLineBlock this[int i]
		{
			get
			{
				return (EditMultiLineBlock)multiLineBlockList[i];
			}
			set
			{
				multiLineBlockList[i] = value;
			}
		}

		/// <summary>
		/// Gets the count of EditMultiLineBlock objects in the internal 
		/// arraylist.
		/// </summary>
		internal int Count
		{
			get
			{
				return multiLineBlockList.Count;
			}
		}

		/// <summary>
		/// Gets or sets the multiline tag index of the multiline block being 
		/// processed.
		/// </summary>
		internal short MultiLineTagIndex
		{
			get
			{
				return multiLineTagIndex;
			}
			set
			{
				multiLineTagIndex = value;
			}
		}

		/// <summary>
		/// Gets or sets the location that has been updated to.
		/// </summary>
		internal EditLocation LocationUpdated
		{
			get
			{
				return locationUpdated;
			}
			set
			{
				locationUpdated = value;
			}
		}

		#endregion
	}
}
