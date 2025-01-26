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
	/// The EditIndicatorList class stores EditIndicator objects for EditControl.
	/// </summary>
	internal class EditIndicatorList
	{
		#region Data Members

		/// <summary>
		/// The ArrayList for EditIndicator objects.
		/// </summary>
		ArrayList editIndicatorList = new ArrayList();

		#endregion

		#region Methods

		/// <summary>
		/// Adds an EditIndicator object.
		/// </summary>
		/// <param name="idc">The EditIndicator object to be added.</param>
		/// <returns>true if the object is inserted; otherwise, false.</returns>
		internal bool Add(EditIndicator idc)
		{
			if (editIndicatorList.Count == 0)
			{
				editIndicatorList.Add(idc);
				return true;
			}
			else if (GetIndicatorIndex(idc.GetName()) == -1)
			{
				editIndicatorList.Add(idc);
				editIndicatorList.Sort();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the EditIndicator object for the specified indicator name.
		/// </summary>
		/// <param name="indicatorName">The name of the indicator.</param>
		/// <returns>The EditIndicator object for the indicator name.</returns>
		internal EditIndicator GetIndicator(string indicatorName)
		{
			return ((EditIndicator)editIndicatorList[GetIndicatorIndex(indicatorName)]);
		}

		/// <summary>
		/// Gets the indicator index for the specified indicator name.
		/// </summary>
		/// <param name="indicatorName">The name of the indicator.</param>
		/// <returns>The index of the indicator.</returns>
		internal int GetIndicatorIndex(string indicatorName)
		{
			for (int i = 0; i < editIndicatorList.Count; i++)
			{
				if (((EditIndicator)editIndicatorList[i]).GetName() 
					== indicatorName)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Removes the EditIndicator object at the specified index.
		/// </summary>
		/// <param name="index">The index of the EditIndicator object to be 
		/// removed.</param>
		internal void RemoveAt(int index)
		{
			editIndicatorList.RemoveAt(index);
		}

		#endregion

		#region Properties

		/// <summary>
		/// The index operator.
		/// </summary>
		internal EditIndicator this[int i]
		{
			get
			{
				return (EditIndicator)editIndicatorList[i];
			}
			set
			{
				editIndicatorList[i] = value;
			}
		}

		/// <summary>
		/// The count of EditIndicator objects.
		/// </summary>
		internal int Count
		{
			get
			{
				return editIndicatorList.Count;
			}
		}

		#endregion
	}
}
