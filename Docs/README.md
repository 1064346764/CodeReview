# 开始构建拓展

### 环境变量

- 设置环境变量MSBUILD_ROOT 指向Visual studio 下的 MSBuild.exe（C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin）
- 设置环境变量VSIXCREATOR指向输出目录

### 插件输出准备工作

下载[GitHub - madskristensen/PrivateGalleryCreator: Create private extension galleries for Visual Studio](https://github.com/madskristensen/PrivateGalleryCreator/tree/master)
解压至输出目录

### 打包插件

- 运行.\Analyzer.Vsix 下的publish.ps1脚本
