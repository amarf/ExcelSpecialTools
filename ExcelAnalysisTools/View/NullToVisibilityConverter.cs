﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ExcelAnalysisTools.View
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public bool IsNullToVisible { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return IsNullToVisible ? Visibility.Visible : Visibility.Collapsed;
            return IsNullToVisible ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}