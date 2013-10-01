using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace ResourceCalculator.Tests
{
    [TestFixture]
    class ParseScriptTests
    {
        [Test]
        public void CanParseScript()
        {
            // Arrange
            const string script = "a = b";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            
            // Assert

        } 
        
        [Test]
        public void CanReadSingleLine()
        {
            // Arrange
            const string script = "a = b";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            var table = resourceEngine.GetResourceTable();

            // Assert
            table["a"].Should().BeOfType<ResourceExpression>();
        }

        [Test]
        public void CanEvaluateSimpleExpression()
        {
            // Arrange
            const string script = "a = b";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            cost.Count.Should().Be(1);
            cost["b"].Should().Be(1);
        }

        [Test]
        public void CanEvaluateCombinationExpression()
        {
            // Arrange
            const string script = "a = b + c";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            cost.Count.Should().Be(2);
            cost["b"].Should().Be(1);
            cost["c"].Should().Be(1);
        }

        [Test]
        public void CanEvaluateMultipleInput()
        {
            // Arrange
            const string script = "a = b + c";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a", 2).Items;

            // Assert
            cost.Count.Should().Be(2);
            cost["b"].Should().Be(2);
            cost["c"].Should().Be(2);
        }

        [Test]
        public void CanEvaluateCombinationExpressionWithMultiple()
        {
            // Arrange
            const string script = "a = b + 2c";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            cost.Count.Should().Be(2);
            cost["b"].Should().Be(1);
            cost["c"].Should().Be(2);
        }

        [Test]
        public void CanEvaluateMultipleLines()
        {
            // Arrange
            const string script = @"a = b + 2c
                                    b = 3c + d";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            cost.Count.Should().Be(2);
            cost["c"].Should().Be(5);
            cost["d"].Should().Be(1);
        }

        [Test]
        public void CanEvaluateMultipleRecursion()
        {
            // Arrange
            const string script = @"a = 2b + 2c
                                    b = 3c + d";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            cost.Count.Should().Be(2);
            cost["c"].Should().Be(8);
            cost["d"].Should().Be(2);
        }

        [Test]
        public void CanEvaluateMultipleProducts()
        {
            // Arrange
            const string script = @"a = 2b + 2c
                                    2b = 3c + d";
            var resourceEngine = new ResourceEngine();

            // Act
            resourceEngine.Parse(script);
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            cost.Count.Should().Be(2);
            cost["c"].Should().Be(5);
            cost["d"].Should().Be(1);
        }
    }
}
