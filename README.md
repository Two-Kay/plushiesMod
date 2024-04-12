<p align="center">
    <img src="https://steamuserimages-a.akamaihd.net/ugc/1648839814002163309/364DFD2C42A908873D1D10D52EADF3EBCC82E83E/" alt="Plushies" />
</p>
<p align="center">
	<a href="https://github.com/Two-Kay/plushiesMod/releases/">
		<img src="https://img.shields.io/badge/release-1.0.7-4BC51D.svg?style=flat" alt="v1.0.7" />
  </a>
  <a href="https://steamcommunity.com/sharedfiles/filedetails/?id=2259582816">
    <img src="https://img.shields.io/badge/RimWorld-1.2/1.3/1.4/1.5-purple.svg?longCache=true&style=plastic)" alt="Compatible Rimworld 1.2-1.5" />
  </a>
</p>

# Plushies

A mod that adds plushies to Rimworld!

## Features

- Craft cute plushies at the art bench!
- Plushies are similar to sculptures, but made from textiles
- Cute artwork descriptions that make sense for plushies!
- Attach up to 10 plushies to a bed to give a small stacking comfort bonus
- Sleeping in a bed with a plushie gives a "Cuddled plushie" thought (+1)
- "Psychopath" colonists are not too excited about plushies...
- but "Kind" colonists really like plushie cuddles and get +3!
- Colonists can also spend their free time cuddling plushies to gain joy!
- Biotech: Children particularly like plushies! Babies also enjoy having some near their crib.

## Compatibility

Plushies are patched in as linkables for anything with "Bed" in the defName, so it should work with mods like Vanilla Furniture Expanded.

## Development

Put the repository in your `RimWorld/Mods` folder. The assembly references in `Plushies.csproj` are relative paths that expect the mod to be located here. On Windows+Steam the mods folder will be at `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods`. The mod will be listed in the Rimworld UI with a folder icon next to it (unlike the steam workshop versions which have a steam icon).

For developing in Visual Studio Code: If you open the `Source/Plushies` folder, it should autodetect that it's a C# project and download the necessary plugins. You can then run `dotnet build` in a terminal to build the Rimworld 1.5 assembly for the mod. If it fails to automatically download the necessary plugins, you can install the .NET SDK on [https://dotnet.microsoft.com/en-us/download/visual-studio-sdks](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks), make sure to install .NET 6.0 as any other version may not be supported.

## Changelog

- 1.0.5
  - Plushies are now made from the art bench and have no crafting skill requirement (less hassle for players)
  - Biotech: Added specific thoughts for children cuddling plushies (extra +1, stacks with Kind)
  - Biotech: Babies now get a small +1 thought for having a plushie near their crib
