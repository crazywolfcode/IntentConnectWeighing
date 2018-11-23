@echo off
chcp 936
rem 称重系统Git助手

echo ----------------------------------------
echo    注意：请确保您的git命令可以直接在cmd中运行，如果不能运行，请查看path环境变量
echo	修改 url = https://用户名:密码@项目地址
echo ----------------------------------------
echo        1，提交Dev
echo        2，推送dev
echo        3，合并到Master
echo        4，推送Master
echo        5，退出

cd /d D:\WorkSpace\VisualStudio\WPF\IntentConnectWeighing

goto select

:select
set/p n=  请选择：
if "%n%" =="1"( goto commit) else(if "%n%" =="2"( goto pushDev)else(if "%n%" =="3"( goto mergetomaster ) else(if "%n%" =="1"( goto pushmaster) else(if "%n%" =="5"( goto exitt) else( exit )))))
:commit
git add .
set/p  ca=  请输入提交内容描述：
git commit -m "%ca%"
echo  完成
echo  去 推送
set/p  r=  请输入 y/n：
if "%r%" == "y"( goto pushDev) else( if "%r%" == "n" ( exit ))

:pushDev 
git push origin dev
echo  完成
echo  是否合并到Master
set/p  f=  请输入 y/n：
if "%f%" == "y"( goto mergetomaster )else( if "%f%" == "n" ( exit ))

:mergetomaster 
git checkout master
git merge dev

echo  完成
echo  是否推送Master
set/p  f=  请输入 y/n：
if "%f%" == "y"( goto pushmaster )else( if "%f%" == "n" ( exit ))

:pushmaster 
git push origin master
git checkout dev
:exitt
exit