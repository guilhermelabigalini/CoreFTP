﻿namespace CoreFtp.Infrastructure.Extensions
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Infrastructure;

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty( this string operand )
        {
            return string.IsNullOrEmpty( operand );
        }

        public static bool IsNullOrWhiteSpace( this string operand )
        {
            return string.IsNullOrWhiteSpace( operand );
        }

        public static int? ExtractEpsvPortNumber( this string operand )
        {
            var regex = new Regex( @"(?:\|)(?<PortNumber>\d+)(?:\|)", RegexOptions.Compiled );

            var match = regex.Match( operand );

            if ( !match.Success )
                return null;

            return int.Parse( match.Groups[ "PortNumber" ].Value );
        }


        public static FtpNodeInformation ToFtpNode( this string operand )
        {
            var dictionary = operand.Split( ';' )
                                    .Select( s => s.Split( '=' ) )
                                    .ToDictionary( strings => strings.Length == 2 ? strings[ 0 ] : "name", strings => strings.Length == 2 ? strings[ 1 ] : strings[ 0 ] );

            return new FtpNodeInformation
            {
                Name = dictionary.GetValueOrDefault( "name" ).Trim(),
                Size = dictionary.GetValueOrDefault( "size" ).ParseOrDefault(),
                DateModified = dictionary.GetValueOrDefault( "modify" ).ParseExactOrDefault( "yyyyMMddHHmmss" )
            };
        }
    }
}