echo off
echo "executing: copysiteoutput.bat %1"

echo copying app_config  to %1app_config
robocopy %1..\config\app_config %1App_config /e /xf *.cs *.bak connectionstrings.* /nfl /ndl /njh /njs /ns /nc /np > nul

echo copying app.config
robocopy %1..\config %1 App.Config /e /xf *.cs *.bak /nfl /ndl /njh /njs /ns /nc /np > nul

if errorlevel 7 echo OKCOPY + MISMATCHES + XTRA & goto end1
if errorlevel 5 echo OKCOPY + MISMATCHES & goto end1
if errorlevel 3 echo OKCOPY + XTRA & goto end1
if errorlevel 2 echo XTRA & goto end1
if errorlevel 1 echo OKCOPY & goto end1
if errorlevel 0 echo No Change & goto end1
:end1  