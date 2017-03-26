using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace MR.AspNetCore.ApiVersioning
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ApiVersionAttribute : Attribute, IActionConstraint
	{
		private VersionDescriptor[] _vds;

		public ApiVersionAttribute(string version)
		{
			if (version == null) throw new ArgumentNullException(nameof(version));
			if (string.IsNullOrWhiteSpace(version)) throw new ArgumentException(nameof(version));

			Version = version;

			if (version[0] == '<' || version[0] == '>')
			{
				ForRanges();
			}
			else
			{
				ForDistinct();
			}
		}

		private void ForDistinct()
		{
			_vds = Version.Split(',')
				.Select(v => VersionDescriptor.Parse(v.Trim()))
				.ToArray();
			if (_vds.Any(vd => VersionDescriptor.IsInvalid(vd)))
			{
				throw new ArgumentException($"version is not in a correct format.", "version");
			}
		}

		private void ForRanges()
		{
			var version = Version;
			if (version[0] == '<')
			{
				LessThan = true;
			}
			else
			{
				GreaterThan = true;
			}
			version = version.Substring(1);

			if (version[0] == '=')
			{
				OrEqual = true;
				version = version.Substring(1);
			}

			var vd = VersionDescriptor.Parse(version.Trim());
			if (VersionDescriptor.IsInvalid(vd))
			{
				throw new ArgumentException($"version is not in a correct format.", "version");
			}
			_vds = new[] { vd };
		}

		public string Version { get; }

		public IEnumerable<VersionDescriptor> Versions => _vds;

		public int Order { get; set; }

		private bool LessThan { get; set; }

		private bool GreaterThan { get; set; }

		private bool OrEqual { get; set; }

		public bool Accept(ActionConstraintContext context)
		{
			var raw = context.RouteContext.RouteData.Values["version"] as string;
			if (string.IsNullOrWhiteSpace(raw))
				return false;

			var version = VersionDescriptor.Parse(raw);
			if (VersionDescriptor.IsInvalid(version))
				return false;

			if (LessThan || GreaterThan)
			{
				var vd = _vds[0];
				if (OrEqual && version == vd)
				{
					return true;
				}
				if (LessThan)
				{
					return version < vd;
				}
				else
				{
					return version > vd;
				}
			}
			else
			{
				for (int i = 0; i < _vds.Length; i++)
				{
					if (_vds[i] == version) return true;
				}
			}

			return false;
		}
	}
}
