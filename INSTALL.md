# TweakScale Companion :: Frameworks

Adds (up to date) TweakScale /L patches for some KSP Enhancing Add'Ons.


## Installation Instructions

To install, place the GameData folder inside your Kerbal Space Program folder:

* **REMOVE ANY OLD VERSIONS OF THE PRODUCT BEFORE INSTALLING**, including any other fork:
	+ Delete `<KSP_ROOT>/GameData/TweakScaleCompanion/Visuals` if existent
	+ Delete `<KSP_ROOT>/GameData/TweakScaleCompanion/Frameworks`
* Extract the package's `GameData/` folder into your KSP's as follows:
	+ `<PACKAGE>/GameData/TweakScaleCompanion/Frameworks` --> `<KSP_ROOT>/GameData/TweakScaleCompanion`
		- Overwrite any preexisting file.

The following file layout must be present after installation:

```
<KSP_ROOT>
	[GameData]
		[TweakScale]
			[Plugins]
				KSPe.Light.TweakScale.dll
				Scale.dll
			[patches]
				...
			CHANGE_LOG.md
			DefaultScales.cfg
			Examples.cfg
			LICENSE
			NOTICE
			README.md
			ScaleExponents.cfg
			TweakScale.version
			documentation.txt
		999_Scale_Redist.dll
		ModuleManager.dll
		...
		[TweakScaleCompanion]
			[...]
			[Frameworks]
				CHANGE_LOG.md
				LICENSE*
				NOTICE
				README.md
				[patches]
					...
			[...]
	KSP.log
	PastDatabase.cfg
	...
```


### Dependencies

* TweakScale /L 2.4.5 or later
	+ **NOT** included
* Waterfall
	+ **NOT** included 
* Module Manager 3.1.3 or later
	+ **NOT** included
