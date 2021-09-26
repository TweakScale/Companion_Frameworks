using System.Reflection;
using System.Runtime.CompilerServices;

// Information about this assembly is defined by the following attributes. 
// Change them to the values specific to your project.

[assembly: AssemblyTitle("TweakScalerWaterfallFXIntegrator")]
[assembly: AssemblyDescription("Integrates the TweakScaler to the Target Module")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(TweakScaleCompanion.Visuals.LegalMamboJambo.Company)]
[assembly: AssemblyProduct(TweakScaleCompanion.Visuals.LegalMamboJambo.Product)]
[assembly: AssemblyCopyright(TweakScaleCompanion.Visuals.LegalMamboJambo.Copyight)]
[assembly: AssemblyTrademark(TweakScaleCompanion.Visuals.LegalMamboJambo.Trademark)]
[assembly: AssemblyCulture("")]

// The assembly version has the format "{Major}.{Minor}.{Build}.{Revision}".
// The form "{Major}.{Minor}.*" will automatically update the build and revision,
// and "{Major}.{Minor}.{Build}.*" will update just the revision.

//[assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion(TweakScaleCompanion.Visuals.Version.Number)]
[assembly: AssemblyFileVersion(TweakScaleCompanion.Visuals.Version.Number)]
[assembly: KSPAssembly("TweakScalerWaterfallFXIntegrator", TweakScaleCompanion.Visuals.Version.major, TweakScaleCompanion.Visuals.Version.minor)]
[assembly: KSPAssemblyDependency("TweakScaleCompanion_Visuals", TweakScaleCompanion.Visuals.Version.major, TweakScaleCompanion.Visuals.Version.minor)]
[assembly: KSPAssemblyDependency("TweakScalerModuleWaterfallFX", TweakScaleCompanion.Visuals.Version.major, TweakScaleCompanion.Visuals.Version.minor)]
[assembly: KSPAssemblyDependency("Waterfall", 0, 0)]
[assembly: KSPAssemblyDependency("KSPe.Light.TweakScale", 2, 4)]
[assembly: KSPAssemblyDependency("Scale", 2, 4)]
//[assembly: KSPAssemblyDependency("Scale_Redist", 1, 0)] // KSP 1.12.2 screwed up the Dependency check!!!
