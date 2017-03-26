using FluentAssertions;
using Xunit;

namespace MR.AspNetCore.ApiVersioning
{
	public class VersionDescriptorTest
	{
		[Fact]
		public void Equals_True()
		{
			var x = new VersionDescriptor(0, 1);
			var y = new VersionDescriptor(0, 1);

			(x == y).Should().BeTrue();
		}

		[Fact]
		public void Equals_False()
		{
			var x = new VersionDescriptor(0, 1);
			var y = new VersionDescriptor(0, 2);

			(x == y).Should().BeFalse();
		}

		[Fact]
		public void GreaterThan_True()
		{
			var x = new VersionDescriptor(0, 2);
			var y = new VersionDescriptor(0, 1);

			(x > y).Should().BeTrue();
		}

		[Fact]
		public void GreaterThan_False()
		{
			var x = new VersionDescriptor(0, 1);
			var y = new VersionDescriptor(0, 2);

			(x > y).Should().BeFalse();
		}
	}
}
