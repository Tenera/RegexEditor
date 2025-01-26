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
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The delegate for the UserMarginPaint event.
	/// </summary>
	public delegate void UserMarginPaintEventHandler(object sender, 
		UserMarginPaintEventArgs e);

	/// <summary>
	/// The delegate for the ContextTooltipPopup event.
	/// </summary>
	public delegate void ContextTooltipPopupEventHandler(object sender,
		ContextTooltipPopupEventArgs e);

	/// <summary>
	/// The delegate for the ContextChoicePopup event.
	/// </summary>
	public delegate void ContextChoicePopupEventHandler(object sender,
		ContextChoicePopupEventArgs e);

	/// <summary>
	/// The delegate for the ContextPromptPopup event.
	/// </summary>
	public delegate void ContextPromptPopupEventHandler(object sender,
		ContextPromptPopupEventArgs e);

	/// <summary>
	/// The delegate for the ContentChanged event.
	/// </summary>
	public delegate void ContentChangedEventHandler(object sender, 
		ContentChangedEventArgs e);

	/// <summary>
	/// The delegate for the OutliningCollapsedChanged event.
	/// </summary>
	public delegate void OutliningCollapsedChangedEventHandler(object sender, 
		OutliningCollapsedChangedEventArgs e);

	/// <summary>
	/// The delegate for the DoubleClickSelect event.
	/// </summary>
	public delegate void DoubleClickSelectEventHandler(object sender, 
		DoubleClickSelectEventArgs e);

	/// <summary>
	/// The delegate for the LineInfoUpdate event.
	/// </summary>
	public delegate void LineInfoUpdateEventHandler(object sender, 
		LineInfoUpdateEventArgs e);

	/// <summary>
	/// The arguments for the UserMarginPaint event.
	/// </summary>
	public class UserMarginPaintEventArgs : PaintEventArgs
	{
		/// <summary>
		/// The line at which the user margin is to be painted.
		/// </summary>
		private int line;

		/// <summary>
		/// Creates a UserMarginPaintEventArgs object with the specified values
		/// for data members.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="clipRect">The rectangle to paint in.</param>
		/// <param name="line">The line at which the user margin is to be 
		/// painted.</param>
		public UserMarginPaintEventArgs(Graphics g, Rectangle clipRect, int line)
			: base(g, clipRect)
		{
			this.line = line;
		}

		/// <summary>
		/// The line at which the user margin is to be painted.
		/// </summary>
		public int Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the ContextTooltipPopup event.
	/// </summary>
	public class ContextTooltipPopupEventArgs : EventArgs
	{
		/// <summary>
		/// The name of the item.
		/// </summary>
		private string itemName;
		/// <summary>
		/// The location of the cursor.
		/// </summary>
		private EditLocation location;
		/// <summary>
		/// The tooltip for the item.
		/// </summary>
		private string itemTooltip;

		/// <summary>
		/// Creates a ContextTooltipPopupEventArgs object with the specified 
		/// values for data members.
		/// </summary>
		/// <param name="itemName">The name of the item.</param>
		/// <param name="location">The location of the cursor.</param>
		public ContextTooltipPopupEventArgs(string itemName, EditLocation location)
		{
			this.itemName = itemName;
			this.location = location;
		}

		/// <summary>
		/// The name of the item.
		/// </summary>
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
			}
		}

		/// <summary>
		/// The location of the cursor.
		/// </summary>
		public EditLocation Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}

		/// <summary>
		/// The tooltip for the item.
		/// </summary>
		public string ItemTooltip
		{
			get
			{
				return this.itemTooltip;
			}
			set
			{
				this.itemTooltip = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the ContextChoicePopup event.
	/// </summary>
	public class ContextChoicePopupEventArgs : EventArgs
	{	
		/// <summary>
		/// The context choice char after the item.
		/// </summary>
		private string contextChoiceChar;
		/// <summary>
		/// The name of the item.
		/// </summary>
		private string itemName;
		/// <summary>
		/// The location after the context choice char.
		/// </summary>
		private EditLocation location;

		/// <summary>
		/// The context choices for the item.
		/// </summary>
		private ArrayList choices = null;
		/// <summary>
		/// The image indexes for the context choices for the item.
		/// </summary>
		private ArrayList imageIndexes = null;
		/// <summary>
		/// The imagelist for the listbox.
		/// </summary>
		private ImageList itemImageList = null;
		/// <summary>
		/// The font for the listbox.
		/// </summary>
		private Font itemFont = null;
		/// <summary>
		/// The number of items per page in the listbox.
		/// </summary>
		private int itemsPerPage = -1;

		/// <summary>
		/// Creates a ContextChoicePopupEventArgs object with the specified 
		/// values for data members.
		/// </summary>
		/// <param name="contextChoiceChar">The context choice char after the 
		/// item.</param>
		/// <param name="itemName">The name of the item.</param>
		/// <param name="location">The location after the context choice char.
		/// </param>
		public ContextChoicePopupEventArgs(string contextChoiceChar, 
			string itemName, EditLocation location)
		{
			this.contextChoiceChar = contextChoiceChar;
			this.itemName = itemName;
			this.location = location;
		}

		/// <summary>
		/// The context choice char after the item.
		/// </summary>
		public string ContextChoiceChar
		{
			get
			{
				return this.contextChoiceChar;
			}
			set
			{
				this.contextChoiceChar = value;
			}
		}

		/// <summary>
		/// The name of the item.
		/// </summary>
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
			}
		}

		/// <summary>
		/// The location after the context choice char.
		/// </summary>
		public EditLocation Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}

		/// <summary>
		/// The context choices for the item.
		/// </summary>
		public ArrayList Choices
		{
			get
			{
				return this.choices;
			}
			set
			{
				this.choices = value;
			}
		}

		/// <summary>
		/// The image indexes for the context choices for the item.
		/// </summary>
		public ArrayList ImageIndexes
		{
			get
			{
				return this.imageIndexes;
			}
			set
			{
				this.imageIndexes = value;
			}
		}

		/// <summary>
		/// The imagelist for the listbox.
		/// </summary>
		public ImageList ItemImageList
		{
			get
			{
				return this.itemImageList;
			}
			set
			{
				this.itemImageList = value;
			}
		}

		/// <summary>
		/// The font for the listbox.
		/// </summary>
		public Font ItemFont
		{
			get
			{
				return this.itemFont;
			}
			set
			{
				this.itemFont = value;
			}
		}

		/// <summary>
		/// The number of items per page in the listbox.
		/// </summary>
		public int ItemsPerPage
		{
			get
			{
				return this.itemsPerPage;
			}
			set
			{
				this.itemsPerPage = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the ContextPromptPopup event.
	/// </summary>
	public class ContextPromptPopupEventArgs : EventArgs
	{
		/// <summary>
		/// The name of the item.
		/// </summary>
		private string itemName;
		/// <summary>
		/// The location after the context prompt begin char.
		/// </summary>
		private EditLocation location;
		/// <summary>
		/// The context prompts for the item.
		/// </summary>
		private ArrayList prompts = null;

		/// <summary>
		/// Creates a ContextPromptPopupEventArgs object with the specified 
		/// values for data members.
		/// </summary>
		/// <param name="itemName">The name of the item.</param>
		/// <param name="location">The location after the context prompt begin 
		/// char.</param>
		public ContextPromptPopupEventArgs(string itemName, EditLocation location)
		{
			this.itemName = itemName;
			this.location = location;
		}

		/// <summary>
		/// The name of the item.
		/// </summary>
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
			}
		}

		/// <summary>
		/// The location after the context prompt begin char.
		/// </summary>
		public EditLocation Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}

		/// <summary>
		/// The context prompts for the item.
		/// </summary>
		public ArrayList Prompts
		{
			get
			{
				return this.prompts;
			}
			set
			{
				this.prompts = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the ContentChanged event.
	/// </summary>
	public class ContentChangedEventArgs : EventArgs
	{
		/// <summary>
		/// The text involved in the change.
		/// </summary>
		private string text;
		/// <summary>
		/// The location range of the change.
		/// </summary>
		private EditLocationRange locationRange;
		/// <summary>
		/// A value indicating whether the change is an insertion.
		/// </summary>
		private bool bIsInsertion;

		/// <summary>
		/// Creates a ContentChangedEventArgs object with the specified 
		/// values for data members.
		/// </summary>
		/// <param name="text">The text involved in the change.</param>
		/// <param name="locationRange">The location range of the change.</param>
		/// <param name="bIsInsertion">A value indicating whether the change 
		/// is an insertion.</param>
		public ContentChangedEventArgs(string text, 
			EditLocationRange locationRange, bool bIsInsertion)
		{
			this.text = text;
			this.locationRange = locationRange;
			this.bIsInsertion = bIsInsertion;
		}

		/// <summary>
		/// The text involved in the change.
		/// </summary>
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		/// <summary>
		/// The location range of the change.
		/// </summary>
		public EditLocationRange LocationRange
		{
			get
			{
				return this.locationRange;
			}
			set
			{
				this.locationRange = value;
			}
		}

		/// <summary>
		/// A value indicating whether the change is an insertion.
		/// </summary>
		public bool IsInsertion
		{
			get
			{
				return this.bIsInsertion;
			}
			set
			{
				this.bIsInsertion = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the OutliningCollapsedChanged event.
	/// </summary>
	public class OutliningCollapsedChangedEventArgs : EventArgs
	{
		/// <summary>
		/// The location range of the outlining.
		/// </summary>
		private EditLocationRange locationRange;
		/// <summary>
		/// The current collapsed state of the outlining.
		/// </summary>
		private bool bIsCollapsed;

		/// <summary>
		/// Creates a ContentChangedEventArgs object with the specified 
		/// values for data members.
		/// </summary>
		/// <param name="locationRange">The location range of the outlining.
		/// </param>
		/// <param name="bIsCollapsed">The current collapsed state of the 
		/// outlining.</param>
		public OutliningCollapsedChangedEventArgs(EditLocationRange locationRange, bool bIsCollapsed)
		{
			this.locationRange = new EditLocationRange(locationRange);
			this.bIsCollapsed = bIsCollapsed;
		}

		/// <summary>
		/// The location range of the outlining.
		/// </summary>
		public EditLocationRange LocationRange
		{
			get
			{
				return this.locationRange;
			}
			set
			{
				this.locationRange = value;
			}
		}

		/// <summary>
		/// The current collapsed state of the outlining.
		/// </summary>
		public bool IsCollapsed
		{
			get
			{
				return this.bIsCollapsed;
			}
			set
			{
				this.bIsCollapsed = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the DoubleClickSelect event.
	/// </summary>
	public class DoubleClickSelectEventArgs : EventArgs
	{
		/// <summary>
		/// The location of the double-click.
		/// </summary>
		private EditLocation location;
		/// <summary>
		/// The location range of double-click selected text.
		/// </summary>
		private EditLocationRange locationRange = null;

		/// <summary>
		/// Creates a DoubleClickSelectEventArgs object with the specified 
		/// values for data members.
		/// </summary>
		/// <param name="location">The location of double-click.</param>
		public DoubleClickSelectEventArgs(EditLocation location)
		{
			this.location = new EditLocation(location);
		}

		/// <summary>
		/// The location of the double-click.
		/// </summary>
		public EditLocationRange LocationRange
		{
			get
			{
				return this.locationRange;
			}
			set
			{
				this.locationRange = value;
			}
		}

		/// <summary>
		/// The location range of double-click selected text.
		/// </summary>
		public EditLocation Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}
	}

	/// <summary>
	/// The arguments for the LineInfoUpdate event.
	/// </summary>
	public class LineInfoUpdateEventArgs : EventArgs
	{
		/// <summary>
		/// The line for which information is to be updated.
		/// </summary>
		private int line;
		/// <summary>
		/// A value indicating whether the line information has been fully 
		/// handled, i.e., no automatic parsing is needed.
		/// </summary>
		private bool bHandled = false;

		/// <summary>
		/// Creates a LineInfoUpdate object with the specified values 
		/// for data members.
		/// </summary>
		/// <param name="line">The line for which information is to be updated.
		/// </param>
		public LineInfoUpdateEventArgs(int line)
		{
			this.line = line;
		}

		/// <summary>
		/// The line for which information is to be updated.
		/// </summary>
		public int Line
		{
			get
			{
				return this.line;
			}
			set
			{
				this.line = value;
			}
		}

		/// <summary>
		/// A value indicating whether the line information has been fully 
		/// handled, i.e., no automatic parsing is needed.
		/// </summary>
		public bool Handled
		{
			get
			{
				return this.bHandled;
			}
			set
			{
				this.bHandled = value;
			}
		}
	}
}
