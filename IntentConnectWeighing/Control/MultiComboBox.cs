﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace IntentConnectWeighing
{
    /// <summary>
    /// MultiComboBox.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_ListBox", Type = typeof(ListBox))]
    public partial class MultiComboBox : ComboBox
    {
        /// <summary>
        /// 获取选择项集合
        /// </summary>
        public System.Collections.IList SelectedItems
        {
            get { return this._ListBox.SelectedItems; }
        }

        /// <summary>
        /// 设置或获取选择项
        /// </summary>
        public new int SelectedIndex
        {
            get { return this._ListBox.SelectedIndex; }
            set { this._ListBox.SelectedIndex = value; }
        }

        private ListBox _ListBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this._ListBox = Template.FindName("PART_ListBox", this) as ListBox;
            this._ListBox.SelectionChanged += _ListBox_SelectionChanged;
        }

        void _ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in this.SelectedItems)
            {
                sb.Append(item.ToString()).Append(";");
            }
            this.Text = sb.ToString().TrimEnd(';');
        }


        static MultiComboBox()
        {
            //OverridesDefaultStyleProperty.OverrideMetadata(typeof(MultiComboBox), new FrameworkPropertyMetadata(typeof(MultiComboBox)));
        }

        public void SelectAll()
        {
            this._ListBox.SelectAll();

        }

        public void UnselectAll()
        {
            this._ListBox.UnselectAll();
        }

        internal class MultiCbxBaseData
        {
            public MultiCbxBaseData()
            {
            }

            public int ID { get; set; }
            public string ViewName { get; set; }
            public bool IsCheck { get; set; }
        }
    }
}
