﻿#pragma checksum "D:\BRB\BRB\Report.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9DA9EA4E7FBC602C85327EAB97FDDFD5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BRB
{
    partial class Report : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.Title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 2:
                {
                    this.Ring = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 3:
                {
                    this.listview = (global::Windows.UI.Xaml.Controls.ListBox)(target);
                }
                break;
            case 4:
                {
                    this.Day = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 33 "..\..\..\Report.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.Day).Tapped += this.FillTapped;
                    #line default
                }
                break;
            case 5:
                {
                    this.Yesterday = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 34 "..\..\..\Report.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.Yesterday).Tapped += this.FillTapped;
                    #line default
                }
                break;
            case 6:
                {
                    this.Week = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 35 "..\..\..\Report.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.Week).Tapped += this.FillTapped;
                    #line default
                }
                break;
            case 7:
                {
                    this.Month = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 36 "..\..\..\Report.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.Month).Tapped += this.FillTapped;
                    #line default
                }
                break;
            case 8:
                {
                    this.All = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                    #line 37 "..\..\..\Report.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBlock)this.All).Tapped += this.FillTapped;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

