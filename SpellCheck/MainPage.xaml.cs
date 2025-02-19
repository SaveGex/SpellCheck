using SpellCheck.Auxilillary_classes;
using SpellCheck.Interfaces;

namespace SpellCheck
{
    public partial class MainPage : ContentPage
    {

        private readonly int _row = DeviceInfo.Platform == DevicePlatform.WinUI ? 5 : DeviceInfo.Platform == DevicePlatform.Android ? 3 : 3;
        
        private readonly int _col = DeviceInfo.Platform == DevicePlatform.WinUI ? 4 : DeviceInfo.Platform == DevicePlatform.Android ? 2 : 1;

        private List<BaseElement> _Elements {  get; set; } = new List<BaseElement>();


        public MainPage()
        {
            InitializeComponent();
            CreateGrid();
        }


        private void CreateGrid()
        {
            viewGrid.Children.Clear();
            viewGrid.RowDefinitions.Clear();
            viewGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < _row; i++)
                viewGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            for (int i = 0; i < _col; i++)
                viewGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            if (_Elements.Count <= 0)
                return;

            AddContent();            
        }


        private void AddContent()
        {
            int cols = 0, rows = 0;
            
            foreach(var element in _Elements)
            {
                if(cols == (_col - 1))
                    rows++;
                
                if (rows == (_row - 1))
                    break;

                Frame some_frame = CreatorFrames.CreateFrame(element);

                viewGrid.Children.Add(some_frame);
                viewGrid.SetRow(some_frame, rows);
                viewGrid.SetColumn(some_frame, cols);

                cols++;
            }
        }


    }

}
