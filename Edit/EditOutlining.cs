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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditOutlining class represents outlining text blocks.
	/// </summary>
	internal class EditOutlining : EditLocationRange
	{
		#region Data Members

		/// <summary>
		/// The associated EditData object.
		/// </summary>
		private EditData editData;

		/// <summary>
		/// The outlining tag.
		/// </summary>
		internal EditOutliningTagInfo OutliningTag;
		/// <summary>
		/// A value indicating whether the outlining is collapsed.
		/// </summary>
		private bool bCollapsed;

		/// <summary>
		/// The parent outlining object.
		/// </summary>
		internal EditOutlining ParentOutlining;
		/// <summary>
		/// The list of child outlining objects.
		/// </summary>
		internal ArrayList ChildOutliningList;

		/// <summary>
		/// The level of internal outlining.
		/// </summary>
		internal int InternalLevel = 0;

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new EditOutlining object from the specified values 
		/// for data members.
		/// </summary>
		/// <param name="editData">The associated EditData object.</param>
		/// <param name="start">The starting location.</param>
		/// <param name="end">The ending location.</param>
		/// <param name="outliningTag">The outlining tag.</param>
		internal EditOutlining(EditData editData, EditLocation start, 
			EditLocation end, EditOutliningTagInfo outliningTag)
		{
			this.editData = editData;
			this.Start = start;
			this.End = end;
			this.OutliningTag = outliningTag;
			this.bCollapsed = false;
			this.ParentOutlining = null;
			this.ChildOutliningList = new ArrayList();
		}

		/// <summary>
		/// Updates the information of the current outlining and its descendants
		/// due to an insertion.
		/// </summary>
		/// <param name="lcrNorm">The normalized location range of the insertion.
		/// </param>
		/// <returns>A value indicating whether to redraw all the viewport.
		/// </returns>
		internal bool UpdateFromInsertion(EditLocationRange lcrNorm)
		{
			bool bRedrawAll = false;
			UpdateLocationsFromInsertion(lcrNorm);
			for (int i = lcrNorm.Start.L; i <= lcrNorm.End.L; i++)
			{
				editData.LineList[i-1].Hidden = editData.IsHidden(i);
			}
			return bRedrawAll;
		}

		/// <summary>
		/// Updates the information of the current outlining and its descendants
		/// due to a deletion.
		/// </summary>
		/// <param name="lcrNorm">The normalized location range of the deletion.
		/// </param>
		/// <returns>A value indicating whether to redraw all the viewport.
		/// </returns>
		internal bool UpdateFromDeletion(EditLocationRange lcrNorm)
		{
			bool bRedrawAll = false;
			UpdateLocationsFromDeletion(lcrNorm);
			bRedrawAll = RemoveInvalidDescendants();
			editData.LineList[lcrNorm.Start.L-1].Hidden = 
				editData.IsHidden(lcrNorm.Start.L);
			return bRedrawAll;
		}

		/// <summary>
		/// Removes all the invalid descendants. 
		/// </summary>
		internal bool RemoveInvalidDescendants()
		{
			bool bRedrawAll = false;
			for (int i = ChildOutliningList.Count-1; i >= 0; i--)
			{
				EditOutlining otln = (EditOutlining)ChildOutliningList[i];
				if (otln.RemoveInvalidDescendants())
				{
					bRedrawAll = true;
				}
				if (otln.Start == otln.End)
				{
					Remove(otln, false);
				}
				else if (!this.IsRoot)
				{
					if ((otln.Start.L == this.Start.L) 
						|| (otln.End.L == this.End.L))
					{
						Remove(otln, true);
						bRedrawAll = true;
					}
				}
			}
			return bRedrawAll;
		}

		/// <summary>
		/// Updates the locations of the current outlining and its descendants 
		/// after an insertion.
		/// </summary>
		/// <param name="lcr">The location range of the insertion.</param>
		internal void UpdateLocationsFromInsertion(EditLocationRange lcr)
		{
			for (int i = 0; i < ChildOutliningList.Count; i++)
			{
				((EditOutlining)ChildOutliningList[i]).
					UpdateLocationsFromInsertion(lcr);
			}
			if (!this.IsRoot)
			{
				editData.UpdateFromInsertion(ref this.Start, lcr, true);
				editData.UpdateFromInsertion(ref this.End, lcr, false);
			}
		}

		/// <summary>
		/// Updates the locations of the current outlining and its descendants 
		/// after a deletion.
		/// </summary>
		/// <param name="lcr">The location range of the deletion.</param>
		internal void UpdateLocationsFromDeletion(EditLocationRange lcr)
		{
			for (int i = 0; i < ChildOutliningList.Count; i++)
			{
				((EditOutlining)ChildOutliningList[i]).
					UpdateLocationsFromDeletion(lcr);
			}
			if (!this.IsRoot)
			{
				editData.UpdateFromDeletion(ref this.Start, lcr);
				editData.UpdateFromDeletion(ref this.End, lcr);
			}
		}

		/// <summary>
		/// Determines if the specified line is contained within the line 
		/// range of the current outlining.
		/// </summary>
		/// <param name="ln">The line to test.</param>
		/// <returns>true if the specified line is contained in the line range 
		/// of the current outlining; otherwise, false.</returns>
		internal bool Intersects(int ln)
		{
			return ((ln >= Start.L) && (ln <= EndLine));
		}

		/// <summary>
		/// Adds an EditOutlining object to the list of child outlining objects.
		/// </summary>
		/// <param name="otln">An EditOutlining object to be added.</param>
		/// <returns>The index in the child ArrayList at which the new
		/// EditOutlining object has been added.</returns>
		internal int Add(EditOutlining otln)
		{
			otln.ParentOutlining = this;
			if (ChildOutliningList == null)
			{
				ChildOutliningList = new ArrayList();
				return ChildOutliningList.Add(otln);
			}
			else
			{
				int index = ChildOutliningList.BinarySearch(otln);
				if (index < 0)
				{
					index = ~index;
					ChildOutliningList.Insert(index, otln);
				}
				return index;
			}
		}

		/// <summary>
		/// Adds an EditOutlining object to the internal ArrayList.
		/// </summary>
		/// <param name="start">The starting location of the outlining.</param>
		/// <param name="end">The ending location of the outlining.</param>
		/// <param name="oti">The outlining tag.</param>
		/// <returns>The newly added EditOutlining object.</returns>
		internal EditOutlining AddChild(EditLocation start, EditLocation end, 
			EditOutliningTagInfo oti)
		{
			RemoveDescendants(start.L);
			RemoveDescendants(end.L);
			EditOutlining otln = new EditOutlining(editData, start, end, oti);
			for (int i = start.L + 1; i <= end.L - 1; i++)
			{
				EditOutlining otlnTemp = GetLeafOutlining(i);
				if (otlnTemp.ParentOutlining == this)
				{
					otln.Add(otlnTemp);
					i = otlnTemp.EndLine;
					ChildOutliningList.Remove(otlnTemp);
				}
			}
			Add(otln);
			return otln;
		}

		/// <summary>
		/// Removes the specified EditOutlining object.
		/// </summary>
		/// <param name="otln">The EditOutlining object to be removed.</param>
		/// <param name="bKeepDescendants">A value indicating whether to keep
		/// the descendants.</param>
		internal void Remove(EditOutlining otln, bool bKeepDescendants)
		{
			if (otln.ParentOutlining != this)
			{
				return;	
			}
			if (bKeepDescendants)
			{
				ArrayList alTemp = otln.ChildOutliningList;
				ChildOutliningList.Remove(otln);
				for (int i = 0; i < alTemp.Count; i++)
				{
					Add((EditOutlining)alTemp[i]);
				}
			}
			else
			{
				otln.RemoveAllDescendants();
				ChildOutliningList.Remove(otln);
			}
		}

		/// <summary>
		/// Removes all descendants of the current outlining.
		/// </summary>
		internal void RemoveAllDescendants()
		{
			for (int i = ChildOutliningList.Count-1; i >= 0; i--)
			{
				EditOutlining otln = (EditOutlining)ChildOutliningList[i];
				otln.RemoveAllDescendants();
			}
			ChildOutliningList.Clear();
		}

		/// <summary>
		/// Removes the descendants at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the descendants are to be removed.
		/// <param>
		internal void RemoveDescendants(int ln)
		{
			EditOutlining otln = GetChildOutlining(ln);
			if (otln != null)
			{
				otln.RemoveDescendants(ln);
				Remove(otln, true);
			}
		}

		/// <summary>
		/// Removes the descendants in the specified range.
		/// </summary>
		/// <param name="lcr">The specified range in which the descendants are 
		/// to be removed.</param>
		internal void RemoveDescendants(EditLocationRange lcr)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			for (int i = ChildOutliningList.Count-1; i >= 0; i--)
			{
				EditOutlining otln = (EditOutlining)ChildOutliningList[i];
				if (lcrNorm.Contains(otln) || lcrNorm.Intersects(otln))
				{
					otln.RemoveDescendants(lcrNorm);
					Remove(otln, true);
				}
			}
		}

		/// <summary>
		/// Clears the whole internal ArrayList.
		/// </summary>
		internal void Clear()
		{
			RemoveAllDescendants();
			ChildOutliningList.TrimToSize();
		}

		/// <summary>
		/// Tests if the current outlining is an ancestor of the specified 
		/// outlining.
		/// </summary>
		/// <param name="otln">The outlining object to be tested against.</param>
		/// <returns>true if the current outlining is an ancestor of the 
		/// outlining; otherwise, false.</returns>
		internal bool IsAncestorOf(EditOutlining otln)
		{
			EditOutlining otlnTemp = otln;
			while (!otlnTemp.IsRoot)
			{
				if (otlnTemp.ParentOutlining == this)
				{
					return true;
				}
				otlnTemp = otlnTemp.ParentOutlining;
			}
			return false;
		}

		/// <summary>
		/// Tests if the current outlining is a descendant of the specified 
		/// outlining.
		/// </summary>
		/// <param name="otln">The outlining object to be tested against.</param>
		/// <returns>true if the current outlining is a descendant of the 
		/// outlining; otherwise, false.</returns>
		internal bool IsDescendantOf(EditOutlining otln)
		{
			return otln.IsAncestorOf(this);
		}

		/// <summary>
		/// Gets the shared ancestor of the current outlining and the 
		/// specified outlining.
		/// </summary>
		/// <param name="otln">The outlining object to be tested against.</param>
		/// <returns>The shared ancestor of the current outlining and the 
		/// outlining</returns>
		internal EditOutlining GetSharedAncestor(EditOutlining otln)
		{
			if ((this.IsRoot) || (otln.IsRoot))
			{
				return editData.OutliningRoot;
			}
			else if (this == otln)
			{
				return this.ParentOutlining;
			}
			else if (IsAncestorOf(otln))
			{
				return this;
			}
			else if (IsDescendantOf(otln))
			{
				return otln;
			}
			else
			{
				EditOutlining otlnTemp = otln;
				while (!otlnTemp.IsRoot)
				{
					if (otlnTemp.ParentOutlining.IsAncestorOf(this))
					{
						return otlnTemp.ParentOutlining;
					}
					otlnTemp = otlnTemp.ParentOutlining;
				}
				return editData.OutliningRoot;
			}
		}

		/// <summary>
		/// Tests if the specified line is a boundary line of the current 
		/// outlining.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the specified line is a boundary line of the current 
		/// outlining; otherwise, false.</returns>
		internal bool IsBoundaryLine(int ln)
		{
			if (!IsRoot)
			{
				return ((ln == StartLine) || (ln == EndLine));
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the number of collapsed lines.
		/// </summary>
		internal int GetCollapsedLines()
		{
			if (this.bCollapsed)
			{
				return (EndLine - Start.L);
			}
			else
			{
				int nTemp = 0;
				for (int i = 0; i < ChildOutliningList.Count; i++)
				{
					nTemp += ((EditOutlining)ChildOutliningList[i]).GetCollapsedLines();
				}
				return nTemp;
			}
		}

		/// <summary>
		/// Expands all descendant outlining objects.
		/// </summary>
		internal void ExpandAllDescendants()
		{
			for (int i = 0; i < ChildOutliningList.Count; i++)
			{
				((EditOutlining)ChildOutliningList[i]).Collapsed = false;
				((EditOutlining)ChildOutliningList[i]).ExpandAllDescendants();
			}
		}

		/// <summary>
		/// Collapses all descendant outlining objects.
		/// </summary>
		internal void CollapseAllDescendants()
		{
			for (int i = 0; i < ChildOutliningList.Count; i++)
			{
				((EditOutlining)ChildOutliningList[i]).Collapsed = true;
				((EditOutlining)ChildOutliningList[i]).CollapseAllDescendants();
			}
		}

		/// <summary>
		/// Sets the Hidden property for associated lines when the current 
		/// outlining is collapsed/expanded.
		/// </summary>
		/// <param name="bCollapsed">A value indicating the collapsed status
		/// of the current outlining.</param>
		internal void UpdateAssociatedLines(bool bCollapsed)
		{
			if (bCollapsed)
			{
				editData.LineList[Start.L-1].Hidden = HasCollapsedAncestor;
				for (int i = Start.L + 1; i <= EndLine; i++)
				{
					editData.LineList[i-1].Hidden = true;
				}
			}
			else
			{
				for (int i = Start.L; i <= End.L; i++)
				{
					EditOutlining otln = GetLeafOutlining(i);
					if (otln.Collapsed)
					{
						if (i == otln.StartLine)
						{
							editData.LineList[i-1].Hidden = otln.HasCollapsedAncestor;
						}
						else
						{
							editData.LineList[i-1].Hidden = true;
						}
					}
					else
					{
						editData.LineList[i-1].Hidden = otln.HasCollapsedAncestor;
					}
				}
			}
		}

		/// <summary>
		/// Tests if the specified EditOutliningTagInfo is a valid sub-outlining 
		/// of the current outlining.
		/// </summary>
		/// <param name="otli">The EditOutliningTagInfo object to be tested.
		/// </param>
		/// <returns>true if the specified EditOutliningTagInfo is a valid 
		/// sub-outlining of the current outlining; otherwise, false.</returns>
		internal bool IsSubOutlining(EditOutliningTagInfo otli)
		{
			for (int i = 0; i < OutliningTag.SubOutlining.Length; i++)
			{
				if (OutliningTag.SubOutlining[i] == otli.Name)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the collapsed string of the outlining.
		/// </summary>
		/// <param name="chStart">The starting char of the collapsed string.</param>
		/// <param name="chEnd">The ending char of the collapsed string.</param>
		/// <returns>The collapsed string of the outlining.</returns>
		internal string GetCollapsedString(ref int chStart, ref int chEnd)
		{
			chStart = Start.C;
			chEnd = -1;
			string strTemp;
			if (OutliningTag == EditData.CommentOutliningTag)
			{
				strTemp = editData.Edit.CollapsedComments;
			}
			else if (OutliningTag == EditData.ManualOutliningTag)
			{
				strTemp = editData.Edit.CollapsedDefault;
			}
			else
			{
				if (OutliningTag.CollapseAs == "")
				{
					strTemp = editData.GetStringObject(Start.L).Substring(chStart - 1);
				}
				else
				{
					strTemp = OutliningTag.CollapseAs;
				}
			}
			chEnd = chStart + strTemp.Length;
			return strTemp;
		}

		/// <summary>
		/// Gets the leaf outlining at the specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>The leaf EditOutlining object at the specified location.
		/// </returns>
		internal EditOutlining GetLeafOutlining(int ln, int ch)
		{
			return GetLeafOutlining(new EditLocation(ln, ch));
		}

		/// <summary>
		/// Gets the leaf outlining at the specified location.
		/// </summary>
		/// <param name="lc">The specified location.</param>
		/// <returns>The leaf EditOutlining object at the specified location.
		/// </returns>
		internal EditOutlining GetLeafOutlining(EditLocation lc)
		{
			if (Contains(lc))
			{
				EditOutlining outliningTemp = GetChildOutlining(lc);
				if (outliningTemp == null)
				{
					return this;
				}
				else
				{
					return outliningTemp.GetLeafOutlining(lc);
				}
			}
			else
			{
				return editData.OutliningRoot;
			}
		}

		/// <summary>
		/// Gets the leaf outlining at the specified line.
		/// </summary>
		/// <param name="ln">The specified line.</param>
		/// <returns>The leaf EditOutlining object at the specified line.
		/// </returns>
		internal EditOutlining GetLeafOutlining(int ln)
		{
			if (Intersects(ln))
			{
				EditOutlining outliningTemp = GetChildOutlining(ln);
				if (outliningTemp == null)
				{
					return this;
				}
				else
				{
					return outliningTemp.GetLeafOutlining(ln);
				}
			}
			else
			{
				return editData.OutliningRoot;
			}
		}

		/// <summary>
		/// Gets the child outlining object at the specified location.
		/// </summary>
		/// <param name="lc">The location for the child outlining.</param>
		/// <returns>The child outlining at the location.</returns>
		internal EditOutlining GetChildOutlining(EditLocation lc)
		{
			int iMin = 0;
			int iMax = ChildOutliningList.Count - 1;
			int iMid;
			while (iMin <= iMax)
			{
				iMid = (iMin + iMax)/2;
				if (((EditOutlining)ChildOutliningList[iMid]).Contains(lc))
				{
					return (EditOutlining)ChildOutliningList[iMid];
				}
				else if (((EditOutlining)ChildOutliningList[iMid]).Start > lc)
				{
					iMax = iMid - 1;
				}
				else
				{
					iMin = iMid + 1;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the child outlining object at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the child outlining it to be 
		/// obtained.</param>
		/// <returns>The child outlining at the line.</returns>
		internal EditOutlining GetChildOutlining(int ln)
		{
			int iMin = 0;
			int iMax = ChildOutliningList.Count - 1;
			int iMid;
			while (iMin <= iMax)
			{
				iMid = (iMin + iMax)/2;
				if (((EditOutlining)ChildOutliningList[iMid]).Intersects(ln))
				{
					return (EditOutlining)ChildOutliningList[iMid];
				}
				else if (((EditOutlining)ChildOutliningList[iMid]).
					Start.L > ln)
				{
					iMax = iMid - 1;
				}
				else
				{
					iMin = iMid + 1;
				}
			}
			return null;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Returns the child EditOutlining object at the specified index.
		/// </summary>
		internal EditOutlining this[int i]
		{
			get
			{
				return (EditOutlining)ChildOutliningList[i];
			}
			set
			{
				ChildOutliningList[i] = value;
			}
		}

		/// <summary>
		/// Gets the starting line.
		/// </summary>
		internal int StartLine
		{
			get
			{
				return Start.L;
			}
		}

		/// <summary>
		/// Gets the ending line.
		/// </summary>
		internal int EndLine
		{
			get
			{
				return (End.L != -1) ? End.L : editData.LineList.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the current outlining is the 
		/// root outlining.
		/// </summary>
		internal bool IsRoot
		{
			get
			{
				return (this == editData.OutliningRoot);
			}
		}

		/// <summary>
		/// The count of child outlining objects.
		/// </summary>
		internal int ChildCount
		{
			get
			{
				return ChildOutliningList.Count;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current outlining 
		/// is collapsed.
		/// </summary>
		internal bool Collapsed
		{
			get
			{
				return bCollapsed;
			}
			set
			{
				if (bCollapsed != value)
				{
					bCollapsed = value;
					UpdateAssociatedLines(bCollapsed);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any collapsed ancestor.
		/// </summary>
		internal bool HasCollapsedAncestor
		{
			get
			{
				EditOutlining otln = this.ParentOutlining;
				while (!otln.IsRoot)
				{
					if (otln.Collapsed)
					{
						return true;
					}
					otln = otln.ParentOutlining;
				}
				return false;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any expanded descendant.
		/// </summary>
		internal bool HasExpandedDescendant
		{
			get
			{
				for (int i = 0; i < ChildOutliningList.Count; i++)
				{
					EditOutlining otln = (EditOutlining)ChildOutliningList[i];
					if (!otln.Collapsed)
					{
						return true;
					}
					if (otln.HasExpandedDescendant)
					{
						return true;
					}
				}
				return false;
			}
		}

		#endregion
	}
}
