using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pal.Web.Models
{

    public class EditableTable<T>where T: class
    {
        public List<string> Coulms { get; set; }
        public List<T> Data { get; set; }
    }
    public class SaveButtonViewModel
    {
        public string OnSave { get; set; }
        public string OnSaveAndNew { get; set; }
        public string BackToListhref { get; set; }
        public string SaveButtomName { get; set; } = "btn.save";
    }

    public class AddressViewModel
    {
        /// <summary>
        /// <list type="number">
        /// <item>Country</item>
        /// <item>Country + City</item>
        /// <item>Country + City + Region</item>
        /// <item>Full Address</item>
        /// </list>
        /// </summary>
        public int FieldsCount { get; set; }

        [Required(ErrorMessage = "Select Country!")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Select City!")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Select Region!")]
        public int? RegionId { get; set; }

        [Required(ErrorMessage = "Select Neighborhood!")]
        public int? NeighborhoodId { get; set; }

        public bool IsDisabled { get; set; } = false;
    }

    public class GridViewModel
    {
        public string GridId { get; set; } = "Grid";
        public dynamic Model { get; set; }
        public string OnCommandClick { get; set; } = "(e)=>commandClick(e)";
        public bool AllowSorting { get; set; } = true;
        public bool AllowMultiSorting { get; set; }
        public bool AllowFiltering { get; set; } = true;
        public bool AllowReordering { get; set; } = true;
        public bool AllowResize { get; set; }
        public bool AllowGrouping { get; set; }
        public bool AllowSelection { get; set; } = true;
        public bool AllowEditing { get; set; } = false;
        public string ActionBeginName { get; set; } = " ";


        public bool AllowToEdit { get; set; } = true;
		public bool IsAllowToDelete { get; set; } = true;
        public bool AllowToShowDetails { get; set; } = false;
        public bool AllowToChat { get; set; } = false;

        public string BlueButtonName { get; set; } = "btn.edit";
        public string RedButtonName { get; set; } = "btn.delete";
        public string RedButtonType { get; set; } = "DeleteBtn";
        public string BlueButtonType { get; set; } = "EditBtn";
        public bool IsShowFilterBarOperator { get; set; } = true;
        public bool IsInformationManaged { get; set; } = true;
        public string Url { get; set; }
        public string OnDataBound { get; set; } = "(e)=>OnDataBound(e)";


        public List<GridViewColumn> Columns { get; set; }


        public class GridViewColumn
        {
            public string Id { get; set; }
            public string Field { get; set; }
            public string HeaderText { get; set; }
            public string Width { get; set; }
            public GridColumnType ColumnType { get; set; } = GridColumnType.Text;
            public string Format { get; set; }
            public bool Visiable { get; set; } = true;
            public bool IsAllowEditing { get; set; } = true;
            public string Required { get; set; }
            public GridEditType EditType { get; set; } = GridEditType.TextBox;

        }

        public enum GridEditType
        {
            datetimepickeredit,
            numericedit,
            booleanedit,
            TextBox,

        }

        public enum GridColumnType
        {
            Text,
            RequiredText,
            Date,
            Number,
            Boolean,
            RateEditor,
            combobox,
        }
    }
    public class GridEditModel
    {
        public string GridId { get; set; } = "Grid";
        public dynamic Model { get; set; }
        public string OnCommandClick { get; set; } = "(e)=>commandClick(e)";
        public bool AllowSorting { get; set; }
        public bool AllowMultiSorting { get; set; }
        public bool AllowFiltering { get; set; }
        public bool AllowReordering { get; set; }
        public bool AllowResize { get; set; }
        public bool AllowGrouping { get; set; }
        public bool AllowSelection { get; set; } = true;
        public bool IsShowFilterBarOperator { get; set; }
        public string UrlAdd { get; set; }
        public string UrlEdit { get; set; }
        public string UrlDelete { get; set; }
        public string OnLoad { get; set; }
        public string OnActionComplete { get; set; }


        public List<GridEditColumn> Columns { get; set; }

        public class GridEditColumn
        {
            public bool IsPrimaryKey { get; set; }
            public bool IsVisible { get; set; } = true;
            public string Field { get; set; }
            public string HeaderText { get; set; }
            public string Width { get; set; }
            public string Format { get; set; }
            public object ValidationRules { get; set; }
            public ColumnType ColumnType { get; set; }
            public EditType EditType { get; set; }

        }

        public enum EditType
        {
            textedit,
            datetimepickeredit,
            booleanedit,
        }
        
        public enum ColumnType
        {
            boolean
        }
    }

    public class HtmlEditorModel
    {
        public string Id { get; set; }
        public string Height { get; set; } = "auto";
        public string ImgSaveUrl { get; set; }
        public string ImgPath { get; set; }
        public string Value { get; set; }
        public bool IsDisabled { get; set; } = false;

    }
}
