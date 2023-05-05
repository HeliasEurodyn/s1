@ECHO OFF

REM The following directory is for .NET 2.0
set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v2.0.50727
set PATH=%PATH%;%DOTNETFX2%

echo Installing WindowsService...
echo ---------------------------------------------------
InstallUtil /i C:\Users\Vitatron\Documents\Visual Studio 2012\Projects\ServiceUnitech\ServiceKazakidisCustomers\WindowsService\bin\Release\WindowsService.exe
echo ---------------------------------------------------
echo Done.

pause