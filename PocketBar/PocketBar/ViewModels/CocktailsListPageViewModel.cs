using PocketBar.Constants;
using PocketBar.Managers;
using PocketBar.Models;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PocketBar.ViewModels
{
    class CocktailsListPageViewModel: BaseViewModel
	{
		public ObservableCollection <Cocktail> Cocktails { get; set; }
		private CategoriesManager categoriesManager;
		private GlassesManager glassesManager;
		private IngredientsManager ingredientsManager;


		public CocktailsListPageViewModel(PageDialogService pageDialogService, INavigationService navigationService,CategoriesManager categoriesManager, GlassesManager glassesManager, IngredientsManager ingredientsManager) : base(pageDialogService, navigationService)
        {

			this.categoriesManager = categoriesManager;
            this.glassesManager = glassesManager;
            this.ingredientsManager = ingredientsManager;

            GetIngredient("Gin");
		}

		public async void GetCategories(string Category)
		{
			if (await this.HasInternetConnection())
			{
				try
				{
					IsLoading = true;
					var response = await categoriesManager.GetCocktailsByCategory(Category);
					Cocktails = response != null ? new ObservableCollection<Cocktail>(response.OrderBy(i => i.DrinkName)) : new ObservableCollection<Cocktail>();
					IsLoading = false;
				}
				catch (Exception e)
				{
					IsLoading = false;
					await this.ShowMessage(ErrorMessages.ErrorOccured, e.Message, ErrorMessages.Ok);
				}
			}

		}

		public async void GetGlass(string Glass)
		{
			if (await this.HasInternetConnection())
			{
				try
				{
					IsLoading = true;
					var resutl = await glassesManager.GetCocktailsByGlass(Glass);
					Cocktails = resutl != null ? new ObservableCollection<Cocktail>(resutl.OrderBy(i => i.DrinkName)) : new ObservableCollection<Cocktail>();
					IsLoading = false;
				}
				catch (Exception e)
				{
					IsLoading = false;
					await this.ShowMessage(ErrorMessages.ErrorOccured, e.Message, ErrorMessages.Ok);
				}
			}

		}

		public async void GetIngredient(string Ingredient)
		{
			if (await this.HasInternetConnection())
			{
				try
				{
					IsLoading = true;
					var resutl = await ingredientsManager.GetCocktailsByIngredient(Ingredient);
					Cocktails = resutl != null ? new ObservableCollection<Cocktail>(resutl.OrderBy(i => i.DrinkName)) : new ObservableCollection<Cocktail>();
					IsLoading = false;
				}
				catch (Exception e)
				{
					IsLoading = false;
					await this.ShowMessage(ErrorMessages.ErrorOccured, e.Message, ErrorMessages.Ok);
				}
			}

		}

		public async void Initialize(INavigationParameters parameters)
		{
			try
			{
				SearchType? type;
				string searchTerm = "";
				string Title = "";
				if (parameters == null || parameters.Count == 0) return;
				if (parameters.ContainsKey("type"))
				{
					type = (SearchType)parameters["type"];
				}
				else
				{
					await ShowMessage(ErrorMessages.ErrorOccured, ErrorMessages.MissingInformation, ErrorMessages.Ok);
					return;
				}

				if (parameters.ContainsKey("searchTerm"))
				{
					searchTerm = parameters["searchTerm"] as string;
				}
				else
				{
					await ShowMessage(ErrorMessages.ErrorOccured, ErrorMessages.MissingInformation, ErrorMessages.Ok);
					return;
				}

				if (parameters.ContainsKey("title"))
				{
					Title = parameters.GetValue<string>("title");
				}
				else
				{
					await ShowMessage(ErrorMessages.ErrorOccured, ErrorMessages.MissingInformation, ErrorMessages.Ok);
					return;
				}

				if(type != null && !string.IsNullOrEmpty(searchTerm))
				{
					switch (type)
					{
						case SearchType.Category:
							GetCocktailsByCategory(searchTerm);
							break;
						case SearchType.Glass:
							GetCocktailsByGlass(searchTerm);
							break;
						case SearchType.Ingredient:
							GetCocktailsByIngredient(searchTerm);
							break;
					}
				}
			}catch(Exception e)
			{
				await this.ShowMessage(ErrorMessages.ErrorOccured, e.Message, ErrorMessages.Ok);
			}
		}
	}
}
