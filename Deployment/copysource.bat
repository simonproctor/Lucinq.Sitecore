echo off
echo "executing: copysource.bat %1 %2"

echo copying %1 to %2
robocopy %1 %2 *.cs /xd bin obj properties /e /nfl /ndl /njh /njs /ns /nc /np

if errorlevel 7 echo OKCOPY + MISMATCHES + XTRA & goto end1
if errorlevel 5 echo OKCOPY + MISMATCHES & goto end1
if errorlevel 3 echo OKCOPY + XTRA & goto end1
if errorlevel 2 echo XTRA & goto end1
if errorlevel 1 echo OKCOPY & goto end1
if errorlevel 0 echo No Change & goto end1
:end1  