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

namespace AHIFusion
{
	public sealed partial class LinkText : ContentDialog
	{
        public string selectedLink { get; set; }
        public SelectableNote selectedNote { get; set; }

		public LinkText(List<SelectableNote> notes)
		{
			this.InitializeComponent();

            NotesListView.ItemsSource = notes;
        }

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
            selectedLink = LinkTextBox.Text;
            selectedNote = NotesListView.SelectedItem as SelectableNote;
        }
	}
}
