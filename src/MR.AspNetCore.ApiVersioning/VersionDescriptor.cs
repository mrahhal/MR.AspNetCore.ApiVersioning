using System;
using System.Collections.Generic;

namespace MR.AspNetCore.ApiVersioning
{
	public struct VersionDescriptor : IComparable<VersionDescriptor>
	{
		private static VersionDescriptor _invalid = new VersionDescriptor(-1, -1);
		private static _Comparer _comparer = new _Comparer();
		public int Major;
		public int Minor;

		public VersionDescriptor(int major, int minor)
		{
			Major = major;
			Minor = minor;
		}

		public static VersionDescriptor Parse(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
				return _invalid;

			var v = new VersionDescriptor();
			var split = version.Split('.');
			if (split.Length != 2)
			{
				return _invalid;
			}

			if (!int.TryParse(split[0], out v.Major))
			{
				return _invalid;
			}

			if (!int.TryParse(split[1], out v.Minor))
			{
				return _invalid;
			}

			return v;
		}

		public static IComparer<VersionDescriptor> Comparer => _comparer;

		public override bool Equals(object obj) => this == (VersionDescriptor)obj;

		public override int GetHashCode() => base.GetHashCode();

		public static bool operator ==(VersionDescriptor v1, VersionDescriptor v2)
			=> v1.Major == v2.Major && v1.Minor == v2.Minor;

		public static bool operator !=(VersionDescriptor v1, VersionDescriptor v2)
			=> !(v1 == v2);

		public static bool operator >(VersionDescriptor v1, VersionDescriptor v2)
		{
			if (v1.Major > v2.Major)
				return true;

			if (v1.Major == v2.Major && v1.Minor > v2.Minor)
				return true;

			return false;
		}

		public static bool operator <(VersionDescriptor v1, VersionDescriptor v2)
		{
			if (v1.Major < v2.Major)
				return true;

			if (v1.Major == v2.Major && v1.Minor < v2.Minor)
				return true;

			return false;
		}

		public static bool IsInvalid(VersionDescriptor v)
			=> v == _invalid;

		public override string ToString() => $"{Major}.{Minor}";

		public int CompareTo(VersionDescriptor other)
		{
			if (this == other)
				return 0;

			if (this > other)
				return 1;

			return -1;
		}

		private class _Comparer : IComparer<VersionDescriptor>
		{
			public int Compare(VersionDescriptor x, VersionDescriptor y)
			{
				if (x == y)
					return 0;

				if (x > y)
					return 1;

				return -1;
			}
		}
	}
}
