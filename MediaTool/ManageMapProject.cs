using GR.Memory;
using RetroDevStudio.Formats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleMapProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "map project file", ArgParser.Parameter( "TYPE" ), new string[] { "MAPDATA", "MAPDATAVERT", "MAPDATAASM", "MAPDATAVERTASM", "CHARSET" } ) )
      {
        return 1;
      }

      int     count = -1;
      int     firstEntry = 0;
      if ( ArgParser.IsParameterSet( "OFFSET" ) )
      {
        firstEntry = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
      }
      if ( ArgParser.IsParameterSet( "COUNT" ) )
      {
        count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "MAPPROJECT" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + ArgParser.Parameter( "MAPPROJECT" ) );
        return 1;
      }

      var mapProject = new RetroDevStudio.Formats.MapProject();
      if ( !mapProject.ReadFromBuffer( data ) )
      {
        System.Console.WriteLine( "Couldn't read map project from file " + ArgParser.Parameter( "MAPPROJECT" ) );
        return 1;
      }

      GR.Memory.ByteBuffer    resultingData = new GR.Memory.ByteBuffer();

      if ( ArgParser.Parameter( "TYPE" ).Contains( "MAPDATAASM" ) )
      {
        if ( !ValidateFirstAndCount( firstEntry, ref count, mapProject.Maps.Count ) )
        {
          return 1;
        }

        bool isVertical = ArgParser.Parameter( "TYPE" ).Contains( "VERT" );

        for ( int mapIndex = 0; mapIndex < count; ++mapIndex )
        {
          var   map = mapProject.Maps[firstEntry + mapIndex];

          var   mapData = new ByteBuffer( (uint)( map.Tiles.Width * map.Tiles.Height ) );

          if ( isVertical )
          {
            for ( int i = 0; i < 0 + map.Tiles.Width; ++i )
            {
              for ( int j = 0; j < 0 + map.Tiles.Height; ++j )
              {
                mapData.SetU8At( i + j * map.Tiles.Width, (byte)map.Tiles[i, j] );
              }
            }
          }
          else
          {
            for ( int j = 0; j < 0 + map.Tiles.Height; ++j )
            {
              for ( int i = 0; i < 0 + map.Tiles.Width; ++i )
              {
                mapData.SetU8At( i + j * map.Tiles.Width, (byte)map.Tiles[i, j] );
              }
            }
          }
          resultingData.Append( Encoding.ASCII.GetBytes( "MAP_" + map.Name + "\n!hex \"" + mapData.ToString() + "\"\n" ) );
        }
      }
      else if ( ArgParser.Parameter( "TYPE" ).Contains( "MAPDATA" ) )
      {
        if ( !ValidateFirstAndCount( firstEntry, ref count, mapProject.Maps.Count ) )
        {
          return 1;
        }

        bool isVertical = ArgParser.Parameter( "TYPE" ).Contains( "VERT" );

        for ( int mapIndex = 0; mapIndex < count; ++mapIndex )
        {
          var   map = mapProject.Maps[firstEntry + mapIndex];
          if ( isVertical )
          {
            for ( int i = 0; i < 0 + map.Tiles.Width; ++i )
            {
              for ( int j = 0; j < 0 + map.Tiles.Height; ++j )
              {
                resultingData.AppendU8( (byte)map.Tiles[i, j] );
              }
            }
          }
          else
          {
            for ( int j = 0; j < 0 + map.Tiles.Height; ++j )
            {
              for ( int i = 0; i < 0 + map.Tiles.Width; ++i )
              {
                resultingData.AppendU8( (byte)map.Tiles[i, j] );
              }
            }
          }
        }
      }
      else if ( ArgParser.Parameter( "TYPE" ).Contains( "CHARSET" ) )
      {
        if ( !ValidateFirstAndCount( firstEntry, ref count, mapProject.Charset.Characters.Count ) )
        {
          return 1;
        }
        resultingData = new GR.Memory.ByteBuffer( (uint)( count * 8 ) );
        for ( int i = 0; i < count; ++i )
        {
          mapProject.Charset.Characters[firstEntry + i].Tile.Data.CopyTo( resultingData, 0, 8, i * 8 );
        }
      }
      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), resultingData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }




  }
}
