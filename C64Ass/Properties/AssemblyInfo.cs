﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if OS_WINDOWS
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "C64Ass" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "" )]
[assembly: AssemblyProduct( "C64Ass" )]
[assembly: AssemblyCopyright( "Copyright ©  2016 - 2024" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "88fb7a10-8dfe-427a-8c0e-300dc86f6a3a" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion( RetroDevStudio.Version.VersionBase + "." + RetroDevStudio.Version.BuildNumber )]
[assembly: AssemblyFileVersion( RetroDevStudio.Version.VersionBase + "." + RetroDevStudio.Version.BuildNumber )]

#if OS_WINDOWS
#if NET5_0_OR_GREATER
[assembly: SupportedOSPlatform( "windows" )]
#endif
#endif