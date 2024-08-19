@ECHO OFF
SET "MultiTargets=%1"
IF "%MultiTargets%"=="" SET "MultiTargets=all"

powershell .\tooling\GenerateAllSolution.ps1 -MultiTargets %MultiTargets%