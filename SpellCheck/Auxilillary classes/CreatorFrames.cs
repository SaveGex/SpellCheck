using SpellCheck.Interfaces;
using SpellCheck.Models;

namespace SpellCheck.Auxilillary_classes
{
    internal static class CreatorFrames
    {
        public static Frame CreateFrame(BaseElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
           

            if (element is CountingElement)
                return CountingFrame((CountingElement)element);
            else
                return new Frame();
            
        }


        private static Frame CountingFrame(CountingElement element)
        {
            var title = new Label
            {
                Text = element.Description,
                FontAttributes = FontAttributes.Italic,
                FontSize = 12,
                TextColor = Colors.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 2, 0, 2),
            };

            var id = new Label
            {
                Text = element.Id.ToString(),
                IsVisible = false,
            };

            var button = new Button
            {
                Text = "Details",
                BackgroundColor = Colors.LightBlue,
                WidthRequest = 80,
                FontSize = 12,
                TextColor = Colors.FloralWhite,
                //Command = new Command(() => ShowDetails(element)),
            };

            var buttonDelImg = new ImageButton
            {
                Source = "red_trash_canon_icon.png",
                WidthRequest = 30,
                HeightRequest = 30,
                BackgroundColor = Colors.Transparent // При потребі
            };

            var horizontalLayout = new HorizontalStackLayout
            {
                Children = { button, buttonDelImg }
            };

            var stackLayout = new StackLayout
            {
                Padding = new Thickness(5, 2, 5, 2),
                Children = { title, horizontalLayout, id }
            };

            var frame = new Frame
            {
                CornerRadius = 10,
                BorderColor = Colors.White,
                BackgroundColor = Colors.LightGray,
                //Content = stackLayout,
                Margin = new Thickness(5),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            return frame;
        }
    }
}
