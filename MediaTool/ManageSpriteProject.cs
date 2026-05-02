using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    private int HandleSpriteProject( GR.Text.ArgumentParser ArgParser )
    {
      if ( !ValidateExportType( "sprite project", ArgParser.Parameter( "TYPE" ), new string[] { "SPRITES" } ) )
      {
        return 1;
      }

      var spriteProject = new RetroDevStudio.Formats.SpriteProject();

      if ( !spriteProject.ReadFromBuffer( GR.IO.File.ReadAllBytes( ArgParser.Parameter( "SPRITEPROJECT" ) ) ) )
      {
        System.Console.WriteLine( "Couldn't read sprite project from file " + ArgParser.Parameter( "SPRITEPROJECT" ) );
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

      if ( !ValidateFirstAndCount( firstEntry, ref count, spriteProject.TotalNumberOfSprites ) )
      {
        return 1;
      }

      int bytesOfSingleSprite = Lookup.NumBytesOfSingleSprite( spriteProject.Mode );
      int bytesOfPaddedSingleSprite = Lookup.NumPaddedBytesOfSingleSprite( spriteProject.Mode );

      GR.Memory.ByteBuffer    spriteData = new GR.Memory.ByteBuffer( (uint)( count * bytesOfPaddedSingleSprite ) );
      for ( int i = 0; i < count; ++i )
      {
        spriteProject.Sprites[firstEntry + i].Tile.Data.CopyTo( spriteData, 0, bytesOfSingleSprite, i * bytesOfPaddedSingleSprite );
        if ( bytesOfPaddedSingleSprite > bytesOfSingleSprite )
        {
          // pad with color
          byte color = (byte)spriteProject.Sprites[firstEntry + i].Tile.CustomColor;
          if ( spriteProject.Sprites[firstEntry + i].Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR )
          {
            color |= 0x80;
          }
          spriteData.SetU8At( i * bytesOfPaddedSingleSprite + bytesOfSingleSprite, color );
        }
      }

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), spriteData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }
      return 0;
    }

  }
}
