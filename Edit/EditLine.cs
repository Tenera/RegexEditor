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

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditLine class represents lines in EditControl. The text content 
	/// of a line is stored as a string. Color information for the text is 
	/// contained in an array of short integers. The storage format for color 
	/// information of one block of color text is: the index of the beginning 
	/// char, the index of the ending char, and the index of the color group.
	/// </summary>
	internal class EditLine
	{
		#region Data Members

		/// <summary>
		/// The text string of the line.
		/// </summary>
		internal string LineString;
		/// <summary>
		/// The data for color information.
		/// </summary>
		internal short [] ColorTextData;
		/// <summary>
		/// The data for indicators and wave lines.
		/// </summary>
		internal short [] IndicatorData;
		/// <summary>
		/// The number of occupied lines.
		/// </summary>
		internal short LinesOccupied;
		/// <summary>
		/// A variable indicating whether the color information is updated.
		/// </summary>
		internal bool HasUpdatedColor;
		/// <summary>
		/// A variable indicating whether the foreground color is custom-set.
		/// </summary>
		internal bool IsCustomForeColor;
		/// <summary>
		/// A variable indicating whether the background color is custom-set.
		/// </summary>
		internal bool IsCustomBackColor;
		/// <summary>
		/// A variable indicating whether the line is hidden, e.g., inside a 
		/// collapsed outlining.
		/// </summary>
		internal bool Hidden;
		/// <summary>
		/// A variable indicating whether the line is highlighted.
		/// </summary>
		internal bool Highlighted;
		/// <summary>
		/// A variable indicating whether the line is readonly.
		/// </summary>
		internal bool IsReadOnly;
		/// <summary>
		/// User defined data for the line.
		/// </summary>
		internal object UserData;

		#endregion

		#region Methods

		/// <summary>
		/// Overloaded constructor. Creates an EditLine object with an empty 
		/// string. Since no coloring information is needed for an empty string, 
		/// the HasUpdatedColor flag is set to be true.
		/// </summary>
		internal EditLine()
		{
			LineString = string.Empty;
			ColorTextData = null;
			IndicatorData = null;
			LinesOccupied = 1;
			HasUpdatedColor = true;
			IsCustomForeColor = false;
			IsCustomBackColor = false;
			Hidden = false;
			Highlighted = false;
			IsReadOnly = false;
			UserData = null;
		}

		/// <summary>
		/// Overloaded constructor. Creates an EditLine object with the 
		/// specified string.
		/// </summary>
		/// <param name="str">The text string for the line.</param>
		internal EditLine(string str)
		{
			LineString = str;
			ColorTextData = null;
			IndicatorData = null;
			LinesOccupied = 1;
			HasUpdatedColor = false;
			IsCustomForeColor = false;
			IsCustomBackColor = false;
			Hidden = false;
			Highlighted = false;
			IsReadOnly = false;
			UserData = null;
		}

		/// <summary>
		/// Overloaded constructor. Creates an EditLine object from the 
		/// specified EditLine object.
		/// </summary>
		/// <param name="editLn">The EditLine object from which the values 
		/// of data members will be copied.</param>
		internal EditLine(EditLine editLn)
		{
			LineString = string.Copy(editLn.LineString);
			if ((editLn.ColorTextData != null) && 
				(editLn.ColorTextData.Length > 0))
			{
				ColorTextData = new short[editLn.ColorTextData.Length];
				editLn.ColorTextData.CopyTo(ColorTextData, 0);
			}
			if ((editLn.IndicatorData != null) && 
				(editLn.IndicatorData.Length > 0))
			{
				IndicatorData = new short[editLn.IndicatorData.Length];
				editLn.IndicatorData.CopyTo(IndicatorData, 0);
			}
			LinesOccupied = editLn.LinesOccupied;
			HasUpdatedColor = editLn.HasUpdatedColor;
			IsCustomForeColor = editLn.IsCustomForeColor;
			IsCustomBackColor = editLn.IsCustomBackColor;
			Hidden = editLn.Hidden;
			Highlighted = editLn.Highlighted;
			IsReadOnly = editLn.IsReadOnly;
		}

		internal void RemoveCustomForeColor()
		{
			if (IndicatorData == null)
			{
				return;
			}
			if (!IsCustomBackColor)
			{
				if (IndicatorData.Length <= 8)
				{
					IndicatorData = null;
				}
				else
				{
					short [] sTemp = new short[IndicatorData.Length - 8];
					for (int i = 8; i < IndicatorData.Length; i++)
					{
						sTemp[i-8] = IndicatorData[i];
					}
					IndicatorData = sTemp;
				}
			}
			IsCustomForeColor = false;
		}

		internal void RemoveCustomBackColor()
		{
			if (IndicatorData == null)
			{
				return;
			}
			if (!IsCustomForeColor)
			{
				if (IndicatorData.Length <= 8)
				{
					IndicatorData = null;
				}
				else
				{
					short [] sTemp = new short[IndicatorData.Length - 8];
					for (int i = 8; i < IndicatorData.Length; i++)
					{
						sTemp[i-8] = IndicatorData[i];
					}
					IndicatorData = sTemp;
				}
			}
			IsCustomBackColor = false;
		}

		/// <summary>
		/// Gets or sets the custom forecolor.
		/// </summary>
		internal Color CustomForeColor
		{
			get
			{
				if (IsCustomForeColor)
				{
					return Color.FromArgb(-IndicatorData[0], -IndicatorData[1], 
						-IndicatorData[2], -IndicatorData[3]);
				}
				return Color.Black;
			}
			set
			{
				if (IsCustomForeColor || IsCustomBackColor)
				{
					IndicatorData[0] = (short)-((short)value.A);
					IndicatorData[1] = (short)-((short)value.R);
					IndicatorData[2] = (short)-((short)value.G);
					IndicatorData[3] = (short)-((short)value.B);
				}
				else
				{
					if (IndicatorData == null)
					{
						IndicatorData = new short[8];
						IndicatorData[0] = (short)(-(short)value.A);
						IndicatorData[1] = (short)(-(short)value.R);
						IndicatorData[2] = (short)(-(short)value.G);
						IndicatorData[3] = (short)(-(short)value.B);
					}
					else
					{
						short [] sTemp = new short[IndicatorData.Length + 8];
						sTemp[0] = (short)(-(short)value.A);
						sTemp[1] = (short)(-(short)value.R);
						sTemp[2] = (short)(-(short)value.G);
						sTemp[3] = (short)(-(short)value.B);
						for (int i = 0; i < IndicatorData.Length; i++)
						{
							sTemp[i+8] = IndicatorData[i];
						}
						IndicatorData = sTemp;
					}
				}
				IsCustomForeColor = true;
			}
		}

		/// <summary>
		/// Gets or sets the custom backcolor.
		/// </summary>
		internal Color CustomBackColor
		{
			get
			{
				if (IsCustomBackColor)
				{
					return Color.FromArgb(-IndicatorData[4], -IndicatorData[5], 
						-IndicatorData[6], -IndicatorData[7]);
				}
				return Color.White;
			}
			set
			{
				if (IsCustomForeColor || IsCustomBackColor)
				{
					IndicatorData[4] = (short)(-(short)value.A);
					IndicatorData[5] = (short)(-(short)value.R);
					IndicatorData[6] = (short)(-(short)value.G);
					IndicatorData[7] = (short)(-(short)value.B);
				}
				else
				{
					if (IndicatorData == null)
					{
						IndicatorData = new short[8];
						IndicatorData[4] = (short)(-(short)value.A);
						IndicatorData[5] = (short)(-(short)value.R);
						IndicatorData[6] = (short)(-(short)value.G);
						IndicatorData[7] = (short)(-(short)value.B);
					}
					else
					{
						short [] sTemp = new short[IndicatorData.Length + 8];
						sTemp[4] = (short)(-(short)value.A);
						sTemp[5] = (short)(-(short)value.R);
						sTemp[6] = (short)(-(short)value.G);
						sTemp[7] = (short)(-(short)value.B);
						for (int i = 0; i < IndicatorData.Length; i++)
						{
							sTemp[i+8] = IndicatorData[i];
						}
						IndicatorData = sTemp;
					}
				}
				IsCustomBackColor = true;
			}
		}

		#endregion
	}
}
