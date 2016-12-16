Dim WShell
Set WShell = CreateObject("WScript.Shell")
WShell.Run "dotnet FesteloNetBOT.dll work", 0
Set WShell = Nothing