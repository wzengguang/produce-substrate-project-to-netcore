using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class XmlConst
    {
        public const string ProjectReference = "ProjectReference";
        public const string ItemGroup = "ItemGroup";
        public const string PackageReference = "PackageReference";
        public const string Include = "Include";
        public const string Reference = "Reference";
        public const string HintPath = "HintPath";
        public const string PropertyGroup = "PropertyGroup";
        public const string Condition = "Condition";
        public const string ConditionValueNet472 = "'$(TargetFramework)' == 'net472'";
        public const string ConditionValueNet6 = "'$(TargetFramework)' != '' and '$(TargetFramework)' != 'net472'";
        public const string IncludeValuePrefix = @"$(Inetroot)\";
        public const string GeneratePathProperty = "GeneratePathProperty";
        public const string ExcludeAssets = "ExcludeAssets";
        public const string ExcludeAssetsValue = "All";
        public const string Sdk = "Sdk";
        public const string SdkValue = "Microsoft.NET.Sdk";
        public const string PkgPrefix = "$(Pkg";
        public const string PackageVersion = "PackageVersion";
    }
}
