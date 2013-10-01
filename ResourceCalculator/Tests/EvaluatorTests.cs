using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace ResourceCalculator.Tests
{
    [TestFixture]
    public class EvaluatorTests
    {
        [Test]
        public void NonRecursiveExample()
        {
            // Arrange
            var resourceEngine = new ResourceEngine();
            var rt = resourceEngine.GetResourceTable();
            rt["a"] = new ResourceExpression
            (
                new Dictionary<string, float>
                {
                    {"b", 1},
                    {"c", 2}
                }
            );

            // Act
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            Print(cost);
            cost.Count.Should().Be(2);
            cost["b"].Should().Be(1);
            cost["c"].Should().Be(2);
        }

        [Test]
        public void RecursiveExample()
        {
            // Arrange
            var resourceEngine = new ResourceEngine();
            var rt = resourceEngine.GetResourceTable();
            rt["a"] = new ResourceExpression
            (
                new Dictionary<string, float>
                {
                    {"b", 1},
                    {"c", 2}
                }
            );

            rt["b"] = new ResourceExpression
            (
                new Dictionary<string, float>
                {
                    {"c", 2}
                }
            );

            // Act
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            Print(cost);
            cost.Count.Should().Be(1);
            cost["c"].Should().Be(4);
        }

        [Test]
        public void RecursiveMultipleExample()
        {
            // Arrange
            var resourceEngine = new ResourceEngine();
            var rt = resourceEngine.GetResourceTable();
            rt["a"] = new ResourceExpression
            (
                new Dictionary<string, float>
                {
                    {"b", 1},
                    {"c", 2}
                }
            );

            rt["b"] = new ResourceExpression
            (
                new Dictionary<string, float>
                {
                    {"c", 2}
                }
            );

            // Act
            Dictionary<string, float> cost = resourceEngine.Evaluate("a").Items;

            // Assert
            Print(cost);
            cost.Count.Should().Be(1);
            cost["c"].Should().Be(4);
        }

        private void Print(Dictionary<string, float> cost)
        {
            Console.WriteLine(string.Join(", ", cost.Select(kvp => kvp.Key + ": " + kvp.Value)));
        }
    }
}