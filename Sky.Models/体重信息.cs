using System;
using System.Collections.Generic;
using System.ComponentModel;
using XCode;
using XCode.Configuration;
using XCode.DataAccessLayer;

namespace Sky.Models
{
    /// <summary>体重信息</summary>
    [Serializable]
    [DataObject]
    [Description("体重信息")]
    [BindTable("Weight", Description = "体重信息", ConnName = "Conn", DbType = DatabaseType.SqlServer)]
    public partial class Weight : IWeight
    {
        #region 属性
        private Int32 _Id;
        /// <summary>Id</summary>
        [DisplayName("Id")]
        [Description("Id")]
        [DataObjectField(true, true, false, 0)]
        [BindColumn("Id", "Id", "int")]
        public Int32 Id { get { return _Id; } set { if (OnPropertyChanging(__.Id, value)) { _Id = value; OnPropertyChanged(__.Id); } } }

        private DateTime _AddDate;
        /// <summary>添加日期</summary>
        [DisplayName("添加日期")]
        [Description("添加日期")]
        [DataObjectField(false, false, true, 0)]
        [BindColumn("AddDate", "添加日期", "datetime", Master = true)]
        public DateTime AddDate { get { return _AddDate; } set { if (OnPropertyChanging(__.AddDate, value)) { _AddDate = value; OnPropertyChanged(__.AddDate); } } }

        private String _Value;
        /// <summary>体重</summary>
        [DisplayName("体重")]
        [Description("体重")]
        [DataObjectField(false, false, true, 50)]
        [BindColumn("Value", "体重", "nvarchar(50)", Master = true)]
        public String Value { get { return _Value; } set { if (OnPropertyChanging(__.Value, value)) { _Value = value; OnPropertyChanged(__.Value); } } }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public override Object this[String name]
        {
            get
            {
                switch (name)
                {
                    case __.Id : return _Id;
                    case __.AddDate : return _AddDate;
                    case __.Value : return _Value;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case __.Id : _Id = Convert.ToInt32(value); break;
                    case __.AddDate : _AddDate = Convert.ToDateTime(value); break;
                    case __.Value : _Value = Convert.ToString(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>取得体重信息字段信息的快捷方式</summary>
        public partial class _
        {
            /// <summary>Id</summary>
            public static readonly Field Id = FindByName(__.Id);

            /// <summary>添加日期</summary>
            public static readonly Field AddDate = FindByName(__.AddDate);

            /// <summary>体重</summary>
            public static readonly Field Value = FindByName(__.Value);

            static Field FindByName(String name) { return Meta.Table.FindByName(name); }
        }

        /// <summary>取得体重信息字段名称的快捷方式</summary>
        public partial class __
        {
            /// <summary>Id</summary>
            public const String Id = "Id";

            /// <summary>添加日期</summary>
            public const String AddDate = "AddDate";

            /// <summary>体重</summary>
            public const String Value = "Value";
        }
        #endregion
    }

    /// <summary>体重信息接口</summary>
    public partial interface IWeight
    {
        #region 属性
        /// <summary>Id</summary>
        Int32 Id { get; set; }

        /// <summary>添加日期</summary>
        DateTime AddDate { get; set; }

        /// <summary>体重</summary>
        String Value { get; set; }
        #endregion

        #region 获取/设置 字段值
        /// <summary>获取/设置 字段值</summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        Object this[String name] { get; set; }
        #endregion
    }
}