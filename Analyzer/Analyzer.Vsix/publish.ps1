$MSBuild = $env:MSBUILD_ROOT + "\MSBuild.exe"
$Out = $env:VSIXCREATOR
if($MSBuild -eq ""){
    Write-Error "[ERROR]could not find MSBuild env where point to MSBuild.exe for visual studio"
}
Write-Host "[INFO]当前MSBuild文件目录:"+$MSBuild
Write-Host "[INFO]当前VSIXCREATOR文件目录:"+$Out
# $xmldata = [xml](Get-Content ".\source.extension.vsixmanifest")
# $version = $xmldata.PackageManifest.GetAttribute("Version").split(".")
# $version[2] = ($version[2] -as [int] + 1) -as [String]
# $new_version = $version | Out-String
# $xmldata.PackageManifest.SetAttribute("Version", $new_version)

#找到工程文件，并执行打包命令
Write-Host "[INFO]build Analyzer.Vsix => .\bin\net472\Relase\Analyzer.Vsix.vsix"
dotnet restore
&$MSBuild Analyzer.Vsix.csproj -t:Rebuild -p:Configuration=Release

#复制文件到输出目录
Copy-Item ".\bin\Release\net472\Analyzer.Vsix.vsix" -Destination $Out


Write-Host "[INFO]Copy Item .\bin\Release\net472\Analyzer.Vsix.vsix => " + $Out
&($Out + "\PrivateGalleryCreator.exe")
Write-Host "[INFO]Build gallery file => " + $Out
