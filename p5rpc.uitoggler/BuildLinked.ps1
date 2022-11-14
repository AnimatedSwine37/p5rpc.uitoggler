# Set Working Directory
Split-Path $MyInvocation.MyCommand.Path | Push-Location
[Environment]::CurrentDirectory = $PWD

Remove-Item "$env:RELOADEDIIMODS/p5rpc.uitoggler/*" -Force -Recurse
dotnet publish "./p5rpc.uitoggler.csproj" -c Release -o "$env:RELOADEDIIMODS/p5rpc.uitoggler" /p:OutputPath="./bin/Release" /p:ReloadedILLink="true"

# Restore Working Directory
Pop-Location