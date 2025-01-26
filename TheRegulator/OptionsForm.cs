using System;
using System.Windows.Forms;
using Regulator.SDK.Proxy;
using Regulator.SDK;
using AppContext = Regulator.SDK.AppContext;

namespace Regulator.GUI
{
	/// <summary>
	/// Summary description for OptionsForm.
	/// </summary>
	public class OptionsForm : System.Windows.Forms.Form,IOptionsDialog
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.CheckBox chkMinimizeToTray;
		private System.Windows.Forms.Label lbllabel1;
		private System.Windows.Forms.Label lbllabel2;
		private System.Windows.Forms.CheckBox chkLogin;
		private System.Windows.Forms.Label lbllabel3;
		private System.Windows.Forms.Label lbllabel4;
		private System.Windows.Forms.Label lbllabel5;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.TextBox txtDomain;
		private System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.CheckBox chkOverrideDefaultProxy;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOk;
		private System.Windows.Forms.CheckBox chkBypassLocal;
		private System.Windows.Forms.Panel pnlpanel1;
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabConnection;
		private System.Windows.Forms.CheckBox chkFillUnNamedCaptures;
		private System.Windows.Forms.Label lblUnNamedCapturesInfo;
		private System.Windows.Forms.Label lblPasswordHelp;
		private System.Windows.Forms.CheckBox chkIntellisenseRegex;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public  DialogResult ShowConnectionOptions()
		{
			tabControl1.SelectedTab= tabConnection;
			return ShowDialog();
		}

		public OptionsForm()
		{
			InitializeComponent();

		}

		private void SetCurrentOptionsFromTheForm()
		{
			ProxyInfo pi = AppContext.Instance.Settings.ProxySettings;
			
			pi.OverrideDefaultProxy=chkOverrideDefaultProxy.Checked;
			pi.RequiresLogin= chkLogin.Checked;
			pi.BypassLocal=chkBypassLocal.Checked;
			
			pi.ProxyName= txtServerName.Text;
			pi.ProxyPort= int.Parse( txtPort.Text);
			pi.Domain = txtDomain.Text;
			pi.UserId = txtUserName.Text;
			pi.Password= txtPassword.Text;

			AppContext.Instance.Settings.MinimizeToTray= chkMinimizeToTray.Checked;
			AppContext.Instance.Settings.FillUnNamedCapturesInTree=chkFillUnNamedCaptures.Checked;
			AppContext.Instance.Settings.IntelliseSenseInRegex= chkIntellisenseRegex.Checked;

			AppContext.Instance.Settings.Save();
			
		}

		private void SetCurrentOptionsInForm()
		{
			ProxyInfo pi = AppContext.Instance.Settings.ProxySettings;
			
			chkOverrideDefaultProxy.Checked=pi.OverrideDefaultProxy;
			chkLogin.Checked= pi.RequiresLogin;
			chkBypassLocal.Checked= pi.BypassLocal;
			
			txtServerName.Text= pi.ProxyName;
			txtPort.Text= pi.ProxyPort.ToString();
			txtDomain.Text = pi.Domain;
			txtUserName.Text = pi.UserId;
			txtPassword.Text= pi.Password;

			SetProxyOptionsEnabledStatuses();

			chkFillUnNamedCaptures.Checked=AppContext.Instance.Settings.FillUnNamedCapturesInTree;
			chkMinimizeToTray.Checked= AppContext.Instance.Settings.MinimizeToTray;
			chkIntellisenseRegex.Checked = AppContext.Instance.Settings.IntelliseSenseInRegex;

		}

		private void SetProxyOptionsEnabledStatuses()
		{
			txtServerName.Enabled= chkOverrideDefaultProxy.Checked;
			txtPort.Enabled= chkOverrideDefaultProxy.Checked;
			chkLogin.Enabled= chkOverrideDefaultProxy.Checked;
			chkBypassLocal.Enabled= chkOverrideDefaultProxy.Checked;

			
			
			txtDomain.Enabled = (chkOverrideDefaultProxy.Checked && chkLogin.Checked);
			txtUserName.Enabled = (chkOverrideDefaultProxy.Checked && chkLogin.Checked);
			txtPassword.Enabled= (chkOverrideDefaultProxy.Checked && chkLogin.Checked);
			lblPasswordHelp.Visible= (chkOverrideDefaultProxy.Checked && chkLogin.Checked);

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkIntellisenseRegex = new System.Windows.Forms.CheckBox();
            this.lblUnNamedCapturesInfo = new System.Windows.Forms.Label();
            this.chkFillUnNamedCaptures = new System.Windows.Forms.CheckBox();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.tabConnection = new System.Windows.Forms.TabPage();
            this.lblPasswordHelp = new System.Windows.Forms.Label();
            this.pnlpanel1 = new System.Windows.Forms.Panel();
            this.chkBypassLocal = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.lbllabel5 = new System.Windows.Forms.Label();
            this.lbllabel4 = new System.Windows.Forms.Label();
            this.lbllabel3 = new System.Windows.Forms.Label();
            this.chkLogin = new System.Windows.Forms.CheckBox();
            this.lbllabel2 = new System.Windows.Forms.Label();
            this.lbllabel1 = new System.Windows.Forms.Label();
            this.chkOverrideDefaultProxy = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabConnection);
            this.tabControl1.Location = new System.Drawing.Point(11, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(364, 294);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.SystemColors.Control;
            this.tabGeneral.Controls.Add(this.chkIntellisenseRegex);
            this.tabGeneral.Controls.Add(this.lblUnNamedCapturesInfo);
            this.tabGeneral.Controls.Add(this.chkFillUnNamedCaptures);
            this.tabGeneral.Controls.Add(this.chkMinimizeToTray);
            this.tabGeneral.Location = new System.Drawing.Point(4, 26);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(356, 264);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            // 
            // chkIntellisenseRegex
            // 
            this.chkIntellisenseRegex.Location = new System.Drawing.Point(11, 146);
            this.chkIntellisenseRegex.Name = "chkIntellisenseRegex";
            this.chkIntellisenseRegex.Size = new System.Drawing.Size(303, 29);
            this.chkIntellisenseRegex.TabIndex = 4;
            this.chkIntellisenseRegex.Text = "Enable Intellisense for Regex Input";
            // 
            // lblUnNamedCapturesInfo
            // 
            this.lblUnNamedCapturesInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblUnNamedCapturesInfo.Location = new System.Drawing.Point(34, 97);
            this.lblUnNamedCapturesInfo.Name = "lblUnNamedCapturesInfo";
            this.lblUnNamedCapturesInfo.Size = new System.Drawing.Size(448, 39);
            this.lblUnNamedCapturesInfo.TabIndex = 3;
            this.lblUnNamedCapturesInfo.Text = "(if unchecked - tree loading will be much faster in some cases, but you will not " +
    "see any sub-captures for groups.)\r\n";
            // 
            // chkFillUnNamedCaptures
            // 
            this.chkFillUnNamedCaptures.Location = new System.Drawing.Point(11, 68);
            this.chkFillUnNamedCaptures.Name = "chkFillUnNamedCaptures";
            this.chkFillUnNamedCaptures.Size = new System.Drawing.Size(247, 29);
            this.chkFillUnNamedCaptures.TabIndex = 2;
            this.chkFillUnNamedCaptures.Text = "Fill unnamed captures in tree";
            // 
            // chkMinimizeToTray
            // 
            this.chkMinimizeToTray.Location = new System.Drawing.Point(11, 19);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(146, 30);
            this.chkMinimizeToTray.TabIndex = 1;
            this.chkMinimizeToTray.Text = "Minimize to tray";
            this.chkMinimizeToTray.CheckedChanged += new System.EventHandler(this.chkMinimizeToTray_CheckedChanged_1);
            // 
            // tabConnection
            // 
            this.tabConnection.BackColor = System.Drawing.SystemColors.Control;
            this.tabConnection.Controls.Add(this.lblPasswordHelp);
            this.tabConnection.Controls.Add(this.pnlpanel1);
            this.tabConnection.Controls.Add(this.chkBypassLocal);
            this.tabConnection.Controls.Add(this.txtPassword);
            this.tabConnection.Controls.Add(this.txtUserName);
            this.tabConnection.Controls.Add(this.txtDomain);
            this.tabConnection.Controls.Add(this.txtPort);
            this.tabConnection.Controls.Add(this.txtServerName);
            this.tabConnection.Controls.Add(this.lbllabel5);
            this.tabConnection.Controls.Add(this.lbllabel4);
            this.tabConnection.Controls.Add(this.lbllabel3);
            this.tabConnection.Controls.Add(this.chkLogin);
            this.tabConnection.Controls.Add(this.lbllabel2);
            this.tabConnection.Controls.Add(this.lbllabel1);
            this.tabConnection.Controls.Add(this.chkOverrideDefaultProxy);
            this.tabConnection.Location = new System.Drawing.Point(4, 26);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Size = new System.Drawing.Size(510, 339);
            this.tabConnection.TabIndex = 1;
            this.tabConnection.Text = "Connection";
            // 
            // lblPasswordHelp
            // 
            this.lblPasswordHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblPasswordHelp.Location = new System.Drawing.Point(314, 175);
            this.lblPasswordHelp.Name = "lblPasswordHelp";
            this.lblPasswordHelp.Size = new System.Drawing.Size(179, 48);
            this.lblPasswordHelp.TabIndex = 16;
            this.lblPasswordHelp.Text = "NOTE: Password will be encrypted in text on your hard drive!";
            this.lblPasswordHelp.Visible = false;
            // 
            // pnlpanel1
            // 
            this.pnlpanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlpanel1.Location = new System.Drawing.Point(-11, 155);
            this.pnlpanel1.Name = "pnlpanel1";
            this.pnlpanel1.Size = new System.Drawing.Size(526, 10);
            this.pnlpanel1.TabIndex = 15;
            // 
            // chkBypassLocal
            // 
            this.chkBypassLocal.Location = new System.Drawing.Point(34, 117);
            this.chkBypassLocal.Name = "chkBypassLocal";
            this.chkBypassLocal.Size = new System.Drawing.Size(268, 29);
            this.chkBypassLocal.TabIndex = 12;
            this.chkBypassLocal.Text = "Bypass proxy for local addresses";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Location = new System.Drawing.Point(157, 301);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(325, 24);
            this.txtPassword.TabIndex = 11;
            // 
            // txtUserName
            // 
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Location = new System.Drawing.Point(157, 272);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(325, 24);
            this.txtUserName.TabIndex = 10;
            // 
            // txtDomain
            // 
            this.txtDomain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDomain.Location = new System.Drawing.Point(157, 243);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(325, 24);
            this.txtDomain.TabIndex = 9;
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Location = new System.Drawing.Point(157, 87);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(67, 24);
            this.txtPort.TabIndex = 8;
            this.txtPort.Text = "80";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtServerName
            // 
            this.txtServerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtServerName.Location = new System.Drawing.Point(157, 58);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(325, 24);
            this.txtServerName.TabIndex = 7;
            // 
            // lbllabel5
            // 
            this.lbllabel5.Location = new System.Drawing.Point(56, 301);
            this.lbllabel5.Name = "lbllabel5";
            this.lbllabel5.Size = new System.Drawing.Size(78, 20);
            this.lbllabel5.TabIndex = 6;
            this.lbllabel5.Text = "Password:";
            // 
            // lbllabel4
            // 
            this.lbllabel4.Location = new System.Drawing.Point(56, 272);
            this.lbllabel4.Name = "lbllabel4";
            this.lbllabel4.Size = new System.Drawing.Size(90, 19);
            this.lbllabel4.TabIndex = 5;
            this.lbllabel4.Text = "User name:";
            // 
            // lbllabel3
            // 
            this.lbllabel3.Location = new System.Drawing.Point(56, 243);
            this.lbllabel3.Name = "lbllabel3";
            this.lbllabel3.Size = new System.Drawing.Size(67, 19);
            this.lbllabel3.TabIndex = 4;
            this.lbllabel3.Text = "Domain:";
            // 
            // chkLogin
            // 
            this.chkLogin.Location = new System.Drawing.Point(34, 175);
            this.chkLogin.Name = "chkLogin";
            this.chkLogin.Size = new System.Drawing.Size(263, 39);
            this.chkLogin.TabIndex = 3;
            this.chkLogin.Text = "My Proxy server requires me to login";
            this.chkLogin.CheckedChanged += new System.EventHandler(this.chkLogin_CheckedChanged);
            // 
            // lbllabel2
            // 
            this.lbllabel2.Location = new System.Drawing.Point(34, 87);
            this.lbllabel2.Name = "lbllabel2";
            this.lbllabel2.Size = new System.Drawing.Size(44, 20);
            this.lbllabel2.TabIndex = 2;
            this.lbllabel2.Text = "Port:";
            // 
            // lbllabel1
            // 
            this.lbllabel1.Location = new System.Drawing.Point(34, 58);
            this.lbllabel1.Name = "lbllabel1";
            this.lbllabel1.Size = new System.Drawing.Size(112, 20);
            this.lbllabel1.TabIndex = 1;
            this.lbllabel1.Text = "Server Name:";
            // 
            // chkOverrideDefaultProxy
            // 
            this.chkOverrideDefaultProxy.Location = new System.Drawing.Point(11, 19);
            this.chkOverrideDefaultProxy.Name = "chkOverrideDefaultProxy";
            this.chkOverrideDefaultProxy.Size = new System.Drawing.Size(303, 30);
            this.chkOverrideDefaultProxy.TabIndex = 0;
            this.chkOverrideDefaultProxy.Text = "Override default proxy settings";
            this.chkOverrideDefaultProxy.CheckedChanged += new System.EventHandler(this.chkOverrideDefaultProxy_CheckedChanged);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdCancel.Location = new System.Drawing.Point(260, 314);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(112, 27);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "&Cancel";
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdOk.Location = new System.Drawing.Point(137, 314);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(112, 27);
            this.cmdOk.TabIndex = 3;
            this.cmdOk.Text = "&Ok";
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // OptionsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(7, 17);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(386, 351);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.Text = "Regulator options";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.OptionsForm_Closing);
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion


		private void OptionsForm_Load(object sender, System.EventArgs e)
		{
			SetCurrentOptionsInForm();
			
		}

		private void chkOverrideDefaultProxy_CheckedChanged(object sender, System.EventArgs e)
		{
			SetProxyOptionsEnabledStatuses();
		}

		private void chkLogin_CheckedChanged(object sender, System.EventArgs e)
		{
			SetProxyOptionsEnabledStatuses();
		}

		private void chkMinimizeToTray_CheckedChanged_1(object sender, System.EventArgs e)
		{
		
		}

		private void OptionsForm_Closing(object sender, System.ComponentModel.CancelEventArgs ce)
		{
			if(DialogResult==DialogResult.OK)
			{
				if(false == ApplyOptions())
				{
					ce.Cancel=true;
				}
			}
		}

		private bool ApplyOptions()
		{
			try
			{
				SetCurrentOptionsFromTheForm();
				return true;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message);
				return false;
			}
		}

		private void cmdOk_Click(object sender, System.EventArgs e)
		{
			
		}

		private void cmbAuthenticationType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
		
		}
		#region IOptionsDialog Members

		void Regulator.SDK.IOptionsDialog.ShowConnectionOptions()
		{
			this.ShowConnectionOptions();
		}

		public void ShowGeneralOptions()
		{
			this.ShowDialog();
		}

		#endregion
	}
}
