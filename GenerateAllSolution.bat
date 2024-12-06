@ECHO OFF
SET "MultiTargets=%1"
IF "%MultiTargets%"=="" SET "MultiTargets=uwp,wasdk,wasm"

powershell .\tooling\GenerateAllSolution.ps1 -MultiTargets %MultiTargets%