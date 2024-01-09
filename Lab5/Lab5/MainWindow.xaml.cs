using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace Lab5
{
    public partial class MainWindow
    {
        private ObservableCollection<CalculationResult> Results { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Results = new ObservableCollection<CalculationResult>();
            logListView.ItemsSource = Results;
        }

        private void ExecuteCommand_Click(object sender, RoutedEventArgs e)
        {
            var command = inputTextBox.Text.Replace(',', '.');
            if (Results.Count == 0)
            {
                command = $"0{command}";
            }
            else if (Results[^1].IsError)
            {
                command = $"0{command}";
            }
            else
            {
                command = $"{Results[^1].Result}{command}";
            }

            try
            {
                var result = EvaluateCommand(command);
                Results.Add(new CalculationResult { Operation = command, Result = result.ToString().Replace(',', '.') });
            }
            catch (Exception ex)
            {
                Results.Add(new CalculationResult { Operation = command, Result = $"Error: {ex.Message}", IsError = true });
            }

            inputTextBox.Clear();
        }
        
        private void inputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ExecuteCommand_Click(sender, new RoutedEventArgs());
            }
        }
        
        private void logListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (logListView.SelectedItem == null) return;
            var selectedResult = (CalculationResult)logListView.SelectedItem;

            if (selectedResult.IsError) return;
            logListView.ScrollIntoView(selectedResult);
            Results.Add(selectedResult);
        }

        private double EvaluateCommand(string command)
        {
            var dataTable = new DataTable();
            return Convert.ToDouble(dataTable.Compute(command, ""));
        }
    }

    public class CalculationResult
    {
        public string Operation { get; set; }
        public string Result { get; set; }
        public bool IsError { get; set; }
    }
}