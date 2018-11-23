@echo off
chcp 936
rem 称重系统Git助手

echo ----------------------------------------
echo    注意：请确保您的git命令可以直接在cmd中运行，如果不能运行，请查看path环境变量
echo	修改 url = https://用户名:密码@项目地址
echo ----------------------------------------

cd /d D:\WorkSpace\VisualStudio\WPF\IntentConnectWeighing

git add .
set/p  ca=  请输入提交内容描述：
git commit -m "%ca%"

pause