::Configuration Settings
set SolutionName=Anomalous\AnomalousMSBuild.sln

::Less likely to need to change these.
set ThisFolder=%~dp0
set RootDependencyFolder=%ThisFolder%..\
set CurrentDirectory=%CD%

set SolutionPath=%ThisFolder%%SolutionName%

msbuild.exe "%SolutionPath%" /property:Configuration=Release;Platform="Any CPU" /target:Clean
msbuild.exe "%SolutionPath%" /property:Configuration=Release;Platform="Any CPU"