using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace ResourceCalculator.Tests
{
    class RealTests
    {

        [Test]
        public void CanEvaluateMultipleProducts()
        {
            // Arrange
            const string script = @"
It => ""Ion Thrustor""
Ii => ""Invar Ingot""
Fe => ""Force Field Emiter""
Gs => ""Glowstone""
W  => ""Wiring""
Fm => ""Ferus Metal""
C  => ""Copper""
S  => ""Silver""
Sl => ""Solenoid""
Tu => ""Untethered Tesseract""
I  => ""Iron""
Ti => ""Tin""
Gh => ""Hardened glass""
D  => ""Diamond""
O  => ""Obsidian""
Pb => ""Lead""
Ep => ""Ender pearl""


It  = 3Ii + 2Fe + Gs + W
Fe  = 2W + 2Sl + Tu
Tu  = 4Ti + 4Gh + D + 4Ep
Sl  = 6W + 3I
2Gh = 2O + Pb
3Ii = 2I + Fm
8W  = 6C + 3S
Ep  = 4I
I   = 4O
";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);

            Console.WriteLine("Ion Thrustor\r\n" + resourceEngine.RecipieFor("4It")); 
        }

    }
}
