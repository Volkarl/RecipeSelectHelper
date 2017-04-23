# RecipeSelectHelper
The purpose of the program is to rank recipes in regards to the needs/preferences that the user has for food, in order to ease the process of figuring out which food to cook. 

The program does this by ranking recipes by categories, type and ingredients, and allowing the user to mix and match search parameters into custom search methods. Additionally, the program allows ranking recipes by the ingredients that are currently in the fridge, even being able to take into account their individual expiration dates. 

------------------------------------------------------

Author: Jonathan Karlsson
The software is written in C#6 (.NET 4.6.2) using Visual Studio 2015 and WPF.

While the program is an interesting and useful project by itself, the deeper meaning of the creation of this project was to also learn about and experiment with: 
  * The C# programming language and programming through the object-oriented programming paradigm in general 
  * Creating a GUI through Windows Presentation Foundation (WPF) and writing XAML code
  * The supposed harm of not utilizing the MVVM pattern, and not shying away from using code-behind if easier
  
This means that there are lots of instances of "bad code" throughout the project, partly because it was written before I leart myself how to do it better (and why to do it better). The best example here is keeping the actual C# code clean of information from the user interface, because, as I figured out along the way, it's really difficult to read and hell to maintain. Slowly these old designs and implementations have been swapped out, however, there is certainly much left to do. I particularly chose not to use the MVVM pattern for the program, because it felt so overly restrictive and harmful to productivity and the gain was negligible in my previous semester project, 3. Semester: Danmission Inventory Management. 
