   M S B A M L   `   `   `  ����M  IWpfApp_NetworkGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:WpfApp_NetworkGameWpfApp_NetworkGame  5   	   #    WpfApp_NetworkGame.ShopWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:WpfApp_NetworkGame     5   	     � Title$  
ShopWindow��5   	   $	��582��6   $	��400��6)   .��5	      � }�. .+�5
      �� -   5         ��Name$ CardImg��5
   
   $��CardImg/Ver1/A014.png��6   6:   � 5      $��1��6	   $��1��6   z�5      �� 5      -   5        gdScale1�� �� ScaleX$ 1Q�5   '    �� ScaleY$	 0.5Q�62    � Triggers 5      :� 5       :� RoutedEvent$ Grid.MouseDown��6   [�5      �� 5      .��5   	   �� .��5   
   W�  �� 
TargetName$ gdScale1��6    �� TargetProperty$ ScaleY�6;   	 W� To$ 0X�6^   	 g� Duration$	 0:0:0.2D�6e   5   
   5   	   5      .+�5      �� $ ��CardImg/Default/back_1.jpg��6   $
��Fill=�6/   5      � 5      $��1��6	   $��1��6   
���iii�
 J� RenderTransformOrigin$	
 1,1&�6;   z�5      �� 5      -   5        gdScale2��$ 1Q�5   '   $	 0.5Q�62    5       :� 5!      $ Grid.MouseDown��6   [�5"      �� 5#      .��5$   	   �� .��5%   
   W� $ gdScale2��6   $ ScaleY�6;   $ 0X�6^   $	 0:0:0.2D�6e   5&   
   5'   	   5)      .+�5+      �� $ ��CardImg/Default/back_2.jpg��6   $
��Fill=�6/   5,      5-      5.      