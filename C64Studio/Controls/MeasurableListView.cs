﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GR.Image;

namespace RetroDevStudio.Controls
{
  public class MeasurableListView : System.Windows.Forms.ListView
  {
    public struct MEASUREITEMSTRUCT
    {
      public int CtlType;
      public int CtlID;
      public int itemID;
      public int itemWidth;
      public int itemHeight;
      public IntPtr itemData;
    }

    public enum ScrollBarConstants
    {
      /// <summary>
      /// The horizontal scroll bar of the specified window
      /// </summary>
      SB_HORZ = 0,
      /// <summary>
      /// The vertical scroll bar of the specified window
      /// </summary>
      SB_VERT = 1,
      /// <summary>
      /// A scroll bar control
      /// </summary>
      SB_CTL = 2,
      /// <summary>
      /// The horizontal and vertical scroll bars of the specified window
      /// </summary>
      SB_BOTH = 3
    }

    public enum ScrollInfoMask : uint
    {
      SIF_RANGE = 0x1,
      SIF_PAGE = 0x2,
      SIF_POS = 0x4,
      SIF_DISABLENOSCROLL = 0x8,
      SIF_TRACKPOS = 0x10,
      SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS),
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct SCROLLINFO
    {
      public uint cbSize;
      public uint fMask;
      public int nMin;
      public int nMax;
      public uint nPage;
      public int nPos;
      public int nTrackPos;
    }

    [DllImport( "user32.dll", SetLastError = true )]
    public static extern bool GetScrollInfo( IntPtr hwnd, int nBar, ref SCROLLINFO lpsi );

    [StructLayout( LayoutKind.Sequential )]
    public struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
    }

    public struct DrawItemStruct
    {
      public int ctlType;
      public int ctlID;
      public int itemID;
      public int itemAction;
      public int itemState;
      public IntPtr hWndItem;
      public IntPtr hDC;
      public RECT  rcItem;
      public IntPtr itemData;
    }

    private enum ReflectedMessages
    {
      OCM__BASE = ( 0x0400 + 0x1c00 ),
      OCM_DRAWITEM = ( OCM__BASE + 0x002B ),
    }

    public new event System.Windows.Forms.DrawItemEventHandler DrawItem;
    public event MeasureItemEventHandler MeasureItem;
    public const int LVS_OWNERDRAWFIXED      = 0x0400;
    private int m_ItemHeight = 14;
    private DrawMode drawMode;

    public List<System.Drawing.Font>      ItemFonts = new List<Font>();



    public int ItemHeight
    {
      get
      { 
        return m_ItemHeight;
      }
      set
      {
        m_ItemHeight = (int)( value * DPIHandler.DPIY / 96.0f + 0.5f );
      }
    }



    public MeasurableListView()
    {
      this.drawMode = DrawMode.Normal;
    } 



    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.Style |= ( drawMode != DrawMode.Normal ) ? LVS_OWNERDRAWFIXED : 0;
        return cp;
      }
    }

    public virtual DrawMode DrawMode
    {
      get
      {
        return drawMode;
      }
      set
      {
        drawMode = value;
      }
    }



    protected virtual void OnDrawItem( System.Windows.Forms.DrawItemEventArgs e )
    {
      if ( Items[e.Index].Selected )
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Highlight, e.Bounds );
      }
      else
      {
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Window, e.Bounds );
      }

      var format = new StringFormat
        {
          FormatFlags = StringFormatFlags.NoWrap,
          Trimming = StringTrimming.EllipsisCharacter
        };

      var info = new SCROLLINFO();
      info.fMask  = (uint)ScrollInfoMask.SIF_ALL;
      info.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf( info );
      GetScrollInfo( this.Handle, (int)ScrollBarConstants.SB_HORZ, ref info );
      int x = -info.nTrackPos;
      for ( int i = 0; i < Columns.Count; ++i )
      {
        Font  fontToUse = Font;

        if ( i < ItemFonts.Count )
        {
          fontToUse = ItemFonts[i];
        }

        var subItemBounds = Items[e.Index].SubItems[i].Bounds;
        subItemBounds.X = x;
        subItemBounds.Width = Columns[i].Width;
        x += subItemBounds.Width;
        e.Graphics.Clip = new Region( subItemBounds );
        if ( Items[e.Index].Selected )
        {
          e.Graphics.DrawString( Items[e.Index].SubItems[i].Text, fontToUse, System.Drawing.SystemBrushes.HighlightText, subItemBounds, format );
        }
        else
        {
          e.Graphics.DrawString( Items[e.Index].SubItems[i].Text, fontToUse, System.Drawing.SystemBrushes.WindowText, subItemBounds, format );
        }
      }      

      if ( ( e.State & DrawItemState.Focus ) != 0 )
      {
        e.DrawFocusRectangle();
      }
    }



    public virtual void OnMeasureItem( MeasureItemEventArgs e )
    {
      e.ItemHeight = ItemHeight;
    }



    protected override void WndProc( ref System.Windows.Forms.Message m )
    {
      try
      {
        base.WndProc( ref m );
      }
      catch ( Exception )
      {
        // oh boy -> clicking outside the right would throw an exception inside the bowels of WinForms otherwise
      }

      switch ( m.Msg )
      {
        case (int)ReflectedMessages.OCM_DRAWITEM:
          {
            DrawItemStruct dis = (DrawItemStruct)m.GetLParam( typeof( DrawItemStruct ) );

            Graphics graph = Graphics.FromHdc( dis.hDC );
            Rectangle rect = new Rectangle( dis.rcItem.left, dis.rcItem.top, dis.rcItem.right - dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top );
            int index = dis.itemID;
            DrawItemState state = DrawItemState.None;

            System.Windows.Forms.DrawItemEventArgs e = new System.Windows.Forms.DrawItemEventArgs( graph, Font, rect, index, state, ForeColor, BackColor );
            if ( this.DrawItem != null )
            {
              this.DrawItem( this, e );
            }
            OnDrawItem( e );

            graph.Dispose();
            break;
          }
        case 8236:
          WmReflectMeasureItem( ref m );
          break;

      }
    }

    private void WmReflectMeasureItem( ref Message m )
    {
      Graphics graphics1;
      MeasureItemEventArgs args1;
      MEASUREITEMSTRUCT measureitemstruct1 = (MEASUREITEMSTRUCT)m.GetLParam( typeof( MEASUREITEMSTRUCT ) );
      if ( ( ( this.drawMode == DrawMode.OwnerDrawVariable ) 
      ||     ( this.drawMode == DrawMode.OwnerDrawFixed ) )
      &&   ( measureitemstruct1.itemID >= 0 ) )
      {
        graphics1 = Graphics.FromHwnd( this.Handle );
        args1 = new MeasureItemEventArgs( graphics1, measureitemstruct1.itemID, 20 );
        try
        {
          if ( this.MeasureItem != null )
          {
            this.MeasureItem( this, args1 );
          }
          this.OnMeasureItem( args1 );
          measureitemstruct1.itemHeight = args1.ItemHeight;
        }
        finally
        {
          graphics1.Dispose();
        }
      }

      Marshal.StructureToPtr( measureitemstruct1, m.LParam, false );
      m.Result = ( (IntPtr)1 );
    }
  }
}
