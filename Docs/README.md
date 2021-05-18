# 使用准备

### 环境变量

- 设置环境变量MSBUILD_ROOT 指向Visual studio 下的 MSBuild.exe
- 设置环境变量VSIXCREATOR指向输出目录

### Atom feed 生成器准备

插件输出目录下若无PrivateGalleryCreator.exe 请参考[GitHub - madskristensen/PrivateGalleryCreator: Create private extension galleries for Visual Studio](https://github.com/madskristensen/PrivateGalleryCreator/tree/master)

重新生成相关生成器文件

# 推包

同时构建插件和CLI工具

- 运行.\Analyzer.Vsix 下的publish.ps1脚本
- 在\\\192.168.1.18\public\visuals



# 使用插件

- visual studio 选项 => 扩展 => Add 
- Url内填写私有插件库的地址, Apply即可



# 使用CLI

- Analyzer.CLI.exe sln文件目录
- Anlayzer.CLI 项目目录下运行 run.ps1 sln文件目录
- 注: run.ps1 得在pusblish.ps1之后执行