    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace RecipeAppUpdated
    {
        public delegate void CaloriesExceededEventHandler(object sender, EventArgs e);

        public class Recipe
        {
            public event CaloriesExceededEventHandler CaloriesExceeded;

            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }
            public List<string> Steps { get; set; }
            public double ScaleFactor { get; set; }

            public Recipe(string name)
            {
                Name = name;
                Ingredients = new List<Ingredient>();
                Steps = new List<string>();
                ScaleFactor = 1;
            }

            public void AddIngredient(string name, double quantity, string unit, int calories, string foodGroup)
            {
                Ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
            }

            public void AddStep(string step)
            {
                Steps.Add(step);
            }

            public void ScaleRecipe(double factor)
            {
                ScaleFactor = factor;
            }

            public void ResetScale()
            {
                ScaleFactor = 1;
            }

            public int TotalCalories
            {
                get
                {
                    return (int)(Ingredients.Sum(i => i.Calories) * ScaleFactor);
                }
            }

            public override string ToString()
            {
                string result = $"Recipe: {Name}\n\nIngredients:\n";
                foreach (var ingredient in Ingredients)
                {
                    result += $"{ingredient.Name}: {ingredient.Quantity * ScaleFactor} {ingredient.Unit}\n";
                }
                result += $"\nTotal Calories: {TotalCalories}\n";
                if (TotalCalories > 300 && CaloriesExceeded != null)
                    CaloriesExceeded(this, EventArgs.Empty);
                result += "\nSteps:\n";
                foreach (var step in Steps)
                {
                    result += $"{step}\n";
                }
                return result;
            }
        }

        public class Ingredient
        {
            public string Name { get; set; }
            public double Quantity { get; set; }
            public string Unit { get; set; }
            public int Calories { get; set; }
            public string FoodGroup { get; set; }

            public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
            {
                Name = name;
                Quantity = quantity;
                Unit = unit;
                Calories = calories;
                FoodGroup = foodGroup;
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                var recipes = new List<Recipe>();

                while (true)
                {
                    Console.WriteLine("Enter recipe name (or 'exit' to quit):");
                    var recipeName = Console.ReadLine();
                    if (recipeName == "exit")
                        break;

                    var recipe = new Recipe(recipeName);
                    recipe.CaloriesExceeded += Recipe_CaloriesExceeded;

                    Console.WriteLine("Enter number of ingredients:");
                    int numIngredients = int.Parse(Console.ReadLine());
                    for (int i = 0; i < numIngredients; i++)
                    {
                        Console.WriteLine("Enter ingredient name:");
                        var ingredientName = Console.ReadLine();
                        Console.WriteLine("Enter ingredient quantity:");
                        var ingredientQuantity = double.Parse(Console.ReadLine());
                        Console.WriteLine("Enter ingredient unit:");
                        var ingredientUnit = Console.ReadLine();
                        Console.WriteLine("Enter ingredient calories:");
                        var ingredientCalories = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter ingredient food group:");
                        var ingredientFoodGroup = Console.ReadLine();
                        recipe.AddIngredient(ingredientName, ingredientQuantity, ingredientUnit, ingredientCalories, ingredientFoodGroup);
                    }

                    Console.WriteLine("Enter number of steps:");
                    int numSteps = int.Parse(Console.ReadLine());
                    for (int i = 0; i < numSteps; i++)
                    {
                        Console.WriteLine("Enter step description:");
                        var stepDescription = Console.ReadLine();
                        recipe.AddStep(stepDescription);
                    }

                    recipes.Add(recipe);
                }

                recipes.Sort((x, y) => x.Name.CompareTo(y.Name));

                while (true)
                {
                    Console.WriteLine("\nRecipes:");
                    for (int i = 0; i < recipes.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                    }
                    Console.WriteLine("\nEnter recipe number to display (or 'exit' to quit):");
                    var input = Console.ReadLine();
                    if (input == "exit")
                        break;

                    int recipeIndex = int.Parse(input) - 1;
                    if (recipeIndex >= 0 && recipeIndex < recipes.Count)
                    {
                        var recipe = recipes[recipeIndex];
                        Console.WriteLine(recipe);

                        while (true)
                        {
                            Console.WriteLine("\nEnter scale factor (0.5, 2, 3), 'reset' to reset scale, 'clear' to clear recipe data, or 'back' to go back:");
                            input = Console.ReadLine();
                            if (input == "back")
                                break;
                            if (input == "clear")
                            {
                                recipes.Clear();
                                break;
                            }
                            if (input == "reset")
                            {
                                recipe.ResetScale();
                            }
                            else
                            {
                                double scaleFactor = double.Parse(input);
                                recipe.ScaleRecipe(scaleFactor);
                            }
                            Console.WriteLine(recipe);
                        }
                    }
                }
            }

            private static void Recipe_CaloriesExceeded(object sender, EventArgs e)
            {
                Console.WriteLine("Warning: Recipe exceeds 300 calories!");
            }
        }
    }
