﻿using System.Windows;
using System.Windows.Controls;

namespace MorenoSystem.Common.Converter.Helper
{
    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxAssistant),
            new PropertyMetadata(false, OnBindPasswordChanged));

        private static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant),
                new PropertyMetadata(false));

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (PasswordBox)d;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d == null || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            var newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword ?? string.Empty;
            }

            box.LostFocus += HandlePasswordChanged;
        }

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            var box = (PasswordBox)dp;

            if (box == null)
            {
                return;
            }

            var wasBound = (bool)(e.OldValue);
            var needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.LostFocus -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.LostFocus += HandlePasswordChanged;
            }
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var box = (PasswordBox)sender;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        private static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }
    }
}