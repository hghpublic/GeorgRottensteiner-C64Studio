namespace RetroDevStudio.Documents
{
  partial class Bookmarks
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bookmarks));
      this.listMessages = new DecentForms.ListControl();
      this.contextMenuBookmarks = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.jumpToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.deleteBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteAllBookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.contextMenuBookmarks.SuspendLayout();
      this.SuspendLayout();
      // 
      // listMessages
      // 
      this.listMessages.BorderStyle = DecentForms.BorderStyle.SUNKEN;
      this.listMessages.ContextMenuStrip = this.contextMenuBookmarks;
      this.listMessages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listMessages.FirstVisibleItemIndex = 0;
      this.listMessages.HasHeader = true;
      this.listMessages.HeaderHeight = 24;
      this.listMessages.ImageList = null;
      this.listMessages.ItemHeight = 15;
      this.listMessages.ListViewItemSorter = null;
      this.listMessages.Location = new System.Drawing.Point(0, 0);
      this.listMessages.Name = "listMessages";
      this.listMessages.ScrollAlwaysVisible = false;
      this.listMessages.SelectedIndex = -1;
      this.listMessages.SelectedItem = null;
      this.listMessages.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listMessages.Size = new System.Drawing.Size(678, 200);
      this.listMessages.SortColumn = -1;
      this.listMessages.SortOrder = DecentForms.SortOrder.NONE;
      this.listMessages.TabIndex = 0;
      this.listMessages.ItemActivate += new DecentForms.EventHandler(this.listMessages_ItemActivate);
      this.listMessages.ColumnClicked += new DecentForms.EventHandler(this.listMessages_ColumnClick);
      // 
      // contextMenuBookmarks
      // 
      this.contextMenuBookmarks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteBookmarkToolStripMenuItem,
            this.deleteAllBookmarksToolStripMenuItem});
      this.contextMenuBookmarks.Name = "contextCompilerMessage";
      this.contextMenuBookmarks.Size = new System.Drawing.Size(185, 76);
      // 
      // jumpToFileToolStripMenuItem
      // 
      this.jumpToFileToolStripMenuItem.Name = "jumpToFileToolStripMenuItem";
      this.jumpToFileToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.jumpToFileToolStripMenuItem.Text = "&Jump to file";
      this.jumpToFileToolStripMenuItem.Click += new System.EventHandler(this.jumpToFileToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
      // 
      // deleteBookmarkToolStripMenuItem
      // 
      this.deleteBookmarkToolStripMenuItem.Name = "deleteBookmarkToolStripMenuItem";
      this.deleteBookmarkToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.deleteBookmarkToolStripMenuItem.Text = "Delete bookmark";
      this.deleteBookmarkToolStripMenuItem.Click += new System.EventHandler(this.deleteBookmarkToolStripMenuItem_Click);
      // 
      // deleteAllBookmarksToolStripMenuItem
      // 
      this.deleteAllBookmarksToolStripMenuItem.Name = "deleteAllBookmarksToolStripMenuItem";
      this.deleteAllBookmarksToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.deleteAllBookmarksToolStripMenuItem.Text = "Delete all bookmarks";
      this.deleteAllBookmarksToolStripMenuItem.Click += new System.EventHandler(this.deleteAllBookmarksToolStripMenuItem_Click);
      // 
      // Bookmarks
      // 
      this.ClientSize = new System.Drawing.Size(678, 200);
      this.Controls.Add(this.listMessages);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Bookmarks";
      this.Text = "Bookmarks";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.contextMenuBookmarks.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private DecentForms.ListControl listMessages;
    private System.Windows.Forms.ContextMenuStrip contextMenuBookmarks;
    private System.Windows.Forms.ToolStripMenuItem jumpToFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem deleteBookmarkToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteAllBookmarksToolStripMenuItem;
  }
}
