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

- 文件搜索

```csharp
PathResolver.Instance.List(@"path:\%ProgramData%\*\*\*.txt");
//C:\ProgramData\chocolatey\bin\_processed.txt
//...
//C:\ProgramData\NVIDIA Corporation\NvStreamSrv\settings.txt
```

- 文件夹搜索

```csharp
PathResolver.Instance.List(@"path:\%ProgramData%\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis*\");
//C:\ProgramData\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis.Compilers,version=4.2.0.2228105,productarch=neutral
//...
//C:\ProgramData\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis.VisualStudio.Setup.Resources,version=15.9.28218.60,language=zh-CN
```

- 获取嵌入文件内容

```csharp
PathResolver.Instance.Get(@"embedded:\Configs\config.txt");
```

- 获取文件内容

```csharp
PathResolver.Instance.Get(@"path_content:\Configs\config2.txt");
```

- 获取文件版本号

```csharp
var res = PathResolver.Instance.Get(@$"version:\C:\Windows\System32\cmd.exe");
//10.0.19041.1826 (WinBuild.160101.0800)
```

## 写入

[代码](https://github.com/DaZiYuan/path-engine/blob/main/src/TestProject/PathResolverTest.cs)

# 使用路径字符串轻松读写 Windows 资源

- [x] 真实路径解析
  - [x] 环境变量解析
    - 例：`path:\%appdata%\path`
  - [ ] 自定义路径解析
    - 例：`path:\%xx%\path`
  - [x] 模糊匹配文件
    - 例：`path:\%xx%\path\*\*\*.txt`
  - [x] 模糊匹配文件夹
    - 例：`path:\%xx%\path\*\*\`，注意斜线结尾
- [ ] 内容获取
  - [x] 获取获取注册表内容
    - 例：`registry:\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\path`
  - [x] 获取文件夹版本号
    - 例：`version:\path`
  - [x] 获取文件内容
    - 例：`content:\path`
  - [x] 获取 Dll 嵌入资源内容
    - 例：`embedded:\path`
- [ ] 写入内容
  - [ ] todo

# Library:

```
Install-Package PathEngine -Version
```

https://www.nuget.org/packages/PathEngine/

# CLI:

todo
