using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleSpriteFile( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "sprite file", ArgParser.Parameter( "TYPE" ), new string[] { "SPRITES" } ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( ArgParser.Parameter( "SPRITES" ) );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary sprite file " + ArgParser.Parameter( "SPRITES" ) );
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

      // we don't know any format here
      int numBytesPerSprite = 64;
      if ( !ValidateFirstAndCount( firstEntry, ref count, (int)data.Length / numBytesPerSprite ) )
      {
        return 1;
      }

      GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( count * numBytesPerSprite ) );
      data.CopyTo( spriteData, firstEntry * numBytesPerSprite, count * numBytesPerSprite );

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), spriteData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }




  }
}
