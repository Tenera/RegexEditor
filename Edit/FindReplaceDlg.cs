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
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Summary description for FindReplaceDlg.
	/// </summary>
	internal class FindReplaceDlg : System.Windows.Forms.Form
	{
		private EditView editView;
		private EditLocationRange lcrFoundOld;
		private bool bFoundMessageDisplayed;
		private int editOriginalMarkAllTop;
		private int offsetY;
		private bool bFound;
		private System.Windows.Forms.CheckBox checkBoxMatchCase;
		private System.Windows.Forms.CheckBox checkBoxSearchHiddenText;
		private System.Windows.Forms.CheckBox checkBoxMatchWholeWord;
		private System.Windows.Forms.CheckBox checkBoxUse;
		private System.Windows.Forms.CheckBox checkBoxSearchUp;
		private System.Windows.Forms.ComboBox comboBoxUse;
		private System.Windows.Forms.Button buttonReplace;
		private System.Windows.Forms.Button buttonMarkAll;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonFindNext;
		private System.Windows.Forms.Button buttonReplaceAll;
		private System.Windows.Forms.Button buttonFindEx;
		private System.Windows.Forms.Button buttonReplaceEx;
		private System.Windows.Forms.ComboBox comboBoxFindWhat;
		private System.Windows.Forms.ComboBox comboBoxReplaceWith;
		private System.Windows.Forms.CheckBox checkBoxSelectionOnly;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.ContextMenu contextMenu2;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.ContextMenu contextMenu3;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItem30;
		private System.Windows.Forms.MenuItem menuItem31;
		private System.Windows.Forms.MenuItem menuItem32;
		private System.Windows.Forms.MenuItem menuItem33;
		private System.Windows.Forms.MenuItem menuItem34;
		private System.Windows.Forms.MenuItem menuItem35;
		private System.Windows.Forms.Label labelReplaceWith;
		private System.Windows.Forms.Label labelFindWhat;
		private System.ComponentModel.IContainer components;

		internal FindReplaceDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			comboBoxUse.SelectedIndex = 0;
			editOriginalMarkAllTop = buttonMarkAll.Top;
			offsetY = editOriginalMarkAllTop - buttonReplaceAll.Top;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FindReplaceDlg));
			this.buttonReplaceAll = new System.Windows.Forms.Button();
			this.buttonMarkAll = new System.Windows.Forms.Button();
			this.checkBoxUse = new System.Windows.Forms.CheckBox();
			this.labelReplaceWith = new System.Windows.Forms.Label();
			this.checkBoxSearchUp = new System.Windows.Forms.CheckBox();
			this.comboBoxReplaceWith = new System.Windows.Forms.ComboBox();
			this.buttonReplaceEx = new System.Windows.Forms.Button();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.checkBoxMatchCase = new System.Windows.Forms.CheckBox();
			this.labelFindWhat = new System.Windows.Forms.Label();
			this.comboBoxFindWhat = new System.Windows.Forms.ComboBox();
			this.checkBoxMatchWholeWord = new System.Windows.Forms.CheckBox();
			this.checkBoxSearchHiddenText = new System.Windows.Forms.CheckBox();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonReplace = new System.Windows.Forms.Button();
			this.buttonFindNext = new System.Windows.Forms.Button();
			this.buttonFindEx = new System.Windows.Forms.Button();
			this.comboBoxUse = new System.Windows.Forms.ComboBox();
			this.checkBoxSelectionOnly = new System.Windows.Forms.CheckBox();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.contextMenu2 = new System.Windows.Forms.ContextMenu();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuItem25 = new System.Windows.Forms.MenuItem();
			this.contextMenu3 = new System.Windows.Forms.ContextMenu();
			this.menuItem26 = new System.Windows.Forms.MenuItem();
			this.menuItem27 = new System.Windows.Forms.MenuItem();
			this.menuItem28 = new System.Windows.Forms.MenuItem();
			this.menuItem29 = new System.Windows.Forms.MenuItem();
			this.menuItem30 = new System.Windows.Forms.MenuItem();
			this.menuItem31 = new System.Windows.Forms.MenuItem();
			this.menuItem32 = new System.Windows.Forms.MenuItem();
			this.menuItem33 = new System.Windows.Forms.MenuItem();
			this.menuItem34 = new System.Windows.Forms.MenuItem();
			this.menuItem35 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// buttonReplaceAll
			// 
			this.buttonReplaceAll.Location = new System.Drawing.Point(408, 60);
			this.buttonReplaceAll.Name = "buttonReplaceAll";
			this.buttonReplaceAll.TabIndex = 15;
			this.buttonReplaceAll.Text = "Replace &All";
			this.buttonReplaceAll.Click += new System.EventHandler(this.buttonReplaceAll_Click);
			// 
			// buttonMarkAll
			// 
			this.buttonMarkAll.Location = new System.Drawing.Point(408, 88);
			this.buttonMarkAll.Name = "buttonMarkAll";
			this.buttonMarkAll.TabIndex = 16;
			this.buttonMarkAll.Text = "&Mark All";
			this.buttonMarkAll.Click += new System.EventHandler(this.buttonMarkAll_Click);
			// 
			// checkBoxUse
			// 
			this.checkBoxUse.Enabled = false;
			this.checkBoxUse.Location = new System.Drawing.Point(8, 112);
			this.checkBoxUse.Name = "checkBoxUse";
			this.checkBoxUse.Size = new System.Drawing.Size(48, 16);
			this.checkBoxUse.TabIndex = 8;
			this.checkBoxUse.Text = "Us&e";
			this.checkBoxUse.Visible = false;
			this.checkBoxUse.CheckedChanged += new System.EventHandler(this.checkBoxUse_CheckedChanged);
			// 
			// labelReplaceWith
			// 
			this.labelReplaceWith.Location = new System.Drawing.Point(8, 36);
			this.labelReplaceWith.Name = "labelReplaceWith";
			this.labelReplaceWith.Size = new System.Drawing.Size(84, 14);
			this.labelReplaceWith.TabIndex = 3;
			this.labelReplaceWith.Text = "Re&place with:";
			// 
			// checkBoxSearchUp
			// 
			this.checkBoxSearchUp.Location = new System.Drawing.Point(208, 64);
			this.checkBoxSearchUp.Name = "checkBoxSearchUp";
			this.checkBoxSearchUp.Size = new System.Drawing.Size(124, 16);
			this.checkBoxSearchUp.TabIndex = 10;
			this.checkBoxSearchUp.Text = "Search &up";
			// 
			// comboBoxReplaceWith
			// 
			this.comboBoxReplaceWith.DropDownWidth = 280;
			this.comboBoxReplaceWith.Location = new System.Drawing.Point(96, 32);
			this.comboBoxReplaceWith.Name = "comboBoxReplaceWith";
			this.comboBoxReplaceWith.Size = new System.Drawing.Size(280, 21);
			this.comboBoxReplaceWith.TabIndex = 4;
			// 
			// buttonReplaceEx
			// 
			this.buttonReplaceEx.Enabled = false;
			this.buttonReplaceEx.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonReplaceEx.Image")));
			this.buttonReplaceEx.ImageIndex = 0;
			this.buttonReplaceEx.ImageList = this.imageList1;
			this.buttonReplaceEx.Location = new System.Drawing.Point(380, 32);
			this.buttonReplaceEx.Name = "buttonReplaceEx";
			this.buttonReplaceEx.Size = new System.Drawing.Size(18, 20);
			this.buttonReplaceEx.TabIndex = 5;
			this.buttonReplaceEx.Click += new System.EventHandler(this.buttonReplaceEx_Click);
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.White;
			// 
			// checkBoxMatchCase
			// 
			this.checkBoxMatchCase.Location = new System.Drawing.Point(8, 64);
			this.checkBoxMatchCase.Name = "checkBoxMatchCase";
			this.checkBoxMatchCase.Size = new System.Drawing.Size(124, 16);
			this.checkBoxMatchCase.TabIndex = 6;
			this.checkBoxMatchCase.Text = "Match &case";
			// 
			// labelFindWhat
			// 
			this.labelFindWhat.Location = new System.Drawing.Point(8, 8);
			this.labelFindWhat.Name = "labelFindWhat";
			this.labelFindWhat.Size = new System.Drawing.Size(84, 14);
			this.labelFindWhat.TabIndex = 0;
			this.labelFindWhat.Text = "Fi&nd what:";
			// 
			// comboBoxFindWhat
			// 
			this.comboBoxFindWhat.DropDownWidth = 256;
			this.comboBoxFindWhat.Location = new System.Drawing.Point(96, 4);
			this.comboBoxFindWhat.Name = "comboBoxFindWhat";
			this.comboBoxFindWhat.Size = new System.Drawing.Size(280, 21);
			this.comboBoxFindWhat.TabIndex = 1;
			this.comboBoxFindWhat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxFindWhat_KeyPress);
			this.comboBoxFindWhat.TextChanged += new System.EventHandler(this.comboBoxFindWhat_TextChanged);
			// 
			// checkBoxMatchWholeWord
			// 
			this.checkBoxMatchWholeWord.Location = new System.Drawing.Point(8, 88);
			this.checkBoxMatchWholeWord.Name = "checkBoxMatchWholeWord";
			this.checkBoxMatchWholeWord.Size = new System.Drawing.Size(124, 16);
			this.checkBoxMatchWholeWord.TabIndex = 7;
			this.checkBoxMatchWholeWord.Text = "Match &whole word";
			// 
			// checkBoxSearchHiddenText
			// 
			this.checkBoxSearchHiddenText.Enabled = false;
			this.checkBoxSearchHiddenText.Location = new System.Drawing.Point(208, 112);
			this.checkBoxSearchHiddenText.Name = "checkBoxSearchHiddenText";
			this.checkBoxSearchHiddenText.Size = new System.Drawing.Size(124, 16);
			this.checkBoxSearchHiddenText.TabIndex = 12;
			this.checkBoxSearchHiddenText.Text = "Search &hidden text";
			this.checkBoxSearchHiddenText.Visible = false;
			// 
			// buttonClose
			// 
			this.buttonClose.Location = new System.Drawing.Point(408, 116);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.TabIndex = 17;
			this.buttonClose.Text = "Close";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonReplace
			// 
			this.buttonReplace.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonReplace.Image")));
			this.buttonReplace.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonReplace.ImageIndex = 1;
			this.buttonReplace.ImageList = this.imageList1;
			this.buttonReplace.Location = new System.Drawing.Point(408, 32);
			this.buttonReplace.Name = "buttonReplace";
			this.buttonReplace.TabIndex = 14;
			this.buttonReplace.Text = "&Replace";
			this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
			// 
			// buttonFindNext
			// 
			this.buttonFindNext.Location = new System.Drawing.Point(408, 4);
			this.buttonFindNext.Name = "buttonFindNext";
			this.buttonFindNext.TabIndex = 13;
			this.buttonFindNext.Text = "&Find Next";
			this.buttonFindNext.Click += new System.EventHandler(this.buttonFindNext_Click);
			// 
			// buttonFindEx
			// 
			this.buttonFindEx.Enabled = false;
			this.buttonFindEx.Image = ((System.Drawing.Bitmap)(resources.GetObject("buttonFindEx.Image")));
			this.buttonFindEx.ImageIndex = 0;
			this.buttonFindEx.ImageList = this.imageList1;
			this.buttonFindEx.Location = new System.Drawing.Point(380, 4);
			this.buttonFindEx.Name = "buttonFindEx";
			this.buttonFindEx.Size = new System.Drawing.Size(18, 20);
			this.buttonFindEx.TabIndex = 2;
			this.buttonFindEx.Click += new System.EventHandler(this.buttonFindEx_Click);
			// 
			// comboBoxUse
			// 
			this.comboBoxUse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxUse.DropDownWidth = 128;
			this.comboBoxUse.Enabled = false;
			this.comboBoxUse.Items.AddRange(new object[] {
															 "Regular expressions",
															 "Wildcards"});
			this.comboBoxUse.Location = new System.Drawing.Point(60, 112);
			this.comboBoxUse.Name = "comboBoxUse";
			this.comboBoxUse.Size = new System.Drawing.Size(128, 21);
			this.comboBoxUse.TabIndex = 9;
			this.comboBoxUse.Visible = false;
			this.comboBoxUse.SelectedIndexChanged += new System.EventHandler(this.comboBoxUse_SelectedIndexChanged);
			// 
			// checkBoxSelectionOnly
			// 
			this.checkBoxSelectionOnly.Enabled = false;
			this.checkBoxSelectionOnly.Location = new System.Drawing.Point(208, 88);
			this.checkBoxSelectionOnly.Name = "checkBoxSelectionOnly";
			this.checkBoxSelectionOnly.Size = new System.Drawing.Size(124, 16);
			this.checkBoxSelectionOnly.TabIndex = 11;
			this.checkBoxSelectionOnly.Text = "Se&lection only";
			this.checkBoxSelectionOnly.Visible = false;
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem1,
																						 this.menuItem2,
																						 this.menuItem3,
																						 this.menuItem4,
																						 this.menuItem5,
																						 this.menuItem6,
																						 this.menuItem7,
																						 this.menuItem8,
																						 this.menuItem9,
																						 this.menuItem10,
																						 this.menuItem11,
																						 this.menuItem12,
																						 this.menuItem13,
																						 this.menuItem14,
																						 this.menuItem15,
																						 this.menuItem16,
																						 this.menuItem17,
																						 this.menuItem18,
																						 this.menuItem19,
																						 this.menuItem20});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = ". Any single character";
			this.menuItem1.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "* Zero or more";
			this.menuItem2.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.Text = "+ One or more";
			this.menuItem3.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.Text = "^ Beginning of line";
			this.menuItem5.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 5;
			this.menuItem6.Text = "$ End of line";
			this.menuItem6.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 6;
			this.menuItem7.Text = "< Beginning of word";
			this.menuItem7.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 7;
			this.menuItem8.Text = "> End of word";
			this.menuItem8.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 8;
			this.menuItem9.Text = "\\n Line break";
			this.menuItem9.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 9;
			this.menuItem10.Text = "-";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 10;
			this.menuItem11.Text = "[ ] Any one character in the set";
			this.menuItem11.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 11;
			this.menuItem12.Text = "[^ ] Any one character not in the set";
			this.menuItem12.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 12;
			this.menuItem13.Text = "| Or";
			this.menuItem13.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 13;
			this.menuItem14.Text = "\\ Escapse Special Character";
			this.menuItem14.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 14;
			this.menuItem15.Text = "{ } Tag expression";
			this.menuItem15.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 15;
			this.menuItem16.Text = "-";
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 16;
			this.menuItem17.Text = ":! C/C++ identifier";
			this.menuItem17.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 17;
			this.menuItem18.Text = ":q Quoted string";
			this.menuItem18.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 18;
			this.menuItem19.Text = ":b Space or Tab";
			this.menuItem19.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 19;
			this.menuItem20.Text = ":z Integer";
			this.menuItem20.Click += new System.EventHandler(this.ContextMenu1Item_Click);
			// 
			// contextMenu2
			// 
			this.contextMenu2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem21,
																						 this.menuItem22,
																						 this.menuItem23,
																						 this.menuItem24,
																						 this.menuItem25});
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 0;
			this.menuItem21.Text = "* Zero or more of any character";
			this.menuItem21.Click += new System.EventHandler(this.ContextMenu2Item_Click);
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 1;
			this.menuItem22.Text = "? Any single character";
			this.menuItem22.Click += new System.EventHandler(this.ContextMenu2Item_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 2;
			this.menuItem23.Text = "# Any single digit";
			this.menuItem23.Click += new System.EventHandler(this.ContextMenu2Item_Click);
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 3;
			this.menuItem24.Text = "[ ] Any one character in the set";
			this.menuItem24.Click += new System.EventHandler(this.ContextMenu2Item_Click);
			// 
			// menuItem25
			// 
			this.menuItem25.Index = 4;
			this.menuItem25.Text = "[! ] Any one character not in the set";
			this.menuItem25.Click += new System.EventHandler(this.ContextMenu2Item_Click);
			// 
			// contextMenu3
			// 
			this.contextMenu3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.menuItem26,
																						 this.menuItem27,
																						 this.menuItem28,
																						 this.menuItem29,
																						 this.menuItem30,
																						 this.menuItem31,
																						 this.menuItem32,
																						 this.menuItem33,
																						 this.menuItem34,
																						 this.menuItem35});
			// 
			// menuItem26
			// 
			this.menuItem26.Index = 0;
			this.menuItem26.Text = "Find What Text";
			this.menuItem26.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem27
			// 
			this.menuItem27.Index = 1;
			this.menuItem27.Text = "Tagged Expression 1";
			this.menuItem27.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem28
			// 
			this.menuItem28.Index = 2;
			this.menuItem28.Text = "Tagged Expression 2";
			this.menuItem28.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem29
			// 
			this.menuItem29.Index = 3;
			this.menuItem29.Text = "Tagged Expression 3";
			this.menuItem29.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem30
			// 
			this.menuItem30.Index = 4;
			this.menuItem30.Text = "Tagged Expression 4";
			this.menuItem30.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem31
			// 
			this.menuItem31.Index = 5;
			this.menuItem31.Text = "Tagged Expression 5";
			this.menuItem31.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem32
			// 
			this.menuItem32.Index = 6;
			this.menuItem32.Text = "Tagged Expression 6";
			this.menuItem32.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem33
			// 
			this.menuItem33.Index = 7;
			this.menuItem33.Text = "Tagged Expression 7";
			this.menuItem33.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem34
			// 
			this.menuItem34.Index = 8;
			this.menuItem34.Text = "Tagged Expression 8";
			this.menuItem34.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// menuItem35
			// 
			this.menuItem35.Index = 9;
			this.menuItem35.Text = "Tagged Expression 9";
			this.menuItem35.Click += new System.EventHandler(this.ContextMenu3Item_Click);
			// 
			// FindReplaceDlg
			// 
			this.AcceptButton = this.buttonFindNext;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(490, 147);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.checkBoxSelectionOnly,
																		  this.buttonReplaceEx,
																		  this.buttonFindEx,
																		  this.comboBoxReplaceWith,
																		  this.labelReplaceWith,
																		  this.buttonReplaceAll,
																		  this.buttonClose,
																		  this.buttonMarkAll,
																		  this.buttonReplace,
																		  this.buttonFindNext,
																		  this.comboBoxFindWhat,
																		  this.comboBoxUse,
																		  this.checkBoxUse,
																		  this.checkBoxSearchUp,
																		  this.checkBoxSearchHiddenText,
																		  this.checkBoxMatchWholeWord,
																		  this.checkBoxMatchCase,
																		  this.labelFindWhat});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FindReplaceDlg";
			this.ShowInTaskbar = false;
			this.Text = "Find/Replace";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.FindReplaceDlg_Closing);
			this.VisibleChanged += new System.EventHandler(this.This_VisibleChanged);
			this.ResumeLayout(false);

		}
		#endregion

		private void This_VisibleChanged(object sender, System.EventArgs e)
		{
			if (this.Visible)
			{
				comboBoxFindWhat.Select();
			}
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
			this.Owner.Activate();
		}

		private void buttonFindNext_Click(object sender, System.EventArgs e)
		{
			if (editView != null)
			{
				AddToSearchedList(comboBoxFindWhat.Text);
				bool bUseRegex = false;
				bool bUseWildcards = false;
				if (checkBoxUse.Checked) 
				{
					if (comboBoxUse.Text == "Regular expressions")
					{
						bUseRegex = true;
						bUseWildcards = false;
					}
					else if (comboBoxUse.Text == "Wildcards")
					{
						bUseRegex = false;
						bUseWildcards = true;
					}
				}
				EditLocationRange lcrFound = editView.Find(comboBoxFindWhat.Text, 
					checkBoxMatchCase.Checked, checkBoxMatchWholeWord.Checked, 
					checkBoxSearchHiddenText.Checked, checkBoxSearchUp.Checked, 
					bUseRegex, bUseWildcards, true);
				if (lcrFound == EditLocationRange.Empty)
				{
					EditControl.ShowInfoMessage(
						editView.Edit.GetResourceString("TextNotFound"), 
						editView.Edit.GetResourceString("SearchResults"));
				}
				else
				{
					int xTemp;
					int yTemp;
					editView.GetPoint(lcrFound.End, out xTemp, out yTemp);
					Point ptTemp = editView.PointToScreen(new Point(xTemp, yTemp));
					if (this.Bounds.Contains(ptTemp))
					{
						if (Screen.PrimaryScreen.WorkingArea.Contains(new 
							Rectangle(ptTemp.X + 8, this.Top, this.Width, this.Height)))
						{
							this.Left = ptTemp.X + 8;
						}
						else if (Screen.PrimaryScreen.WorkingArea.Contains(new 
							Rectangle(this.Left, ptTemp.Y - 8 - this.Height, 
							this.Width, this.Height)))
						{
							this.Top = ptTemp.Y - 8 - this.Height;
						}
						else 
						{
							editView.GetPoint(lcrFound.Start, out xTemp, out yTemp);
							ptTemp = editView.PointToScreen(new Point(xTemp, yTemp));
							if (Screen.PrimaryScreen.WorkingArea.Contains(new 
								Rectangle(ptTemp.X - 8 - this.Width, this.Top, 
								this.Width, this.Height)))
							{
								this.Left = ptTemp.X - 8 - this.Width;
							}
							else
							{
								this.Left = 1;
								this.Top = 1;
							}
						}
					}
					if (lcrFoundOld == lcrFound)
					{
						if (!bFoundMessageDisplayed)
						{
							EditControl.ShowInfoMessage(
								editView.Edit.GetResourceString("ReachedStartingPoint"), 
								editView.Edit.GetResourceString("SearchResults"));
							bFoundMessageDisplayed = true;
						}
						else
						{
							bFoundMessageDisplayed = false;
						}
					}
					else
					{
						if (lcrFoundOld == EditLocationRange.Empty)
						{
							lcrFoundOld = lcrFound;
						}
						bFoundMessageDisplayed = false;
					}
					bFound = true;
				}
			}
		}

		private void comboBoxFindWhat_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r')
			{
				if (editView != null)
				{
					if (!editView.FindNext(comboBoxFindWhat.Text, 
						checkBoxSearchUp.Checked))
					{
						EditControl.ShowInfoMessage(
							editView.Edit.GetResourceString("TextNotFound"), 
							editView.Edit.GetResourceString("SearchResults"));
					}
				}
			}
		}

		private void buttonFindEx_Click(object sender, System.EventArgs e)
		{
			if (comboBoxUse.Text == "Regular expressions")
			{
				contextMenu1.Show(buttonFindEx, 
					new Point(buttonFindEx.Width, 0));
			}
			else if (comboBoxUse.Text == "Wildcards")
			{
				contextMenu2.Show(buttonFindEx, 
					new Point(buttonFindEx.Width, 0));
			}
		}

		private void buttonReplaceEx_Click(object sender, System.EventArgs e)
		{
			if (comboBoxUse.Text == "Regular expressions")
			{
				contextMenu3.Show(buttonReplaceEx, 
					new Point(buttonReplaceEx.Width, 0));
			}
		}

		private void buttonReplace_Click(object sender, System.EventArgs e)
		{
			if (buttonMarkAll.Top != editOriginalMarkAllTop)
			{
				SetToReplaceState();
			}
			else
			{
				if (!bFound)
				{
					if (editView.Edit.HasSelection)
					{
						if (editView.Edit.SelectedText == comboBoxFindWhat.Text)
						{
							string strTemp = GetReplacedString(editView.Edit.SelectedText);
							editView.Edit.ReplaceSelection(strTemp);
						}
					}
					buttonFindNext_Click(sender, e);
				}
				else
				{
					if (bFound)
					{
						string strTemp = GetReplacedString(editView.Edit.SelectedText);
						editView.Edit.ReplaceSelection(strTemp);
						buttonFindNext_Click(sender, e);
					}
				}
			}
		}

		private void checkBoxUse_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBoxUse.Checked)
			{
				comboBoxUse.Enabled = true;
				if (comboBoxUse.Text == "Regular expressions")
				{
					buttonFindEx.Enabled = true;
					buttonReplaceEx.Enabled = true;
				}
				else if (comboBoxUse.Text == "Wildcards")
				{
					buttonFindEx.Enabled = true;
					buttonReplaceEx.Enabled = false;
				}
			}
			else
			{
				comboBoxUse.Enabled = false;
				buttonFindEx.Enabled = false;
				buttonReplaceEx.Enabled = false;
			}
		}

		private void comboBoxUse_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (checkBoxUse.Checked)
			{
				if (comboBoxUse.Text == "Regular expressions")
				{
					buttonFindEx.Enabled = true;
					buttonReplaceEx.Enabled = true;
				}
				else if (comboBoxUse.Text == "Wildcards")
				{
					buttonFindEx.Enabled = true;
					buttonReplaceEx.Enabled = false;
				}
			}
		}

		private void buttonMarkAll_Click(object sender, System.EventArgs e)
		{
			AddToSearchedList(comboBoxFindWhat.Text);
			editView.Edit.MarkAll(comboBoxFindWhat.Text, checkBoxMatchCase.Checked,
				checkBoxMatchWholeWord.Checked);
		}

		private void ContextMenu1Item_Click(object sender, System.EventArgs e)
		{
			string itemText = ((MenuItem)sender).Text.Substring(0, 2);
			int oldSelectionStart = comboBoxFindWhat.SelectionStart;
			string strTemp = string.Empty;
			switch (itemText)
			{
				case ". ":
					strTemp = ".";
					break;
				case "* ":
					strTemp = "*";
					break;
				case "+ ":
					strTemp = "+";
					break;
				case "^ ":
					strTemp = "^";
					break;
				case "$ ":
					strTemp = "$";
					break;
				case "< ":
					strTemp = "<";
					break;
				case "> ":
					strTemp = ">";
					break;
				case "\\n":
					strTemp = "\\n";
					break;
				case "[ ":
					strTemp = "[]";
					break;
				case "[^":
					strTemp = "[^]";
					break;
				case "| ":
					strTemp = "|";
					break;
				case "\\ ":
					strTemp = "\\";
					break;
				case "{ ":
					strTemp = "{}";
					break;
				case ":!":
					strTemp = ":!";
					break;
				case ":q":
					strTemp = ":q";
					break;
				case ":b":
					strTemp = ":b";
					break;
				case ":z":
					strTemp = ":z";
					break;
				default:
					break;
			}
//			comboBoxFindWhat.Text = 
//				comboBoxFindWhat.Text.Insert(oldSelectionStart, strTemp);
			comboBoxFindWhat.Text = 
				comboBoxFindWhat.Text + strTemp;

			comboBoxFindWhat.Focus();
			comboBoxFindWhat.SelectionStart = oldSelectionStart + strTemp.Length;
			comboBoxFindWhat.SelectionLength = 0;
		}

		private void ContextMenu2Item_Click(object sender, System.EventArgs e)
		{
			string itemText = ((MenuItem)sender).Text.Substring(0, 2);
			int oldSelectionStart = comboBoxFindWhat.SelectionStart;
			string strTemp = string.Empty;
			switch (itemText)
			{
				case "* ":
					strTemp = "*";
					break;
				case "? ":
					strTemp = "?";
					break;
				case "# ":
					strTemp = "#";
					break;
				case "[ ":
					strTemp = "[]";
					break;
				case "[!":
					strTemp = "[!]";
					break;
				default:
					break;
			}
//			comboBoxFindWhat.Text = 
//				comboBoxFindWhat.Text.Insert(oldSelectionStart, strTemp);
			comboBoxFindWhat.Text = 
				comboBoxFindWhat.Text + strTemp;

			comboBoxFindWhat.Focus();
			comboBoxFindWhat.SelectionStart = oldSelectionStart + strTemp.Length;
			comboBoxFindWhat.SelectionLength = 0;
		}

		private void ContextMenu3Item_Click(object sender, System.EventArgs e)
		{
			string itemText = ((MenuItem)sender).Text.
				Substring(((MenuItem)sender).Text.Length - 1, 1);
			int oldSelectionStart = comboBoxFindWhat.SelectionStart;
			string strTemp = string.Empty;
			switch (itemText)
			{
				case "t":
					strTemp = "\\0";
					break;
				case "1":
					strTemp = "\\1";
					break;
				case "2":
					strTemp = "\\2";
					break;
				case "3":
					strTemp = "\\3";
					break;
				case "4":
					strTemp = "\\4";
					break;
				case "5":
					strTemp = "\\5";
					break;
				case "6":
					strTemp = "\\6";
					break;
				case "7":
					strTemp = "\\7";
					break;
				case "8":
					strTemp = "\\8";
					break;
				case "9":
					strTemp = "\\9";
					break;
				default:
					break;
			}
//			comboBoxReplaceWith.Text = 
//				comboBoxReplaceWith.Text.Insert(oldSelectionStart, strTemp);
			comboBoxReplaceWith.Text =
				comboBoxReplaceWith.Text + strTemp;

			comboBoxReplaceWith.Focus();
			comboBoxReplaceWith.SelectionStart = oldSelectionStart + strTemp.Length;
			comboBoxReplaceWith.SelectionLength = 0;
		}

		private void comboBoxFindWhat_TextChanged(object sender, System.EventArgs e)
		{
			CheckButtonEnabledState();
			lcrFoundOld = EditLocationRange.Empty;
			bFoundMessageDisplayed = false;
		}

		internal void SetFindText(string str)
		{
			comboBoxFindWhat.Text = str;
		}

		internal void SetState(EditView view, string str, bool bReplace)
		{
			this.editView = view;
			comboBoxFindWhat.Text = str;
			bFound = false;
			if (bReplace)
			{
				SetToReplaceState();
			}
			else
			{
				SetToFindState();
			}
		}

		private void SetToFindState()
		{
			if (buttonMarkAll.Top == editOriginalMarkAllTop)
			{
				this.Height -= offsetY;
				labelReplaceWith.Visible = false;
				comboBoxReplaceWith.Visible = false;
				buttonReplaceEx.Visible = false;
				buttonReplaceAll.Visible = false;
				buttonReplace.ImageIndex = 1;
				buttonMarkAll.Top -= offsetY;
				buttonClose.Top -= offsetY;
				checkBoxMatchCase.Top -= offsetY;
				checkBoxMatchWholeWord.Top -= offsetY;
				checkBoxUse.Top -= offsetY;
				comboBoxUse.Top -= offsetY;
				checkBoxSearchHiddenText.Top -= offsetY;
				checkBoxSearchUp.Top -= offsetY;
				checkBoxSelectionOnly.Top -= offsetY;
				CheckButtonEnabledState();
			}
		}

		private void SetToReplaceState()
		{
			if (buttonMarkAll.Top != editOriginalMarkAllTop)
			{
				this.Height += offsetY;
				labelReplaceWith.Visible = true;
				comboBoxReplaceWith.Visible = true;
				buttonReplaceEx.Visible = true;
				buttonReplaceAll.Visible = true;
				buttonReplace.ImageIndex = -1;
				buttonMarkAll.Top += offsetY;
				buttonClose.Top += offsetY;
				checkBoxMatchCase.Top += offsetY;
				checkBoxMatchWholeWord.Top += offsetY;
				checkBoxUse.Top += offsetY;
				comboBoxUse.Top += offsetY;
				checkBoxSearchHiddenText.Top += offsetY;
				checkBoxSearchUp.Top += offsetY;
				checkBoxSelectionOnly.Top += offsetY;
				if (comboBoxFindWhat.Text == string.Empty)
				{
					buttonReplace.Enabled = false;
				}
				comboBoxFindWhat.Focus();
			}
		}

		private void CheckButtonEnabledState()
		{
			if (comboBoxFindWhat.Text == string.Empty)
			{
				buttonFindNext.Enabled = false;
				if (buttonMarkAll.Top == editOriginalMarkAllTop)
				{
					buttonReplace.Enabled = false;
				}
				else
				{
					buttonReplace.Enabled = true;
				}
				buttonReplaceAll.Enabled = false;
				buttonMarkAll.Enabled = false;
			}
			else
			{
				buttonFindNext.Enabled = true;
				buttonReplace.Enabled = true;
				buttonReplaceAll.Enabled = true;
				buttonMarkAll.Enabled = true;
			}
		}

		internal void AddToSearchedList(string str)
		{
			if (!comboBoxFindWhat.Items.Contains(comboBoxFindWhat.Text))
			{
				comboBoxFindWhat.Items.Insert(0, comboBoxFindWhat.Text);
			}
		}

		private void buttonReplaceAll_Click(object sender, System.EventArgs e)
		{
			editView.Edit.ReplaceAll(comboBoxFindWhat.Text, 
				comboBoxReplaceWith.Text,
				checkBoxMatchCase.Checked,
				checkBoxMatchWholeWord.Checked);
		}

		private string GetReplacedString(string strOld)
		{
			bool bMatchCase = checkBoxMatchCase.Checked;
			bool bMatchWholeWord = checkBoxMatchWholeWord.Checked;
			bool bUseRegex = false;
			bool bUseWildcards = false;
			if (checkBoxUse.Checked) 
			{
				if (comboBoxUse.Text == "Regular expressions")
				{
					bUseRegex = true;
					bUseWildcards = false;
				}
				else if (comboBoxUse.Text == "Wildcards")
				{
					bUseRegex = false;
					bUseWildcards = true;
				}
			}
			else
			{
				return comboBoxReplaceWith.Text;
			}
			string str = comboBoxFindWhat.Text;
			string str2 = comboBoxReplaceWith.Text;
			RegexOptions rgOpt = RegexOptions.Singleline;
			if (!bMatchCase)
			{
				rgOpt |= RegexOptions.IgnoreCase;
			}
			if (!bUseRegex)
			{
				if (bUseWildcards)
				{
					str = editView.Edit.Data.GetRegExpFromWildcards(str);
				}
				else
				{
					str = Regex.Escape(str);
				}
			}
			else
			{
				str = str.Replace("{", "(");
				str = str.Replace("}", ")");
				str2 = str2.Replace("\\0", "$0");
				str2 = str2.Replace("\\1", "$1");
				str2 = str2.Replace("\\2", "$2");
				str2 = str2.Replace("\\3", "$3");
				str2 = str2.Replace("\\4", "$4");
				str2 = str2.Replace("\\5", "$5");
				str2 = str2.Replace("\\6", "$6");
				str2 = str2.Replace("\\7", "$7");
				str2 = str2.Replace("\\8", "$8");
				str2 = str2.Replace("\\9", "$9");
			}
			if (bMatchWholeWord)
			{
				str = "\\w" + str + "\\w";
			}
			return Regex.Replace(strOld, str, str2, rgOpt);
		}

		private void FindReplaceDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}

		/// <summary>
		/// Sets text for dialog items.
		/// </summary>
		internal void SetDialogItemText(EditControl edit)
		{
			this.Text = edit.GetResourceString("DialogItemFindReplace"); // "Find/Replace";
			labelFindWhat.Text = edit.GetResourceString("DialogItemFindWhat"); // "Fi&nd what:";
			labelReplaceWith.Text = edit.GetResourceString("DialogItemReplaceWith"); // "Re&place with:";
			checkBoxMatchCase.Text = edit.GetResourceString("DialogItemMatchCase"); // "Match &case"
			checkBoxMatchWholeWord.Text = edit.GetResourceString("DialogItemMatchWholeWord"); // "Match &whole word"
			checkBoxUse.Text = edit.GetResourceString("DialogItemUse"); // "Us&e"
			checkBoxSearchUp.Text = edit.GetResourceString("DialogItemSearchUp"); // "Search &up"
			checkBoxSelectionOnly.Text = edit.GetResourceString("DialogItemSelectionOnly"); // "Se&lection only"
			checkBoxSearchHiddenText.Text = edit.GetResourceString("DialogItemSearchHiddenText"); // "Search &hidden text"
			buttonFindNext.Text = edit.GetResourceString("DialogItemFindNext"); // "&Find Next"
			buttonReplace.Text = edit.GetResourceString("DialogItemReplace"); // "&Replace"
			buttonReplaceAll.Text = edit.GetResourceString("DialogItemReplaceAll"); // "Replace &All"
			buttonMarkAll.Text = edit.GetResourceString("DialogItemMarkAll"); // "&Mark All"
			buttonClose.Text = edit.GetResourceString("DialogItemClose"); // "Close"
		}

		internal string TextSearched
		{
			get
			{
				return comboBoxFindWhat.Text;
			}
			set
			{
				comboBoxFindWhat.Text = value;
			}
		}
	}
}
