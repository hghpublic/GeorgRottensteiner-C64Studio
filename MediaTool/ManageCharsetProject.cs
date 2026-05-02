using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleCharsetProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "charset project", ArgParser.Parameter( "TYPE" ), new string[] { "CHARS" } ) )
      {
        return 1;
      }

      var charsetProject = new RetroDevStudio.Formats.CharsetProject();

      if ( !charsetProject.ReadFromBuffer( GR.IO.File.ReadAllBytes( ArgParser.Parameter( "CHARSETPROJECT" ) ) ) )
      {
        System.Console.WriteLine( "Couldn't read charset project from file " + ArgParser.Parameter( "CHARSETPROJECT" ) );
        return 1;
      }

      int     firstEntry = 0;
      int     count = -1;
      if ( ArgParser.IsParameterSet( "OFFSET" ) )
      {
        firstEntry = GR.Convert.ToI32( ArgParser.Parameter( "OFFSET" ) );
      }
      if ( ArgParser.IsParameterSet( "COUNT" ) )
      {
        count = GR.Convert.ToI32( ArgParser.Parameter( "COUNT" ) );
      }

      if ( !ValidateFirstAndCount( firstEntry, ref count, charsetProject.Characters.Count ) )
      {
        return 1;
      }

      int numBytesPerCharacter = Lookup.NumBytesOfSingleCharacter( charsetProject.Mode );

      GR.Memory.ByteBuffer    exportData = new GR.Memory.ByteBuffer( (uint)( count * numBytesPerCharacter ) );
      for ( int i = 0; i < count; ++i )
      {
        charsetProject.Characters[firstEntry + i].Tile.Data.CopyTo( exportData, 0, numBytesPerCharacter, i * numBytesPerCharacter );
      }

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), exportData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }

  }
}
