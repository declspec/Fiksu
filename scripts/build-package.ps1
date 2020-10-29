#
# This script is designed to modify an existing .nuspec file
# prior to a NuGet pack command to add in all <PackageReference>
# elements found in a co-located csproj file. 
#
# NuGet pack is pretty good but for some reason doesn't support the 
# PackageReference elements (despite them being a native NuGet feature)
#
param ([String] $project, [String] $version)

$ErrorActionPreference = "Stop"

$nuspec = New-Object System.Xml.XmlDocument
$nuspec.Load("../src/$project/$project.nuspec")

$csproj = New-Object System.Xml.XmlDocument
$csproj.Load("../src/$project/$project.csproj")

# Clear any existing dependencies (optional, you may want to specify hardcoded initial dependencies)
$dependencies = $nuspec.SelectSingleNode("//*[local-name()='dependencies']")
$dependencies.RemoveAll()

# Find all PackageReference nodes and create a matching 'dependency' node in the nuspec file.
foreach($reference in $csproj.SelectNodes("//*[local-name()='PackageReference']")) {
    $element = $nuspec.CreateElement("dependency", $nuspec.DocumentElement.NamespaceUri)
    $element.SetAttribute("id", $reference.Include)
    $element.SetAttribute("version", $reference.Version)
    
    $dependencies.AppendChild($element) | Out-Null
}

# Overwrite the existing nuspec file.
$nuspec.Save("../src/$project/$project.nuspec")

# Build the project
dotnet pack "../src/$project/$project.csproj" -c Release -p:PackageVersion="$version" -o "./packages/$version"
