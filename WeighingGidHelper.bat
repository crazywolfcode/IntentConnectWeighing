@echo off
chcp 936
rem ����ϵͳGit����

echo ----------------------------------------
echo    ע�⣺��ȷ������git�������ֱ����cmd�����У�����������У���鿴path��������
echo	�޸� url = https://�û���:����@��Ŀ��ַ
echo ----------------------------------------

cd /d D:\WorkSpace\VisualStudio\WPF\IntentConnectWeighing

git add .
set/p  ca=  �������ύ����������
git commit -m "%ca%"

pause