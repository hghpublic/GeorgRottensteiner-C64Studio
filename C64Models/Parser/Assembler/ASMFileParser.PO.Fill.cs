﻿using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POFill( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, string parseLine, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;
      ClearErrorInfo();

      List<List<TokenInfo>>   lineParams;

      if ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, false, out lineParams ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }
      if ( ( lineParams.Count < 1 )
      ||   ( lineParams.Count > 3 ) )
      {
        AddError( lineIndex, ErrorCode.E1302_MALFORMED_MACRO, "Macro malformed, expect " + lineTokenInfos[0].Content + " <Count>[,<Value>], or " + lineTokenInfos[0].Content + " <Count>,<From> to <To>[,<Times>]" );
        return ParseLineResult.ERROR_ABORT;
      }

      int numBytes = -1;
      if ( !EvaluateTokens( lineIndex, lineParams[0], out SymbolInfo numBytesSymbol ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Could not determine fill count parameter " + TokensToExpression( lineParams[0] ) );
        return ParseLineResult.RETURN_NULL;
      }
      numBytes = numBytesSymbol.ToInt32();
      if ( numBytes < 0 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro malformed, fill count must be positive or zero" );
        return ParseLineResult.RETURN_NULL;
      }

      info.NumBytes               = numBytes;
      info.Line                   = parseLine;

      int fillValue = 0;
      GR.Memory.ByteBuffer lineData = null;

      if ( ( lineParams.Count >= 2 )
      &&   ( IsList( lineParams[1] ) ) )
      {
        if ( lineParams.Count == 2 )
        {
          List<List<TokenInfo>>   listParams;

          if ( !ParseLineInParameters( lineParams[1], 1, lineParams[1].Count - 2, lineIndex, false, out listParams ) )
          {
            return ParseLineResult.ERROR_ABORT;
          }

          // in case of a list the number is the number of repeats
          info.NumBytes *= listParams.Count;
          numBytes *= listParams.Count;

          lineData = new GR.Memory.ByteBuffer( (uint)numBytes );
          int     listLoopIndex = 0;

          for ( int i = 0; i < numBytes; ++i )
          {
            m_TemporaryFillLoopPos = listLoopIndex; // was i;

            int expressionResult = 0;

            if ( ( i % listParams.Count ) == listParams.Count - 1 )
            {
              ++listLoopIndex;
            }

            if ( !EvaluateTokens( lineIndex, listParams[i % listParams.Count], out SymbolInfo expressionResultSymbol ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Could not evaluate fill expression for byte " + i.ToString() + ":" + TokensToExpression( listParams[i % listParams.Count] ) );
              return ParseLineResult.RETURN_NULL;
            }
            expressionResult = expressionResultSymbol.ToInt32();
            if ( !ValidByteValue( expressionResult ) )
            {
              AddError( lineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "Fill expression for byte " + i.ToString() + " out of bounds, resulting in value " + expressionResult );
              return ParseLineResult.RETURN_NULL;
            }
            lineData.SetU8At( i, (byte)expressionResult );
          }
          m_TemporaryFillLoopPos = -1;
        }
      }
      else if ( IsStartToEndNumericRange( lineParams, out bool hadError, out var symbolStart, out var symbolEnd, out var symbolTimes ) )
      {
        if ( hadError )
        {
          return ParseLineResult.ERROR_ABORT;
        }

        long startValue = symbolStart.ToInteger();
        long endValue   = symbolEnd.ToInteger();
        long numTimes = 1;
        if ( symbolTimes != null )
        {
          numTimes = symbolTimes.ToInteger();
          if ( numTimes <= 0 )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Count value must not be zero or negative!" + TokensToExpression( lineParams[1] ) );
            return ParseLineResult.RETURN_NULL;
          }
        }

        lineData = new GR.Memory.ByteBuffer( (uint)( numBytes * numTimes ) );
        info.NumBytes = (int)( numBytes * numTimes );

        for ( long j = 0; j < numTimes; ++j )
        {
          if ( numBytes == 1 )
          {
            lineData.SetU8At( (int)j, (byte)startValue );
          }
          else
          {
            for ( int i = 0; i < numBytes; ++i )
            {
              lineData.SetU8At( (int)( j * numBytes + i ), (byte)( startValue + i * ( endValue - startValue ) / ( numBytes - 1 ) ) );
            }
          }
        }
      }
      else if ( lineParams.Count == 2 )
      {
        lineData = new GR.Memory.ByteBuffer( (uint)numBytes );

        for ( int i = 0; i < numBytes; ++i )
        {
          m_TemporaryFillLoopPos = i;

          int expressionResult = 0;
          if ( !EvaluateTokens( lineIndex, lineParams[1], out SymbolInfo expressionResultSymbol ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Could not evaluate fill expression for byte " + i.ToString() + ":" + TokensToExpression( lineParams[1] ) );
            return ParseLineResult.RETURN_NULL;
          }
          expressionResult = expressionResultSymbol.ToInt32();
          if ( !ValidByteValue( expressionResult ) )
          {
            AddError( lineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "Fill expression for byte " + i.ToString() + " out of bounds, resulting in value " + expressionResult );
            return ParseLineResult.RETURN_NULL;
          }
          lineData.SetU8At( i, (byte)expressionResult );
        }
        m_TemporaryFillLoopPos = -1;
      }

      if ( !m_CurrentSegmentIsVirtual )
      {
        if ( lineData != null )
        {
          info.LineData = lineData;
        }
        else
        {
          info.LineData = new GR.Memory.ByteBuffer( (uint)numBytes );
          for ( int i = 0; i < numBytes; ++i )
          {
            info.LineData.SetU8At( i, (byte)fillValue );
          }
        }
      }
      lineSizeInBytes = info.NumBytes;
      return ParseLineResult.OK;
    }



  }
}
