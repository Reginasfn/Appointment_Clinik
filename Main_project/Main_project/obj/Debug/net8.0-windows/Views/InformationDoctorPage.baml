   M S B A M L   `   `   `  ����G  CMain_project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Main_project.ViewsMain_project.Views  5      .    (Main_project.Views.InformationDoctorPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:Main_project.Views     5        N� Title$  InformationDoctor��5	      .��5      � 
������  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��20* �6   �� 5      $
��1.5* �6    � RowDefinitions 5   
   �� 5      $
��1.3* �6   �� 5      $	��10* �6   .+�5   
   �� -   5         ��Name$ Back_button��5      $��
Назад��5      $��22(�5      $��80��5      $	��Top=�5      $��Center=�6)   $��0��5      $��2��6   $��0,20,0,0q�6/   �� 5   
   ���	#FF002558$I��CПодробная информация об специалисте��5      $��Source Code Pro Semibold)�5       $��36(�5!      $��1��5"      $��20 0 0 0q�5#      $
��Left=�5$      $��Center=�6/   �� 5&   
   -   5         ��Name$ EmptySpecTxt��5&      $!��Информации нет��5'      $y�Hidden=�5(      ���	#FF002558$��Source Code Pro Black)�5*      $��34(�5+      $��1��5,      $��2��6$   $��
20 150 0 0q�5-      $
��Left=�5.      $	��Top=�6/   � 50   
   $��1��6   $��1��6    � ShowGridLines$ False��6,    51      �� 52      $��5* �6    �� 53      $
��150* �6     55      �� 56      $	��12* �6#   �� 57      $��* �6#   �� 58      $	��19* �6#   .+�5;      �� -   5         ��Name$ IconDoctorView��5;      $	��400��5=      $��1��5>      $��0��6   $	��450��5?      $��20 0 0 0q�5@      $
��Left=�5A      $	��Top=�5B      ��5<      �� *DisplayIconDoctor+5B      � 5D      $��2��6   $��1��6#    5E      �� 5F      $	��48* �6$   �� 5G      $
��110* �6$   �� 5H      $
��227* �6$   .+�5K      �� -   5        $ nameDoctorsTxt��5K      ���	#FF002558$��Source Code Pro Semibold)�5N      $��40(�5O      $��0 0 0 0q�5P      $��0��6)   $
��Left=�5Q      $	��Top=�63   ��5L      �� *DisplayNameDoctor+5Q   3   �� 5T      -   5        $ specialtyNameTxt��5T      ���	#FF2D4F7C$��Source Code Pro Semibold)�5V      $��28(�5W      $��5 0 0 0q�5X      $��1��6)   $
��Left=�5Y      $	��Top=�63   �� 5[      ���	#FF2D4F7C$��Source Code Pro Semibold)�5]      $��28(�5^      $��2 0 0 0q�5_      $��1��6)   $
��Left=�5`      $��Bottom=�63   ��5[      �� *DisplayMedExperience+5`   3   �� 5b      ���	#FF2D4F7C$��Source Code Pro Semibold)�5d      $��28(�5e      $��2 0 0 0q�5f      $��2��6)   $
��Left=�5g      $	��Top=�63   ��5b      �� *DisplayStatusWork+5g   3   5j      5k      5l      5m      