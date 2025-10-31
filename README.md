[Прочитать эту страницу по-русски](https://github.com/vazhka-dolya/bodystates/blob/main/README.ru.md) · [Посмотреть список изменений в обновлениях по-русски](https://github.com/vazhka-dolya/bodystates/blob/main/Changelog.ru.md) | **Read this page in English**

# BodyStates
This is a simple add-on for [*Mario 64 Movie Maker 3*](https://github.com/projectcomet64/M64MM) that allows you to easily change the body states of eyes,[^1] hands, and caps.

<p align="center">
  <img src="https://github.com/vazhka-dolya/bodystates/blob/main/GitHubImages/ReadmeImage4_eng.png"/>
</p>

# Installing and using
1. Make sure you have the [latest version](https://github.com/projectcomet64/M64MM/releases/latest) of *M64MM3* installed.
2. Download the [latest version](https://github.com/vazhka-dolya/bodystates/releases/latest) of the add-on. It will be in an archive.
3. Extract the downloaded archive's contents[^2] into the root folder[^3] of *M64MM3*. If it prompts you to replace files, then do it.
4. That's all.

If the add-on refuses to work for you: launch the game, go to **Options** > **Settings…** > collapse **Config: (ROM name)**[^4] > **Recompiler**, and make sure **Start changed** is enabled.

# Building prerequisites
<details>
  <summary>Click here to view</summary>
  
- *Visual Studio 2022*.
- *M64MM3*'s repository in a folder called `M64MM` outside of where this repository is.
  - Example: if the `.sln` for *BodyStates* is in `C:/projects/BodyStates/BodyStates.sln`, the whole *M64MM3* repository must be in `C:/projects/M64MM`.
- If you're on *Windows*, then, before extracting the archives, make sure to right-click the archive, open **Properties** and see if you have an **Unblock** checkbox. If you do, tick it and press **Apply**. If you don't do this and the archive(s) remain blocked, you may run into issues.
- *Depending on the circumstances*, you *may* have to do the following: go to **Menu** > **Tools** > **NuGet Package Manager** > **Package Manager Console** and enter `Install-Package HtmlRenderer.WinForms`. After that, go to **Menu** > **Project** > **Manage NuGet Packages…**, and make sure that both `HtmlRenderer.Core` and `HtmlRenderer.WinForms` are up-to-date.

</details>

# Credits
- @GlitchyPSI ([his website](https://glitchypsi.xyz)) — a lot of help with how to make an add-on.
  - This add-on is also based on [XStudio MiNi](https://github.com/projectcomet64/xstudio-mini), which is made by him.
- @SMG1OFFICIAL ([YouTube](https://www.youtube.com/channel/UCU5kWc-wqBOiAwDYPRvhCHg)) — kind of suggested me to make this add-on.
- @sm64rise ([YouTube](https://www.youtube.com/channel/UCY09vjVz1t8QssTqeeRnjdg)) — original code for changing body states.

### “If I use *BodyStates* for my work, do I have to credit you?”
Credit is highly appreciated, but completely optional!

[^1]: Not the eyes' textures themselves, but what eye texture is being displayed. If you want to use custom eye (and not only eye) textures, check out my other project, **[katarakta](https://github.com/vazhka-dolya/katarakta)**!
[^2]: That means *all* the contents, including the `deps` folder. If it crashes when opening the **About** window, make sure that you have `HtmlRenderer.dll` and `HtmlRenderer.WinForms.dll` in M64MM's `deps` folder.
[^3]: That's the same folder where `M64MM.exe` is located.
[^4]: The ROM name is usually `SUPER MARIO 64`, but it may be different if it's a ROM hack.
