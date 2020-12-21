﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Desktop_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WorkVM _viewModel = new WorkVM();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void txtBox_Stunde_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            TextBox t1 = (TextBox)sender;
            Regex regex2 = new Regex("^([0-2]?[0-9])$");
            if (t1.Text.Length == 1)
            {
                string time = string.Format("{0}{1}", t1.Text, e.Text);
                e.Handled = !regex2.IsMatch(time);
                return;
            }
            if (t1.Text.Length > 1)
            {
                e.Handled = true;
                return;
            }
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtBox_Minute_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            TextBox t1 = (TextBox)sender;
            Regex regex2 = new Regex("^([0-5]?[0-9])$");
            if (t1.Text.Length == 1)
            {
                string time = string.Format("{0}{1}", t1.Text, e.Text);
                e.Handled = !regex2.IsMatch(time);
                return;
            }
            if (t1.Text.Length > 1)
            {
                e.Handled = true;
                return;
            }
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cal_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            //dataGrid_ViewEntries.SelectedIndex = _viewModel.DisplayEntries.IndexOf(_viewModel.DisplayEntries.Where(x => x.Begin.Date == ((Calendar)sender).SelectedDate).FirstOrDefault());
        }

        private void btn_SaveEntry_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBox_Minute.Text) ||
                string.IsNullOrWhiteSpace(txtBox_MinuteEnd.Text) ||
                string.IsNullOrWhiteSpace(txtBox_MinuteBreak.Text) ||
                string.IsNullOrWhiteSpace(txtBox_Stunde.Text) ||
                string.IsNullOrWhiteSpace(txtBox_StundeEnd.Text) ||
                string.IsNullOrWhiteSpace(txtBox_StundeBreak.Text)) return;

            var start = new DateTime(
                _viewModel.SelectedDate.Year,
                _viewModel.SelectedDate.Month,
                _viewModel.SelectedDate.Day,
                int.Parse(txtBox_Stunde.Text),
                int.Parse(txtBox_Minute.Text),
                0);

            var end = new DateTime(
                _viewModel.SelectedDate.Year,
                _viewModel.SelectedDate.Month,
                _viewModel.SelectedDate.Day,
                int.Parse(txtBox_StundeEnd.Text),
                int.Parse(txtBox_MinuteEnd.Text),
                0);

            var breakTime = new TimeSpan(
                int.Parse(txtBox_StundeBreak.Text),
                int.Parse(txtBox_MinuteBreak.Text),
                0);

            _viewModel.SaveEntries(start, end, breakTime);

            txtBoxDescription.Text = txtBox_Minute.Text = txtBox_MinuteBreak.Text = txtBox_MinuteEnd.Text = txtBox_Stunde.Text = txtBox_StundeBreak.Text = txtBox_StundeEnd.Text = string.Empty;
        }

        private void btn_DeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DeleteEntries();
        }

        private void MenuItem_OpenDB_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GetConnection();
        }

        private void MenuItem_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dataGrid_ViewEntries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.SelectedEntry = ((DataGrid)sender).SelectedItem as WorkEntry;
            txtBlock_CurrentInfos.Text = _viewModel.SelectedEntry?.Comment;
            btn_DeleteEntry.IsEnabled = !(_viewModel.SelectedEntry == null);
        }

        private void cal_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
        }
    }

}
