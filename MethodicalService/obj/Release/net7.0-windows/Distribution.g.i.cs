﻿#pragma checksum "..\..\..\Distribution.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E3B1BFBC4FF44E60EE5CA9B89D4735EB173C90C2"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using MethodicalService;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MethodicalService {
    
    
    /// <summary>
    /// Distribution
    /// </summary>
    public partial class Distribution : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 22 "..\..\..\Distribution.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid distributionGrid;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Distribution.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContextMenu MenuAddEditDelete;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Distribution.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MenuItemEdit;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Distribution.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem MenuItemDelete;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Distribution.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTextColumn id;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Distribution.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox textSearch;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/MethodicalService;component/distribution.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Distribution.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.distributionGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 22 "..\..\..\Distribution.xaml"
            this.distributionGrid.Loaded += new System.Windows.RoutedEventHandler(this.DistributionGrid_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MenuAddEditDelete = ((System.Windows.Controls.ContextMenu)(target));
            return;
            case 3:
            this.MenuItemEdit = ((System.Windows.Controls.MenuItem)(target));
            
            #line 25 "..\..\..\Distribution.xaml"
            this.MenuItemEdit.Click += new System.Windows.RoutedEventHandler(this.MenuItemEdit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MenuItemDelete = ((System.Windows.Controls.MenuItem)(target));
            
            #line 26 "..\..\..\Distribution.xaml"
            this.MenuItemDelete.Click += new System.Windows.RoutedEventHandler(this.MenuItemDelete_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.id = ((System.Windows.Controls.DataGridTextColumn)(target));
            return;
            case 6:
            this.textSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\Distribution.xaml"
            this.textSearch.GotFocus += new System.Windows.RoutedEventHandler(this.TextSearch_GotFocus);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\Distribution.xaml"
            this.textSearch.LostFocus += new System.Windows.RoutedEventHandler(this.TextSearch_LostFocus);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\Distribution.xaml"
            this.textSearch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.TextSearch_TextChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
