using Microsoft.UI.Xaml.Input;

namespace AHIFusion;
public sealed partial class AddRectControl : UserControl
{
    public AddRectControl()
    {
        this.InitializeComponent();
    }
    private void Rect_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        rect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 149, 149, 149)); // Change to a darker color when the pointer enters
    }

    private void Rect_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        rect.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 169, 169, 169)); // Change back to the original color when the pointer exits
    }
}
