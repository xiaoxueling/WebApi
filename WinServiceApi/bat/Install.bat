@echo off
echo ��װ FSTAPI ����
echo %cd%
%systemroot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe %cd%..\..\WinServiceApi.exe
net start FSTAPI
pause