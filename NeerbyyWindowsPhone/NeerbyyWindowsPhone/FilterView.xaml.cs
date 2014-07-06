using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NeerbyyWindowsPhone
{
    /// <summary>
    /// Set filter for the navigation
    /// </summary>
    public partial class FilterView : PhoneApplicationPage
    {

        /// <summary>
        /// Display the categories
        /// </summary>
        private void displayCategories() {
            foreach (Category cat in ((App)Application.Current).categories.categories)
                        {
                            UI.CategoryPreview display_category = new UI.CategoryPreview();

                            display_category.myCategory = cat;

                            display_category.display_name.Text = cat.name;
                            listingCategories.Children.Add(display_category);
                            scrollViewer.UpdateLayout();
                        }
        }

        /// <summary>
        ///  Default constructor
        /// </summary>
        public FilterView()
        {
            InitializeComponent();

            if (((App)Application.Current).categories == null)
            {
                CategoryListResult categories = ((App)Application.Current).categories;

                (WebApi.Singleton).CategoriesAsync((string responseMessage, CategoryListResult result) =>
                {
                    ((App)Application.Current).categories = result;
                    displayCategories();
                }, (String responseMessage, Exception exception) =>
                {

                });
            }
            else
            {
                displayCategories();
            }
        }

        /// <summary>
        /// View will appear
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            updateView();
        }

        /// <summary>
        /// Update the selected category
        /// </summary>
        private void updateView()
        {

            if (((App)Application.Current).currentCategory != null)
                actual_filter.Text = "Filtre actuel : " + ((App)Application.Current).currentCategory.name;
            else
                actual_filter.Text = "Filtre actuel : Aucun";
        }
            
        /// <summary>
        /// Changed the current category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollViewer_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            updateView();
        }
    }

}