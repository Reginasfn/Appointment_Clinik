   M S B A M L   `   `   `  ����G  CMain_project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:Main_project.ViewsMain_project.Views  5      +    %Main_project.Views.AllSpecialtiesPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:Main_project.Views     5        N� Title$  AllSpecialist��5	      .��5      � 
������  � ColumnDefinitions 5   
   �� 5      $��2* �6   �� 5      $	��20* �6   �� 5      $
��1.5* �6    � RowDefinitions 5   
   �� 5      $
��1.3* �6   �� 5      $��1* �6   �� 5      $	��10* �6   .+�5   
   �� -   5         ��Name$ Back_button��5      $��
Назад��5      $��22(�5      $��80��5      $	��Top=�5      $��Center=�6)   $��0��5      $��2��6   $��0,20,0,0q�6/   �� 5   
   -   5         ��Name$ EmptySpecTxt��5      $%��Специалистов нет��5       $y�Visible=�5!      ���	#FF002558$��Source Code Pro Black)�5#      $��34(�5$      $��1��5%      $��2��6$   $��
20 150 0 0q�5&      $
��Left=�5'      $	��Top=�6/   �� 5)   
   ���	#FF002558$-��'Выберите специалиста��5*      $��Source Code Pro Semibold)�5+      $��36(�5,      $��1��5-      $��20 0 0 0q�5.      $
��Left=�5/      $��Center=�6/   �� 51   
   -   5        $ selectedSpecialtyTxtbx��51      ���	#FF002558$��1��52      $��1��6!   $��Source Code Pro Semibold)�53      $��26(�54      $��20 0 0 0q�55      $
��Left=�56      $	��Top=�6/   � 58   
   $��3��6   $��1��6   .+�59      �� -   5         ��Name$ doctorsListView��59      
�������$��Visible=�5;      ��5=      �� 5>      .R�5?      �  � Orientation$ 
Horizontal=�5@       � 	ItemWidth$
 1200��5C       � 
ItemHeight$	 360��6*   ��5A      �� *ActualWidth+	 �� RelativeSource	 �� 
 �� AncestorType
 ScrollContentPresenter5C   *   5D      5F      5G      5H      5I      