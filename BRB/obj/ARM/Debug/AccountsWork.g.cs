﻿#pragma checksum "D:\BRB\BRB\AccountsWork.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "49D79081F9D86BEEC83E33912C8FCE9A"
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
    partial class AccountsWork : 
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
                    this.AccountLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 3:
                {
                    this.ClientsView = (global::Windows.UI.Xaml.Controls.FlipView)(target);
                }
                break;
            case 4:
                {
                    this.ChooseLabel = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.Ring = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 6:
                {
                    this.AddDeleteGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 7:
                {
                    this.EditBlockGrid = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 8:
                {
                    this.ProgRing = (global::Windows.UI.Xaml.Controls.ProgressRing)(target);
                }
                break;
            case 9:
                {
                    this.AddEditDialog = (global::Windows.UI.Xaml.Controls.ContentDialog)(target);
                }
                break;
            case 10:
                {
                    this.AddID = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 79 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.AddID).TextChanged += this.AddID_TextChanged;
                    #line default
                }
                break;
            case 11:
                {
                    this.AddName = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 80 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.AddName).TextChanged += this.AddName_TextChanged;
                    #line default
                }
                break;
            case 12:
                {
                    this.AddLogin = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 81 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.AddLogin).TextChanged += this.LoginCode_TextChanged;
                    #line default
                }
                break;
            case 13:
                {
                    this.AddPass = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 82 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.AddPass).TextChanged += this.AddPass_TextChanged;
                    #line default
                }
                break;
            case 14:
                {
                    this.AddCode = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 83 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.AddCode).TextChanged += this.LoginCode_TextChanged;
                    #line default
                }
                break;
            case 15:
                {
                    this.AddCash = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 84 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.AddCash).TextChanged += this.AddCash_TextChanged;
                    #line default
                }
                break;
            case 16:
                {
                    this.EditPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    #line 66 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.StackPanel)this.EditPanel).Tapped += this.Edit_Tapped;
                    #line default
                }
                break;
            case 17:
                {
                    this.BlockPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    #line 70 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.StackPanel)this.BlockPanel).Tapped += this.Block_Tapped;
                    #line default
                }
                break;
            case 18:
                {
                    this.AddPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    #line 52 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.StackPanel)this.AddPanel).Tapped += this.Add_Tapped;
                    #line default
                }
                break;
            case 19:
                {
                    this.ClearPanel = (global::Windows.UI.Xaml.Controls.StackPanel)(target);
                    #line 56 "..\..\..\AccountsWork.xaml"
                    ((global::Windows.UI.Xaml.Controls.StackPanel)this.ClearPanel).Tapped += this.Clear_Tapped;
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

