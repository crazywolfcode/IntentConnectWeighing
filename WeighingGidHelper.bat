@echo off
chcp 936
rem ����ϵͳGit����

echo ----------------------------------------
echo    ע�⣺��ȷ������git�������ֱ����cmd�����У�����������У���鿴path��������
echo	�޸� url = https://�û���:����@��Ŀ��ַ
echo ----------------------------------------
echo        1���ύDev
echo        2������dev
echo        3���ϲ���Master
echo        4������Master
echo        5���˳�

cd /d D:\WorkSpace\VisualStudio\WPF\IntentConnectWeighing

goto select

:select
set/p n=  ��ѡ��
if "%n%" =="1"( goto commit) else(if "%n%" =="2"( goto pushDev)else(if "%n%" =="3"( goto mergetomaster ) else(if "%n%" =="1"( goto pushmaster) else(if "%n%" =="5"( goto exitt) else( exit )))))
:commit
git add .
set/p  ca=  �������ύ����������
git commit -m "%ca%"
echo  ���
echo  ȥ ����
set/p  r=  ������ y/n��
if "%r%" == "y"( goto pushDev) else( if "%r%" == "n" ( exit ))

:pushDev 
git push origin dev
echo  ���
echo  �Ƿ�ϲ���Master
set/p  f=  ������ y/n��
if "%f%" == "y"( goto mergetomaster )else( if "%f%" == "n" ( exit ))

:mergetomaster 
git checkout master
git merge dev

echo  ���
echo  �Ƿ�����Master
set/p  f=  ������ y/n��
if "%f%" == "y"( goto pushmaster )else( if "%f%" == "n" ( exit ))

:pushmaster 
git push origin master
git checkout dev
:exitt
exit