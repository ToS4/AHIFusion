using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Serilog;

namespace AHIFusion
{
	public sealed partial class LinkText : ContentDialog
	{
        public string selectedLink { get; set; }
        public SelectableNote selectedNote { get; set; }

		public LinkText(List<SelectableNote> notes)
		{
            try 
            {
                this.InitializeComponent();

                NotesListView.ItemsSource = notes;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while initializing LinkText");
                throw new ArgumentException("Error, please check logs!");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
            try
            {
                selectedLink = LinkTextBox.Text;
                selectedNote = NotesListView.SelectedItem as SelectableNote;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while handling ContentDialog_PrimaryButton click event");
                throw new ArgumentException("Error, please check logs!");
            }
            finally 
            {
                Log.CloseAndFlush();
            }    
        }
	}
}
