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
	/// The EditAction class defines the base abstract class for actions 
	/// in EditControl.
	/// </summary>
	public abstract class EditAction
	{
		/// <summary>
		/// The type of the action.
		/// </summary>
		protected EditActionType editActionType;
		/// <summary>
		/// The associated EditControl object.
		/// </summary>
		protected EditControl edit;
		/// <summary>
		/// Redoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		abstract public bool Redo();
		/// <summary>
		/// Undoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		abstract public bool Undo();

		/// <summary>
		/// Gets or sets the action type.
		/// </summary>
		public EditActionType ActionType
		{
			get
			{
				return editActionType;
			}
			set
			{
				editActionType = value;
			}
		}
	}

	/// <summary>
	/// The EditCompositeAction class defines the base class for composite 
	/// actions in EditControl.
	/// </summary>
	public class EditCompositeAction : EditAction
	{
		/// <summary>
		/// The list of internal actions.
		/// </summary>
		private ArrayList editActionList = new ArrayList();

		/// <summary>
		/// Creates a new EditCompositeAction object with the specified 
		/// action type and EditControl object.
		/// </summary>
		/// <param name="at">The type of the action.</param>
		/// <param name="edit">The EditControl associated with the action.
		/// </param>
		public EditCompositeAction(EditActionType at, EditControl edit)
		{
			this.editActionType = at;
			this.edit = edit;
		}

		/// <summary>
		/// Adds an action to the internal action list.
		/// </summary>
		/// <param name="act">The action to be added.</param>
		public void AddAction(EditAction act)
		{
			editActionList.Add(act);
		}

		/// <summary>
		/// Redoes all the internal actions.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Redo()
		{
			for (int i = 0; i < editActionList.Count; i++)
			{
				if (!((EditAction)editActionList[i]).Redo())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Undoes all the internal actions.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Undo()
		{
			for (int i = editActionList.Count - 1; i >= 0; i--)
			{
				if (!((EditAction)editActionList[i]).Undo())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Gets the count of internal actions.
		/// </summary>
		public int Count
		{
			get
			{
				return editActionList.Count;
			}
		}

		/// <summary>
		/// Gets the list of actions.
		/// </summary>
		public ArrayList ActionList
		{
			get
			{
				return editActionList;
			}
		}
	}

	/// <summary>
	/// The EditActionCaretMove class represents the action of moving caret.
	/// </summary>
	public class EditActionCaretMove : EditAction
	{
		/// <summary>
		/// The old caret location.
		/// </summary>
		public EditLocation LcOld;
		/// <summary>
		/// The new caret location.
		/// </summary>
		public EditLocation LcNew;

		/// <summary>
		/// Creates a new EditActionCaretMove object with the specified 
		/// EditControl object, old caret location, and new caret location.
		/// </summary>
		/// <param name="edit">The EditControl object associated with the 
		/// action.</param>
		/// <param name="lcOld">The old caret location.</param>
		/// <param name="lcNew">The new caret location.</param>
		public EditActionCaretMove(EditControl edit, EditLocation lcOld, 
			EditLocation lcNew)
		{
			this.editActionType = EditActionType.MoveCaret;
			this.edit = edit;
			this.LcOld = new EditLocation(lcOld);
			this.LcNew = new EditLocation(lcNew);
		}

		/// <summary>
		/// Redoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Redo()
		{
			edit.InAction = true;
			edit.CurrentLineChar = LcNew;
			edit.UpdateCaretPos();
			edit.InAction = false;
			return true;
		}

		/// <summary>
		/// Undoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Undo()
		{
			edit.InAction = true;
			edit.CurrentLineChar = LcOld;
			edit.UpdateCaretPos();
			edit.InAction = false;
			return true;
		}
	}

	/// <summary>
	/// The EditActionInsert class represents the action of inserting text.
	/// </summary>
	public class EditActionInsert : EditAction
	{
		/// <summary>
		/// The text string to be inserted.
		/// </summary>
		public string StrText;
		/// <summary>
		/// The starting location where the text string is inserted.
		/// </summary>
		public EditLocation LcStart;
		/// <summary>
		/// The ending location of the text string after the insertion.
		/// </summary>
		public EditLocation LcEnd;

		/// <summary>
		/// Creates a new EditActionInsert object with the specified 
		/// EditControl object, starting location of insertion, and ending 
		/// location of insertion.
		/// </summary>
		/// <param name="edit">The EditControl object associated with the 
		/// action.</param>
		/// <param name="lcStart">The starting location of the insertion.</param>
		/// <param name="lcEnd">The ending location of the insertion.</param>
		public EditActionInsert(EditControl edit, EditLocation lcStart, 
			EditLocation lcEnd)
		{
			this.editActionType = EditActionType.Insert;
			this.edit = edit;
			this.StrText = null;
			this.LcStart = new EditLocation(lcStart);
			this.LcEnd = new EditLocation(lcEnd);
		}

		/// <summary>
		/// Redoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Redo()
		{
			edit.InAction = true;
			edit.Insert(LcStart, StrText);
			this.StrText = null;
			edit.InAction = false;
			return true;
		}

		/// <summary>
		/// Undoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Undo()
		{
			edit.InAction = true;
			this.StrText = edit.GetString(LcStart, LcEnd);
			edit.Delete(LcStart, LcEnd);
			edit.InAction = false;
			return true;
		}
	}

	/// <summary>
	/// The EditActionDelete class represents the action of deleting text.
	/// </summary>
	public class EditActionDelete : EditAction
	{
		/// <summary>
		/// The text string that has been deleted.
		/// </summary>
		public string StrText;
		/// <summary>
		/// The starting location where the text string is deleted.
		/// </summary>
		public EditLocation LcStart;
		/// <summary>
		/// The ending location of the text string before the deletion.
		/// </summary>
		public EditLocation LcEnd;

		/// <summary>
		/// Creates a new EditActionDelete object with the specified 
		/// EditControl object, starting location of deletion, and ending 
		/// location of deletion.
		/// </summary>
		/// <param name="edit">The EditControl object associated with the 
		/// action.</param>
		/// <param name="lcStart">The starting location of the deletion.</param>
		/// <param name="lcEnd">The ending location of the deletion.</param>
		public EditActionDelete(EditControl edit, EditLocation lcStart, 
			EditLocation lcEnd)
		{
			this.editActionType = EditActionType.Delete;
			this.edit = edit;
			this.StrText = edit.GetString(lcStart, lcEnd);
			this.LcStart = new EditLocation(lcStart);
			this.LcEnd = new EditLocation(lcEnd);
		}

		/// <summary>
		/// Redoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Redo()
		{
			edit.InAction = true;
			this.StrText = edit.GetString(LcStart, LcEnd);
			edit.Delete(LcStart, LcEnd);
			edit.InAction = false;
			return true;
		}

		/// <summary>
		/// Undoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Undo()
		{
			edit.InAction = true;
			edit.Insert(LcStart, StrText);
			this.StrText = null;
			edit.InAction = false;
			return true;
		}
	}

	/// <summary>
	/// The EditActionSelect class represents the action of selecting text.
	/// </summary>
	public class EditActionSelect : EditAction
	{
		/// <summary>
		/// The starting location of selection.
		/// </summary>
		public EditLocation LcStart;
		/// <summary>
		/// The ending location of selection.
		/// </summary>
		public EditLocation LcEnd;
		/// <summary>
		/// A value indicating whether the selection is linewise.
		/// </summary>
		public bool IsLineWise;

		/// <summary>
		/// Creates a new EditActionSelect object with the specified 
		/// EditControl object, starting location of selection, ending
		/// location of selection, and selection type.
		/// </summary>
		/// <param name="edit">The EditControl object associated with the 
		/// action.</param>
		/// <param name="lcStart">The starting location of the selection.</param>
		/// <param name="lcEnd">The ending location of the selection.</param>
		/// <param name="isLineWise">The type of the selection.</param>
		public EditActionSelect(EditControl edit, EditLocation lcStart, 
			EditLocation lcEnd, bool isLineWise)
		{
			this.editActionType = EditActionType.SelectText;
			this.edit = edit;
			this.LcStart = new EditLocation(lcStart);
			this.LcEnd = new EditLocation(lcEnd);
			this.IsLineWise = isLineWise;
		}

		/// <summary>
		/// Redoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Redo()
		{
			edit.InAction = true;
			edit.Select(LcStart, LcEnd, IsLineWise);
			edit.CurrentLineChar = LcEnd;
			edit.UpdateCaretPos();
			edit.InAction = false;
			return true;
		}

		/// <summary>
		/// Undoes the action.
		/// </summary>
		/// <returns>true if the execution is successful; otherwise, false.
		/// </returns>
		public override bool Undo()
		{
			edit.InAction = true;
			edit.UnSelect();
			edit.CurrentLineChar = LcStart;
			edit.UpdateCaretPos();
			edit.InAction = false;
			return true;
		}
	}
}
