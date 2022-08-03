# PathEngine

Almighty Path Parser | 全能路径解析器

---

# 功能：

## 读取

- 解析路径

```csharp
PathResolver.Instance.Get("%ProgramFiles(x86)%");
//C:\Program Files (x86)
PathResolver.Instance.Get(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}");
//HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}
PathResolver.Instance.Get("TestProject.Configs.config.txt");
//TestProject.Configs.config.txt

```

- 文件/子项搜索

```csharp
//文件目录
PathResolver.Instance.List(@"path_list:\%ProgramData%\*\*\*.txt");
//C:\ProgramData\chocolatey\bin\_processed.txt
//...
//C:\ProgramData\NVIDIA Corporation\NvStreamSrv\settings.txt

//注册表目录
PathResolver.Instance.List(@"path_list:\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\*");
//HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\:Enable64Bit
...
//HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\:DbgJITDebugLaunchSetting
```

- 目录搜索

注意：搜索目录一定要以\结尾

```csharp
// 文件目录
PathResolver.Instance.List(@"path_list:\%ProgramData%\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis*\");
//C:\ProgramData\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis.Compilers,version=4.2.0.2228105,productarch=neutral
//...
//C:\ProgramData\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis.VisualStudio.Setup.Resources,version=15.9.28218.60,language=zh-CN

//注册表目录
PathResolver.Instance.List(@"path_list:\HKEY_LOCAL_MACHINE\SOFTWARE\win*\");
//HKEY_LOCAL_MACHINE\SOFTWARE\Windows
```

- 获取内容

```csharp
//文件
PathResolver.Instance.Get(@"path_content:\Configs\config.txt");
//注册表
PathResolver.Instance.Get(@"path_content:\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}:pv");
//嵌入资源
var res3 = PathResolver.Instance.Get(@"path_content:\Configs.config.txt");
```

- 获取文件版本号

```csharp
var res = PathResolver.Instance.Get(@$"version:\C:\Windows\System32\cmd.exe");
//10.0.19041.1826 (WinBuild.160101.0800)
```

## 写入

[更多示例](https://github.com/DaZiYuan/path-engine/blob/main/src/TestProject/PathResolverTest.cs)

# Library:

```
Install-Package PathEngine -Version
```

https://www.nuget.org/packages/PathEngine/

# CLI:

todo
