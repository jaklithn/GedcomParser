using System;
using System.Collections.Generic;
using Shouldly;


namespace GedcomParser.Test.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// To make the test result feedback easier to read.
        /// </summary>
        public static void ShouldBeEmptyWithFeedback(this IEnumerable<string> values)
        {
            var result = string.Join(Environment.NewLine, values);
            result.ShouldBeEmpty();
        }
    }
}