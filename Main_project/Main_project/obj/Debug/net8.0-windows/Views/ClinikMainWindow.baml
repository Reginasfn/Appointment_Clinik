   M S B A M L   `   `   `  ����G  CMain_project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Main_project.ViewsMain_project.Views  5   	   )    #Main_project.Views.ClinikMainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:Main_project.Views     5   	     � Title$  Главное меню��5   	   $	��500��5	   	   $	��500��6   $	��800��5
   	    � WindowState$ 	Maximized=�5   	    � Icon$ /Resources/icon.png��6!   .��5      � ���	#FFBCD2E3 � ShowGridLines$ False��6"    � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��5* �6   .+�5   
   � $��0��6   $ False��6    � ColumnDefinitions 5      �� 5      $	��15* �6#   �� 5      $
��200* �6#   �� 5      $	��50* �6#   �� 5      $	��50* �6#   �� 5      $	��10* �6#   .+�5      �� $��/Resources/image.png��6   $��1��5      $
��Left=�5      $��Center=�5      $	��200��5      $	��200��5        J� Effect 5!      3  -System.Windows.Media.Effects.DropShadowEffect  5"         ShadowDepth$ 1Q�6'      	Direction$ 0Q�67   	   Color$	 	#FF0063BD��6E   
   Opacity$	
 0.5Q�6W      
BlurRadius$ 30Q�6e   5$      �� 5&      -   5        $��Admin_panel��5&   %   $��
Админ��6   
�������$��25(�5(      $��	340,0,0,0q�5)      $��1��5*      $��Center=�5+      $��Center=�5,         TextBlockStyle#��[  5.       5/        50      $ 1Q�6'   $ 0Q�67   $	 	#FF0063BD��6E   $
 1Q�6W   $ 10Q�6c   52      �� 54      -   5        $��Click_aboutClinik��55      $��О клинике��54      
�������$��25(�57      $��40q�58      $��1��59      $��Right=�5:      $��Center=�5;      #��[  5=       5>        5?      $ 1Q�6'   $ 0Q�67   $	 	#FF0063BD��6E   $
 1Q�6W   $ 10Q�6c   5A      �� 5C      -   5        $��Click_contactsClinik��5D      $��Контакты��5C      
�������$��25(�5F      $��Black"�5G      $��0,0,20,0q�5H      $��2��5I      $��Center=�5J      $��Center=�5K      #��[  5M       5N        5O      $ 1Q�6'   $ 0Q�67   $	 	#FF0063BD��6E   $
 1Q�6W   $ 10Q�6c   5Q      �� 5S       �� CornerRadius$ 20��6   $��3��5T      ���	#FF3F51B5$��60��5V      $��Stretch=�5W      $��Center=�5X       �� Padding$ 10q�5Y      ._�5[      �� -   5        $��Click_appointmentClinik��5\      $"��Запись на прием��5[      $��23(�5]      $��Black"�5^      
�������$��Center=�5`      $��Center=�5a      #��[  5c       5d        5e      $ 1Q�6+   $ 0Q�6;   $	 	#FFA4C9E9��6I   $
 1Q�6[   $ 12Q�6g   5g      5h      5j      �� 5l   
   $��2��6   $
��1450��6   ���	#FF035096$	��Top=�5n      $��	0,130,0,0q�5o      $��3��6'    � Cursor$
 Hand��6;    5p        5q      $ 1Q�6#   $ 0Q�63   $	 	#FF0063BD��6A   $
 1Q�6S   $ 3Q�6_   5s      � 5u   
   $��1��6    5v      �� 5w      $��* �6#   �� 5x      $	��20* �6#   �� 5y      $��* �6#   .+�5|       � -   5          �Name$ 	mainframe��5}        � NavigationUIVisibility$ Hidden=�5~      $��1��5�      $��0,20,0,0q�6    5�      5�      5�      