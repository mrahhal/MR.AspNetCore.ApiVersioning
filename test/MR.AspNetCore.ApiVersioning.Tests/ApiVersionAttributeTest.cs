using System;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace MR.AspNetCore.ApiVersioning
{
	public class ApiVersionAttributeTest
	{
		[Fact]
		public void Ctor_NullCheck()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				new ApiVersionAttribute(null);
			});
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		public void Ctor_Invalid_Throws(string version)
		{
			Assert.Throws<ArgumentException>(() =>
			{
				new ApiVersionAttribute(version);
			});
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		[InlineData("asd")]
		public void Accept_Invalid_NoMatch(string version)
		{
			var attribute = new ApiVersionAttribute("0.1");
			attribute.Accept(Create(version)).Should().BeFalse();
		}

		[Fact]
		public void Accept_Match()
		{
			var attribute = new ApiVersionAttribute("0.1");
			attribute.Accept(Create("0.1")).Should().BeTrue();
		}

		[Fact]
		public void Accept_NoMatch()
		{
			var attribute = new ApiVersionAttribute("0.1");
			attribute.Accept(Create("0.2")).Should().BeFalse();
		}

		[Fact]
		public void Accept_MultiVersions_Match()
		{
			var attribute = new ApiVersionAttribute("0.1, 0.2");
			attribute.Accept(Create("0.1")).Should().BeTrue();
			attribute.Accept(Create("0.2")).Should().BeTrue();
		}

		[Fact]
		public void Accept_MultiVersions_NoMatch()
		{
			var attribute = new ApiVersionAttribute("0.1, 0.2");
			attribute.Accept(Create("0.3")).Should().BeFalse();
		}

		[Fact]
		public void Accept_Range_Greater_Same_NoMatch()
		{
			var attribute = new ApiVersionAttribute(">0.1");
			attribute.Accept(Create("0.1")).Should().BeFalse();
		}

		[Fact]
		public void Accept_Range_GreaterOrEqual_Same_Match()
		{
			var attribute = new ApiVersionAttribute(">=0.1");
			attribute.Accept(Create("0.1")).Should().BeTrue();
		}

		[Fact]
		public void Accept_Range_Less_Same_NoMatch()
		{
			var attribute = new ApiVersionAttribute("<0.1");
			attribute.Accept(Create("0.1")).Should().BeFalse();
		}

		[Fact]
		public void Accept_Range_LessOrEqual_Same_Match()
		{
			var attribute = new ApiVersionAttribute("<=0.1");
			attribute.Accept(Create("0.1")).Should().BeTrue();
		}

		[Fact]
		public void Accept_Range_Greater_Match()
		{
			var attribute = new ApiVersionAttribute(">0.1");
			attribute.Accept(Create("0.2")).Should().BeTrue();
		}

		[Fact]
		public void Accept_Range_Less_Match()
		{
			var attribute = new ApiVersionAttribute("<1.2");
			attribute.Accept(Create("0.1")).Should().BeTrue();
		}

		private ActionConstraintContext Create(string versionValue)
		{
			var context = new ActionConstraintContext();
			var httpContext = new DefaultHttpContext();
			context.RouteContext = new RouteContext(httpContext)
			{
				RouteData = new RouteData()
			};
			context.RouteContext.RouteData.Values.Add("version", $"{versionValue}");
			return context;
		}
	}
}
